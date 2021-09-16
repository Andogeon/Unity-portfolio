using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 실제 플레이어의 장비창 오브젝트의 대한 클래스입니다.

// 장비창에 해당 아이템에 장착시 실제 아이템이 플레이어에게 적용되게 구현했습니다

public class Equip : MonoBehaviour // 장비창 클래스입니다.
{
    [SerializeField] LayerMask _DivisionLayer = 0; // 레이 캐스트 선택

    private EquipInventory m_pEquipInventory = null;

    private Player m_pPlayer = null;

    private LayerMask m_RayCastLayer;

    private Slot[] m_pSlot = null;

    private bool m_bIsClick = false;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();
    public Slot[] AccessSlot
    {
        get { return m_pSlot; }

        set { m_pSlot = value; }
    }

    public Player AccessPlayer
    {
        get { return m_pPlayer; }

        set { m_pPlayer = value; }
    }

    private void Awake()
    {
        m_pSlot = GetComponentsInChildren<Slot>();

        GameObject _Object = transform.parent.Find("Inventory Box").gameObject;

        _Object = _Object.transform.Find("Inventory").gameObject;

        m_pEquipInventory = _Object.transform.Find("Equip Window").GetComponent<EquipInventory>(); // 인벤토리의 장비 슬롯

        m_RayCastLayer = _DivisionLayer;
    }

    // 장비창 해당 슬롯을 마우스로 더블클릭시 아이템을 인벤토리로 이동할 수 있게 구현한 함수입니다.

    private void Update()
    {
        Vector2 _PickingPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Ray2D _Ray = new Ray2D(_PickingPosition, Vector2.zero);

        RaycastHit2D _HitCast = Physics2D.Raycast(_Ray.origin, _Ray.direction, 1.0f, m_RayCastLayer);

        if (_HitCast.collider != null && Input.GetMouseButtonDown(0))
        {
            if (m_bIsClick == false) 
            {
                m_bIsClick = true; // 한 번 클릭시 

                m_pSoundManager.PlaySound("Start Drop Click Sound");
            }
            else // 더블클릭시 
            {
                MoveInventory(_HitCast.collider.gameObject); // 해당 아이콘 장비를 인벤토리로 보낸다

                m_pSoundManager.PlaySound("End Drop Click Sound");

                m_bIsClick = false; // 더블클릭 해재 
            }
        }
    }

    // 인벤토리에서 장비로 아이템 이동, 장비에서 아이템으로 이동하고 아이템의 장착, 해재를 하는 함수입니다.
    public void InsertItem(Slot _InventoryInsertSlot)
    {
        if (null == _InventoryInsertSlot) // 슬롯 여부 확인 
            return;

        for (int i = 0; i < m_pSlot.Length; ++i)
        {
            if (m_pSlot[i].AccessICon == null) // 장비창에 슬롯에 아이콘이 존재 하지 않을 경우 
            {
                if (m_pSlot[i].AccessSlotType == _InventoryInsertSlot.AccessICon.AccessIConSlotType) // 슬롯의 타입과 같은 타입으로 
                {
                    _InventoryInsertSlot.AccessICon.AccessIConState = ICONSTATE.ICON_EQUIP; // 해당 장비를 장비창으로 이동

                    m_pSlot[i].AccessICon = _InventoryInsertSlot.AccessICon; // 아이콘을 전달 및 실제 아이템을 장착 

                    m_pEquipInventory.ClearInvertorySlot(_InventoryInsertSlot); // 장비창에 있는 인벤토리의 슬롯 정보를 비움

                    return;
                }
            }
            else // 장착한 장비와 인벤토리의 장비 교환시 
            {
                if (m_pSlot[i].AccessSlotType == _InventoryInsertSlot.AccessICon.AccessIConSlotType) // 인자로 들어온 아이템의 타입이 
                    // 슬롯에 타입과 같다면 
                {
                    ICON _Inventory_RemoveIcon = m_pEquipInventory.RemoveSlotICon(_InventoryInsertSlot); // 인벤토리의 아이콘을 가지고 온다

                    ICON _Equip_RemoveIcon = RemoveSlotICon(m_pSlot[i]); // 슬롯에 잇는 아이콘을 가지고 온다

                    m_pEquipInventory.InsertItem(_Equip_RemoveIcon.gameObject);  // 장비 인벤토리에 장비창에서 꺼내온 아이콘을 넣는다.

                    _Inventory_RemoveIcon.AccessIConState = ICONSTATE.ICON_EQUIP; // 인벤토리에서 꺼내온 아이템을 장비창상태로 정하고 

                    m_pSlot[i].AccessICon = _Inventory_RemoveIcon; // 장비창 슬롯으로 이동

                    return;
                }
            }
        }
    }

    public void MoveInventory(GameObject _MoveObject)
    {
        if (null == _MoveObject)
            return;

        m_bIsClick = false; // 더블클릭을 해재한다.

        Slot _RemoveSlot = _MoveObject.GetComponent<Slot>(); // 인자로 들어온 게임 오브젝트 즉 레이 캐스트로 찍은 게임 오브젝트의 

        // 슬롯 컴포넌트 클래스를 찾는다.

        if (null == _RemoveSlot) // 없다면 
            return; // 종료 

        ICON _RemoveICon = _RemoveSlot.AccessICon; // 슬롯에 존재하는 아이콘을 찾는다.

        if (null == _RemoveICon) // 아이콘이 존재 하지 않는다면 
            return; // 종료 

        _RemoveSlot.ClearPartSlot(); // 파츠에 연결된 아이템을 삭제한다

        m_pEquipInventory.InsertItem(_RemoveICon.gameObject); // 해당 아이콘을 장비 인벤토리로 이동한다.
    } // 더블클릭시 레이캐스트로 슬롯감지하고 감지된 아이콘을 인벤토리로 이동하고 파츠에 연결된 아이템을 삭제하는 함수입니다.


    public ICON RemoveSlotICon(Slot _Slot) // 인벤토리의 슬롯이나 장비 슬롯을 비우는 함수입니다.
    {
        if (null == _Slot)
            return null;

        ICON _ReturnICon = _Slot.AccessICon;

        _Slot.ClearPartSlot();

        return _ReturnICon;
    }
}






//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Equip : MonoBehaviour
//{
//    [SerializeField] LayerMask _DivisionLayer = 0; // 레이 캐스트 선택

//    private EquipInventory m_pEquipInventory = null;

//    //private Inventory m_pEquipInventory = null;

//    private Player m_pPlayer = null;

//    private LayerMask m_RayCastLayer;

//    private Slot[] m_pSlot = null;

//    private bool m_bIsClick = false;

//    private SoundManager m_pSoundManager = SoundManager.GetInstance();

//    //private bool m_bIsDoubleclick = false;
//    public Slot[] AccessSlot
//    {
//        get { return m_pSlot; }

//        set { m_pSlot = value; }
//    }

//    public Player AccessPlayer
//    {
//        get { return m_pPlayer; }

//        set { m_pPlayer = value; }
//    }

//    private void Awake()
//    {
//        m_pSlot = GetComponentsInChildren<Slot>();

//        GameObject _Object = transform.parent.Find("Inventory Box").gameObject;

//        _Object = _Object.transform.Find("Inventory").gameObject;

//        m_pEquipInventory = _Object.transform.Find("Equip Window").GetComponent<EquipInventory>();

//        m_RayCastLayer = _DivisionLayer;
//    }

//    private void Update()
//    {
//        Vector2 _PickingPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

//        Ray2D _Ray = new Ray2D(_PickingPosition, Vector2.zero);

//        RaycastHit2D _HitCast = Physics2D.Raycast(_Ray.origin, _Ray.direction, 1.0f, m_RayCastLayer);

//        if (_HitCast.collider != null && Input.GetMouseButtonDown(0))
//        {
//            if (m_bIsClick == false)
//            {
//                m_bIsClick = true;

//                m_pSoundManager.PlaySound("Start Drop Click Sound");
//            }
//            else // 인벤으로 보낸다 !! 
//            {
//                MoveInventory(_HitCast.collider.gameObject);

//                m_pSoundManager.PlaySound("End Drop Click Sound");

//                m_bIsClick = false;
//            }
//        }
//    }

//    public void InsertItem(Slot _InventoryInsertSlot)
//    {
//        if (null == _InventoryInsertSlot) // 슬롯 여부 확인 
//            return;

//        for (int i = 0; i < m_pSlot.Length; ++i)
//        {
//            if (m_pSlot[i].AccessICon == null)
//            {
//                if (m_pSlot[i].AccessSlotType == _InventoryInsertSlot.AccessICon.AccessIConSlotType)
//                {
//                    //GameObject _GameObject = GameObject.Instantiate(_InventoryInsertSlot.AccessICon.OriginalItem.gameObject);

//                    _InventoryInsertSlot.AccessICon.AccessIConState = ICONSTATE.ICON_EQUIP;

//                    m_pSlot[i].AccessICon = _InventoryInsertSlot.AccessICon;

//                    m_pEquipInventory.ClearInvertorySlot(_InventoryInsertSlot);

//                    return;
//                }
//            }
//            else
//            {
//                if (m_pSlot[i].AccessSlotType == _InventoryInsertSlot.AccessICon.AccessIConSlotType)
//                {
//                    ICON _Inventory_RemoveIcon = m_pEquipInventory.RemoveSlotICon(_InventoryInsertSlot);

//                    ICON _Equip_RemoveIcon = RemoveSlotICon(m_pSlot[i]);

//                    m_pEquipInventory.InsertItem(_Equip_RemoveIcon.gameObject);

//                    _Inventory_RemoveIcon.AccessIConState = ICONSTATE.ICON_EQUIP;

//                    m_pSlot[i].AccessICon = _Inventory_RemoveIcon;

//                    return;
//                }
//            }
//        }
//    }

//    public void MoveInventory(GameObject _MoveObject)
//    {
//        if (null == _MoveObject)
//            return;

//        m_bIsClick = false;

//        Slot _RemoveSlot = _MoveObject.GetComponent<Slot>();

//        if (null == _RemoveSlot)
//            return;

//        ICON _RemoveICon = _RemoveSlot.AccessICon;

//        if (null == _RemoveICon)
//            return;

//        _RemoveSlot.ClearPartSlot(); // 기존 객체를 소유하고 날린다 !! 

//        m_pEquipInventory.InsertItem(_RemoveICon.gameObject);
//    }

//    public ICON RemoveSlotICon(Slot _Slot)
//    {
//        if (null == _Slot)
//            return null;

//        ICON _ReturnICon = _Slot.AccessICon;

//        _Slot.ClearPartSlot();

//        return _ReturnICon;
//    }
//}



////public void InsertItem(GameObject _InsertItemObject)
////{
////    if (null == _InsertItemObject)
////        return;

////    for (int i = 0; i < m_pSlot.Length; ++i)
////    {
////        if (m_pSlot[i].AccessICon == null)
////        {
////            if (m_pSlot[i].AccessSlotType == _InsertICon.AccessIConSlotType)
////            {
////                _InsertICon.AccessIConState = ICONSTATE.ICON_EQUIP;

////                m_pInventory.ClearInvertorySlot(_InsertICon);

////                m_pSlot[i].AccessICon = _InsertICon;

////                return;
////            }
////        }
////    }
////}

////public void ChangeItem(ICON _InsertItem)
////{
////    if (null == _InsertItem)
////        return;

////    for (int i = 0; i < m_pSlot.Length; ++i)
////    {
////        if (m_pSlot[i].AccessICon == null && _InsertItem.AccessIConSlotType == m_pSlot[i].AccessSlotType)
////        {
////            _InsertItem.AccessIConState = ICONSTATE.ICON_EQUIP;

////            m_pSlot[i].AccessICon = _InsertItem;

////            return;
////        }
////    }
////}

////public void InsertItem(ICON _InsertItem) // 여기서 잘못됨!! 
////{
////    if (null == _InsertItem)
////        return;

////    for(int i = 0; i < m_pSlot.Length; ++i)
////    {
////        if (m_pSlot[i].AccessICon == null && _InsertItem.AccessIConSlotType == m_pSlot[i].AccessSlotType)
////        {
////            _InsertItem.AccessIConState = ICONSTATE.ICON_EQUIP;

////            m_pSlot[i].AccessICon = _InsertItem;

////            return;
////        }
////    }
////}

















////public void ResetICon(ICON _ICON)
////{
////    Slot[] _Slots = m_pInventory.AccessSlots;

////    for (int i = 0; i < _Slots.Length; ++i)
////    {
////        if (_Slots[i].AccessICon == _ICON)
////        {
////            //_Slots[i].ResetPartSlot();

////            break;
////        }
////    }
////}

////public ICON OutResetICon(ICON _ICON)
////{
////    Slot[] _Slots = m_pInventory.AccessSlots;

////    for (int i = 0; i < _Slots.Length; ++i)
////    {
////        if (_Slots[i].AccessICon == _ICON)
////            return _Slots[i].ReturnICon();
////    }

////    return null;
////}

////// 장비창에 있는 아이템을 꺼내오고 해당 장비를 비운다.

////public ICON MovingInventoryIcon(GameObject _CollisionObject)
////{
////    if (null == _CollisionObject)
////        return null;

////    ICON _CollisionICon = _CollisionObject.GetComponent<ICON>();

////    if (null == _CollisionICon)
////        return null;

////    ICON _OutIcon = null;

////    for (int i = 0; i < m_pSlot.Length; ++i)
////    {
////        if (m_pSlot[i].AccessICon == _CollisionICon)
////        {
////            _OutIcon = m_pSlot[i].AccessICon;

////            m_pSlot[i].AccessICon = null; // 혹시 문제 생기면 여기서 한번 볼 것

////            break;
////        }
////    }

////    return _OutIcon;
////}

////public ICON MovingInventoryIcon(ICON _CollisionICon)
////{
////    if (null == _CollisionICon)
////        return null;

////    ICON _OutIcon = null;

////    for (int i = 0; i < m_pSlot.Length; ++i)
////    {
////        if (m_pSlot[i].AccessICon == _CollisionICon)
////        {
////            _OutIcon = m_pSlot[i].AccessICon;

////            //m_pSlot[i].ResetPartSlot();

////            m_pSlot[i].AccessICon = null; // 혹시 문제 생기면 여기서 한번 볼 것

////            _OutIcon.AccessIConState = ICONSTATE.ICON_INVENTORY;

////            break;
////        }
////    }

////    return _OutIcon;
////}
