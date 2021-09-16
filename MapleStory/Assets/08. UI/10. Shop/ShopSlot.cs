using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour // 상점 슬롯 클래스입니다.
{
    private Image m_pImage = null;

    private Sprite m_pSprite = null;

    private ICON m_pSellerICon = null;

    private Inventory m_pInventory = null;

    private bool m_bIsCilck = false;

    public Image AccessSlotImage
    {
        get
        {
            if (null == m_pImage)
            {
                m_pImage = transform.GetChild(0).GetComponent<Image>();

                m_pSprite = m_pImage.sprite;
            }

            return m_pImage;
        }

        set 
        { 
            if(m_pSprite == null)
                m_pSprite = m_pImage.sprite;

            m_pImage = value; 
        }
    }

    // 외부로부터 스프라이트를 넘겨받는 프로퍼티 입니다.
    public Sprite AccessSprite
    {
        get
        {
            if (null == m_pImage)
            {
                m_pImage = transform.GetChild(0).GetComponent<Image>();

                m_pSprite = m_pImage.sprite; // 현재 빈 슬롯의 스프라이트 이미지를 가지고 있게 변수로 이동
            }

            return m_pSprite;
        }
        
        set { m_pSprite = value; }
    }

    public ICON AccessICon
    {
        get { return m_pSellerICon; }

        set { m_pSellerICon = value; }
    }

    public Inventory AccessInventory
    {
        get { return m_pInventory; }

        set { m_pInventory = value; }
    }

    // 상점 슬롯을 더블 클릭시 아이템을 구매하고 장비, 소비 인벤토리에 삽입하는 함수입니다.

    public void BuyCilck() 
    {
        if(false == m_bIsCilck) // 한번 클릭시 
        {
            m_bIsCilck = true;

            return;
        }

        GameObject _IConobject = GameObject.Instantiate(m_pSellerICon.gameObject); // 아이콘 복사 오브젝트를 생성 

        GameObject _CopyItem = null;

        ICON CopyICon = _IConobject.GetComponent<ICON>();

        if (m_pSellerICon.AccessOrlIConType == ICONTYPE.ICON_EQUIP) // 해당 판매하는 아이콘의 타입이 장비쪽이라면 
        {
            _CopyItem = GameObject.Instantiate(m_pSellerICon.OriginalItem.gameObject); // 해당 아이콘의 원본 아이템 오브젝트를 통하여 복사본 생성 

            CopyICon.AccessItem = _CopyItem.GetComponent<ITEM>(); // 복사본 아이템 객체를 대입

            CopyICon.AccessItem.AccessIcon = CopyICon; // 아이콘의 복사본 아이콘 대입

            CopyICon.AccessOrlGameObejct = _CopyItem; // 아이콘의 복사본 아이템의 대입

            m_pInventory.EquipItemInsert(_IConobject); // 장비 아이콘을 장비 인벤토리의 삽입
        }
        else // 그러지 않을 경우 소비창
            m_pInventory.ConsumptionItemInsert(_IConobject); // 소비 아이콘을 소비 인벤토리의 삽입

        m_bIsCilck = false;
    }

    // 상점 플레이어 인벤토리에서 슬롯을 더블클릭시 판매할 때 호출 되는 함수입니다.

    public void OnSaleButton() 
    {
        if (false == m_bIsCilck)
        {
            m_bIsCilck = true;

            return;
        }

        if (null == m_pSellerICon)
            return;

        ICON _DeleteICon = m_pSellerICon;

        m_pSellerICon.AccessMySlot.AccessICon = null;

        m_pSellerICon.AccessMySlot = null;

        if(m_pSellerICon.AccessItem != null) // 판매하는 아이콘의 아이템 즉 장비가 달려 있다면 
            Destroy(m_pSellerICon.AccessItem.gameObject); // 아이템의 오브젝트 자체를 소멸

        Destroy(_DeleteICon.gameObject); // 아이콘의 소멸 

        m_pImage.sprite = m_pSprite; // 해당 슬롯 이미지를 빈 아이콘으로 변경

        m_bIsCilck = false;
    }

    public void OnDestroy()
    {
        m_pSellerICon = null;

        m_pInventory = null;
    }
}
