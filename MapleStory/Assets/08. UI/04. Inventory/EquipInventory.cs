using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipInventory : Inventory
{
    [SerializeField] LayerMask _DivisionLayer = 0; // 레이 캐스트 선택

    //private Slot[] m_pSlots = null;

    private Equip m_pEquip = null;

    private float m_fClickTimeAcc = 0.0f;

    private float m_fDoudleClickTimeAcc = 0.0f;

    private ICON m_pMouseSelectICon = null;

    private bool m_bIsClick = false;

    private bool m_bIsDoubleclick = false;

    private bool m_bIsMoving = false;

    private RectTransform m_pRectTransform = null;

    //public Slot[] AccessSlots
    //{
    //    get { return m_pSlots; }
    //}

    private void Awake()
    {
        if(null == m_pSlots)
            m_pSlots = GetComponentsInChildren<Slot>();

        GameObject _Object = GameObject.Find("Inventory UI");

        m_pEquip = _Object.transform.Find("Equip").gameObject.GetComponent<Equip>();

        m_pRectTransform = transform.parent.parent.GetComponent<RectTransform>();
    }


    private void Update()
    {
        Vector2 _PickingPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Ray2D _Ray = new Ray2D(_PickingPosition, Vector2.zero);

        RaycastHit2D _HitCast = Physics2D.Raycast(_Ray.origin, _Ray.direction, 1.0f, _DivisionLayer);

        if (_HitCast.collider != null && Input.GetMouseButton(0))
        {
            if (_HitCast.collider.CompareTag("Inventory") == true)
                m_bIsMoving = true;
        }

        if (m_bIsMoving == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_bIsMoving = false;

                return;
            }

            m_pRectTransform.position = _PickingPosition;

            return;
        }

        if (_HitCast.collider != null && Input.GetMouseButtonDown(0))
        {
            if (m_bIsClick == false)
            {
                InstacneCopyICon(_HitCast);

                m_pSoundManager.PlaySound("Start Drop Click Sound");

                m_bIsClick = true;

                return;
            }

            m_bIsClick = false;

            MountingItem(_HitCast);

            m_pSoundManager.PlaySound("End Drop Click Sound");
        }
        else if (_HitCast.collider == null && Input.GetMouseButtonDown(0) && m_bIsClick == true)
        {
            if (null != m_pMouseSelectICon)
            {
                Destroy(m_pMouseSelectICon.gameObject);

                m_pMouseSelectICon = null;

                m_bIsClick = false;

                m_pSoundManager.PlaySound("End Drop Click Sound");
            }
        }
    }

    private void InstacneCopyICon(RaycastHit2D _HitCast)
    {
        GameObject _GameObject = _HitCast.collider.gameObject;

        if (null == _GameObject)
            return;

        Vector2 _MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Vector2 _MyPosition = _GameObject.transform.position; // 아이콘의 부모는 슬롯

        Vector2 _Direction = _MyPosition - _MousePosition;

        if (_Direction.magnitude <= 20.0f && null == m_pMouseSelectICon)
        {
            Slot _CollisionSolt = _GameObject.GetComponent<Slot>();

            if (null == _CollisionSolt)
                return;

            ICON _CollisionICon = _CollisionSolt.AccessICon;

            if (null == _CollisionICon)
                return;

            GameObject _InstanceCopyIconObject = GameObject.Instantiate(_CollisionICon.gameObject);

            _InstanceCopyIconObject.layer = LayerMask.NameToLayer("Copy");

            _InstanceCopyIconObject.transform.SetParent(transform.parent);

            ICON _CopyICon = _InstanceCopyIconObject.GetComponent<ICON>();

            if (null == _CopyICon)
            {
                Destroy(_InstanceCopyIconObject);

                return;
            }

            _CopyICon.AccessMySlot = _CollisionSolt;

            _CopyICon.AccessMovingICon = true;

            _CopyICon.AccessOrlGameObejct = _CollisionICon.gameObject;

            m_pMouseSelectICon = _CopyICon;
        }
    }

    private void MountingItem(RaycastHit2D _HitCast)
    {
        GameObject _CollisionGameObject = _HitCast.collider.gameObject;

        if (null == _CollisionGameObject || null == m_pMouseSelectICon) // 빈 슬롯 누르면 에러 나오는데 ??
            return;

        if (m_pMouseSelectICon.AccessMySlot.gameObject == _CollisionGameObject) // 이 때 장착한다 !! 
        {
            m_pEquip.InsertItem(m_pMouseSelectICon.AccessMySlot);

            if (null != m_pMouseSelectICon)
            {
                Destroy(m_pMouseSelectICon.gameObject);

                m_pMouseSelectICon = null;
            }
        }
        else // 다르다면 스왑
        {
            for (int i = 0; i < m_pSlots.Length; ++i)
            {
                if (m_pSlots[i].gameObject == _CollisionGameObject) // 같은 슬롯이면 
                {
                    if (m_pSlots[i].AccessICon == null)
                    {
                        ICON _ICon = m_pMouseSelectICon.AccessMySlot.AccessICon;

                        m_pSlots[i].AccessICon = _ICon;

                        m_pMouseSelectICon.AccessMySlot.ClearPartSlot();

                        Destroy(m_pMouseSelectICon.gameObject);

                        m_pMouseSelectICon = null;

                        break;
                    }
                    else
                    {
                        ICON _FirstICon = m_pMouseSelectICon.AccessMySlot.AccessICon;

                        ICON _SecondICon = m_pSlots[i].AccessICon;

                        m_pSlots[i].ClearPartSlot();

                        m_pSlots[i].AccessICon = _FirstICon;

                        m_pMouseSelectICon.AccessMySlot.ClearPartSlot();

                        m_pMouseSelectICon.AccessMySlot.AccessICon = _SecondICon;

                        Destroy(m_pMouseSelectICon.gameObject);

                        m_pMouseSelectICon = null;

                        break;
                    }
                }
            }
        }
    }

    public void ClearInvertorySlot(Slot _ClearSlot)
    {
        if (null == _ClearSlot) // 왜 복사본이 들어와 ??
            return;

        for (int i = 0; i < m_pSlots.Length; ++i)
        {
            if (m_pSlots[i] == _ClearSlot)
            {
                m_pSlots[i].ClearPartSlot();

                break;
            }
        }
    }

    private void Doubleclickconfirm()
    {
        if (false == m_bIsClick)
            return;

        if (m_bIsDoubleclick == true)
        {
            m_fDoudleClickTimeAcc += Time.deltaTime;

            if (m_fDoudleClickTimeAcc >= 1.0f)
            {
                m_fDoudleClickTimeAcc = 0.0f;

                m_bIsDoubleclick = false;
            }
        }

        m_fClickTimeAcc += Time.deltaTime;
    }

    public ICON RemoveSlotICon(Slot _InventoryInsertSlot) // 이거 수정 
    {
        if (_InventoryInsertSlot == null)
            return null;

        ICON _ReturnICon = null;

        for (int i = 0; i < m_pSlots.Length; ++i)
        {
            if (m_pSlots[i] == _InventoryInsertSlot)
            {
                _ReturnICon = m_pSlots[i].AccessICon;

                m_pSlots[i].ClearPartSlot();

                break;
            }
        }

        return _ReturnICon;
    }

    public void InsertItem(GameObject _InstanceObject)
    {
        ICON _InstanceICon = _InstanceObject.GetComponent<ICON>();

        if(null == m_pSlots)
            m_pSlots = GetComponentsInChildren<Slot>();

        for (int i = 0; i < m_pSlots.Length; ++i)
        {
            if (m_pSlots[i].AccessICon == null)
            {
                _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY;

                _InstanceICon.AccessMySlot = m_pSlots[i];

                m_pSlots[i].AccessICon = _InstanceICon;

                return;
            }
        }
    }

    public override void EnableInventory()
    {
        gameObject.SetActive(true);
    }

    public override void DisabledInventory()
    {
        gameObject.SetActive(false);
    }

    public void OnDestroy()
    {
        m_pEquip = null;

        for (int i = 0; i < m_pSlots.Length; ++i)
            m_pSlots[i] = null;

        m_pMouseSelectICon = null;

        m_pRectTransform = null;
    }
}







//public class EquipInventory : Inventory
//{
//    [SerializeField] LayerMask _DivisionLayer = 0; // 레이 캐스트 선택

//    //private Slot[] m_pSlots = null;

//    private Equip m_pEquip = null;

//    private float m_fClickTimeAcc = 0.0f;

//    private float m_fDoudleClickTimeAcc = 0.0f;

//    private ICON m_pMouseSelectICon = null;

//    private bool m_bIsClick = false;

//    private bool m_bIsDoubleclick = false;

//    private bool m_bIsMoving = false;

//    private RectTransform m_pRectTransform = null;

//    //public Slot[] AccessSlots
//    //{
//    //    get { return m_pSlots; }
//    //}

//    private void Awake()
//    {
//        if (null == m_pSlots)
//            m_pSlots = GetComponentsInChildren<Slot>();

//        GameObject _Object = GameObject.Find("Inventory UI");

//        m_pEquip = _Object.transform.Find("Equip").gameObject.GetComponent<Equip>();

//        m_pRectTransform = transform.parent.parent.GetComponent<RectTransform>();
//    }


//    private void Update()
//    {
//        Vector2 _PickingPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

//        Ray2D _Ray = new Ray2D(_PickingPosition, Vector2.zero);

//        RaycastHit2D _HitCast = Physics2D.Raycast(_Ray.origin, _Ray.direction, 1.0f, _DivisionLayer);

//        if (_HitCast.collider != null && Input.GetMouseButton(0))
//        {
//            if (_HitCast.collider.CompareTag("Inventory") == true)
//                m_bIsMoving = true;
//        }

//        if (m_bIsMoving == true)
//        {
//            if (Input.GetMouseButtonUp(0))
//            {
//                m_bIsMoving = false;

//                return;
//            }

//            m_pRectTransform.position = _PickingPosition;

//            return;
//        }

//        if (_HitCast.collider != null && Input.GetMouseButtonDown(0))
//        {
//            if (m_bIsClick == false)
//            {
//                InstacneCopyICon(_HitCast);

//                m_pSoundManager.PlaySound("Start Drop Click Sound");

//                m_bIsClick = true;

//                return;
//            }

//            m_bIsClick = false;

//            MountingItem(_HitCast);

//            m_pSoundManager.PlaySound("End Drop Click Sound");
//        }
//    }

//    private void InstacneCopyICon(RaycastHit2D _HitCast)
//    {
//        GameObject _GameObject = _HitCast.collider.gameObject;

//        if (null == _GameObject)
//            return;

//        Vector2 _MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

//        Vector2 _MyPosition = _GameObject.transform.position; // 아이콘의 부모는 슬롯

//        Vector2 _Direction = _MyPosition - _MousePosition;

//        if (_Direction.magnitude <= 20.0f && null == m_pMouseSelectICon)
//        {
//            Slot _CollisionSolt = _GameObject.GetComponent<Slot>();

//            if (null == _CollisionSolt)
//                return;

//            ICON _CollisionICon = _CollisionSolt.AccessICon;

//            if (null == _CollisionICon)
//                return;

//            GameObject _InstanceCopyIconObject = GameObject.Instantiate(_CollisionICon.gameObject);

//            _InstanceCopyIconObject.layer = LayerMask.NameToLayer("Copy");

//            _InstanceCopyIconObject.transform.SetParent(transform.parent);

//            ICON _CopyICon = _InstanceCopyIconObject.GetComponent<ICON>();

//            if (null == _CopyICon)
//            {
//                Destroy(_InstanceCopyIconObject);

//                return;
//            }

//            _CopyICon.AccessMySlot = _CollisionSolt;

//            _CopyICon.AccessMovingICon = true;

//            _CopyICon.AccessOrlGameObejct = _CollisionICon.gameObject;

//            m_pMouseSelectICon = _CopyICon;
//        }
//    }

//    private void MountingItem(RaycastHit2D _HitCast)
//    {
//        GameObject _CollisionGameObject = _HitCast.collider.gameObject;

//        if (null == _CollisionGameObject || null == m_pMouseSelectICon) // 빈 슬롯 누르면 에러 나오는데 ??
//            return;

//        if (m_pMouseSelectICon.AccessMySlot.gameObject == _CollisionGameObject) // 이 때 장착한다 !! 
//        {
//            m_pEquip.InsertItem(m_pMouseSelectICon.AccessMySlot);

//            if (null != m_pMouseSelectICon)
//            {
//                Destroy(m_pMouseSelectICon.gameObject);

//                m_pMouseSelectICon = null;
//            }
//        }
//        else // 다르다면 스왑
//        {
//            for (int i = 0; i < m_pSlots.Length; ++i)
//            {
//                if (m_pSlots[i].gameObject == _CollisionGameObject) // 같은 슬롯이면 
//                {
//                    if (m_pSlots[i].AccessICon == null)
//                    {
//                        ICON _ICon = m_pMouseSelectICon.AccessMySlot.AccessICon;

//                        m_pSlots[i].AccessICon = _ICon;

//                        m_pMouseSelectICon.AccessMySlot.ClearPartSlot();

//                        Destroy(m_pMouseSelectICon.gameObject);

//                        m_pMouseSelectICon = null;

//                        break;
//                    }
//                    else
//                    {
//                        ICON _FirstICon = m_pMouseSelectICon.AccessMySlot.AccessICon;

//                        ICON _SecondICon = m_pSlots[i].AccessICon;

//                        m_pSlots[i].ClearPartSlot();

//                        m_pSlots[i].AccessICon = _FirstICon;

//                        m_pMouseSelectICon.AccessMySlot.ClearPartSlot();

//                        m_pMouseSelectICon.AccessMySlot.AccessICon = _SecondICon;

//                        Destroy(m_pMouseSelectICon.gameObject);

//                        m_pMouseSelectICon = null;

//                        break;
//                    }
//                }
//            }
//        }
//    }

//    public void ClearInvertorySlot(Slot _ClearSlot)
//    {
//        if (null == _ClearSlot) // 왜 복사본이 들어와 ??
//            return;

//        for (int i = 0; i < m_pSlots.Length; ++i)
//        {
//            if (m_pSlots[i] == _ClearSlot)
//            {
//                m_pSlots[i].ClearPartSlot();

//                break;
//            }
//        }
//    }

//    private void Doubleclickconfirm()
//    {
//        if (false == m_bIsClick)
//            return;

//        if (m_bIsDoubleclick == true)
//        {
//            m_fDoudleClickTimeAcc += Time.deltaTime;

//            if (m_fDoudleClickTimeAcc >= 1.0f)
//            {
//                m_fDoudleClickTimeAcc = 0.0f;

//                m_bIsDoubleclick = false;
//            }
//        }

//        m_fClickTimeAcc += Time.deltaTime;
//    }

//    public ICON RemoveSlotICon(Slot _InventoryInsertSlot) // 이거 수정 
//    {
//        if (_InventoryInsertSlot == null)
//            return null;

//        ICON _ReturnICon = null;

//        for (int i = 0; i < m_pSlots.Length; ++i)
//        {
//            if (m_pSlots[i] == _InventoryInsertSlot)
//            {
//                _ReturnICon = m_pSlots[i].AccessICon;

//                m_pSlots[i].ClearPartSlot();

//                break;
//            }
//        }

//        return _ReturnICon;
//    }

//    public void InsertItem(GameObject _InstanceObject)
//    {
//        ICON _InstanceICon = _InstanceObject.GetComponent<ICON>();

//        if (null == m_pSlots)
//            m_pSlots = GetComponentsInChildren<Slot>();

//        for (int i = 0; i < m_pSlots.Length; ++i)
//        {
//            if (m_pSlots[i].AccessICon == null)
//            {
//                _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY;

//                _InstanceICon.AccessMySlot = m_pSlots[i];

//                m_pSlots[i].AccessICon = _InstanceICon;

//                return;
//            }
//        }
//    }

//    public override void EnableInventory()
//    {
//        gameObject.SetActive(true);
//    }

//    public override void DisabledInventory()
//    {
//        gameObject.SetActive(false);
//    }

//    public void OnDestroy()
//    {
//        m_pEquip = null;

//        for (int i = 0; i < m_pSlots.Length; ++i)
//            m_pSlots[i] = null;

//        m_pMouseSelectICon = null;

//        m_pRectTransform = null;
//    }
//}