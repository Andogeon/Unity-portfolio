using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopSaleList : MonoBehaviour
{
    private Inventory m_pInventory = null;

    private ShopSlot[] m_pShopSlots = null;

    public Inventory AccessInventory
    {
        get { return m_pInventory; }

        set { m_pInventory = value;}
    }

    private void Awake()
    {
        if (null == m_pShopSlots)
            m_pShopSlots = GetComponentsInChildren<ShopSlot>();
    }

    private void Update()
    {
        if (null == m_pInventory)
            return;

        if (null == m_pShopSlots)
            m_pShopSlots = GetComponentsInChildren<ShopSlot>();

        for (int i = 0; i < m_pShopSlots.Length; ++i)
        {
            if (m_pInventory.AccessSlots[i] == null || m_pInventory.AccessSlots[i].AccessICon == null) // 슬롯 자체에서 값을 못찾는데ㅐ ??
            {
                m_pShopSlots[i].AccessSlotImage.sprite = m_pShopSlots[i].AccessSprite;

                m_pShopSlots[i].AccessICon = null;

                continue;
            }

            m_pShopSlots[i].AccessICon = m_pInventory.AccessSlots[i].AccessICon;

            m_pShopSlots[i].AccessSlotImage.sprite = m_pShopSlots[i].AccessICon.GET_ICONIMAGE.sprite;
        }
    }

    private void OnEnable()
    {
        //if (null == m_pShopSlots)
        //    m_pShopSlots = GetComponentsInChildren<ShopSlot>();

        //for (int i = 0; i < m_pShopSlots.Length; ++i)
        //{
        //    if (m_pInventory.AccessSlots[i] == null || m_pInventory.AccessSlots[i].AccessICon == null) // 슬롯 자체에서 값을 못찾는데ㅐ ??
        //    {
        //        m_pShopSlots[i].AccessSlotImage.sprite = m_pShopSlots[i].AccessSprite;

        //        m_pShopSlots[i].AccessICon = null;

        //        continue;
        //    }

        //    m_pShopSlots[i].AccessICon = m_pInventory.AccessSlots[i].AccessICon;

        //    m_pShopSlots[i].AccessSlotImage.sprite = m_pShopSlots[i].AccessICon.GET_ICONIMAGE.sprite;
        //}
    }

    //private void Awake()
    //{
    //    if (null == m_pInventory)
    //        return;

    //    m_pShopSlots = GetComponentsInChildren<ShopSlot>();

    //    for(int i = 0; i < m_pShopSlots.Length; ++i)
    //    {
    //        m_pShopSlots[i].AccessICon = m_pInventory.AccessSlots[i].AccessICon;

    //        m_pShopSlots[i].AccessSlotImage = m_pShopSlots[i].AccessICon.GET_ICONIMAGE;
    //    }
    //}
}
