using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum SlotTpye
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

public class Slot : MonoBehaviour // 각 슬롯마다 부위를 지정하고 넘겨서 연결할 수 있게 
{
    [SerializeField] private SelectSlotTpye _SelectSlotType = SelectSlotTpye.Slot_Equip;

    [SerializeField] private SlotTpye _SlotType = SlotTpye.Slot_Weapon;

    [SerializeField] private GameObject _Partobject = null;

    private ICON m_pItemICon = null;

    private Image m_pIConImage = null;

    private PART m_pPart = null;

    private void Awake()
    {
        if(_Partobject != null)
            m_pPart = _Partobject.GetComponent<PART>();
    }

    public ICON AccessICon
    {
        get { return m_pItemICon; }

        set 
        {
            m_pItemICon = value;

            if (m_pItemICon != null)
            {
                m_pIConImage = m_pItemICon.GET_ICONIMAGE;

                SetParent();

                mounting();
            }
        }
    }

    public SlotTpye AccessSlotType
    {
        get { return _SlotType; }
    }

    private void SetParent() // 아이콘 연결 
    {
        if (null == m_pItemICon || null ==  m_pIConImage)
            return;

        m_pItemICon.transform.SetParent(gameObject.transform);

        m_pIConImage.rectTransform.anchoredPosition = new Vector2(0.0f, 0.0f);
    }

    private void mounting() // 장착 여부인데 ??
    {
        if (_SelectSlotType == SelectSlotTpye.Slot_Inventory) // 이거 수정
            return;

        if(null == m_pItemICon.AccessItem)
        {
            GameObject _InstanGameObejct = GameObject.Instantiate(m_pItemICon.OriginalItem.gameObject);

            ITEM _Item = _InstanGameObejct.GetComponent<ITEM>();

            m_pItemICon.AccessItem = _Item;
        }

        m_pPart.AccessItem = m_pItemICon.AccessItem; //??

        //m_pPart.AccessItem = m_pItemICon.OriginalItem;
    }

    public void ClearPartSlot()
    {
        m_pIConImage = null;

        m_pItemICon = null;

        if (m_pPart != null)
            m_pPart.DestoryItem();
    }

    //public void ResetPartSlot()
    //{
    //    m_pIConImage = null;

    //    m_pItemICon = null;

    //    if(m_pPart != null)
    //        m_pPart.DestoryItem();
    //}

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