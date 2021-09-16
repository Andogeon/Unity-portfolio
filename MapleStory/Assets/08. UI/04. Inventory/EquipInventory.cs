using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Consumption_Inventory 클래스와 동일하게 아이콘의 복사본 생성, 인벤토리창의 이동 함수는 동일하며 

// 장비창에 있는 아이콘을 더블클릭 할시 장비창으로 아이콘이 이동 

// 장비창에서 받아온 아이콘으로 아이템을 적용하게 구현한 클래스입니다.

public class EquipInventory : Inventory // 장비 인벤토리 클래스입니다.
{
    public void InsertItem(GameObject _InstanceObject)
    {
        ICON _InstanceICon = _InstanceObject.GetComponent<ICON>(); // 인자로 들어오는 게임오브젝트에서 아이콘 컴포넌트가 있는지 확인 

        if (null == m_pSlots) // 해당 슬롯 컴포넌트가 존재하지 않는다면 
            m_pSlots = GetComponentsInChildren<Slot>(); // 찾는다 

        for (int i = 0; i < m_pSlots.Length; ++i) // 슬롯의 개수만큼 순회 
        {
            if (m_pSlots[i].AccessICon == null) // 해당 슬롯에 아이콘이 존재하지 않는다면
            {
                _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY; // 아이콘의 상태를 인벤토리로 설정

                _InstanceICon.AccessMySlot = m_pSlots[i]; // 해당 아이콘이 소유한 슬롯을 해당 슬롯으로 연결

                m_pSlots[i].AccessICon = _InstanceICon; // 인자로 들어오는 아이콘을 해당 슬롯아이콘에 연결

                return; // 종료 
            }
        }
    }


    [SerializeField] LayerMask _DivisionLayer = 0; // 레이 캐스트 선택

    private Equip m_pEquip = null; // 장비창에 대한 참조변수

    private ICON m_pMouseSelectICon = null;

    private bool m_bIsClick = false;

    private bool m_bIsMoving = false;

    private RectTransform m_pRectTransform = null;

    private void Awake()
    {
        if(null == m_pSlots)
            m_pSlots = GetComponentsInChildren<Slot>();

        GameObject _Object = GameObject.Find("Inventory UI");

        m_pEquip = _Object.transform.Find("Equip").gameObject.GetComponent<Equip>();

        m_pRectTransform = transform.parent.parent.GetComponent<RectTransform>();
    }


    // Update 함수는 Consumption_Inventory 클래스와 비슷한 구조입니다.

    // 다른 점은 장비창에 참조 변수를 통해서 복사본 오브젝트의 아이콘을 장착하게 구현했습니다.

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

            MountingItem(_HitCast); // 더블클릭시 인자로 충돌된 레이케스트를 전달

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

    // 인자로 들어온 레이캐스트를 받아와 장비창의 참조변수를 통해 장착하거나 아이템 아이콘의 위치를 변경하게 구현한 함수입니다.

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
                if (m_pSlots[i].gameObject == _CollisionGameObject)
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
                    else // 두 개에 아이템 아이콘의 위치변경 
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

    public void ClearInvertorySlot(Slot _ClearSlot) // 인자로 넘어오는 슬롯을 비움 
    {
        if (null == _ClearSlot)
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

    // 인자로 들어오는 슬롯과 같은 슬롯을 탐색하여 같은 슬롯이라면 슬롯을 비우는 함수입니다.

    public ICON RemoveSlotICon(Slot _InventoryInsertSlot) 
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

    // 인자로 넘어오는 게임오브젝트로 아이콘을 찾아 인벤토리로 이동하는 함수입니다.

    //public void InsertItem(GameObject _InstanceObject)
    //{
    //    ICON _InstanceICon = _InstanceObject.GetComponent<ICON>(); // 인자로 들어오는 게임오브젝트에서 아이콘 컴포넌트가 있는지 확인 

    //    if(null == m_pSlots) // 해당 슬롯 컴포넌트가 존재하지 않는다면 
    //        m_pSlots = GetComponentsInChildren<Slot>(); // 찾는다 

    //    for (int i = 0; i < m_pSlots.Length; ++i) // 슬롯의 개수만큼 순회 
    //    {
    //        if (m_pSlots[i].AccessICon == null) // 해당 슬롯에 아이콘이 존재하지 않는다면
    //        {
    //            _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY; // 아이콘의 상태를 인벤토리로 설정

    //            _InstanceICon.AccessMySlot = m_pSlots[i]; // 해당 아이콘이 소유한 슬롯을 해당 슬롯으로 연결

    //            m_pSlots[i].AccessICon = _InstanceICon; // 인자로 들어오는 아이콘을 해당 슬롯아이콘에 연결

    //            return; // 종료 
    //        }
    //    }
    //}

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