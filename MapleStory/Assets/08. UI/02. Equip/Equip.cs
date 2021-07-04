using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equip : MonoBehaviour
{
    [SerializeField] LayerMask _DivisionLayer = 0; // 레이 캐스트 선택

    private EquipInventory m_pEquipInventory = null;

    //private Inventory m_pEquipInventory = null;

    private Player m_pPlayer = null;

    private LayerMask m_RayCastLayer;

    private Slot[] m_pSlot = null;

    private bool m_bIsClick = false;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    //private bool m_bIsDoubleclick = false;
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

        m_pEquipInventory = _Object.transform.Find("Equip Window").GetComponent<EquipInventory>();

        m_RayCastLayer = _DivisionLayer;
    }

    private void Update()
    {
        Vector2 _PickingPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        Ray2D _Ray = new Ray2D(_PickingPosition, Vector2.zero);

        RaycastHit2D _HitCast = Physics2D.Raycast(_Ray.origin, _Ray.direction, 1.0f, m_RayCastLayer);

        if (_HitCast.collider != null && Input.GetMouseButtonDown(0))
        {
            if (m_bIsClick == false)
            {
                m_bIsClick = true;

                m_pSoundManager.PlaySound("Start Drop Click Sound");
            }
            else // 인벤으로 보낸다 !! 
            {
                MoveInventory(_HitCast.collider.gameObject);

                m_pSoundManager.PlaySound("End Drop Click Sound");

                m_bIsClick = false;
            }
        }
    }

    public void InsertItem(Slot _InventoryInsertSlot)
    {
        if (null == _InventoryInsertSlot) // 슬롯 여부 확인 
            return;

        for (int i = 0; i < m_pSlot.Length; ++i)
        {
            if (m_pSlot[i].AccessICon == null)
            {
                if (m_pSlot[i].AccessSlotType == _InventoryInsertSlot.AccessICon.AccessIConSlotType)
                {
                    //GameObject _GameObject = GameObject.Instantiate(_InventoryInsertSlot.AccessICon.OriginalItem.gameObject);

                    _InventoryInsertSlot.AccessICon.AccessIConState = ICONSTATE.ICON_EQUIP;

                    m_pSlot[i].AccessICon = _InventoryInsertSlot.AccessICon;

                    m_pEquipInventory.ClearInvertorySlot(_InventoryInsertSlot);

                    return;
                }
            }
            else
            {
                if (m_pSlot[i].AccessSlotType == _InventoryInsertSlot.AccessICon.AccessIConSlotType)
                {
                    ICON _Inventory_RemoveIcon = m_pEquipInventory.RemoveSlotICon(_InventoryInsertSlot);

                    ICON _Equip_RemoveIcon = RemoveSlotICon(m_pSlot[i]);

                    m_pEquipInventory.InsertItem(_Equip_RemoveIcon.gameObject);

                    _Inventory_RemoveIcon.AccessIConState = ICONSTATE.ICON_EQUIP;

                    m_pSlot[i].AccessICon = _Inventory_RemoveIcon;

                    return;
                }
            }
        }
    }

    public void MoveInventory(GameObject _MoveObject)
    {
        if (null == _MoveObject)
            return;

        m_bIsClick = false;

        Slot _RemoveSlot = _MoveObject.GetComponent<Slot>();

        if (null == _RemoveSlot)
            return;

        ICON _RemoveICon = _RemoveSlot.AccessICon;

        if (null == _RemoveICon)
            return;

        _RemoveSlot.ClearPartSlot(); // 기존 객체를 소유하고 날린다 !! 

        m_pEquipInventory.InsertItem(_RemoveICon.gameObject);
    }

    public ICON RemoveSlotICon(Slot _Slot)
    {
        if (null == _Slot)
            return null;

        ICON _ReturnICon = _Slot.AccessICon;

        _Slot.ClearPartSlot();

        return _ReturnICon;
    }
}



//public void InsertItem(GameObject _InsertItemObject)
//{
//    if (null == _InsertItemObject)
//        return;

//    for (int i = 0; i < m_pSlot.Length; ++i)
//    {
//        if (m_pSlot[i].AccessICon == null)
//        {
//            if (m_pSlot[i].AccessSlotType == _InsertICon.AccessIConSlotType)
//            {
//                _InsertICon.AccessIConState = ICONSTATE.ICON_EQUIP;

//                m_pInventory.ClearInvertorySlot(_InsertICon);

//                m_pSlot[i].AccessICon = _InsertICon;

//                return;
//            }
//        }
//    }
//}

//public void ChangeItem(ICON _InsertItem)
//{
//    if (null == _InsertItem)
//        return;

//    for (int i = 0; i < m_pSlot.Length; ++i)
//    {
//        if (m_pSlot[i].AccessICon == null && _InsertItem.AccessIConSlotType == m_pSlot[i].AccessSlotType)
//        {
//            _InsertItem.AccessIConState = ICONSTATE.ICON_EQUIP;

//            m_pSlot[i].AccessICon = _InsertItem;

//            return;
//        }
//    }
//}

//public void InsertItem(ICON _InsertItem) // 여기서 잘못됨!! 
//{
//    if (null == _InsertItem)
//        return;

//    for(int i = 0; i < m_pSlot.Length; ++i)
//    {
//        if (m_pSlot[i].AccessICon == null && _InsertItem.AccessIConSlotType == m_pSlot[i].AccessSlotType)
//        {
//            _InsertItem.AccessIConState = ICONSTATE.ICON_EQUIP;

//            m_pSlot[i].AccessICon = _InsertItem;

//            return;
//        }
//    }
//}

















//public void ResetICon(ICON _ICON)
//{
//    Slot[] _Slots = m_pInventory.AccessSlots;

//    for (int i = 0; i < _Slots.Length; ++i)
//    {
//        if (_Slots[i].AccessICon == _ICON)
//        {
//            //_Slots[i].ResetPartSlot();

//            break;
//        }
//    }
//}

//public ICON OutResetICon(ICON _ICON)
//{
//    Slot[] _Slots = m_pInventory.AccessSlots;

//    for (int i = 0; i < _Slots.Length; ++i)
//    {
//        if (_Slots[i].AccessICon == _ICON)
//            return _Slots[i].ReturnICon();
//    }

//    return null;
//}

//// 장비창에 있는 아이템을 꺼내오고 해당 장비를 비운다.

//public ICON MovingInventoryIcon(GameObject _CollisionObject)
//{
//    if (null == _CollisionObject)
//        return null;

//    ICON _CollisionICon = _CollisionObject.GetComponent<ICON>();

//    if (null == _CollisionICon)
//        return null;

//    ICON _OutIcon = null;

//    for (int i = 0; i < m_pSlot.Length; ++i)
//    {
//        if (m_pSlot[i].AccessICon == _CollisionICon)
//        {
//            _OutIcon = m_pSlot[i].AccessICon;

//            m_pSlot[i].AccessICon = null; // 혹시 문제 생기면 여기서 한번 볼 것

//            break;
//        }
//    }

//    return _OutIcon;
//}

//public ICON MovingInventoryIcon(ICON _CollisionICon)
//{
//    if (null == _CollisionICon)
//        return null;

//    ICON _OutIcon = null;

//    for (int i = 0; i < m_pSlot.Length; ++i)
//    {
//        if (m_pSlot[i].AccessICon == _CollisionICon)
//        {
//            _OutIcon = m_pSlot[i].AccessICon;

//            //m_pSlot[i].ResetPartSlot();

//            m_pSlot[i].AccessICon = null; // 혹시 문제 생기면 여기서 한번 볼 것

//            _OutIcon.AccessIConState = ICONSTATE.ICON_INVENTORY;

//            break;
//        }
//    }

//    return _OutIcon;
//}
