using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임에서 퀘스트를 구현해보가 만든 클래스입니다.

// 퀘스트로 수행할 아이콘을 인스팩터로 받아 인벤토리에서 탐색하여 퀘스트 완료를 판별하는 맴버 함수로 구성 되었습니다.
public class ItemQuest : QUEST // 아이템 퀘스트 클래스입니다.
{
    [SerializeField] private ICON _QuestICon = null; // 퀘스트 수행시 필요한 아이콘입니다.

    private Inventory m_pInventory = null; // 탐색할 인벤토리

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

    public override void QuestUpdate() // 퀘스트 상시로 갱신하여 퀘스트 완료를 결정 짓는 함수입니다.
    {
        if (null == m_pInventory) 
            return;

        Slot[] _InventorySlot = m_pInventory.AccessSlots; // 인벤토리의 슬롯을 찾는다.

        if (null == _InventorySlot) // 존재하지 않는다면 
            return; // 종료 

        for(int i = 0; i < _InventorySlot.Length; ++i) // 인벤토리 슬롯 하나씩 순회하여 검색
        {
            if (_InventorySlot[i].AccessICon == null) // 슬롯에 아이콘이 존재하지 않는다면 
                continue; // 다음으로 넘어간다.

            ICON _ICon = _InventorySlot[i].AccessICon; // 존재 할 시 아이콘을 검색 

            if(_ICon.gameObject.name == _QuestICon.gameObject.name + "(Clone)") // 맞는 아이콘의 복사본이 존재한다면 
            {
                m_iCount = _ICon.AccessIconCount; // 개수를 확인 

                if (m_iCount >= _Count) // 퀘스트에 필요한 갯수가 같거나 많다면
                {
                    m_bIsComple = true; // 완료

                    break; // 종료 
                }
            }
        }
    }

    public void RemoveQuestItem() // 인벤토리에서 아이템을 빼는 함수입니다.
    {
        if (m_bIsComple == false)
            return;

        if (null == m_pInventory)
            return;

        Slot[] _InventorySlot = m_pInventory.AccessSlots;

        if (null == _InventorySlot)
            return;

        for (int i = 0; i < _InventorySlot.Length; ++i) // 검색까지는 QuestUpdate 함수와 동일.
        {
            if (_InventorySlot[i].AccessICon == null)
                continue;

            ICON _ICon = _InventorySlot[i].AccessICon;

            if (_ICon.gameObject.name == _QuestICon.gameObject.name + "(Clone)")
            {
                _ICon.AccessIconCount -= _Count; // 현재 아이콘의 카운터 개수를 필요한 개수만큼 빼고

                if (_ICon.AccessIconCount <= 0) // 해당 아이콘의 개수가 없다면 
                {
                    Destroy(_InventorySlot[i].AccessICon.gameObject); // 슬롯에 있는 아이콘 오브젝트를 파괴한다.

                    _InventorySlot[i].AccessICon = null; // 그리고 슬롯에 아이콘이 존재 하지 않음을 null로 초기화 한다.

                    break;
                }

                //if (_ICon.AccessIconCount <= 1) 
                //{
                //    _ICon.AccessIconCount = 0;

                //    Destroy(_InventorySlot[i].AccessICon.gameObject);

                //    _InventorySlot[i].AccessICon = null;

                //    break;
                //}
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