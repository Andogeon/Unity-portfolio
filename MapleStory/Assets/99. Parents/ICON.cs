using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 기존 아이콘 태크 모두 1번으로 되어있음 문제 생기면 확인 !! 

public enum ICONTYPE
{
    ICON_EQUIP,
    ICON_CONSUMABLE,
    ICON_OTHER
}

public enum ICONSTATE
{
    ICON_INVENTORY,
    ICON_EQUIP
}

public class ICON : UI
{
    [SerializeField] private ICONTYPE _ConType = ICONTYPE.ICON_EQUIP; // 아이콘의 상태 

    [SerializeField] private SlotTpye _SlotType = SlotTpye.Slot_Weapon; // 장비창에서 사용될 슬롯 타입

    [SerializeField] private ITEM _OrlItem = null; // 아이콘에서 사용 될 원본 아이템 클래스 오브젝트 

    [SerializeField] private Sprite[] _NumberSprites = null; // 인벤토리에서 사용될 숫자 스프라이트들

    [SerializeField] private Vector3 _Size = Vector3.zero; // 아이콘의 크기를 지정할 변수 

    private ICONSTATE m_eIConState = ICONSTATE.ICON_EQUIP;

    private ICONTYPE m_eIConType = ICONTYPE.ICON_EQUIP;

    private ITEM m_pItem = null; 

    private Equip m_pEquip = null;

    private Slot m_pMySlot = null;

    private Image m_pIConImage = null;

    private Image[] m_pCountNumber = null;

    private GameObject m_pOrlGameObject = null;

    private bool m_bIsMovingIcon = false;

    private int m_iCount = 1;

    private int m_iChildCount = 0;

    public GameObject AccessOrlGameObejct
    {
        get { return m_pOrlGameObject; }

        set { m_pOrlGameObject = value; }
    }

    public bool AccessMovingICon
    {
        get { return m_bIsMovingIcon; }

        set { m_bIsMovingIcon = value; }
    }

    public SlotTpye AccessIConSlotType
    {
        get { return _SlotType; }

        set { _SlotType = value; }
    }

    public Slot AccessMySlot
    {
        get { return m_pMySlot; }

        set { m_pMySlot = value; }
    }

    public ICONTYPE AccessOrlIConType
    {
        get { return _ConType; }
    }

    public ICONTYPE AccessIConType
    {
        get { return m_eIConType; }

        set { m_eIConType = value; }
    }

    public Image GET_ICONIMAGE
    {
        get
        {
            if(null == m_pIConImage)
                m_pIConImage = GetComponent<Image>();

            return m_pIConImage;
        }
    }

    public ITEM AccessItem
    {
        get { return m_pItem; }

        set { m_pItem = value; }
    }

    public ITEM OriginalItem // 
    {
        get { return _OrlItem; }

        set { _OrlItem = value; }
    }

    public SlotTpye AccessSlotType
    {
        get { return _SlotType; }
    }

    public ICONSTATE AccessIConState
    {
        get { return m_eIConState; }

        set { m_eIConState = value; }
    }

    public int AccessIconCount
    {
        get { return m_iCount; }

        set { m_iCount = value; }
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(transform.position, _Size);
    }

    private void Awake() // 여기서 복사본을 만들때 .. 오류 나옴 !!
    {
        if(null == m_pIConImage)
            m_pIConImage = GetComponent<Image>();

        m_pEquip = FindObjectOfType<Equip>();

        m_eIConType = _ConType;

        bool _bIsClone = gameObject.name.Contains("(Clone)(Clone)");

        if (transform.childCount != 0 && _bIsClone == false)
        {
            m_pCountNumber = transform.GetChild(0).GetComponentsInChildren<Image>();

            m_iChildCount = m_pCountNumber.Length - 1;

            for (int i = 2; i > 0; --i)
                m_pCountNumber[i].gameObject.SetActive(false);
        }
    }

    private void LateUpdate()
    {
        CountNumberSprite();

        if (m_bIsMovingIcon == false)
            return;

        transform.position = Input.mousePosition;
    }

    public void UsePotion(Player _Player)
    {
        //if (null == _Player)
        //    return;

        //if (m_iCount <= 0)
        //{
        //    Destroy(gameObject);

        //    return;
        //}

        //m_iCount -= 1;

        //_Player.AccessPlayerHP += 10.0f;
    }

    public void CountNumberSprite()
    {
        if (null == m_pCountNumber)
            return;

        if (m_iCount <= 1) // 에러 ?
            m_pCountNumber[m_iChildCount - 2].gameObject.SetActive(false);
        else
        {// 카운트가 2개 이상일때 !! 

            //bool _MaxNumberSwitch = false;

            int _Count = m_iCount;

            float _Index = _Count / 100;

            if (_Index < 1.0f) //
                m_pCountNumber[m_iChildCount].gameObject.SetActive(false);
            else
            {
                if (m_pCountNumber[m_iChildCount].gameObject.activeSelf == false)
                    m_pCountNumber[m_iChildCount].gameObject.SetActive(true);

                m_pCountNumber[m_iChildCount].sprite = _NumberSprites[(int)_Index];

                //_MaxNumberSwitch = true;

                _Count -= (int)_Index * 100;
            }

            _Index = _Count / 10;

            if (_Index < 1.0f) //
                m_pCountNumber[m_iChildCount - 1].gameObject.SetActive(false);
            else
            {
                if (m_pCountNumber[m_iChildCount - 1].gameObject.activeSelf == false)
                    m_pCountNumber[m_iChildCount - 1].gameObject.SetActive(true);

                m_pCountNumber[m_iChildCount - 1].sprite = _NumberSprites[(int)_Index];

                _Count -= (int)_Index * 10;
            }

            if (m_pCountNumber[m_iChildCount - 2].gameObject.activeSelf == false)
                m_pCountNumber[m_iChildCount - 2].gameObject.SetActive(true);

            m_pCountNumber[m_iChildCount - 2].sprite = _NumberSprites[_Count];
        }
    }

    

    public void OnDestroy()
    {
        m_pItem = null;

        m_pEquip = null;

        m_pIConImage = null;

        m_pMySlot = null;

        m_pOrlGameObject = null;

        m_pCountNumber = null;

        Resources.UnloadUnusedAssets();
    }
}



//public void CopyCollision()
//{
//    if (gameObject.layer == LayerMask.NameToLayer("Copy"))
//    {
//        Collider2D _Collision = Physics2D.OverlapBox(transform.position, _Size, 0.0f);

//        if (null == _Collision)
//        {
//        }
//    }
//}


//public class ICON : UI
//{
//    [SerializeField] private ICONTYPE _ConType = ICONTYPE.ICON_EQUIP;

//    [SerializeField] private SlotTpye _SlotType = SlotTpye.Slot_Weapon;

//    [SerializeField] private ITEM _OrlItem = null;

//    [SerializeField] private Sprite[] _NumberSprites = null;

//    private ICONSTATE m_eIConState = ICONSTATE.ICON_EQUIP;

//    private ICONTYPE m_eIConType = ICONTYPE.ICON_EQUIP;

//    private ITEM m_pItem = null;

//    private Equip m_pEquip = null;

//    private Slot m_pMySlot = null;

//    private Image m_pIConImage = null;

//    private Image[] m_pCountNumber = null;

//    private GameObject m_pOrlGameObject = null;

//    private bool m_bIsMovingIcon = false;

//    private int m_iCount = 1;

//    private int m_iChildCount = 0;

//    public GameObject AccessOrlGameObejct
//    {
//        get { return m_pOrlGameObject; }

//        set { m_pOrlGameObject = value; }
//    }

//    public bool AccessMovingICon
//    {
//        get { return m_bIsMovingIcon; }

//        set { m_bIsMovingIcon = value; }
//    }

//    public SlotTpye AccessIConSlotType
//    {
//        get { return _SlotType; }

//        set { _SlotType = value; }
//    }

//    public Slot AccessMySlot
//    {
//        get { return m_pMySlot; }

//        set { m_pMySlot = value; }
//    }

//    public ICONTYPE AccessOrlIConType
//    {
//        get { return _ConType; }
//    }

//    public ICONTYPE AccessIConType
//    {
//        get { return m_eIConType; }

//        set { m_eIConType = value; }
//    }

//    public Image GET_ICONIMAGE
//    {
//        get
//        {
//            if (null == m_pIConImage)
//                m_pIConImage = GetComponent<Image>();

//            return m_pIConImage;
//        }
//    }

//    public ITEM AccessItem
//    {
//        get { return m_pItem; }

//        set { m_pItem = value; }
//    }

//    public ITEM OriginalItem // 
//    {
//        get { return _OrlItem; }

//        set { _OrlItem = value; }
//    }

//    public SlotTpye AccessSlotType
//    {
//        get { return _SlotType; }
//    }

//    public ICONSTATE AccessIConState
//    {
//        get { return m_eIConState; }

//        set { m_eIConState = value; }
//    }

//    public int AccessIconCount
//    {
//        get { return m_iCount; }

//        set { m_iCount = value; }
//    }

//    private void Awake() // 여기서 복사본을 만들때 .. 오류 나옴 !!
//    {
//        if (null == m_pIConImage)
//            m_pIConImage = GetComponent<Image>();

//        m_pEquip = FindObjectOfType<Equip>();

//        m_eIConType = _ConType;

//        bool _bIsClone = gameObject.name.Contains("(Clone)(Clone)");

//        if (transform.childCount != 0 && _bIsClone == false)
//        {
//            m_pCountNumber = transform.GetChild(0).GetComponentsInChildren<Image>();

//            m_iChildCount = m_pCountNumber.Length - 1;

//            for (int i = 2; i > 0; --i)
//                m_pCountNumber[i].gameObject.SetActive(false);
//        }
//    }

//    private void LateUpdate()
//    {
//        CountNumberSprite();

//        if (m_bIsMovingIcon == false)
//            return;

//        transform.position = Input.mousePosition;
//    }

//    public void UsePotion(Player _Player)
//    {
//        //if (null == _Player)
//        //    return;

//        //if (m_iCount <= 0)
//        //{
//        //    Destroy(gameObject);

//        //    return;
//        //}

//        //m_iCount -= 1;

//        //_Player.AccessPlayerHP += 10.0f;
//    }

//    public void CountNumberSprite()
//    {
//        if (null == m_pCountNumber)
//            return;

//        if (m_iCount <= 1) // 에러 ?
//            m_pCountNumber[m_iChildCount - 2].gameObject.SetActive(false);
//        else
//        {// 카운트가 2개 이상일때 !! 

//            bool _MaxNumberSwitch = false;

//            int _Count = m_iCount;

//            float _Index = _Count / 100;

//            if (_Index < 1.0f) //
//                m_pCountNumber[m_iChildCount].gameObject.SetActive(false);
//            else
//            {
//                if (m_pCountNumber[m_iChildCount].gameObject.activeSelf == false)
//                    m_pCountNumber[m_iChildCount].gameObject.SetActive(true);

//                m_pCountNumber[m_iChildCount].sprite = _NumberSprites[(int)_Index];

//                _MaxNumberSwitch = true;

//                _Count -= (int)_Index * 100;
//            }

//            _Index = _Count / 10;

//            if (_Index < 1.0f) //
//                m_pCountNumber[m_iChildCount - 1].gameObject.SetActive(false);
//            else
//            {
//                if (m_pCountNumber[m_iChildCount - 1].gameObject.activeSelf == false)
//                    m_pCountNumber[m_iChildCount - 1].gameObject.SetActive(true);

//                m_pCountNumber[m_iChildCount - 1].sprite = _NumberSprites[(int)_Index];

//                _Count -= (int)_Index * 10;
//            }

//            if (m_pCountNumber[m_iChildCount - 2].gameObject.activeSelf == false)
//                m_pCountNumber[m_iChildCount - 2].gameObject.SetActive(true);

//            m_pCountNumber[m_iChildCount - 2].sprite = _NumberSprites[_Count];
//        }
//    }

//    public void OnDestroy()
//    {
//        m_pItem = null;

//        m_pEquip = null;

//        m_pIConImage = null;

//        m_pMySlot = null;

//        m_pOrlGameObject = null;

//        m_pCountNumber = null;

//        Resources.UnloadUnusedAssets();
//    }
//}

















//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//// 기존 아이콘 태크 모두 1번으로 되어있음 문제 생기면 확인 !! 

//public enum ICONTYPE
//{
//    ICON_EQUIP,
//    ICON_CONSUMABLE,
//    //ITEM_CONSUMPTION
//    ICON_OTHER
//}

//public enum ICONSTATE
//{
//    ICON_INVENTORY,
//    ICON_EQUIP
//}

//public class ICON : UI
//{
//    [SerializeField] private ICONTYPE _ConType = ICONTYPE.ICON_EQUIP;

//    [SerializeField] private SlotTpye _SlotType = SlotTpye.Slot_Weapon;

//    [SerializeField] private ITEM _OrlItem = null;

//    private ICONSTATE m_eIConState = ICONSTATE.ICON_EQUIP;

//    private ICONTYPE m_eIConType = ICONTYPE.ICON_EQUIP;

//    private ITEM m_pItem = null;

//    private Equip m_pEquip = null;

//    private Slot m_pMySlot = null;

//    private Image m_pIConImage = null;

//    private GameObject m_pOrlGameObject = null;

//    private bool m_bIsMovingIcon = false;

//    private int m_iCount = 1;

//    public GameObject AccessOrlGameObejct
//    {
//        get { return m_pOrlGameObject; }

//        set { m_pOrlGameObject = value; }
//    }

//    //public GameObject AccessCopyGameObejct
//    //{
//    //    get { return m_pCopyGameObject; }

//    //    set { m_pCopyGameObject = value; }
//    //}

//    public bool AccessMovingICon
//    {
//        get { return m_bIsMovingIcon; }

//        set { m_bIsMovingIcon = value; }
//    }

//    public SlotTpye AccessIConSlotType
//    {
//        get { return _SlotType; }

//        set { _SlotType = value; }
//    }

//    public Slot AccessMySlot
//    {
//        get { return m_pMySlot; }

//        set { m_pMySlot = value; }
//    }

//    public ICONTYPE AccessIConType
//    {
//        get { return m_eIConType; }

//        set { m_eIConType = value; }
//    }

//    public Image GET_ICONIMAGE
//    {
//        get
//        {
//            if (null == m_pIConImage)
//                m_pIConImage = GetComponent<Image>();

//            return m_pIConImage;
//        }
//    }

//    public ITEM AccessItem
//    {
//        get { return m_pItem; }

//        set { m_pItem = value; }
//    }

//    public ITEM OriginalItem
//    {
//        get { return _OrlItem; }

//        set { _OrlItem = value; }
//    }

//    public SlotTpye AccessSlotType
//    {
//        get { return _SlotType; }
//    }

//    public ICONSTATE AccessIConState
//    {
//        get { return m_eIConState; }

//        set { m_eIConState = value; }
//    }

//    private void Awake()
//    {
//        if (null == m_pIConImage)
//            m_pIConImage = GetComponent<Image>();

//        m_pEquip = FindObjectOfType<Equip>();

//        m_eIConType = _ConType;

//        //m_pInventory = FindObjectOfType<Inventory>();

//        //m_pInventory = GameObject.Find("Inventory").GetComponent<Inventory>();
//    }

//    private void LateUpdate()
//    {
//        if (m_bIsMovingIcon == false)
//            return;

//        transform.position = Input.mousePosition;
//    }

//    public void OnDestroy()
//    {
//        m_pItem = null;

//        m_pEquip = null;

//        m_pIConImage = null;

//        m_pMySlot = null;

//        m_pOrlGameObject = null;

//        Resources.UnloadUnusedAssets();
//    }
//}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.UI;

//// 기존 아이콘 태크 모두 1번으로 되어있음 문제 생기면 확인 !! 

//public enum ICONTYPE
//{
//    ICON_EQUIP,
//    ICON_CONSUMABLE,
//    //ITEM_CONSUMPTION
//    ICON_OTHER
//}

//public enum ICONSTATE
//{
//    ICON_INVENTORY,
//    ICON_EQUIP
//}

//public class ICON : UI
//{
//    [SerializeField] private ICONTYPE _ConType = ICONTYPE.ICON_EQUIP;

//    [SerializeField] private SlotTpye _SlotType = SlotTpye.Slot_Weapon;

//    [SerializeField] private ITEM _OrlItem = null;

//    private ICONSTATE m_eIConState = ICONSTATE.ICON_EQUIP;

//    private ICONTYPE m_eIConType = ICONTYPE.ICON_EQUIP;

//    private ITEM m_pItem = null;

//    private Equip m_pEquip = null;

//    //private Inventory m_pInventory = null;

//    private Slot m_pMySlot = null;

//    private Image m_pIConImage = null;

//    private GameObject m_pOrlGameObject = null;

//    //private GameObject m_pCopyGameObject = null;

//    private bool m_bIsMovingIcon = false;

//    public GameObject AccessOrlGameObejct
//    {
//        get { return m_pOrlGameObject; }

//        set { m_pOrlGameObject = value; }
//    }

//    //public GameObject AccessCopyGameObejct
//    //{
//    //    get { return m_pCopyGameObject; }

//    //    set { m_pCopyGameObject = value; }
//    //}

//    public bool AccessMovingICon
//    {
//        get { return m_bIsMovingIcon; }

//        set { m_bIsMovingIcon = value; }
//    }

//    public SlotTpye AccessIConSlotType
//    {
//        get { return _SlotType; }

//        set { _SlotType = value; }
//    }

//    public Slot AccessMySlot
//    {
//        get { return m_pMySlot; }

//        set { m_pMySlot = value; }
//    }

//    public ICONTYPE AccessIConType
//    {
//        get { return m_eIConType; }

//        set { m_eIConType = value; }
//    }

//    public Image GET_ICONIMAGE
//    {
//        get
//        {
//            if (null == m_pIConImage)
//                m_pIConImage = GetComponent<Image>();

//            return m_pIConImage;
//        }
//    }

//    public ITEM AccessItem
//    {
//        get { return m_pItem; }

//        set { m_pItem = value; }
//    }

//    public ITEM OriginalItem
//    {
//        get { return _OrlItem; }

//        set { _OrlItem = value; }
//    }

//    public SlotTpye AccessSlotType
//    {
//        get { return _SlotType; }
//    }

//    public ICONSTATE AccessIConState
//    {
//        get { return m_eIConState; }

//        set { m_eIConState = value; }
//    }

//    private void Awake()
//    {
//        if (null == m_pIConImage)
//            m_pIConImage = GetComponent<Image>();

//        m_pEquip = FindObjectOfType<Equip>();

//        m_eIConType = _ConType;

//        //m_pInventory = FindObjectOfType<Inventory>();

//        //m_pInventory = GameObject.Find("Inventory").GetComponent<Inventory>();
//    }

//    private void LateUpdate()
//    {
//        if (m_bIsMovingIcon == false)
//            return;

//        transform.position = Input.mousePosition;
//    }

//    public void OnDestroy()
//    {
//        m_pItem = null;

//        m_pEquip = null;

//        m_pIConImage = null;

//        Resources.UnloadUnusedAssets();

//        //m_pInventory = null;

//        //m_pOrlGameObject = null;

//        //m_pCopyObject = null;
//    }
//}
