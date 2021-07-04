using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldItem : MonoBehaviour
{
    [SerializeField] private AudioClip _LearnSound = null;

    [SerializeField] private AudioClip _DropSound = null;

    [SerializeField] private ICONTYPE _IConType = ICONTYPE.ICON_EQUIP;

    [SerializeField] private LayerMask _LayerMask = 0;

    [SerializeField] private ICON _icon = null;

    [SerializeField] private float _DownJumpPower = 0.0f;

    [SerializeField] private Vector2 _CollisionSize = Vector2.zero;

    private Inventory m_pInventory = null; // 인벤토리에 대한 정보

    private GameObject m_pInventoryObject = null;

    private SpriteRenderer m_pSpritrRenderer = null;

    private BoxCollider2D m_pBoxCollision = null;

    private Rigidbody2D m_pRigidBody = null;

    private float m_fAngle = 0.0f;

    private float m_fOldPositionY = 0.0f;

    private float m_fTimeAcc = 0.0f;

    private float m_fLearnTimeAcc = 0.3f;

    private bool m_bIsDestroy = false;

    private bool m_bIsCreate = true;

    private bool m_bIsAddForce = false;

    private static bool m_bIsLearn = false;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, _CollisionSize);
    }
    private void Start()
    {
        //GameObject _GameObject = GameObject.Find("Inventory Box");

        //m_pInventoryObject = _GameObject.transform.Find("Inventory").gameObject;

        //m_pInventory = m_pInventoryObject.GetComponent<Inventory>();

        GameObject _GameObject = GameObject.Find("Inventory UI");

        m_pInventoryObject = _GameObject.transform.Find("Inventory Box").gameObject;

        m_pInventoryObject = m_pInventoryObject.transform.Find("Inventory").gameObject;

        m_pInventory = m_pInventoryObject.GetComponent<Inventory>();

        m_pSpritrRenderer = GetComponent<SpriteRenderer>();

        m_pBoxCollision = GetComponent<BoxCollider2D>();

        m_pRigidBody = GetComponent<Rigidbody2D>();

        m_pInventory.EnablesInvetory();

        m_pSoundManager.AddSound("LearnSound", _LearnSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound("DropSound", _DropSound, SoundType.Sound_Effect);

        m_pSoundManager.PlaySound("DropSound");
    }

    private void Update()
    {
        if (m_bIsCreate == true)
        {
            if (m_bIsAddForce == false)
            {
                m_pRigidBody.AddForce(Vector2.up * _DownJumpPower, ForceMode2D.Impulse);

                m_bIsAddForce = true;

                return;
            }

            if (m_pRigidBody.velocity.y > 0.0f)
                return;

            Collider2D _Collisoin = Physics2D.OverlapBox(transform.position, _CollisionSize, m_fAngle, _LayerMask);

            if (_Collisoin != null)
            {
                m_pRigidBody.gravityScale = 0.0f;

                m_pRigidBody.velocity = Vector2.zero;

                m_bIsCreate = false;

                m_fOldPositionY = m_pRigidBody.position.y;

                return;
            }

            return;
        }

        if (m_bIsDestroy)
        {
            if (m_bIsLearn == true)
            {
                m_fTimeAcc += Time.deltaTime;

                if (m_fTimeAcc >= m_fLearnTimeAcc)
                {
                    m_fTimeAcc = 0.0f;

                    m_bIsLearn = false;
                }
            }

            m_pRigidBody.velocity = Vector2.zero;

            m_pBoxCollision.enabled = false;

            Color _SpriteColor = m_pSpritrRenderer.color;

            if (_SpriteColor.a <= 0.0f)
            {
                Destroy(gameObject);

                return;
            }

            _SpriteColor.a -= 1.0f * Time.deltaTime;

            m_pSpritrRenderer.color = _SpriteColor;

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_fAngle += 1.0f);

            transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f * Time.deltaTime);

            return;
        }

        m_pRigidBody.position = new Vector3(m_pRigidBody.position.x, m_fOldPositionY + (Mathf.Sin(m_fAngle += 2.0f * Time.deltaTime) * 0.2f));
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.F) && m_bIsLearn == false)
        {
            m_bIsLearn = true;

            m_bIsDestroy = true;

            m_fAngle = 0.0f;

            GameObject _IConobject = GameObject.Instantiate(_icon.gameObject); // 임시적인 방법 !! 

            ICON CopyICon = _IConobject.GetComponent<ICON>();

            CopyICon.AccessIConType = _IConType;

            // 아이템이 씬 전환시 사라짐

            // 인벤토리의 장착시의 활성화 한다 !! 

            switch (CopyICon.AccessIConType) // 획득하는 아이템에 대해서는 클론 오브젝트의 여부를 확인 해서는 안된다 이거 무조건 해결 해야 한다 !! 
            {
                case ICONTYPE.ICON_EQUIP:
                    m_pInventory.EquipItemInsert(_IConobject);
                    break;
                case ICONTYPE.ICON_CONSUMABLE:
                    m_pInventory.ConsumptionItemInsert(_IConobject);
                    break;
                case ICONTYPE.ICON_OTHER:
                    m_pInventory.OtherItemInsert(_IConobject);
                    break;
            }

            m_pSoundManager.PlaySound("LearnSound");
        }
    }

    private void OnDestroy()
    {
        m_pSoundManager = null;

        m_pInventory = null;

        m_pSpritrRenderer = null;

        m_pBoxCollision = null;

        m_pRigidBody = null;
    }
}



//private void Update()
//{
//    if (m_bIsCreate == true)
//    {
//        //if (m_bIsAddForce == false)
//        //{
//        //    //if (null == m_pRigidBody)
//        //    //    m_pRigidBody = GetComponent<Rigidbody2D>();

//        //    m_pRigidBody.AddForce(Vector2.up * _DownJumpPower, ForceMode2D.Impulse);

//        //    m_bIsAddForce = true;

//        //    return;
//        //}

//        //if (m_pRigidBody.velocity.y >= 0.0f)
//        //    return;

//        //Collider2D _Collisoin = Physics2D.OverlapBox(transform.position, _CollisionSize, m_fAngle, _LayerMask);

//        //if(_Collisoin != null)
//        //{
//        //    //m_bIsCreate = false;

//        //    m_fAngle = 0.0f;

//        //    transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_fAngle);

//        //    m_bIsAddForce = false;

//        //    m_pRigidBody.gravityScale = 0.0f;
//        //}

//        return;
//    }

//    if (m_bIsDestroy)
//    {
//        m_pBoxCollision.enabled = false;

//        Color _SpriteColor = m_pSpritrRenderer.color;

//        if (_SpriteColor.a <= 0.0f)
//        {
//            Destroy(gameObject);

//            return;
//        }

//        _SpriteColor.a -= 1.0f * Time.deltaTime;

//        m_pSpritrRenderer.color = _SpriteColor;

//        transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_fAngle += 1.0f);

//        transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f * Time.deltaTime);

//        return;
//    }

//    transform.position = new Vector3(transform.position.x, (Mathf.Sin(m_fAngle += 3.0f * Time.deltaTime) * 0.2f) + m_fOldPositionY);
//}




//private void OnTriggerStay2D(Collider2D collision)
//{
//    if (Input.GetKey(KeyCode.F) && m_bIsCreating == false)
//    {
//        m_bIsCreating = true;

//        m_bIsDestroy = true;

//        m_fAngle = 0.0f;

//        GameObject _IConobject = GameObject.Instantiate(_icon.gameObject); // 임시적인 방법 !! 

//        ICON CopyICon = _IConobject.GetComponent<ICON>();

//        CopyICon.AccessIConType = _IConType;

//        // 아이템이 씬 전환시 사라짐

//        // 인벤토리의 장착시의 활성화 한다 !! 

//        switch (CopyICon.AccessIConType) // 획득하는 아이템에 대해서는 클론 오브젝트의 여부를 확인 해서는 안된다 이거 무조건 해결 해야 한다 !! 
//        {
//            case ICONTYPE.ICON_EQUIP:
//                m_pInventory.EquipItemInsert(_IConobject);
//                break;
//            case ICONTYPE.ICON_CONSUMABLE:
//                m_pInventory.ConsumptionItemInsert(_IConobject);
//                break;
//            case ICONTYPE.ICON_OTHER:
//                m_pInventory.OtherItemInsert(_IConobject);
//                break;
//        }
//    }
//}

//private void OnTriggerStay2D(Collider2D collision)
//{
//    if (Input.GetKey(KeyCode.F) && m_bIsCreating == false)
//    {
//        m_bIsCreating = true;

//        m_bIsDestroy = true;

//        m_fAngle = 0.0f;

//        GameObject _IConobject = GameObject.Instantiate(_icon.gameObject); // 임시적인 방법 !! 

//        GameObject _CopyItem = null;

//        ICON CopyICon = _IConobject.GetComponent<ICON>();

//        CopyICon.AccessIConType = _IConType;

//        // 아이템이 씬 전환시 사라짐

//        // 인벤토리의 장착시의 활성화 한다 !! 

//        switch (CopyICon.AccessIConType) // 획득하는 아이템에 대해서는 클론 오브젝트의 여부를 확인 해서는 안된다 이거 무조건 해결 해야 한다 !! 
//        {
//            case ICONTYPE.ICON_EQUIP:
//                m_pInventory.EquipItemInsert(_IConobject);
//                break;
//            case ICONTYPE.ICON_CONSUMABLE:
//                _CopyItem = GameObject.Instantiate(_icon.OriginalItem.gameObject); // 여기서 확인을 할 수 없다 ??
//                DontDestroyOnLoad(_CopyItem);
//                CopyICon.AccessItem = _CopyItem.GetComponent<ITEM>();
//                CopyICon.AccessItem.AccessIcon = CopyICon;
//                CopyICon.AccessOrlGameObejct = _CopyItem;
//                m_pInventory.ConsumptionItemInsert(_IConobject);
//                break;
//            case ICONTYPE.ICON_OTHER:
//                _CopyItem = GameObject.Instantiate(_icon.OriginalItem.gameObject); // 여기서 확인을 할 수 없다 ??
//                CopyICon.AccessItem = _CopyItem.GetComponent<ITEM>();
//                CopyICon.AccessItem.AccessIcon = CopyICon;
//                CopyICon.AccessOrlGameObejct = _CopyItem;
//                m_pInventory.OtherItemInsert(_IConobject);
//                break;
//        }
//    }
//}

//private void OnTriggerEnter2D(Collider2D collision)
//{
//    if (Input.GetKey(KeyCode.F))
//    {
//        GameObject _IConobject = GameObject.Instantiate(_icon.gameObject); // 임시적인 방법 !! 

//        GameObject _CopyItem = null;

//        ICON CopyICon = _IConobject.GetComponent<ICON>();

//        CopyICon.AccessIConType = _IConType;

//        switch (CopyICon.AccessIConType)
//        {
//            case ICONTYPE.ICON_EQUIP:
//                m_pInventory.EquipItemInsert(_IConobject);
//                break;
//            case ICONTYPE.ICON_CONSUMABLE:
//                _CopyItem = GameObject.Instantiate(_icon.OriginalItem.gameObject); // 여기서 확인을 할 수 없다 ??
//                CopyICon.AccessItem = _CopyItem.GetComponent<ITEM>();
//                CopyICon.AccessItem.AccessIcon = CopyICon;
//                m_pInventory.ConsumptionItemInsert(_IConobject);
//                break;
//            case ICONTYPE.ICON_OTHER:
//                m_pInventory.OtherItemInsert(_IConobject);
//                break;
//        }

//        Destroy(gameObject);
//    }
//}

//private void OnTriggerStay2D(Collision2D collision)
//{        
//    if (Input.GetKeyDown(KeyCode.F))
//    {
//        GameObject _IConobject = GameObject.Instantiate(_icon.gameObject); // 임시적인 방법 !! 

//        ICON CopyICon = _IConobject.GetComponent<ICON>();

//        CopyICon.AccessIConType = _IConType;

//        switch(CopyICon.AccessIConType)
//        {
//            case ICONTYPE.ICON_EQUIP:
//                //m_pInventory
//                break;
//            case ICONTYPE.ICON_CONSUMABLE:
//                break;
//            case ICONTYPE.ICON_OTHER:
//                break;
//        }

//        Destroy(gameObject);
//    }
//}

//m_pInventory.InsertItem(_IConobject);








//Ray2D _Ray2D = new Ray2D(transform.position, Vector2.down);

//Debug.DrawRay(_Ray2D.origin, _Ray2D.direction * 0.05f);

//RaycastHit2D Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 0.05f, _LayerMask);

//if(Hit2D.collider != null)
//{
//    m_bIsCreate = false;

//    m_fAngle = 0.0f;

//    transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_fAngle);

//    m_bIsAddForce = false;

//    m_pRigidBody.gravityScale = 0.0f;

//    return;
//}