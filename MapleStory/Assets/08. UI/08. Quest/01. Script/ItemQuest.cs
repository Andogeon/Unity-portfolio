using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemQuest : QUEST
{
    [SerializeField] private ICON _QuestICon = null;

    private Inventory m_pInventory = null;

    private NPC m_pNPC = null;

    public Inventory AccessInventory
    {
        set { m_pInventory = value; }
    }

    public NPC AccessNPC
    {
        get { return m_pNPC; }

        set { m_pNPC = value; }
    }

    public ICON AccessQuestICon
    {
        get { return _QuestICon; }
    }

    public override void QuestUpdate()
    {
        if (null == m_pInventory) // 여기서 문제네 ????
            return;

        Slot[] _InventorySlot = m_pInventory.AccessSlots;

        if (null == _InventorySlot)
            return;

        for(int i = 0; i < _InventorySlot.Length; ++i)
        {
            if (_InventorySlot[i].AccessICon == null)
                continue;

            ICON _ICon = _InventorySlot[i].AccessICon;

            if(_ICon.gameObject.name == _QuestICon.gameObject.name + "(Clone)")
            {
                m_iCount = _ICon.AccessIconCount;

                if (m_iCount >= _Count)
                {
                    m_bIsComple = true;

                    break;
                }
            }
        }
    }

    public void RemoveQuestItem()
    {
        if (m_bIsComple == false)
            return;

        if (null == m_pInventory)
            return;

        Slot[] _InventorySlot = m_pInventory.AccessSlots;

        if (null == _InventorySlot)
            return;

        for (int i = 0; i < _InventorySlot.Length; ++i)
        {
            if (_InventorySlot[i].AccessICon == null)
                continue;

            ICON _ICon = _InventorySlot[i].AccessICon;

            if (_ICon.gameObject.name == _QuestICon.gameObject.name + "(Clone)")
            {
                _ICon.AccessIconCount -= _Count;

                if (_ICon.AccessIconCount <= 1)
                {
                    _ICon.AccessIconCount = 0;

                    Destroy(_InventorySlot[i].AccessICon.gameObject);

                    _InventorySlot[i].AccessICon = null;

                    break;
                }
            }
        }
    }
}











//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Quest : MonoBehaviour
//{
//    [SerializeField] private ITEM _QuestItem = null;

//    [SerializeField] private int _Count = 0;

//    [SerializeField] private string m_strQuestSlotName = string.Empty;

//    private Inventory m_pInventory = null;

//    private NPC m_pNPC = null;

//    private bool m_bIsComple = false;

//    private bool m_bIsStart = false;

//    private bool m_bIsFinal = false;

//    private int m_iCount = 0;

//    public string AccessQuestSlotName
//    {
//        get { return m_strQuestSlotName; }

//        set { m_strQuestSlotName = value; }
//    }

//    public bool AccessQuestStart
//    {
//        get { return m_bIsStart; }

//        set { m_bIsStart = value; }
//    }

//    public bool AccessQuestFinal
//    {
//        get { return m_bIsFinal; }

//        set { m_bIsFinal = value; }
//    }

//    public bool AccessQuestComple
//    {
//        get { return m_bIsComple; }

//        set { m_bIsComple = value; }
//    }

//    public Inventory AccessInventory
//    {
//        set { m_pInventory = value; }
//    }

//    public NPC AccessNPC
//    {
//        get { return m_pNPC; }

//        set { m_pNPC = value; }
//    }

//    public ITEM AccessQuestItem
//    {
//        get { return _QuestItem; }
//    }

//    public int AccessQuestItemCount
//    {
//        get { return m_iCount; }
//    }

//    public int AccessQuestCount
//    {
//        get { return _Count; }
//    }

//    public void QuestUpdate()
//    {
//        if (null == m_pInventory)
//            return;

//        Slot[] _InventorySlot = m_pInventory.AccessSlots;

//        if (null == _InventorySlot)
//            return;

//        for (int i = 0; i < _InventorySlot.Length; ++i)
//        {
//            if (_InventorySlot[i].AccessICon == null)
//                continue;

//            ITEM Item = _InventorySlot[i].AccessICon.AccessItem;

//            if (Item.gameObject.name == _QuestItem.gameObject.name + "(Clone)")
//            {
//                m_iCount = Item.AccessItemCount;

//                if (m_iCount >= _Count)
//                {
//                    m_bIsComple = true;

//                    break;
//                }
//            }
//        }
//    }

//    public void RemoveQuestItem()
//    {
//        if (m_bIsComple == false)
//            return;

//        if (null == m_pInventory)
//            return;

//        Slot[] _InventorySlot = m_pInventory.AccessSlots;

//        if (null == _InventorySlot)
//            return;

//        for (int i = 0; i < _InventorySlot.Length; ++i)
//        {
//            if (_InventorySlot[i].AccessICon == null)
//                continue;

//            ITEM Item = _InventorySlot[i].AccessICon.AccessItem;

//            if (Item.gameObject.name == _QuestItem.gameObject.name + "(Clone)")
//            {
//                Item.AccessItemCount -= _Count;

//                if (Item.AccessItemCount <= 1)
//                {
//                    Item.AccessItemCount = 0;

//                    Destroy(Item.gameObject);

//                    _InventorySlot[i].AccessICon.AccessItem = null;

//                    Destroy(_InventorySlot[i].AccessICon.gameObject);

//                    _InventorySlot[i].AccessICon = null;

//                    break;
//                }
//            }
//        }
//    }
//}