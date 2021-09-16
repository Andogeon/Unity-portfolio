using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FieldItem : MonoBehaviour // 몬스터 사냥시 필드에 떨어지는 아이템클래스입니다.
{
    [SerializeField] private AudioClip _LearnSound = null; // 획득시 사운드 

    [SerializeField] private AudioClip _DropSound = null; // 등장시 사운드

    [SerializeField] private ICONTYPE _IConType = ICONTYPE.ICON_EQUIP; // 아이콘의 타입 

    [SerializeField] private LayerMask _LayerMask = 0; // 충돌 레이어 값 

    [SerializeField] private ICON _icon = null; // 어떤 아이콘 아이템을 줄것인지 

    [SerializeField] private float _DownJumpPower = 0.0f; // 중력 파워 

    [SerializeField] private Vector2 _CollisionSize = Vector2.zero; // 중복 박스 사이즈 

    private Inventory m_pInventory = null; // 인벤토리에 대한 정보

    private GameObject m_pInventoryObject = null;

    private SpriteRenderer m_pSpritrRenderer = null;

    private BoxCollider2D m_pBoxCollision = null; // 충돌체 변수 

    private Rigidbody2D m_pRigidBody = null; 

    private float m_fAngle = 0.0f; // 아이템 획득시 

    private float m_fOldPositionY = 0.0f;

    private float m_fTimeAcc = 0.0f;

    private float m_fLearnTimeAcc = 0.3f;

    private bool m_bIsDestroy = false;

    private bool m_bIsCreate = true;

    private bool m_bIsAddForce = false;

    private static bool m_bIsLearn = false; // 중복 획득을 막기 위한 변수 

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, _CollisionSize);
    }
    private void Start()
    {
        GameObject _GameObject = GameObject.Find("Inventory UI");

        m_pInventoryObject = _GameObject.transform.Find("Inventory Box").gameObject;

        m_pInventoryObject = m_pInventoryObject.transform.Find("Inventory").gameObject;

        m_pInventory = m_pInventoryObject.GetComponent<Inventory>(); // 인벤토리의 해당 필드 아이템을 삽입하기 위한 인벤토리 정보 탐색

        m_pSpritrRenderer = GetComponent<SpriteRenderer>();

        m_pBoxCollision = GetComponent<BoxCollider2D>(); // 2D 충돌체 

        m_pRigidBody = GetComponent<Rigidbody2D>();

        m_pInventory.EnablesInvetory(); 

        m_pSoundManager.AddSound("LearnSound", _LearnSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound("DropSound", _DropSound, SoundType.Sound_Effect);

        m_pSoundManager.PlaySound("DropSound");
    }

    // 아이템 생성 시 높게 띄우고 습득시 아이템의 Y축 이동과 알파값 조정 및 회전할 수 있게 구현했습니다.
    private void Update()
    {
        if (m_bIsCreate == true) // 아이템이 생성이 되었다면 
        {
            if (m_bIsAddForce == false)
            {
                m_pRigidBody.AddForce(Vector2.up * _DownJumpPower, ForceMode2D.Impulse); // 높게 띄우고

                m_bIsAddForce = true; // AddForce 함수 호출을 한 번만 호출하기 위한 변수 제어 

                return;
            }

            if (m_pRigidBody.velocity.y > 0.0f) // 아직도 높게 뜨고 있다면
                return;

            // 충돌 박스를 생성 
            Collider2D _Collisoin = Physics2D.OverlapBox(transform.position, _CollisionSize, m_fAngle, _LayerMask);

            if (_Collisoin != null) // 지면과 충돌중이라면 
            {
                m_pRigidBody.gravityScale = 0.0f; // 중력 제어를 0으로 지정

                m_pRigidBody.velocity = Vector2.zero;

                m_bIsCreate = false; // 아이템을 획득할 수 있게 제어

                m_fOldPositionY = m_pRigidBody.position.y; // 현재 Y축을 이전 Y축으로 지정

                return;
            }

            return;
        }

        if (m_bIsDestroy) // 습득했다면 
        {
            if (m_bIsLearn == true) // 플레이어가 획득 했다면 중복 획득을 막기 위해 
            {
                m_fTimeAcc += Time.deltaTime; // 타이머를 잰다

                if (m_fTimeAcc >= m_fLearnTimeAcc) // 해당 시간이 경과 했다면 
                {
                    m_fTimeAcc = 0.0f; // 0으로 초기화하고 

                    m_bIsLearn = false; // 다시 다른 아이템을 획득권한을 연다
                }
            }

            m_pRigidBody.velocity = Vector2.zero;

            m_pBoxCollision.enabled = false; // 충돌 박스를 해재한다

            Color _SpriteColor = m_pSpritrRenderer.color;

            if (_SpriteColor.a <= 0.0f) // 완전히 투명한 상태일시 
            {
                Destroy(gameObject); // 아이템을 파괴 

                return;
            }

            _SpriteColor.a -= 1.0f * Time.deltaTime; // 알파값을 천천히 뺀다

            m_pSpritrRenderer.color = _SpriteColor; // 실제 적용

            transform.rotation = Quaternion.Euler(0.0f, 0.0f, m_fAngle += 1.0f); // 습득히 회전할수 있게 지정

            transform.position = new Vector3(transform.position.x, transform.position.y + 1.0f * Time.deltaTime); // 습득히 Y축 이동

            return;
        }

        // 습득하지 않는 상태일시 sin 값을 활용하여 위 아래로 움직일 수 있게 한다.
        m_pRigidBody.position = new Vector3(m_pRigidBody.position.x, m_fOldPositionY + (Mathf.Sin(m_fAngle += 2.0f * Time.deltaTime) * 0.2f));
    }

    // 충돌 도중 F키를 눌렀다면 아이템을 습득하게 구현한 함수입니다.
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.GetKey(KeyCode.F) && m_bIsLearn == false) // 플레이어가 습득했다면 
        {
            m_bIsLearn = true; // 나머지 아이템들 중복 습득을 막는다

            m_bIsDestroy = true; // 습득으로 인식하게 하고 알파값을 빼기 시작한다.

            m_fAngle = 0.0f;

            GameObject _IConobject = GameObject.Instantiate(_icon.gameObject); // 아이콘의 복사 오브젝트를 생성 

            ICON CopyICon = _IConobject.GetComponent<ICON>();

            CopyICon.AccessIConType = _IConType; // 타입을 지정후 

            switch (CopyICon.AccessIConType) // 복사 오브젝트 타입에 따른 인벤토리에 삽입을 한다
            {
                case ICONTYPE.ICON_EQUIP: // 장비시
                    m_pInventory.EquipItemInsert(_IConobject); // 장비창에 아이콘 삽입
                    break;
                case ICONTYPE.ICON_CONSUMABLE: // 물약 등 소비아이템이라면 
                    m_pInventory.ConsumptionItemInsert(_IConobject); // 소비창에 아이콘 삽입 
                    break;
                case ICONTYPE.ICON_OTHER: // 기타 아아템이라면 
                    m_pInventory.OtherItemInsert(_IConobject); // 기타창에 아이콘 삽입
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