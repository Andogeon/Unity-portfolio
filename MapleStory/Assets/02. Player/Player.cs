using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PARTSELECT 
{
    BODY,
    FOOT,
    HAND,
    LOWERBODY
}

public enum PLAYERSTATE // 플레이어의 상태를 정하기 위한 열거형입니다.
{
    PLAYER_IDLE,
    PLAYER_RUN,
    PLAYER_DOWNJUMP,
    PLAYER_JUMP,
    PLAYER_JUMPATTACK,
    PLAYER_ATTACK,
    PLAYER_LOPE,
    PLAYER_HIT,
    PLAYER_DEAD
}

public enum LADDERMODE // 플레이어가 현재 사다리나 로프 어디에 위치했는지 정하기 위한 열거형입니다. 
{
    LADDER_NORMAL,
    LADDER_UP,
    LADDER_DOWN
}

// 실제 게임에서 사용되는 플레이어 클래스입니다.

public class Player : MonoBehaviour
{
    [SerializeField] private AudioClip _ClickSound = null;

    [SerializeField] private AudioClip _PotalSound = null;

    [SerializeField] private AudioClip _JumpSound = null;

    [SerializeField] private Consumption_Inventory m_pConsumption_Inventory = null;

    [SerializeField] private GameObject _DeadStone = null;

    [SerializeField] private GameObject _DeadUIBar = null;

    [SerializeField] private Playername _PlayerNamebar = null;

    [SerializeField] private GameObject _LevelUpEffect = null;

    [SerializeField] private GameObject _HitFontObject = null;

    [SerializeField] private BAR _StateBar = null;

    [SerializeField] private GameObject _Inventory = null;

    [SerializeField] private QuestWindow _QuestWindows = null;

    [SerializeField] private Equip _Equip = null;

    [SerializeField] private float _MovingSpeed = 0.0f;

    [SerializeField] private float _JumpPower = 0.0f;

    [SerializeField] private float _DownJumpPower = 0.0f;

    [SerializeField] private float _RayLength = 0.0f;

    [SerializeField] private float _Hp = 0.0f;

    [SerializeField] private LayerMask _LayMask = 0;

    [SerializeField] private Vector2 _Direction = Vector2.zero;

    [SerializeField] private Vector2 _JumpDirection = Vector2.zero;

    [SerializeField] private Vector3 _BoxSize = Vector3.zero;

    [SerializeField] private Vector3 _OffSetPosition = Vector3.zero;

    [SerializeField] private Vector3 _OffPosition = Vector3.zero;

    private List<GameObject> m_pGameObejctList = new List<GameObject>();

    private GameObject m_pCollisionObject = null;

    private GameobjectManager m_pGameObejctManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private Rigidbody2D m_pRigidBody = null;

    private BoxCollider2D m_pBoxCollision = null;

    private Animator m_pLevelUpEffectAnimator = null;

    private Body m_pBody = null;

    private QuestList m_pQuestList = null;

    private Potal m_pPotal = null;

    private FollowCamera m_pFollowCamara = null;

    private SceneManaged m_pSceneManaged = null;

    private Vector3 m_ResetPosition = Vector3.zero;

    private Vector3 m_vConnentCenter = Vector3.back;

    private Vector2 m_Direction = Vector2.zero;

    private float m_fMaxHP = 0.0f;

    private float m_fMaxExp = 10.0f;

    private float m_fExp = 0.0f;

    private float m_fAttack = 0.0f;

    private int m_iLevel = 1;

    private bool m_bIsLine = false;

    private bool m_bIsAttack = false;

    private bool m_bIsLadder = false;

    private bool m_bIsHit = false;

    private bool m_bIsPotalSound = false;

    private bool m_bIsTerrainLine = true;

    private bool m_bIsLevelUp = false;

    private string m_strSceneName = string.Empty;

    private PLAYERSTATE m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

    private LADDERMODE m_eLadderMode = LADDERMODE.LADDER_NORMAL;

    private PORTALTYPE m_eInsertPortalType = PORTALTYPE.POTAL_NORMAL;

    public PLAYERSTATE AccessPlayerState
    {
        get { return m_ePlayerState; }

        set { m_ePlayerState = value; }
    }

    public LADDERMODE AccessLadderState
    {
        get { return m_eLadderMode; }

        set { m_eLadderMode = value; }
    }

    public PORTALTYPE AccessPotal
    {
        get { return m_eInsertPortalType; }

        set { m_eInsertPortalType = value; }
    }

    public Vector3 AccessResetPosition
    {
        get { return m_ResetPosition; }

        set { m_ResetPosition = value; }
    }

    public bool AccessLadder
    {
        get { return m_bIsLadder; }

        set { m_bIsLadder = value; }
    }

    public bool AccessAttack
    {
        get { return m_bIsAttack; }

        set { m_bIsAttack = value; }
    }

    public bool AccessTerrainLine
    {
        get { return m_bIsTerrainLine; }

        set { m_bIsTerrainLine = value; }
    }

    public bool AccessPlayerLevelUp
    {
        get { return m_bIsLevelUp; }

        set { m_bIsLevelUp = false; }
    }

    public int GetLevel
    {
        get { return m_iLevel; }
    }

    public float AccessExp
    {
        get { return m_fExp; }

        set { m_fExp = value; }
    }

    public float AccessMaxExp
    {
        get { return m_fMaxExp; }
    }

    public float AccessPlayerHP
    {
        get { return _Hp; }

        set { _Hp = value; }
    }

    public float AccessPlayerMaxHP
    {
        get { return m_fMaxHP; }
    }

    public string AccessSceneName
    {
        get { return m_strSceneName; }

        set { m_strSceneName = value; }
    }

    public float AccessPlayerAttack
    {
        get { return m_fAttack; }

        set { m_fAttack = value; }
    }

    public SceneManaged AccessSceneManaged
    {
        get { return m_pSceneManaged; }

        set { m_pSceneManaged = value; }
    }

    private void Awake()
    {
        if (Application.targetFrameRate != 1000)
        {
            QualitySettings.vSyncCount = 0;

            Application.targetFrameRate = 1000;
        }
    }

    private void Start()
    {
        if (Application.targetFrameRate != 1000)
        {
            QualitySettings.vSyncCount = 0;

            Application.targetFrameRate = 1000;
        } // 플레이어의 몸체와 아이템의 스프라이트가 따로 놀게 되어 프레임 60에 제한을 풀어 진행했습니다.

        if (null != _Equip) 
        {
            if (_Equip.gameObject.activeSelf == false) // 장비창이 현재 켜져있다면 
                _Equip.gameObject.SetActive(true); // 장비창을 비활성화 

            _Equip.AccessPlayer = this;
        }

        _DeadUIBar.SetActive(false);

        m_pBody = transform.GetChild((int)PARTSELECT.BODY).GetComponent<Body>();

        m_pBoxCollision = transform.GetChild((int)PARTSELECT.FOOT).GetComponent<BoxCollider2D>();

        if (null == m_pBody)
            return;

        m_pRigidBody = GetComponent<Rigidbody2D>();

        InitItem();

        DontReleseObject();

        m_pFollowCamara = GameObject.Find("Main Camera").GetComponent<FollowCamera>();

        if (_QuestWindows != null)
        {
            m_pQuestList = _QuestWindows.transform.Find("Quest List").GetComponent<QuestList>();

            m_pQuestList.DisableStart();
        }

        m_fMaxHP = _Hp;

        m_pSoundManager.AddSound("Potal Sound", _PotalSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound("Jump", _JumpSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound("Click Sound", _ClickSound, SoundType.Sound_Effect);

        m_pGameObejctManager.OriginalGamgObjectInsert("Level Up Effect", _LevelUpEffect);

        m_pGameObejctManager.OriginalGamgObjectInsert("Hit Damege Font", _HitFontObject);

        m_pGameObejctManager.OriginalGamgObjectInsert("DEAD STONE", _DeadStone);
    }

    public void LateUpdate()
    {
        if(m_pPotal != null) // 포탈 이동시 포탈의 대한 정보가 존재한다면 
        {
            if (m_pPotal.AccessFaceObject.AccessFaceType != FADETYPE.FADE_IN) // 현재 페이드 인 상태가 아니라면 
                m_pPotal.AccessFaceObject.AccessFaceType = FADETYPE.FADE_IN; // 페이드 인 상태로 

            if (m_pPotal.AccessFaceObject.AccessOnFade == true) // 페이드 인이 완료가 되었다면 
            {
                m_pPotal.MoveScene(m_pGameObejctList); // 씬의 이동

                m_pPotal = null; // 연결과 포탈을 초기화

                m_bIsPotalSound = false; 
            }

            if (m_ePlayerState != PLAYERSTATE.PLAYER_IDLE)
                m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

            Idle();

            return;
        } // 포탈 이동시 페이드 인 적용 및 페이드 인이 완료가 되었다면 이전 씬이나 다음 씬으로 이동하게 했습니다.

        PlayerStateUpdate();

        AnimationUpdate();

        if(null != m_pQuestList)
            m_pQuestList.QuestListUpdate();

        KeyInput();
    }

    public void FixedUpdate() // 케릭터 이동시 떨림 현상 방지를 위해서 해당 함수를 사용했습니다.
    {
        if (m_pPotal != null)
            return;

        if (m_Direction != Vector2.zero)
        {
            m_pRigidBody.position += m_Direction * _MovingSpeed * Time.deltaTime;

            m_Direction = Vector2.zero;
        }
    }

    private void PlayerStateUpdate() // 플레이어의 상태를 갱신하는 함수입니다.
    {
        if(m_fMaxExp <= m_fExp)
        {
            m_iLevel += 1;

            m_fExp -= m_fMaxExp;

            m_fMaxExp += m_fMaxExp * 2.0f;

            m_fMaxHP = m_fMaxHP * 1.5f;

            m_fAttack += 2.5f;

            _Hp = m_fMaxHP;

            m_bIsLevelUp = true;

            _StateBar.AccessLevelObject.LevelUpdate(this);

            // 레벨업 이펙트를 오브젝트 풀링으로 생성 

            GameObject _Effect = m_pGameObejctManager.GameObejctPooling("Level Up Effect", Vector3.zero, Vector3.zero, Quaternion.identity);

            // 플레이어의 머리 위로 월드 좌표를 설정

            _Effect.transform.position = transform.position + Vector3.up;

            m_pLevelUpEffectAnimator = _Effect.GetComponent<Animator>();
        }

        if (m_pLevelUpEffectAnimator != null)
            ChakingAnimatior();

        if (null != _StateBar)
        {
            _StateBar.AccessHPBar = _Hp / m_fMaxHP;

            _StateBar.AccessEXPBar = m_fExp / m_fMaxExp;
        }

        if (_Hp <= 0.0f)
            m_ePlayerState = PLAYERSTATE.PLAYER_DEAD;
    }

    private void KeyInput() // 키 입력을 통하여 플레이어의 상태값을 변화 하는 함수입니다.
    {
        if (m_ePlayerState == PLAYERSTATE.PLAYER_DEAD) // 사망시 어떤 키 입력도 허용하지 않는다.
            return;

        PLAYERSTATE _ePlayerState = PLAYERSTATE.PLAYER_IDLE;

        Vector2 _Position = m_pRigidBody.position;

        if (Input.GetKeyDown(KeyCode.O)) 
            _Hp = 0.0f;

        if (Input.GetKeyDown(KeyCode.M))
            TeleportSceneEx(); // 암허스트 씬으로 강제 이동

        if (Input.GetKeyDown(KeyCode.P))
            TeleportScene(); // 선착장으로 강제 이동

        if (Input.GetKeyDown(KeyCode.I)) // 인벤토리 활성화 및 비활성화 
        {
            if (_Inventory.activeSelf == true)
                _Inventory.SetActive(false);
            else if (_Inventory.activeSelf == false)
                _Inventory.SetActive(true);

            m_pSoundManager.PlaySound("Click Sound");
        }

        if (Input.GetKeyDown(KeyCode.E)) // 장비창 활성화 및 비활성화 
        {
            if (_Equip.gameObject.activeSelf == true)
                _Equip.gameObject.SetActive(false);
            else if (_Equip.gameObject.activeSelf == false)
                _Equip.gameObject.SetActive(true);

            m_pSoundManager.PlaySound("Click Sound");
        }

        if (Input.GetKeyDown(KeyCode.Q)) // 퀘스트 창 활성화 및 비활성화 
        {
            if (_QuestWindows.gameObject.activeSelf == true)
                _QuestWindows.gameObject.SetActive(false);
            else if (_QuestWindows.gameObject.activeSelf == false)
                _QuestWindows.gameObject.SetActive(true);

            m_pSoundManager.PlaySound("Click Sound");
        }

        if (Input.GetKeyDown(KeyCode.Escape)) // 인벤토리, 장비창, 퀘스트창 모두 비활성화
        {
            if (_Equip.gameObject.activeSelf == true)
                _Equip.gameObject.SetActive(false);
            if (_Inventory.activeSelf == true)
                _Inventory.SetActive(false);
            if(_QuestWindows.gameObject.activeSelf == true)
                _QuestWindows.gameObject.SetActive(false);

            m_pSoundManager.PlaySound("Click Sound");
        }

        if (Input.GetKeyDown(KeyCode.S)) // 물약 복용
            m_pConsumption_Inventory.UsePostion(this);

        if (m_ePlayerState != PLAYERSTATE.PLAYER_ATTACK) // 플레이어가 공격중이 아닐시 
        {
            if (m_bIsLadder) // 현재 사다리나 로프를 타고 있다면 
            {
                // 사다리를 타고 있을 때 사다리의 상태를 2가지를 주었습니다.

                // 그 이유는 사다리를 완전히 올라가 지형착지에는 큰 문제는 없었으나 

                // 사다리에서 내려오는 도중 지형착지를 못하고 계속 내려오는 문제가 있었기에 

                // OnTriggerStay2D 함수에서 현재 플레이어가 사다리의 중심으로부터 어디있는지 조사 후 

                // 사다리 중심축 아래 있다면 충돌 박스를 만들어 라인과 충돌시 대기 상태로 변경하게 만들었습니다.

                if (m_eLadderMode == LADDERMODE.LADDER_DOWN && m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                {
                    // 플레이어가 사다리 중심축 보다 아래 있고 현재 플레이어의 상태가 사다리를 타고 있는 상황이라면 

                    Collider2D _Collision = Physics2D.OverlapBox(transform.position + _OffPosition, _BoxSize, 0.0f, _LayMask);

                    // 충돌 박스를 만들어 충돌 검사한다.

                    if (null != _Collision) // 충돌이 되었다면 
                    {
                        m_bIsLadder = false;

                        m_bIsTerrainLine = true;

                        m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

                        m_pBoxCollision.enabled = true;

                        // 플레이어의 상태를 대기상태로 변환한다.

                        return;
                    }
                }

                // 사다리 충돌하며 플레이어의 위치가 사다리 충돌 박스의 아래있을 시 위로 이동

                if (Input.GetKey(KeyCode.UpArrow) && m_eLadderMode == LADDERMODE.LADDER_DOWN) 
                {
                    if (m_pBoxCollision.enabled == true) // 플레이어 체력 감소하는 충돌 박스를 활성화 되어 있다면
                        m_pBoxCollision.enabled = false; // 비활성화

                    _Position.x = m_pCollisionObject.transform.position.x;

                    m_pRigidBody.position = _Position + Vector2.up * 3.0f * Time.deltaTime;

                    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                    m_bIsTerrainLine = false; // 지형 박스와의 충돌을 해재 

                    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                    return;
                }

                // 사다리 충돌하며 플레이어의 위치가 사다리 충돌 박스의 위에 있을 시 아래로 이동

                if (Input.GetKey(KeyCode.DownArrow) && m_eLadderMode == LADDERMODE.LADDER_UP)
                {
                    if (m_pBoxCollision.enabled == true)
                        m_pBoxCollision.enabled = false;

                    _Position.x = m_pCollisionObject.transform.position.x;

                    m_pRigidBody.position = _Position + Vector2.up * -1.0f * 3.0f * Time.deltaTime;

                    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                    m_bIsTerrainLine = false;

                    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                    return;
                }

                // 사다리를 타고 있을 시 위로 이동

                if (Input.GetKey(KeyCode.UpArrow) && m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                {
                    _Position.x = m_pCollisionObject.transform.position.x;

                    m_pRigidBody.position = _Position + Vector2.up * 3.0f * Time.deltaTime;

                    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                    return;
                }
                
                // 사다리를 타고 있을 시 아래로 이동

                if (Input.GetKey(KeyCode.DownArrow) && m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                {
                    _Position.x = m_pCollisionObject.transform.position.x;

                    m_pRigidBody.position = _Position + Vector2.up * -1.0f * 3.0f * Time.deltaTime;

                    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                    return;
                }

                // 사다리를 타는 도중 왼쪽으로 점프 

                if (Input.GetKey(KeyCode.LeftArrow)) 
                {
                    if (Input.GetKey(KeyCode.Space) && m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                    {
                        m_bIsLadder = false;
                        
                        m_pRigidBody.AddForce((Vector2.up * _DownJumpPower), ForceMode2D.Impulse);

                        m_pBoxCollision.enabled = false;

                        m_ePlayerState = PLAYERSTATE.PLAYER_JUMP;

                        return;
                    }
                }

                // 사다리를 타는 도중 오른쪽으로 점프 

                if (Input.GetKey(KeyCode.RightArrow))
                {
                    if (Input.GetKey(KeyCode.Space) && m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                    {
                        m_bIsLadder = false;

                        m_pRigidBody.AddForce((Vector2.up * _DownJumpPower), ForceMode2D.Impulse);

                        m_pBoxCollision.enabled = false;

                        m_ePlayerState = PLAYERSTATE.PLAYER_JUMP;

                        return;
                    }
                }

                if(m_ePlayerState == PLAYERSTATE.PLAYER_LOPE) // 현재 플레이어가 사다리를 타고 있다면 
                {
                    m_pBody.GetAvatarState = AVATARSTATES.AVATAR_LADDER;

                    // 몸체에 애니매이션을 사다리 타고 있는 애니메이션으로 변환
                    
                    return;
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow) && m_ePlayerState != PLAYERSTATE.PLAYER_LOPE)
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

                m_Direction = Vector2.left;

                _ePlayerState = PLAYERSTATE.PLAYER_RUN;
            }

            if (Input.GetKey(KeyCode.RightArrow) && m_ePlayerState != PLAYERSTATE.PLAYER_LOPE)
            {
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

                m_Direction = Vector2.right;

                _ePlayerState = PLAYERSTATE.PLAYER_RUN;
            }
        }

        // 밑으로 떨어지는 점프

        if (Input.GetKey(KeyCode.DownArrow) && m_bIsLine == true)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _ePlayerState = PLAYERSTATE.PLAYER_DOWNJUMP;

                m_pBoxCollision.enabled = false;

                m_ePlayerState = _ePlayerState;

                return;
            }
        }

        // 점프

        if (Input.GetKeyDown(KeyCode.Space) && m_ePlayerState != PLAYERSTATE.PLAYER_JUMP && m_ePlayerState != PLAYERSTATE.PLAYER_HIT)
        {
            if (m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                return;

            m_pRigidBody.AddForce(Vector2.up * _JumpPower, ForceMode2D.Impulse);

            m_pBoxCollision.enabled = false;

            m_ePlayerState = PLAYERSTATE.PLAYER_JUMP;

            m_pSoundManager.PlaySound("Jump");

            return;
        }

        // 공격

        if (Input.GetKey(KeyCode.Z) && m_ePlayerState != PLAYERSTATE.PLAYER_HIT && m_bIsAttack == false)
        {
            _ePlayerState = PLAYERSTATE.PLAYER_ATTACK;

            if (m_ePlayerState == PLAYERSTATE.PLAYER_JUMP)
            {
                m_pBoxCollision.enabled = true;

                m_ePlayerState = PLAYERSTATE.PLAYER_JUMPATTACK;

                return;
            }
        }

        // 최종 키 입력에서 결정된 상태를 실제 적용될 상태로 적용

        if ((int)m_ePlayerState < (int)PLAYERSTATE.PLAYER_DOWNJUMP)
            m_ePlayerState = _ePlayerState;
    }

    private void AnimationUpdate() // 플레이어의 애니메이션 변환을 결정하는 함수입니다.
    {
        switch (m_ePlayerState)
        {
            case PLAYERSTATE.PLAYER_IDLE:
                Idle();
                break;
            case PLAYERSTATE.PLAYER_RUN:
                Run();
                break;
            case PLAYERSTATE.PLAYER_JUMP:
                Jump();
                break;
            case PLAYERSTATE.PLAYER_DOWNJUMP:
                DownJump();
                break;
            case PLAYERSTATE.PLAYER_ATTACK:
                Attack();
                break;
            case PLAYERSTATE.PLAYER_JUMPATTACK:
                JumpAttack();
                break;
            case PLAYERSTATE.PLAYER_LOPE:
                Lope();
                break;
            case PLAYERSTATE.PLAYER_HIT:
                Hit();
                break;
            case PLAYERSTATE.PLAYER_DEAD:
                Dead();
                break;
        }
    }

    private void Idle() // 플레이어의 대기 상태에서 호출하는 함수입니다.
    {
        m_bIsAttack = false;

        m_pBody.GetAvatarState = AVATARSTATES.AVATAR_IDLE;

        Ray2D _Ray2D = new Ray2D(transform.position + (Vector3)_JumpDirection, Vector2.down);

        RaycastHit2D Hit2D = Physics2D.Raycast(_Ray2D.origin + _Direction, _Ray2D.direction, _RayLength, _LayMask);

        if (Hit2D.collider != null)
            m_bIsLine = true;
        else
            m_bIsLine = false;

        if (m_pBoxCollision.enabled == false)
            m_pBoxCollision.enabled = true;
    }

    private void Jump() // 점프 상태에서 호출하는 함수입니다.
    {
        if (m_bIsAttack == true)
            m_bIsAttack = false;

        if (m_pRigidBody.gravityScale == 0.0f)
            m_pRigidBody.gravityScale = 4.0f;

        m_pBody.GetAvatarState = AVATARSTATES.AVATAR_JUMP;

        if (m_pRigidBody.velocity.y > 0)
            return;

        Collider2D _Collision = Physics2D.OverlapBox(transform.position + _OffSetPosition, _BoxSize, 0.0f, _LayMask);

        m_pBoxCollision.enabled = true;

        if (_Collision != null)
        {
            if (_Collision.gameObject.tag == "Jump Stand") // 점프대에 충돌시 
            {
                m_pBoxCollision.enabled = false;

                m_pRigidBody.AddForce(Vector2.up * _JumpPower * 6.0f, ForceMode2D.Impulse); // 방향백터의 힘을 점프보다 더 쎄게 준다 

                return;
            }

            m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;
        }
    }

    private void DownJump() // 하단 점프 할 시 호출하는 함수입니다.
    {
        m_pBody.GetAvatarState = AVATARSTATES.AVATAR_JUMP;

        Vector3 _RayPosition = new Vector3(transform.position.x, transform.position.y - 1.0f);

        Ray2D _Ray2D = new Ray2D(_RayPosition, Vector2.down);

        RaycastHit2D _Hit2D;

        if (_Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 3.0f, _LayMask))
        {
            if (_Hit2D.distance < 0.7f) // 특정 길이보다 적을 시 대기 상태로 돌입
            {
                m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

                m_pBoxCollision.enabled = true;

                m_bIsAttack = false;

                return;
            }
        }
    }

    private void JumpAttack() // 점프 공격시 호출하는 함수입니다.
    {
        if (m_bIsAttack == true)
        {
            if (m_pRigidBody.gravityScale == 0.0f) 
                m_pRigidBody.gravityScale = 4.0f; // 점프를 위한 중력을 다시 설정

            if (m_pRigidBody.velocity.y > 0)
                return;

            Collider2D _Collision = Physics2D.OverlapBox(transform.position + _OffSetPosition, _BoxSize, 0.0f, _LayMask);

            m_pBoxCollision.enabled = true;

            if (_Collision != null) // 충돌 시 
            {
                if (_Collision.gameObject.tag == "Jump Stand") // 그러나 점프대가 존재한다면 
                {
                    m_pBoxCollision.enabled = false;

                    m_pRigidBody.AddForce(Vector2.up * _JumpPower * 6.0f, ForceMode2D.Impulse); // 더욱 강하게 점프 

                    return;
                } // 점프대가 존재할시 더욱 높게 점프하게 했습니다.

                m_ePlayerState = PLAYERSTATE.PLAYER_IDLE; // 대기 상태로 돌입 

                m_bIsAttack = false;
            } // 지형과 충돌시 대기 상태로 돌입

            return;
        }

        int _SelectAttack = Random.Range(0, 3);

        switch (_SelectAttack)
        {
            case 0:
                m_pBody.GetAvatarState = AVATARSTATES.AVATAR_FIRSTNORMALATTACK;
                break;
            case 1:
                m_pBody.GetAvatarState = AVATARSTATES.AVATAR_SECONDNORMALATTACK;
                break;
            case 2:
                m_pBody.GetAvatarState = AVATARSTATES.AVATAR_THIRDNORMALATTACK;
                break;
        }

        m_bIsAttack = true;
    }

    private void Run() // 이동 시 호출하는 함수입니다.
    {
        Ray2D _Ray2D = new Ray2D(transform.position, Vector2.down);

        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.5f, _LayMask);

        if (_Hit2D.collider == null)
        {
            m_ePlayerState = PLAYERSTATE.PLAYER_JUMP;

            return;
        }

        m_pBody.GetAvatarState = AVATARSTATES.AVATAR_RUN;
    }

    private void Attack() // 공격 시 호출하는 함수입니다.
    {
        int _SelectAttack = Random.Range(0, 3);

        switch (_SelectAttack)
        {
            case 0:
                m_pBody.GetAvatarState = AVATARSTATES.AVATAR_FIRSTNORMALATTACK;
                break;
            case 1:
                m_pBody.GetAvatarState = AVATARSTATES.AVATAR_SECONDNORMALATTACK;
                break;
            case 2:
                m_pBody.GetAvatarState = AVATARSTATES.AVATAR_THIRDNORMALATTACK;
                break;
        }

        m_bIsAttack = true;
    }

    private void Lope() // 사다리나 로프를 탈 때 호출하는 함수입니다.
    {
        if (m_pRigidBody.gravityScale != 0.0f)
            InitRigidBody();

        if (m_pBoxCollision.enabled == true) // 사다리 타고 있을 떄 피격 박스가 
            m_pBoxCollision.enabled = false;

        if((int)m_pBody.GetAvatarState < (int)AVATARSTATES.AVATAR_LADDER)
            m_pBody.GetAvatarState = AVATARSTATES.AVATAR_LADDER;
        else
            m_pBody.GetAvatarState = AVATARSTATES.AVATAR_LADDERRUN;
    }

    private void Hit() // 피격시 호출하는 함수입니다.
    {
        if (m_pRigidBody.gravityScale == 0.0f)
            m_pRigidBody.gravityScale = 4.0f;

        if(m_pBody.GetAvatarState != AVATARSTATES.AVATAR_HIT)
            m_pBody.GetAvatarState = AVATARSTATES.AVATAR_HIT;

        if (false == m_bIsHit)
        {
            if(m_pRigidBody.velocity.y <= 0)
                m_pRigidBody.AddForce(Vector2.up * 12.0f, ForceMode2D.Impulse);

            m_bIsHit = true;
        }
        else
            m_pRigidBody.position += (Vector2)transform.right * 2.0f * Time.deltaTime;

        if (m_pRigidBody.velocity.y > 0)
            return;

        Ray2D _Ray2D = new Ray2D(transform.position, Vector2.down);

        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 2.0f, _LayMask);

        m_pBoxCollision.enabled = true;

        if (_Hit2D.collider != null)
        {
            if (_Hit2D.distance < 1.0f)
            {
                if (_Hit2D.collider.gameObject.tag == "Jump Stand")
                {
                    m_pBoxCollision.enabled = false;

                    m_pRigidBody.AddForce(Vector2.up * _JumpPower * 6.0f, ForceMode2D.Impulse);

                    return;
                }
            }

            if(_Hit2D.distance < 0.8f)
            {
                m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

                m_bIsAttack = m_bIsHit = false;
            }
        }
    }

    public void Dead() // 사망시 호출 하는 함수입니다.
    {
        if (m_pBody.GetAvatarState == AVATARSTATES.AVATAR_DEAD)
            return;       
            
        m_bIsLadder = false;

        m_pRigidBody.gravityScale = 4.0f;

        m_pCollisionObject = null;

        m_eLadderMode = LADDERMODE.LADDER_NORMAL;

        if (m_pBoxCollision.enabled == false) // 발판 충돌체가 비활성화 되어 있다면 
            m_pBoxCollision.enabled = true; // 활성화

        m_pBody.GetAvatarState = AVATARSTATES.AVATAR_DEAD;       

        // 사망 비석을 생성 
        m_pGameObejctManager.GameObejctPooling("DEAD STONE", Vector3.zero, transform.position + Vector3.up * 5.0f, Quaternion.identity);

        // 사망시 부활 UI 활성화 
        _DeadUIBar.SetActive(true);

        m_eInsertPortalType = PORTALTYPE.POTAL_NORMAL;
    }

    private void InitItem() // 커스터마이징 캐릭터의 데이터를 플레이어 데이터로 이동하는 함수입니다.
    {
        GameObject _AvaterObject = GameObject.Find("Select Avatar"); // 커스터마이징한 캐릭터를 탐색 

        if (null == _AvaterObject)
            return; // 존재 하지 않는 다면 종료

        PART[] _PlayerPart = GetComponentsInChildren<PART>(); // 현재 플레이어 모든 부위의 클래스를 받아옴

        PART[] _ChangePart = _AvaterObject.GetComponentsInChildren<PART>(); // 커스터마이징한 캐릭터 모든 부위의 클래스를 받아옴

        ITEM[] _InstanceItems = new ITEM[_Equip.AccessSlot.Length]; // 커스터마이징한 케릭터가 착용한 아이템을 장비창에 개수와 할당

        PART[] _NewParts = new PART[_PlayerPart.Length]; 

        int _iPartIndex = 0;

        int _iItemIndex = 0;

        // 아이템과 파츠를 분리하는 작업

        for (int i = 0; i < _ChangePart.Length; ++i) // 모든 파츠를 순회하면서 
        {
            if (_ChangePart[i] is ITEM && _ChangePart[i].tag != "Arm")
            {
                _InstanceItems[_iItemIndex++] = _ChangePart[i] as ITEM; // 아이템 삽입

                continue;
            }
            else if(_ChangePart[i] is ITEM == false)
                _NewParts[_iPartIndex++] = _ChangePart[i]; // 파츠 삽입
        }

        for (int i = 0; i < _Equip.AccessSlot.Length; ++i) // 장비 슬롯만큼 
        {
            for (int j = 0; j < _InstanceItems.Length; ++j) // 아이템을 순회하면서 
            {
                if (_Equip.AccessSlot[i].AccessSlotType == SlotTpye.Slot_Weapon && _InstanceItems[j].tag == "Weapon")
                    InsertEquip(_InstanceItems[j], i); // 해당 장비 슬롯이 무기 타입이고 파츠가 무기일 경우 아이템을 무기로 삽입 
                else if (_Equip.AccessSlot[i].AccessSlotType == SlotTpye.Slot_Clothes && _InstanceItems[j].tag == "Clothes")
                    InsertEquip(_InstanceItems[j], i); // 해당 장비 슬롯이 바지 타입이고 파츠가 바지일 경우 아이템을 바지로 삽입
                else if (_Equip.AccessSlot[i].AccessSlotType == SlotTpye.Slot_Parts && _InstanceItems[j].tag == "Parts")
                    InsertEquip(_InstanceItems[j], i); // 해당 장비 슬롯이 상의 타입이고 파츠가 바지일 경우 아이템을 상의로 삽입
                else if (_Equip.AccessSlot[i].AccessSlotType == SlotTpye.Slot_Foot && _InstanceItems[j].tag == "foot")
                    InsertEquip(_InstanceItems[j], i); // 해당 장비 슬롯이 신발 타입이고 파츠가 신발일 경우 아이템을 신발로 삽입
            }
        }

        for (int i = 0; i < _PlayerPart.Length; ++i) // 플레이어의 파츠
        {
            for (int j = 0; j < _NewParts.Length; ++j) // 커스터마이징에서 분리 된 파츠
            {
                if (_PlayerPart[i].name == _NewParts[j].name)
                {
                    if (_NewParts[j] is Body) // 파츠가 몸체일 경우 
                    {
                        Body _Body = _NewParts[j] as Body;

                        if (m_pBody.AccessPlayerName == string.Empty)
                            _PlayerNamebar.AccessText.text = _Body.AccessPlayerName; // 커스터마이징에서 지정한 이름을 플레이어의 닉네임으로 변경 

                        _Body.GetAvatarState = AVATARSTATES.AVATAR_IDLE;
                    }

                    if(_PlayerPart[i] is Hair) // 파츠가 헤어일 경우
                    {
                        if (_NewParts[j] is Hair)
                        {
                            Hair _Hair = _NewParts[j] as Hair;

                            Hair _NewPlayerHair = _PlayerPart[i] as Hair;

                            _NewPlayerHair.AccessHair = _Hair.AccessHair;
                        }
                    }

                    _PlayerPart[i].transform.localPosition = _NewParts[j].transform.localPosition; // 각 파츠별로 헤어, 손, 머리, 발의 위치값을 지정

                    _PlayerPart[i].SetSprite(_NewParts[j].GetSprite()); // 각 나머지 커스터 마이징에서 정한 스프라이트를 플레이어의 스프라이트로 지정

                    break;
                }
            }
        }
        
        _Equip.gameObject.SetActive(false); // 장비창을 비활성화 

        Destroy(_AvaterObject); // 기존 커스터마이징한 케릭터를 삭제
    }


    // 최초 커스터마이징 케릭터에서 정한 아이템들을 플레이어로 장비창으로 등록하는 함수입니다.
    private void InsertEquip(ITEM _InsertITEM, int _Index) 
    {
        GameObject _InstanceIcon = GameObject.Instantiate(_InsertITEM.AccessIcon.gameObject);

        GameObject _InstanceItem = GameObject.Instantiate(_InsertITEM.AccessIcon.OriginalItem.gameObject);

        ICON _ICons = _InstanceIcon.GetComponent<ICON>();

        ITEM _Item = _InstanceItem.GetComponent<ITEM>();

        _ICons.AccessItem = _Item;

        _ICons.AccessOrlGameObejct = _InstanceItem;

        _ICons.AccessItem.AccessItemModeType = ITEMMODETYPE.ITEM_EQUIP;

        _Equip.AccessSlot[_Index].AccessICon = _ICons;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Line"))
        {
            if (m_bIsTerrainLine == true)
            {
                m_bIsLadder = false;

                m_pCollisionObject = null;

                if (m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                    m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

                m_pRigidBody.gravityScale = 4.0f;
            }
        }

        // 모든 오브젝트의 충돌 시 공격력 표시가 나오는 피격 폰트 이펙트가 등장하여 

        // 오로지 몬스터 계열에만 피격 폰트 이펙트를 생성 

        else if (collision.gameObject.layer == LayerMask.NameToLayer("Monter") && m_ePlayerState != PLAYERSTATE.PLAYER_HIT
            && collision.gameObject.name != "Maple Box" && collision.gameObject.name != "Ticket Box")
        {
            MONTER _Monter = collision.gameObject.GetComponent<MONTER>();

            GameObject _HitObject = m_pGameObejctManager.GameObejctPooling("Hit Damege Font", Vector3.zero, Vector3.zero, Quaternion.identity);

            HitFont _Font = _HitObject.GetComponent<HitFont>();

            _Font.SelectNumberFont((int)_Monter.AccessAttack);

            _Font.transform.position = transform.position;

            _Hp -= _Monter.AccessAttack;

            m_ePlayerState = PLAYERSTATE.PLAYER_HIT;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Connect")) // 충돌되는 레이어의 값이 사다리일 때 
        {
            m_bIsLadder = true; // 사다리와 충돌 

            m_pCollisionObject = collision.gameObject; // 사다리의 게임오브젝트

            Vector2 _Position = collision.bounds.center; // 사다리의 충돌 박스의 중심축

            m_vConnentCenter = collision.bounds.min + Vector3.up;

            // 현재 위치 값 - 사다리의 중심축 위치 = 백터의 정규화를 통해서 방향 백터 얻음

            Vector2 _Direction = ((Vector2)transform.position - _Position).normalized;

            // 얻어온 방향백터를 통해서 현재 플레이어가 사다리 중심축으로 부터 아래에 있는지 위에 있는지 조사했습니다.

            if (m_eLadderMode == LADDERMODE.LADDER_NORMAL)
            {
                if (_Direction.y <= 0.0f)
                    m_eLadderMode = LADDERMODE.LADDER_DOWN; // 사다리 중심축으로 부터 아래에 있다.
                else
                    m_eLadderMode = LADDERMODE.LADDER_UP; // 사다리의 중심축으로 부터 위에 있다.
            }
        }

        if (collision.gameObject.name == "Portal") // 충돌 되고 있는 게임오브젝트가 씬 포탈이라면 
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (m_bIsPotalSound == false) // 포탈 사운드 중복 재생을 막기 위한 분기문
                {
                    m_pSoundManager.PlaySound("Potal Sound"); 

                    m_bIsPotalSound = true;
                }

                Potal _CollisionPotal = collision.gameObject.GetComponent<Potal>();

                if (null == _CollisionPotal)
                    return;

                m_pPotal = _CollisionPotal;

                m_eInsertPortalType = _CollisionPotal.GetPortalType;

                _CollisionPotal.AccessFaceObject.AccessFaceType = FADETYPE.FADE_IN;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // 사다리나 로프의 충돌이 벗어날시 다시 초기화 하기 위한 함수입니다.

        if (collision.gameObject.layer == LayerMask.NameToLayer("Connect"))
        {
            if (m_bIsLadder == false)
                return;
            
            m_bIsLadder = false;

            if(m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

            if (m_eLadderMode != LADDERMODE.LADDER_NORMAL)
                m_eLadderMode = LADDERMODE.LADDER_NORMAL;

            m_pRigidBody.gravityScale = 4.0f;

            m_bIsTerrainLine = true;
        }
    }

    private void InitRigidBody() // 사다리 및 로프를 탈 때 중력의 영향을 받게 되면 떨어져 중력 및 물리적 제어를 0으로 지정하는 함수입니다.
    {
        if (null == m_pRigidBody)
            return;

        m_pRigidBody.gravityScale = 0.0f;

        m_pRigidBody.velocity = Vector2.zero;
    }

    private void ChakingAnimatior() // 레벨업 이펙트 종료시 이펙트를 다시 오브젝트 풀링 객체로 삽입하는 함수입니다.
    {
        if (null == m_pLevelUpEffectAnimator)
            return;

        if(m_pLevelUpEffectAnimator.GetCurrentAnimatorStateInfo(0).IsName("LEVEL UP EFFECT") && m_pLevelUpEffectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            m_pGameObejctManager.Remove("Level Up Effect", m_pLevelUpEffectAnimator.gameObject);

            m_pLevelUpEffectAnimator = null;
        }
    }

    private void TeleportScene() // 실 게임에서 디버그용도로 선착장 씬으로 이동하는 함수입니다.
    {
        for (int i = 0; i < m_pGameObejctList.Count; ++i)
            DontDestroyOnLoad(m_pGameObejctList[i]);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Marina");

        transform.position = new Vector3(52.66f, 9.47f, 0.0f);       
    }

    private void TeleportSceneEx() // 실 게임에서 디버그용도로 암허스트 씬으로 이동하는 함수입니다.
    {
        for (int i = 0; i < m_pGameObejctList.Count; ++i)
            DontDestroyOnLoad(m_pGameObejctList[i]);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Amherst");

        transform.position = new Vector3(32.89f, 10.1f, 0.0f);
    }

    public void CilckInventory() // 버튼으로 인벤토리 활성화 및 비활성화 하는 함수입니다.
    {
        if (null == _Inventory)
            return;

        if (_Inventory.activeSelf == true)
            _Inventory.SetActive(false);
        else if (_Inventory.activeSelf == false)
            _Inventory.SetActive(true);

        m_pSoundManager.PlaySound("Click Sound");
    }

    public void CilckEquip() // 버튼으로 장비창 활성화 및 비활성화 하는 함수입니다.
    {
        if (null == _Equip)
            return;

        if (_Equip.gameObject.activeSelf == true)
            _Equip.gameObject.SetActive(false);
        else if (_Equip.gameObject.activeSelf == false)
            _Equip.gameObject.SetActive(true);

        m_pSoundManager.PlaySound("Click Sound");
    }

    public void CilckQuestWindow() // 버튼으로 퀘스트창 활성화 및 비활성화 하는 함수입니다.
    {
        if (null == _QuestWindows)
            return;

        if (_QuestWindows.gameObject.activeSelf == true)
            _QuestWindows.gameObject.SetActive(false);
        else if (_QuestWindows.gameObject.activeSelf == false)
            _QuestWindows.gameObject.SetActive(true);

        m_pSoundManager.PlaySound("Click Sound");
    }

    public void OnDestroy()
    {
        m_pCollisionObject = null;

        m_pRigidBody = null;

        m_pBoxCollision = null;

        m_pBody = null;

        for (int i = 0; i < m_pGameObejctList.Count; ++i)
            m_pGameObejctList[i] = null;

        m_pGameObejctList.Clear();

        m_pGameObejctList = null;

        m_pGameObejctManager = null;

        m_pSceneManaged = null;
    }

    private void DontReleseObject() // 씬 전환시 소멸 되서는 안되는 오브젝트들을 모아주는 함수입니다.
    {
        GameObject _StateBarGameObject = GameObject.Find("State Bar");

        GameObject _ChildObject = _StateBarGameObject.transform.Find("Back Ground").gameObject;

        _ChildObject = _ChildObject.transform.Find("Level Ground").gameObject;

        _ChildObject = _ChildObject.transform.Find("Player Name").gameObject;

        UnityEngine.UI.Text _Text = _ChildObject.GetComponent<UnityEngine.UI.Text>();

        _Text.text = _PlayerNamebar.AccessText.text;

        m_pGameObejctList.Add(GameObject.Find("Inventory UI"));

        m_pGameObejctList.Add(GameObject.Find("Quest UI"));

        m_pGameObejctList.Add(gameObject);

        m_pGameObejctList.Add(_StateBarGameObject);

        m_pGameObejctList.Add(_PlayerNamebar.transform.parent.gameObject);
    }
}