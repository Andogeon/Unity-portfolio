using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField] private AudioClip _StartDropClickSound = null;
    
    [SerializeField] private AudioClip _EndDropClickSound = null;

    private EquipInventory m_pEquipInventory = null;

    private Consumption_Inventory m_pConsumptionInventory = null;

    private Other_Inventory Other_Inventory = null;

    protected Slot[] m_pSlots = null;

    protected SoundManager m_pSoundManager = SoundManager.GetInstance();

    public Slot[] AccessSlots
    {
        get 
        {
            if (null == m_pSlots)
                m_pSlots = GetComponentsInChildren<Slot>();

            return m_pSlots; 
        }

        set { m_pSlots = value; }
    }

    public EquipInventory AccessEquipInventory
    {
        get 
        {
            if (null == m_pEquipInventory)
                m_pEquipInventory = transform.Find("Equip Window").GetComponent<EquipInventory>();

            return m_pEquipInventory; 
        }

        set { m_pEquipInventory = value; }
    }

    public Consumption_Inventory AccessConsumptionInventory
    {
        get 
        { 
            if(null == m_pConsumptionInventory)
                m_pConsumptionInventory = transform.Find("Consumption Window").GetComponent<Consumption_Inventory>();

            return m_pConsumptionInventory; 
        }

        set { m_pConsumptionInventory = value; }
    }

    public Other_Inventory AccessOtherInventory
    {
        get 
        {
            if (null == Other_Inventory)
                Other_Inventory = transform.Find("Other Window").GetComponent<Other_Inventory>();

            return Other_Inventory; 
        }

        set { Other_Inventory = value; }
    }

    private void Awake()
    {
        m_pSoundManager.AddSound("Start Drop Click Sound", _StartDropClickSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound("End Drop Click Sound", _EndDropClickSound, SoundType.Sound_Effect);

        m_pEquipInventory = transform.Find("Equip Window").GetComponent<EquipInventory>();

        m_pConsumptionInventory = transform.Find("Consumption Window").GetComponent<Consumption_Inventory>();

        Other_Inventory = transform.Find("Other Window").GetComponent<Other_Inventory>();

        if (m_pEquipInventory != null && m_pConsumptionInventory != null && Other_Inventory != null)
        {
            m_pConsumptionInventory.DisabledInventory();

            Other_Inventory.DisabledInventory();
        }
        else
        {
            m_pEquipInventory = null;

            m_pConsumptionInventory = null;

            Other_Inventory = null;
        }
    }

    public void EquipItemInsert(GameObject _FieldObject)
    {
        if (null == _FieldObject)
            return;

        if(null == m_pEquipInventory)
            m_pEquipInventory = transform.Find("Equip Window").GetComponent<EquipInventory>();

        m_pEquipInventory.InsertItem(_FieldObject);
    }

    public void ConsumptionItemInsert(GameObject _FieldObject)
    {
        if (null == _FieldObject)
            return;

        if (null == m_pConsumptionInventory)
            m_pConsumptionInventory = transform.Find("Consumption Window").GetComponent<Consumption_Inventory>();

        // 여기서 수정 들어가야지 ?ㅋ

        m_pConsumptionInventory.InsertItem(_FieldObject);
    }

    public void OtherItemInsert(GameObject _FieldObject)
    {
        if (null == _FieldObject)
            return;

        // 여기서 수정 들어가야지 ?ㅋ

        if(null == Other_Inventory)
            Other_Inventory = transform.Find("Other Window").GetComponent<Other_Inventory>();

        Other_Inventory.InsertItem(_FieldObject);
    }

    public virtual void EnableInventory()
    {
        return;
    }

    public virtual void DisabledInventory()
    {
        return;
    }

    public void EnablesInvetory()
    {
        m_pEquipInventory = GetComponentInChildren<EquipInventory>();

        m_pConsumptionInventory = GetComponentInChildren<Consumption_Inventory>();

        Other_Inventory = GetComponentInChildren<Other_Inventory>();

        if (m_pEquipInventory != null && m_pConsumptionInventory != null && Other_Inventory != null)
        {
            m_pConsumptionInventory.DisabledInventory();

            Other_Inventory.DisabledInventory();
        }
        else
        {
            m_pEquipInventory = null;

            m_pConsumptionInventory = null;

            Other_Inventory = null;
        }
    }
}

//private void Start()
//{
//    m_pEquipInventory = GetComponentInChildren<EquipInventory>();

//    m_pConsumptionInventory = GetComponentInChildren<Consumption_Inventory>();

//    Other_Inventory = GetComponentInChildren<Other_Inventory>();

//    if (m_pEquipInventory != null && m_pConsumptionInventory != null && Other_Inventory != null)
//    {
//        m_pConsumptionInventory.DisabledInventory();

//        Other_Inventory.DisabledInventory();
//    }
//    else
//    {
//        m_pEquipInventory = null;

//        m_pConsumptionInventory = null;

//        Other_Inventory = null;
//    }
//}




//[SerializeField] LayerMask _DivisionLayer = 0; // 레이 캐스트 선택

//private Slot[] m_pSlots = null;

//private Equip m_pEquip = null;

//private float m_fClickTimeAcc = 0.0f;

//private float m_fDoudleClickTimeAcc = 0.0f;

//private ICON m_pMouseSelectICon = null;

//private bool m_bIsClick = false;

//private bool m_bIsDoubleclick = false;

//private bool m_bIsMoving = false;

//private RectTransform m_pRectTransform = null;

//public Slot[] AccessSlots
//{
//    get { return m_pSlots; }
//}

//private void Awake()
//{
//    m_pSlots = GetComponentsInChildren<Slot>();

//    m_pEquip = FindObjectOfType<Equip>();

//    m_pRectTransform = transform.parent.GetComponent<RectTransform>();
//}


//private void Update()
//{
//    Vector2 _PickingPosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

//    Ray2D _Ray = new Ray2D(_PickingPosition, Vector2.zero);

//    RaycastHit2D _HitCast = Physics2D.Raycast(_Ray.origin, _Ray.direction, 1.0f, _DivisionLayer);

//    if (_HitCast.collider != null && Input.GetMouseButton(0))
//    {
//        if (_HitCast.collider.CompareTag("Inventory") == true)
//            m_bIsMoving = true;
//    }

//    if(m_bIsMoving == true)
//    {
//        if (Input.GetMouseButtonUp(0))
//        {
//            m_bIsMoving = false;

//            return;
//        }

//        m_pRectTransform.position = _PickingPosition;
//    }

//    if (_HitCast.collider != null && Input.GetMouseButtonDown(0))
//    {
//        if (m_bIsClick == false)
//        {
//            InstacneCopyICon(_HitCast);

//            m_bIsClick = true;

//            return;
//        }

//        m_bIsClick = false;

//        MountingItem(_HitCast);
//    }
//}

//private void InstacneCopyICon(RaycastHit2D _HitCast)
//{
//    GameObject _GameObject = _HitCast.collider.gameObject;

//    if (null == _GameObject)
//        return;

//    Vector2 _MousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

//    Vector2 _MyPosition = _GameObject.transform.position; // 아이콘의 부모는 슬롯

//    Vector2 _Direction = _MyPosition - _MousePosition;

//    if(_Direction.magnitude <= 20.0f && null == m_pMouseSelectICon)
//    {
//        Slot _CollisionSolt = _GameObject.GetComponent<Slot>();

//        if (null == _CollisionSolt)
//            return;

//        ICON _CollisionICon = _CollisionSolt.AccessICon;

//        if (null == _CollisionICon)
//            return;

//        GameObject _InstanceCopyIconObject = GameObject.Instantiate(_CollisionICon.gameObject);

//        _InstanceCopyIconObject.layer = LayerMask.NameToLayer("Copy");

//        _InstanceCopyIconObject.transform.SetParent(transform.parent);

//        ICON _CopyICon = _InstanceCopyIconObject.GetComponent<ICON>();

//        if (null == _CopyICon)
//        {
//            Destroy(_InstanceCopyIconObject);

//            return;
//        }

//        _CopyICon.AccessMySlot = _CollisionSolt;

//        _CopyICon.AccessMovingICon = true;

//        _CopyICon.AccessOrlGameObejct = _CollisionICon.gameObject;

//        m_pMouseSelectICon = _CopyICon;
//    }
//}

//private void MountingItem(RaycastHit2D _HitCast)
//{
//    GameObject _CollisionGameObject = _HitCast.collider.gameObject;

//    if (null == _CollisionGameObject || null == m_pMouseSelectICon) // 빈 슬롯 누르면 에러 나오는데 ??
//        return;

//    if (m_pMouseSelectICon.AccessMySlot.gameObject == _CollisionGameObject) // 이 때 장착한다 !! 
//    {
//        m_pEquip.InsertItem(m_pMouseSelectICon.AccessMySlot);

//        if (null != m_pMouseSelectICon)
//        {
//            Destroy(m_pMouseSelectICon.gameObject);

//            m_pMouseSelectICon = null;
//        }
//    }
//    else // 다르다면 스왑
//    {
//        for (int i = 0; i < m_pSlots.Length; ++i)
//        {
//            if (m_pSlots[i].gameObject == _CollisionGameObject) // 같은 슬롯이면 
//            {
//                if (m_pSlots[i].AccessICon == null)
//                {
//                    ICON _ICon = m_pMouseSelectICon.AccessMySlot.AccessICon;

//                    m_pSlots[i].AccessICon = _ICon;

//                    m_pMouseSelectICon.AccessMySlot.ClearPartSlot();

//                    Destroy(m_pMouseSelectICon.gameObject);

//                    m_pMouseSelectICon = null;

//                    break;
//                }
//                else
//                {
//                    ICON _FirstICon = m_pMouseSelectICon.AccessMySlot.AccessICon;

//                    ICON _SecondICon = m_pSlots[i].AccessICon;

//                    m_pSlots[i].ClearPartSlot();

//                    m_pSlots[i].AccessICon = _FirstICon;

//                    m_pMouseSelectICon.AccessMySlot.ClearPartSlot();

//                    m_pMouseSelectICon.AccessMySlot.AccessICon = _SecondICon;

//                    Destroy(m_pMouseSelectICon.gameObject);

//                    m_pMouseSelectICon = null;

//                    break;
//                }
//            }
//        }
//    }
//}

//public void ClearInvertorySlot(Slot _ClearSlot)
//{
//    if (null == _ClearSlot) // 왜 복사본이 들어와 ??
//        return;

//    for(int i = 0; i < m_pSlots.Length; ++i)
//    {
//        if(m_pSlots[i] == _ClearSlot)
//        {
//            m_pSlots[i].ClearPartSlot();

//            break;
//        }
//    }
//}

//private void Doubleclickconfirm()
//{
//    if (false == m_bIsClick)
//        return;

//    if(m_bIsDoubleclick == true)
//    {
//        m_fDoudleClickTimeAcc += Time.deltaTime;

//        if(m_fDoudleClickTimeAcc >= 1.0f)
//        {
//            m_fDoudleClickTimeAcc = 0.0f;

//            m_bIsDoubleclick = false;
//        }
//    }

//    m_fClickTimeAcc += Time.deltaTime;
//}

//public ICON RemoveSlotICon(Slot _InventoryInsertSlot) // 이거 수정 
//{
//    if (_InventoryInsertSlot == null)
//        return null;

//    ICON _ReturnICon = null;

//    for (int i = 0; i < m_pSlots.Length; ++i)
//    {
//        if(m_pSlots[i] == _InventoryInsertSlot)
//        {
//            _ReturnICon = m_pSlots[i].AccessICon;

//            m_pSlots[i].ClearPartSlot();

//            break;
//        }
//    }

//    return _ReturnICon;
//}

//public void InsertItem(GameObject _InstanceObject)
//{
//    ICON _InstanceICon = _InstanceObject.GetComponent<ICON>();

//    for (int i = 0; i < m_pSlots.Length; ++i)
//    {
//        if (m_pSlots[i].AccessICon == null)
//        {
//            _InstanceICon.AccessIConState = ICONSTATE.ICON_INVENTORY;

//            _InstanceICon.AccessMySlot = m_pSlots[i];

//            m_pSlots[i].AccessICon = _InstanceICon;

//            return;
//        }
//    }
//}

//public void OnDestroy()
//{
//    m_pEquip = null;

//    for (int i = 0; i < m_pSlots.Length; ++i)
//        m_pSlots[i] = null;
//}