using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlot : MonoBehaviour
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

    public Sprite AccessSprite
    {
        get
        {
            if (null == m_pImage)
            {
                m_pImage = transform.GetChild(0).GetComponent<Image>();

                m_pSprite = m_pImage.sprite;
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

    public void BuyCilck()
    {
        if(false == m_bIsCilck)
        {
            m_bIsCilck = true;

            return;
        }

        GameObject _IConobject = GameObject.Instantiate(m_pSellerICon.gameObject); // 임시적인 방법 !! 

        GameObject _CopyItem = null;

        ICON CopyICon = _IConobject.GetComponent<ICON>();

        if (m_pSellerICon.AccessOrlIConType == ICONTYPE.ICON_EQUIP) // Type
        {
            _CopyItem = GameObject.Instantiate(m_pSellerICon.OriginalItem.gameObject); // 여기서 확인을 할 수 없다 ??

            CopyICon.AccessItem = _CopyItem.GetComponent<ITEM>();

            CopyICon.AccessItem.AccessIcon = CopyICon;

            CopyICon.AccessOrlGameObejct = _CopyItem;

            m_pInventory.EquipItemInsert(_IConobject);
        }
        else
            m_pInventory.ConsumptionItemInsert(_IConobject);

        // 혹시 모르니까 삭제 하지 않는다라고 설정 본다 !! 

        m_bIsCilck = false;



        //if (m_pSellerICon.OriginalItem != null) // Type
        //{
        //    _CopyItem = GameObject.Instantiate(m_pSellerICon.OriginalItem.gameObject); // 여기서 확인을 할 수 없다 ??

        //    CopyICon.AccessItem = _CopyItem.GetComponent<ITEM>();

        //    CopyICon.AccessItem.AccessIcon = CopyICon;

        //    CopyICon.AccessOrlGameObejct = _CopyItem;

        //    m_pInventory.EquipItemInsert(_IConobject);
        //}
        //else
        //    m_pInventory.ConsumptionItemInsert(_IConobject);

        //// 혹시 모르니까 삭제 하지 않는다라고 설정 본다 !! 

        //m_bIsCilck = false;
    }

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

        if(m_pSellerICon.AccessItem != null) // 혹시 기존에 만들어져 있다면 
            Destroy(m_pSellerICon.AccessItem.gameObject);

        Destroy(_DeleteICon.gameObject);

        m_pImage.sprite = m_pSprite;

        m_bIsCilck = false;
    }

    public void OnDestroy()
    {
        m_pSellerICon = null;

        m_pInventory = null;
    }
}
