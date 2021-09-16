using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 물약 상점 및 무기 상점에서 사용되는 상점 클래스입니다.
public class Shop : MonoBehaviour // 상점 클래스입니다.
{
    [SerializeField] private ShopButton[] _ButtonObject = null; // 플레이어의 각각의 인벤토리 버튼

    private ICON[] m_pSellerItemIcons = null; // 판매할 아이템의 아이콘

    private NPC m_pNpc = null; // 상점과 연결된 NPC 

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private Image m_pImage = null; // 판매하고 있는 NPC의 스프라이트 정보를 넣어주기 위한 변수 

    private ShopSlot[] m_pShopSlot = null; // 판매할 상점의 슬롯 정보들

    private ShopSaleList[] m_pShopSaleSlots = null; // 플레이어가 판매할 장비창, 소비창, 기타창의 대한 정보

    private Inventory m_pInventory = null; // 판매 슬롯마다 인벤토리의 각각 정보를 넣어주기 위한 인벤토리의 정보

    public NPC AccessNpc
    {
        set { m_pNpc = value; }
    }


    // NPC의 이미지 정보를 받아오기 위한 변수입니다 .
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

    // 외부로부터 NPC가 판매할 아이템 즉 아이콘의 정보를 받아오고 상점의 대한 초기화를 진행하는 프로퍼티입니다
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

            // 플레이어가 판매할 장비, 소비, 기타창의 대한 정보를 가지고 온다.
        }
    }

    // 혹시 Start 함수 호출이 안될 시 강제로 상점의 대한 초기화를 진행하기 위한 함수입니다.

    private void InitShop()
    {
        if (null == m_pShopSlot) // 상점의 슬롯이 존재하지 않는다면 
        {
            GameObject _Inventory = GameObject.Find("Inventory UI").transform.Find("Inventory Box").gameObject;

            _Inventory = _Inventory.transform.Find("Inventory").gameObject; // 인벤토리를 검색 

            m_pInventory = _Inventory.GetComponent<Inventory>(); // 참조변수 

            m_pShopSlot = GetComponentsInChildren<ShopSlot>(); // 슬롯 추가 

            m_pShopSaleSlots = transform.Find("Shop Sale List").GetComponentsInChildren<ShopSaleList>();

            // 플레이어가 판매할 장비, 소비, 기타창의 대한 정보를 가지고 온다.
        }

        for (int i = 0; i < m_pSellerItemIcons.Length; ++i) // 판매할 아이콘의 개수만큼 순회 
        {
            m_pShopSlot[i].AccessSlotImage.sprite = m_pSellerItemIcons[i].GET_ICONIMAGE.sprite; // 슬롯에 아이템 이미지 추가

            m_pShopSlot[i].AccessICon = m_pSellerItemIcons[i]; // 아이콘 정보 추가 

            m_pShopSlot[i].AccessInventory = m_pInventory; // 슬롯의 인벤토리 정보 추가 
        }

        bool _Enable = false;

        for (int i = 0; i < m_pShopSaleSlots.Length; ++i) // 판매할 슬롯의 길이 만큼 순회 
        {
            if (i == 0)
            {
                if (m_pShopSaleSlots[i].AccessInventory == null)
                    m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessEquipInventory; // 장비창 인벤토리 슬롯 

                if(_ButtonObject[i].AccessEnableImage == true && _Enable == false) // 플레이어가 현재 장비창 인벤을 클릭했을때
                {
                    m_pShopSaleSlots[i].gameObject.SetActive(true); // 장비 창 판매 슬롯 활성화 

                    m_pShopSaleSlots[i + 1].gameObject.SetActive(false); // 소비창 판매슬롯 비활성화 

                    m_pShopSaleSlots[i + 2].gameObject.SetActive(false); // 기타창 판매슬롯 비활성화

                    _Enable = true; // 순회하면서 중복 호출을 막기 위한 변수를 활성화
                }
            }
            else if (i == 1)
            {
                if (m_pShopSaleSlots[i].AccessInventory == null)
                    m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessConsumptionInventory; // 플레이어가 현재 소비창 인벤을 클릭했을때

                if (_ButtonObject[i].AccessEnableImage == true && _Enable == false)
                {
                    m_pShopSaleSlots[i - 1].gameObject.SetActive(false); // 소비창 장비슬롯 비활성화 

                    m_pShopSaleSlots[i + 1].gameObject.SetActive(false); // 소비창 기타슬롯 비활성화 

                    m_pShopSaleSlots[i].gameObject.SetActive(true); // 소비창 판매슬롯 활성화 

                    _Enable = true;
                }
            }
            else if (i == 2)
            {
                if (m_pShopSaleSlots[i].AccessInventory == null)
                    m_pShopSaleSlots[i].AccessInventory = m_pInventory.AccessOtherInventory; // 기타창 인벤슬롯

                if (_ButtonObject[i].AccessEnableImage == true && _Enable == false) // 플레이어가 현재 기타창 인벤을 클릭했을때
                {
                    m_pShopSaleSlots[i - 2].gameObject.SetActive(false); // 소비창 장비슬롯 비활성화 

                    m_pShopSaleSlots[i - 1].gameObject.SetActive(false); // 소비창 판매슬롯 비활성화 

                    m_pShopSaleSlots[i].gameObject.SetActive(true); // 소비창 기타슬롯 활성화 

                    _Enable = true;
                }
            }
        }
    }

    // 상점 클래스를 다시 재사용하기 위한 초기화 함수입니다.

    public void ClearIConList()
    {
        for (int i = 0; i < m_pShopSlot.Length; ++i) // 모든 상점 슬롯을 순회하면서 
        {
            m_pShopSlot[i].AccessSlotImage.sprite = null; // 슬롯 스프라이트 이미지 초기화 

            m_pShopSlot[i].AccessICon = null; // 아이콘 정보 초기화 
        }
    }

    // 상점을 닫기 버튼을 누를시 호출 되는 함수입니다.

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