using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Other_Inventory : Inventory
{
    [SerializeField] LayerMask _DivisionLayer = 0;

    private bool m_bIsMoving = false;

    private bool m_bIsClick = false;

    private RectTransform m_pRectTransform = null;

    private ICON m_pMouseSelectICon = null;

    private void Awake()
    {
        m_pSlots = GetComponentsInChildren<Slot>();

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
        }

        if (_HitCast.collider != null && Input.GetMouseButtonDown(0))
        {
            if (m_bIsClick == false)
            {
                InstacneCopyICon(_HitCast);

                m_bIsClick = true;

                return;
            }

            m_bIsClick = false;

            SwapItem(_HitCast);
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

            m_pMouseSelectICon = _CopyICon;
        }
    }

    public void InsertItem(GameObject _InstanceObject)
    {
        ICON _InstanceICon = _InstanceObject.GetComponent<ICON>();

        if(m_pSlots == null)
            m_pSlots = GetComponentsInChildren<Slot>();

        // 복사본 아이콘이 필요하네 ?? ㅡㅡ 

        for (int i = 0; i < m_pSlots.Length; ++i)
        {
            if (m_pSlots[i].AccessICon != null)
            {
                if (m_pSlots[i].AccessICon.name == _InstanceICon.name)
                {
                    m_pSlots[i].AccessICon.AccessIconCount += 1;

                    Destroy(_InstanceObject);

                    break;
                }
            }
            else if (m_pSlots[i].AccessICon == null)
            {
                _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY;

                _InstanceICon.AccessMySlot = m_pSlots[i];

                m_pSlots[i].AccessICon = _InstanceICon; // 이걸 날려야 한다 !! 

                return;
            }
        }
    }

    public void SwapItem(RaycastHit2D _HitCast)
    {
        if (_HitCast.collider == null || m_pMouseSelectICon == null)
            return;

        GameObject _CollisionObject = _HitCast.collider.gameObject;

        for (int i = 0; i < m_pSlots.Length; ++i)
        {
            if (m_pSlots[i].gameObject == _CollisionObject) // 같은 슬롯이면 
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

    public override void EnableInventory()
    {
        gameObject.SetActive(true);
    }

    public override void DisabledInventory()
    {
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < m_pSlots.Length; ++i)
            m_pSlots[i] = null;

        m_pRectTransform = null;

        m_pMouseSelectICon = null;
    }
}









//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Other_Inventory : Inventory
//{
//    [SerializeField] LayerMask _DivisionLayer = 0;

//    //private Slot[] m_pSlots = null;

//    private bool m_bIsMoving = false;

//    private bool m_bIsClick = false;

//    private RectTransform m_pRectTransform = null;

//    private ICON m_pMouseSelectICon = null;

//    private void Awake()
//    {
//        m_pSlots = GetComponentsInChildren<Slot>();

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
//        }

//        if (_HitCast.collider != null && Input.GetMouseButtonDown(0))
//        {
//            if (m_bIsClick == false)
//            {
//                InstacneCopyICon(_HitCast);

//                m_bIsClick = true;

//                return;
//            }

//            m_bIsClick = false;

//            SwapItem(_HitCast);
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

//            _CopyICon.AccessItem = _CollisionICon.AccessItem;

//            m_pMouseSelectICon = _CopyICon;
//        }
//    }

//    public void InsertItem(GameObject _InstanceObject)
//    {
//        ICON _InstanceICon = _InstanceObject.GetComponent<ICON>();

//        if (m_pSlots == null)
//            m_pSlots = GetComponentsInChildren<Slot>();

//        // 복사본 아이콘이 필요하네 ?? ㅡㅡ 

//        for (int i = 0; i < m_pSlots.Length; ++i)
//        {
//            if (m_pSlots[i].AccessICon != null)
//            {
//                if (m_pSlots[i].AccessICon.name == _InstanceICon.name)
//                {
//                    ITEM _Item = m_pSlots[i].AccessICon.AccessItem;

//                    bool _IsCreate = _Item.AddCount();

//                    if (_IsCreate == false)
//                    {
//                        Destroy(_InstanceICon.AccessItem.gameObject);

//                        Destroy(_InstanceICon.gameObject);

//                        Destroy(_InstanceObject);
//                    }
//                    else
//                    {
//                        Destroy(_InstanceICon.AccessItem.gameObject);

//                        Destroy(_InstanceICon.gameObject);

//                        Destroy(_InstanceObject);
//                    }

//                    break;
//                }
//            }
//            else if (m_pSlots[i].AccessICon == null)
//            {
//                _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY;

//                _InstanceICon.AccessMySlot = m_pSlots[i];

//                DontDestroyOnLoad(_InstanceICon.AccessItem);

//                m_pSlots[i].AccessICon = _InstanceICon; // 이걸 날려야 한다 !! 

//                return;
//            }
//        }
//    }

//    public void SwapItem(RaycastHit2D _HitCast)
//    {
//        if (_HitCast.collider == null || m_pMouseSelectICon == null)
//            return;

//        GameObject _CollisionObject = _HitCast.collider.gameObject;

//        for (int i = 0; i < m_pSlots.Length; ++i)
//        {
//            if (m_pSlots[i].gameObject == _CollisionObject) // 같은 슬롯이면 
//            {
//                if (m_pSlots[i].AccessICon == null)
//                {
//                    ICON _ICon = m_pMouseSelectICon.AccessMySlot.AccessICon;

//                    m_pSlots[i].AccessICon = _ICon;

//                    m_pMouseSelectICon.AccessMySlot.ClearPartSlot();

//                    Destroy(m_pMouseSelectICon.gameObject);

//                    m_pMouseSelectICon = null;

//                    break;
//                }
//                else
//                {
//                    ICON _FirstICon = m_pMouseSelectICon.AccessMySlot.AccessICon;

//                    ICON _SecondICon = m_pSlots[i].AccessICon;

//                    m_pSlots[i].ClearPartSlot();

//                    m_pSlots[i].AccessICon = _FirstICon;

//                    m_pMouseSelectICon.AccessMySlot.ClearPartSlot();

//                    m_pMouseSelectICon.AccessMySlot.AccessICon = _SecondICon;

//                    Destroy(m_pMouseSelectICon.gameObject);

//                    m_pMouseSelectICon = null;

//                    break;
//                }
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

//    private void OnDestroy()
//    {
//        for (int i = 0; i < m_pSlots.Length; ++i)
//            m_pSlots[i] = null;

//        m_pRectTransform = null;

//        m_pMouseSelectICon = null;
//    }
//}