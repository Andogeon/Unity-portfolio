using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 인벤토리 정보를 상점 판매 인벤토리 정보에 갱신하기 위한 클래스입니다.

public class ShopSaleList : MonoBehaviour // 상점 판매 리스트입니다.
{
    private Inventory m_pInventory = null; // 플레이어의 인벤토리 

    private ShopSlot[] m_pShopSlots = null; // 인벤토리 슬롯과 연결될 상점 판매 슬롯

    // 외부로 부터 인벤토리의 정보를 받습니다.
    public Inventory AccessInventory
    {
        get { return m_pInventory; }

        set { m_pInventory = value;}
    }

    private void Awake()
    {
        if (null == m_pShopSlots) // 판매 슬롯이 없다면 
            m_pShopSlots = GetComponentsInChildren<ShopSlot>(); // 슬롯의 정보를 받아옴 
    }

    // 플레이어의 인벤토리 정보를 그대로 상점 판매인벤토리를 수시로 갱신하기 위해 Update함수에서 호출하게 했습니다.

    private void Update()
    {
        if (null == m_pInventory) // 인벤토리 정보가 없다면 
            return; // 종료 

        if (null == m_pShopSlots) // 슬롯 초기화가 되어있지 않다면 
            m_pShopSlots = GetComponentsInChildren<ShopSlot>(); // 슬롯의 대한 정보를 받아옴 

        for (int i = 0; i < m_pShopSlots.Length; ++i) // 상점 슬롯의 개수만큼 하나씩 순회하면서 
        {
            // 인벤토리의 정보가 없거나 혹은 인벤토리의 슬롯 아이콘 정보가 존재하지 않는다면 
            if (m_pInventory.AccessSlots[i] == null || m_pInventory.AccessSlots[i].AccessICon == null)
            {
                m_pShopSlots[i].AccessSlotImage.sprite = m_pShopSlots[i].AccessSprite; // 슬롯 이미지를 비어있는 이미지로 넣어둔다.

                m_pShopSlots[i].AccessICon = null; // 슬롯 아이콘의 정보를 비운다 

                continue; // 다음으로 넘김 
            }

            // 인벤토리 슬롯의 대한 정보가 있거나 혹은 아이콘의 이미지가 존재한다면 

            m_pShopSlots[i].AccessICon = m_pInventory.AccessSlots[i].AccessICon; // 인벤토리의 대한 아이콘의 대한 정보를 판매 슬롯에 정보 대입

            m_pShopSlots[i].AccessSlotImage.sprite = m_pShopSlots[i].AccessICon.GET_ICONIMAGE.sprite; // 이미지 또한 넣어준다.
        }
    }
}
