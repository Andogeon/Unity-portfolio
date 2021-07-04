using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shop : MonoBehaviour
{
    [SerializeField] private ShopButton[] _ButtonObject = null;

    private ICON[] m_pSellerItemIcons = null;

    private NPC m_pNpc = null;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private Image m_pImage = null;

    private ShopSlot[] m_pShopSlot = null;

    private ShopSaleList[] m_pShopSaleSlots = null;

    private Inventory m_pInventory = null;

    public NPC AccessNpc
    {
        set { m_pNpc = value; }
    }

    public Image AccessSprire
    {
        get 
        {
            if(null == m_pImage)
                m_pImage = transform.Find("Shop Seller Sprite").GetComponent<Image>();

            return m_pImage; 
        }

        set { m_pImage = value; }
    }

    public ICON[] AccessShop
    {
        get { return m_pSellerItemIcons; }

        set 
        { 
            m_pSellerItemIcons = value;

            InitShop();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if (null == m_pInventory)
        {
            GameObject _Inventory = GameObject.Find("Inventory UI").transform.Find("Inventory Box").gameObject;

            _Inventory = _Inventory.transform.Find("Inventory").gameObject;

            m_pInventory = _Inventory.GetComponent<Inventory>();

            m_pImage = transform.Find("Shop Seller Sprite").GetComponent<Image>();

            m_pShopSlot = GetComponentsInChildren<ShopSlot>();

            m_pShopSaleSlots = transform.Find("Shop Sale List").GetComponentsInChildren<ShopSaleList>();
        }
    }

    private void InitShop()
    {
        if (null == m_pShopSlot)
        {
            GameObject _Inventory = GameObject.Find("Inventory UI").transform.Find("Inventory Box").gameObject;

            _Inventory = _Inventory.transform.Find("Inventory").gameObject;

            m_pInventory = _Inventory.GetComponent<Inventory>();

            m_pShopSlot = GetComponentsInChildren<ShopSlot>();

            m_pShopSaleSlots = transform.Find("Shop Sale List").GetComponentsInChildren<ShopSaleList>();
        }

        for (int i = 0; i < m_pSellerItemIcons.Length; ++i) // 1. 여기서 문제 
        {
            m_pShopSlot[i].AccessSlotImage.sprite = m_pSellerItemIcons[i].GET_ICONIMAGE.sprite;

            m_pShopSlot[i].AccessICon = m_pSellerItemIcons[i];

            m_pShopSlot[i].AccessInventory = m_pInventory;
        }

        bool _Enable = false;

        for (int i = 0; i < m_pShopSaleSlots.Length; ++i) // 여기서 처리하면 되겟다 
        {
            if (i == 0)
            {
                if (m_pShopSaleSlots[i].AccessInventory == null)
                    m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessEquipInventory;

                if(_ButtonObject[i].AccessEnableImage == true && _Enable == false)
                {
                    m_pShopSaleSlots[i].gameObject.SetActive(true);

                    m_pShopSaleSlots[i + 1].gameObject.SetActive(false);

                    m_pShopSaleSlots[i + 2].gameObject.SetActive(false);

                    _Enable = true;
                }
            }
            else if (i == 1) // 여기서 갱신은 되어 있는 상황이구 .. 
            {
                if (m_pShopSaleSlots[i].AccessInventory == null)
                    m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessConsumptionInventory; // null일 확률 생각

                if (_ButtonObject[i].AccessEnableImage == true && _Enable == false)
                {
                    m_pShopSaleSlots[i - 1].gameObject.SetActive(false);

                    m_pShopSaleSlots[i + 1].gameObject.SetActive(false);

                    m_pShopSaleSlots[i].gameObject.SetActive(true);

                    _Enable = true;
                }
            }
            else if (i == 2)
            {
                if (m_pShopSaleSlots[i].AccessInventory == null)
                    m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessOtherInventory;

                if (_ButtonObject[i].AccessEnableImage == true && _Enable == false)
                {
                    m_pShopSaleSlots[i - 2].gameObject.SetActive(false);

                    m_pShopSaleSlots[i - 1].gameObject.SetActive(false);

                    m_pShopSaleSlots[i].gameObject.SetActive(true);

                    _Enable = true;
                }
            }
        }

        



        //for (int i = 0; i < m_pShopSaleSlots.Length; ++i) // 여기서 처리하면 되겟다 
        //{
        //    if (i == 0)
        //        m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessEquipInventory;
        //    else if (i == 1) // 여기서 갱신은 되어 있는 상황이구 .. 
        //    {
        //        m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessConsumptionInventory; // null일 확률 생각

        //        m_pShopSaleSlots[i].gameObject.SetActive(false);
        //    }
        //    else if (i == 2)
        //    {
        //        m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessOtherInventory;

        //        m_pShopSaleSlots[i].gameObject.SetActive(false);
        //    }
        //}
    }

    public void ClearIConList()
    {
        for (int i = 0; i < m_pShopSlot.Length; ++i)
        {
            m_pShopSlot[i].AccessSlotImage.sprite = null;

            m_pShopSlot[i].AccessICon = null;
        }
    }

    public void OnCancelShopButton()
    {
        if (null == m_pNpc)
            return;

        m_pNpc.ResetButton();

        m_pGameObjectManager.Remove("Shop", gameObject);
    }

    public void OnDestroy()
    {
        m_pInventory = null;

        m_pShopSlot = null;

        m_pImage = null;

        m_pNpc = null;
    }
}












//public class Shop : MonoBehaviour
//{
//    [SerializeField] private ICON[] _SellerItemIcons = null;

//    private NPC m_pNpc = null;

//    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

//    private Image m_pImage = null;

//    private ShopSlot[] m_pShopSlot = null;

//    private ShopSaleList[] m_pShopSaleSlots = null;

//    private Inventory m_pInventory = null;

//    public NPC AccessNpc
//    {
//        set { m_pNpc = value; }
//    }

//    public Image AccessSprire
//    {
//        get { return m_pImage; }

//        set { m_pImage = value; }
//    }

//    // Start is called before the first frame update
//    void Start()
//    {
//        GameObject _Inventory = GameObject.Find("Inventory UI").transform.Find("Inventory Box").gameObject;

//        _Inventory = _Inventory.transform.Find("Inventory").gameObject;

//        m_pInventory = _Inventory.GetComponent<Inventory>();

//        m_pImage = transform.Find("Shop Seller Sprite").GetComponent<Image>();

//        m_pShopSlot = GetComponentsInChildren<ShopSlot>();

//        m_pShopSaleSlots = transform.Find("Shop Sale List").GetComponentsInChildren<ShopSaleList>();

//        //for (int i = 0; i < m_pShopSlot.Length; ++i)

//        for (int i = 0; i < _SellerItemIcons.Length; ++i)
//        {
//            m_pShopSlot[i].AccessSlotImage.sprite = _SellerItemIcons[i].GET_ICONIMAGE.sprite;

//            m_pShopSlot[i].AccessICon = _SellerItemIcons[i];

//            m_pShopSlot[i].AccessInventory = m_pInventory;
//        }

//        for (int i = 0; i < m_pShopSaleSlots.Length; ++i)
//        {
//            if (i == 0)
//                m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessEquipInventory;
//            else if (i == 1)
//            {
//                m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessConsumptionInventory;

//                m_pShopSaleSlots[i].gameObject.SetActive(false);
//            }
//            else if (i == 2)
//            {
//                m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessOtherInventory;

//                m_pShopSaleSlots[i].gameObject.SetActive(false);
//            }
//        }
//    }

//    public void OnCancelShopButton()
//    {
//        if (null == m_pNpc)
//            return;

//        m_pNpc.ResetButton();

//        m_pGameObjectManager.Remove("Shop", gameObject);
//    }

//    public void OnDestroy()
//    {
//        m_pInventory = null;

//        m_pShopSlot = null;

//        m_pImage = null;

//        m_pNpc = null;
//    }
//}
