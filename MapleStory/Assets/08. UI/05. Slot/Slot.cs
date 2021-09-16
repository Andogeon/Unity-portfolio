using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotTpye // 장비창 슬롯을 결정하는 열거형입니다.
{
    Slot_Weapon,
    Slot_Clothes,
    Slot_Parts,
    Slot_Foot,
}

public enum SelectSlotTpye
{
    Slot_Equip,
    Slot_Inventory
}

public class Slot : MonoBehaviour // 인벤토리 및 장비창에서 사용하는 슬롯 클래스입니다.
{
    [SerializeField] private SelectSlotTpye _SelectSlotType = SelectSlotTpye.Slot_Equip;

    [SerializeField] private SlotTpye _SlotType = SlotTpye.Slot_Weapon;

    [SerializeField] private GameObject _Partobject = null;

    private ICON m_pItemICon = null; // 슬롯마다 아이콘을 표현하기 위한 아이콘 참조 변수입니다.

    private Image m_pIConImage = null; 

    private PART m_pPart = null;

    private void Awake()
    {
        if(_Partobject != null)
            m_pPart = _Partobject.GetComponent<PART>();
    }

    public ICON AccessICon // 아이콘을 외부에서 넘겨 받으면 바로 아이콘에 해당하는 아이템 바로 적용하는 프로퍼티입니다.
    {
        get { return m_pItemICon; }

        set 
        {
            m_pItemICon = value; // 아이콘을 넘겨 받을 시 

            if (m_pItemICon != null)
            {
                m_pIConImage = m_pItemICon.GET_ICONIMAGE;

                SetParent(); // 위치를 아이콘의 중심으로 이동하고 

                mounting(); // 아이템을 착용한다! 하지만 해당 슬롯이 인벤토리의 슬롯일 경우 함수내에서 종료합니다.
            }
        }
    }

    public SlotTpye AccessSlotType // 해당 아이템의 슬롯 타입을 외부로 넘기는 프로퍼티입니다.
    {
        get { return _SlotType; }
    }

    private void SetParent() // 아이콘을 장비창에 슬롯이나 인벤토리에 연결할 때 호출 되는 함수입니다.
    {
        if (null == m_pItemICon || null ==  m_pIConImage)
            return;

        m_pItemICon.transform.SetParent(gameObject.transform); // 인벤토리나 장비슬롯에 강제 이동

        m_pIConImage.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
    }

    private void mounting() // 실제 아이템을 장착시 호출하는 함수입니다.
    {
        if (_SelectSlotType == SelectSlotTpye.Slot_Inventory) // 해당 슬롯이 인벤토리의 슬롯일 경우 
            return; // 장착을 하지 않고 종료합니다.

        if(null == m_pItemICon.AccessItem) // 원본 아이템을 착용이 아닌 아이템의 아이콘의 복사본 아이템이 없다면 
        {
            GameObject _InstanGameObejct = GameObject.Instantiate(m_pItemICon.OriginalItem.gameObject);

            ITEM _Item = _InstanGameObejct.GetComponent<ITEM>();

            m_pItemICon.AccessItem = _Item; // 생성후 바로 연결 
        }

        m_pPart.AccessItem = m_pItemICon.AccessItem;  // 해당 파츠의 적용
    }

    public void ClearPartSlot() // 해당 슬롯에 공간을 비우는 함수입니다.
    {
        m_pIConImage = null;

        m_pItemICon = null;

        if (m_pPart != null) // 장비에 착용이 되었다면 
            m_pPart.DestoryItem(); // 해당 장비 착용 파괴 
    }

    public void ResetSlot()
    {
        m_pIConImage = null;

        m_pItemICon = null;
    }

    public ICON ReturnICon()
    {
        return m_pItemICon;
    }

    public void OnDestroy()
    {
        m_pItemICon = null;

        m_pIConImage = null;

        m_pPart = null;

        _Partobject = null;
    }
}