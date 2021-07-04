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

public enum PLAYERSTATE
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

public enum LADDERMODE
{
    LADDER_NORMAL,
    LADDER_UP,
    LADDER_DOWN
}

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

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(transform.position + _OffPosition, _BoxSize);

        //Gizmos.DrawWireCube(transform.position + _OffSetPosition, _BoxSize);
    }

    private void Start()
    {
        if (null != _Equip)
        {
            if (_Equip.gameObject.activeSelf == false)
                _Equip.gameObject.SetActive(true);

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

        // 시작시 위치가 자기 멋대로 이동하는 버그 있음 그런데 왜 그런지 알 수 없음 ..

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

    public void Update()
    {
        //Debug.Log(m_eLadderMode);

        if(m_pPotal != null)
        {
            if (m_pPotal.AccessFaceObject.AccessFaceType != FADETYPE.FADE_IN)
                m_pPotal.AccessFaceObject.AccessFaceType = FADETYPE.FADE_IN;

            if (m_pPotal.AccessFaceObject.AccessOnFade == true)
            {
                m_pPotal.MoveScene(m_pGameObejctList);

                m_pPotal = null;

                m_bIsPotalSound = false;
            }

            if (m_ePlayerState != PLAYERSTATE.PLAYER_IDLE)
                m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

            Idle();

            return;
        }

        PlayerStateUpdate();

        AnimationUpdate();

        if(null != m_pQuestList)
            m_pQuestList.QuestListUpdate();
    }

    public void LateUpdate()
    {
        if (m_pPotal != null)
            return;

        KeyInput();
    }

    private void PlayerStateUpdate()
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

            GameObject _Effect = m_pGameObejctManager.GameObejctPooling("Level Up Effect", Vector3.zero, Vector3.zero, Quaternion.identity);

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

    private void KeyInput()
    {
        if (m_ePlayerState == PLAYERSTATE.PLAYER_DEAD)
            return;

        PLAYERSTATE _ePlayerState = PLAYERSTATE.PLAYER_IDLE;

        Vector2 _Position = m_pRigidBody.position;

        //if (Input.GetKeyDown(KeyCode.O))
        //    m_fExp = m_fMaxExp;

        if (Input.GetKeyDown(KeyCode.O))
            _Hp = 0.0f;

        if (Input.GetKeyDown(KeyCode.P))
            TeleportScene();

        if (Input.GetKeyDown(KeyCode.I))
        {
            if (_Inventory.activeSelf == true)
                _Inventory.SetActive(false);
            else if (_Inventory.activeSelf == false)
                _Inventory.SetActive(true);

            m_pSoundManager.PlaySound("Click Sound");
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (_Equip.gameObject.activeSelf == true)
                _Equip.gameObject.SetActive(false);
            else if (_Equip.gameObject.activeSelf == false)
                _Equip.gameObject.SetActive(true);

            m_pSoundManager.PlaySound("Click Sound");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (_QuestWindows.gameObject.activeSelf == true)
                _QuestWindows.gameObject.SetActive(false);
            else if (_QuestWindows.gameObject.activeSelf == false)
                _QuestWindows.gameObject.SetActive(true);

            m_pSoundManager.PlaySound("Click Sound");
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (_Equip.gameObject.activeSelf == true)
                _Equip.gameObject.SetActive(false);
            if (_Inventory.activeSelf == true)
                _Inventory.SetActive(false);
            if(_QuestWindows.gameObject.activeSelf == true)
                _QuestWindows.gameObject.SetActive(false);

            m_pSoundManager.PlaySound("Click Sound");
        }

        if (Input.GetKeyDown(KeyCode.S))
            m_pConsumption_Inventory.UsePostion(this);

        if (m_ePlayerState != PLAYERSTATE.PLAYER_ATTACK)
        {
            if (m_bIsLadder)
            {
                if (m_eLadderMode == LADDERMODE.LADDER_DOWN && m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)  
                {
                    //Collider2D _Collision = Physics2D.OverlapBox(transform.position + _OffSetPosition, _BoxSize, 0.0f, _LayMask);

                    Collider2D _Collision = Physics2D.OverlapBox(transform.position + _OffPosition, _BoxSize, 0.0f, _LayMask);

                    // 여기서 손봐볼것

                    if (null != _Collision)
                    {
                        m_bIsLadder = false;

                        m_bIsTerrainLine = true;

                        m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

                        m_pBoxCollision.enabled = true;

                        return;
                    }
                }

                

                //if (Input.GetKey(KeyCode.UpArrow) && m_eLadderMode == LADDERMODE.LADDER_DOWN) // ??111111111111111111111111111111
                //{
                //    //if (m_pBoxCollision.enabled == true)
                //    //    m_pBoxCollision.enabled = false;

                //    _Position.x = m_pCollisionObject.transform.position.x;

                //    m_pRigidBody.position = _Position + Vector2.up * 3.0f * Time.deltaTime;

                //    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                //    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                //    m_bIsTerrainLine = false;

                //    return;
                //}

                //if (Input.GetKey(KeyCode.DownArrow)/* && m_eLadderMode == LADDERMODE.LADDER_UP*/)
                //{
                //    _Position.x = m_pCollisionObject.transform.position.x;

                //    m_pRigidBody.position = _Position + Vector2.up * -1.0f * 3.0f * Time.deltaTime;

                //    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                //    //m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                //    m_bIsTerrainLine = false;

                //    return;
                //}

                if (Input.GetKey(KeyCode.UpArrow) && m_eLadderMode == LADDERMODE.LADDER_DOWN) // ??111111111111111111111111111111
                {
                    if (m_pBoxCollision.enabled == true)
                        m_pBoxCollision.enabled = false;

                    _Position.x = m_pCollisionObject.transform.position.x;

                    m_pRigidBody.position = m_vConnentCenter;

                    m_pRigidBody.position = _Position + Vector2.up * 3.0f * Time.deltaTime;

                    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                    m_bIsTerrainLine = false;

                    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                    return;
                }

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

                if (Input.GetKey(KeyCode.UpArrow) && m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                {
                    _Position.x = m_pCollisionObject.transform.position.x;

                    m_pRigidBody.position = _Position + Vector2.up * 3.0f * Time.deltaTime;

                    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                    return;
                }
                if (Input.GetKey(KeyCode.DownArrow) && m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                {
                    _Position.x = m_pCollisionObject.transform.position.x;

                    m_pRigidBody.position = _Position + Vector2.up * -1.0f * 3.0f * Time.deltaTime;

                    m_ePlayerState = PLAYERSTATE.PLAYER_LOPE;

                    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

                    return;
                }

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

                if(m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                {
                    m_pBody.GetAvatarState = AVATARSTATES.AVATAR_LADDER;

                    return;
                }
            }

            if (Input.GetKey(KeyCode.LeftArrow) && m_ePlayerState != PLAYERSTATE.PLAYER_LOPE)
            {
                transform.eulerAngles = new Vector3(0.0f, 0.0f, 0.0f);

                m_pRigidBody.position += Vector2.left * _MovingSpeed * Time.deltaTime;

                _ePlayerState = PLAYERSTATE.PLAYER_RUN;
            }

            if (Input.GetKey(KeyCode.RightArrow) && m_ePlayerState != PLAYERSTATE.PLAYER_LOPE)
            {
                transform.eulerAngles = new Vector3(0.0f, 180.0f, 0.0f);

                m_pRigidBody.position += Vector2.right * _MovingSpeed * Time.deltaTime;

                _ePlayerState = PLAYERSTATE.PLAYER_RUN;
            }
        }

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

        if ((int)m_ePlayerState < (int)PLAYERSTATE.PLAYER_DOWNJUMP)
            m_ePlayerState = _ePlayerState;
    }

    private void AnimationUpdate()
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

    private void Idle()
    {
        m_bIsAttack = false;

        m_pBody.GetAvatarState = AVATARSTATES.AVATAR_IDLE;

        //if(m_eLadderMode != LADDERMODE.LADDER_NORMAL)
        //    m_eLadderMode = LADDERMODE.LADDER_NORMAL;

        Ray2D _Ray2D = new Ray2D(transform.position + (Vector3)_JumpDirection, Vector2.down);

        RaycastHit2D Hit2D = Physics2D.Raycast(_Ray2D.origin + _Direction, _Ray2D.direction, _RayLength, _LayMask);

        if (Hit2D.collider != null)
            m_bIsLine = true;
        else
            m_bIsLine = false;

        if (m_pBoxCollision.enabled == false)
            m_pBoxCollision.enabled = true;
    }

    private void Jump()
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
            if (_Collision.gameObject.tag == "Jump Stand")
            {
                m_pBoxCollision.enabled = false;

                m_pRigidBody.AddForce(Vector2.up * _JumpPower * 6.0f, ForceMode2D.Impulse);

                return;
            }

            m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;
        }
    }

    private void DownJump()
    {
        m_pBody.GetAvatarState = AVATARSTATES.AVATAR_JUMP;

        Vector3 _RayPosition = new Vector3(transform.position.x, transform.position.y - 1.0f);

        Ray2D _Ray2D = new Ray2D(_RayPosition, Vector2.down);

        RaycastHit2D _Hit2D;

        if (_Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 3.0f, _LayMask))
        {
            if (_Hit2D.distance < 0.7f)
            {
                m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

                m_pBoxCollision.enabled = true;

                m_bIsAttack = false;

                return;
            }
        }
    }

    private void JumpAttack()
    {
        if (m_bIsAttack == true)
        {
            if (m_pRigidBody.gravityScale == 0.0f)
                m_pRigidBody.gravityScale = 4.0f;

            if (m_pRigidBody.velocity.y > 0)
                return;

            Collider2D _Collision = Physics2D.OverlapBox(transform.position + _OffSetPosition, _BoxSize, 0.0f, _LayMask);

            m_pBoxCollision.enabled = true;

            if (_Collision != null)
            {
                if (_Collision.gameObject.tag == "Jump Stand")
                {
                    m_pBoxCollision.enabled = false;

                    m_pRigidBody.AddForce(Vector2.up * _JumpPower * 6.0f, ForceMode2D.Impulse);

                    return;
                }

                m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

                m_bIsAttack = false;
            }

            return;
        }

        int _SelectAttack = Random.Range(0, 3);

        // 여기에 들어가지 않고 바로 점프로 넘어가니까 문제다 이거네 ??

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

    private void Run()
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

    private void Attack()
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

    private void Lope()
    {
        if (m_pRigidBody.gravityScale != 0.0f)
            InitRigidBody();

        if (m_pBoxCollision.enabled == true)
            m_pBoxCollision.enabled = false;

        if((int)m_pBody.GetAvatarState < (int)AVATARSTATES.AVATAR_LADDER)
            m_pBody.GetAvatarState = AVATARSTATES.AVATAR_LADDER;
        else
            m_pBody.GetAvatarState = AVATARSTATES.AVATAR_LADDERRUN;
    }

    private void Hit()
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

    public void Dead()
    {
        if (m_pBody.GetAvatarState == AVATARSTATES.AVATAR_DEAD)
            return;

        //if(m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
        //{
        //    m_pRigidBody.gravityScale = 4.0f;
        //}
            
        m_bIsLadder = false;

        m_pRigidBody.gravityScale = 4.0f;

        m_pCollisionObject = null;

        m_eLadderMode = LADDERMODE.LADDER_NORMAL;

        if (m_pBoxCollision.enabled == false) // 문제 생기면 여기서 
            m_pBoxCollision.enabled = true;

        m_pBody.GetAvatarState = AVATARSTATES.AVATAR_DEAD;       

        m_pGameObejctManager.GameObejctPooling("DEAD STONE", Vector3.zero, transform.position + Vector3.up * 5.0f, Quaternion.identity);

        _DeadUIBar.SetActive(true);

        m_eInsertPortalType = PORTALTYPE.POTAL_NORMAL;
    }

    public void ResetPlayerState()
    {
        _DeadUIBar.SetActive(false);

        m_pBody.ResetState();

        _Hp = m_fMaxHP;

        m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

        m_pSceneManaged.FadeOut();

        for (int i = 0; i < m_pGameObejctList.Count; ++i)
            DontDestroyOnLoad(m_pGameObejctList[i]);

        SceneManager.LoadScene(m_strSceneName);

        transform.position = m_ResetPosition;
    }

    private void InitItem()
    {
        GameObject _AvaterObject = GameObject.Find("Select Avatar");

        if (null == _AvaterObject)
            return;

        PART[] _PlayerPart = GetComponentsInChildren<PART>();

        PART[] _ChangePart = _AvaterObject.GetComponentsInChildren<PART>();

        ITEM[] _InstanceItems = new ITEM[_Equip.AccessSlot.Length];

        PART[] _NewParts = new PART[_PlayerPart.Length];

        int _iPartIndex = 0;

        int _iItemIndex = 0;

        for (int i = 0; i < _ChangePart.Length; ++i)
        {
            if (_ChangePart[i] is ITEM && _ChangePart[i].tag != "Arm")
            {
                _InstanceItems[_iItemIndex++] = _ChangePart[i] as ITEM;

                continue;
            }
            else if(_ChangePart[i] is ITEM == false)
                _NewParts[_iPartIndex++] = _ChangePart[i];
        }

        for (int i = 0; i < _Equip.AccessSlot.Length; ++i)
        {
            for (int j = 0; j < _InstanceItems.Length; ++j)
            {
                if (_Equip.AccessSlot[i].AccessSlotType == SlotTpye.Slot_Weapon && _InstanceItems[j].tag == "Weapon")
                    InsertEquip(_InstanceItems[j], i);
                else if (_Equip.AccessSlot[i].AccessSlotType == SlotTpye.Slot_Clothes && _InstanceItems[j].tag == "Clothes")
                    InsertEquip(_InstanceItems[j], i);
                else if (_Equip.AccessSlot[i].AccessSlotType == SlotTpye.Slot_Parts && _InstanceItems[j].tag == "Parts")
                    InsertEquip(_InstanceItems[j], i);
                else if (_Equip.AccessSlot[i].AccessSlotType == SlotTpye.Slot_Foot && _InstanceItems[j].tag == "foot")
                    InsertEquip(_InstanceItems[j], i);
            }
        }

        for (int i = 0; i < _PlayerPart.Length; ++i)
        {
            for (int j = 0; j < _NewParts.Length; ++j)
            {
                if (_PlayerPart[i].name == _NewParts[j].name)
                {
                    if (_NewParts[j] is Body)
                    {
                        Body _Body = _NewParts[j] as Body;

                        if (m_pBody.AccessPlayerName == string.Empty)
                            _PlayerNamebar.AccessText.text = _Body.AccessPlayerName;

                        _Body.GetAvatarState = AVATARSTATES.AVATAR_IDLE;
                    }

                    if(_PlayerPart[i] is Hair)
                    {
                        if (_NewParts[j] is Hair)
                        {
                            Hair _Hair = _NewParts[j] as Hair;

                            Hair _NewPlayerHair = _PlayerPart[i] as Hair;

                            _NewPlayerHair.AccessHair = _Hair.AccessHair;
                        }
                    }

                    _PlayerPart[i].transform.localPosition = _NewParts[j].transform.localPosition;

                    _PlayerPart[i].SetSprite(_NewParts[j].GetSprite());

                    break;
                }
            }
        }
        
        _Equip.gameObject.SetActive(false);

        Destroy(_AvaterObject);
    }

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

    private void OnTriggerEnter2D(Collider2D collision) // 여기일꺼 같은데 ;;
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Line"))
        {
            //if (m_bIsLadder == true)
            //    return;

            if (m_bIsTerrainLine == true)
            {
                m_bIsLadder = false;

                m_pCollisionObject = null;

                if (m_ePlayerState == PLAYERSTATE.PLAYER_LOPE)
                    m_ePlayerState = PLAYERSTATE.PLAYER_IDLE;

                m_pRigidBody.gravityScale = 4.0f;
            }
        }
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Connect"))
        {
            m_bIsLadder = true;

            m_pCollisionObject = collision.gameObject;

            Vector2 _Position = collision.bounds.center;

            m_vConnentCenter = collision.bounds.min + Vector3.up;

            Vector2 _Direction = ((Vector2)transform.position - _Position).normalized;

            if (m_eLadderMode == LADDERMODE.LADDER_NORMAL)
            {
                if (_Direction.y <= 0.0f)
                    m_eLadderMode = LADDERMODE.LADDER_DOWN;
                else
                    m_eLadderMode = LADDERMODE.LADDER_UP;
            }
        }

        if (collision.gameObject.name == "Portal")
        {
            if (Input.GetKey(KeyCode.UpArrow))
            {
                if (m_bIsPotalSound == false)
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
        if (collision.gameObject.layer == LayerMask.NameToLayer("Connect"))
        {
            if (m_bIsLadder == false) // 레이어 충돌시 자동으로 충돌 무시
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

    private void InitRigidBody()
    {
        if (null == m_pRigidBody)
            return;

        m_pRigidBody.gravityScale = 0.0f;

        m_pRigidBody.velocity = Vector2.zero;
    }

    private void ChakingAnimatior()
    {
        if (null == m_pLevelUpEffectAnimator)
            return;

        if(m_pLevelUpEffectAnimator.GetCurrentAnimatorStateInfo(0).IsName("LEVEL UP EFFECT") && m_pLevelUpEffectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            m_pGameObejctManager.Remove("Level Up Effect", m_pLevelUpEffectAnimator.gameObject);

            m_pLevelUpEffectAnimator = null;
        }
    }

    private void TeleportScene()
    {
        for (int i = 0; i < m_pGameObejctList.Count; ++i)
            DontDestroyOnLoad(m_pGameObejctList[i]);

        UnityEngine.SceneManagement.SceneManager.LoadScene("Marina");

        transform.position = new Vector3(52.66f, 9.47f, 0.0f);

        //UnityEngine.SceneManagement.SceneManager.LoadScene("Amherst");

        //transform.position = new Vector3(32.89f, 10.1f, 0.0f);
    }

    public void CilckInventory()
    {
        if (null == _Inventory)
            return;

        if (_Inventory.activeSelf == true)
            _Inventory.SetActive(false);
        else if (_Inventory.activeSelf == false)
            _Inventory.SetActive(true);

        m_pSoundManager.PlaySound("Click Sound");
    }

    public void CilckEquip()
    {
        if (null == _Equip)
            return;

        if (_Equip.gameObject.activeSelf == true)
            _Equip.gameObject.SetActive(false);
        else if (_Equip.gameObject.activeSelf == false)
            _Equip.gameObject.SetActive(true);

        m_pSoundManager.PlaySound("Click Sound");
    }

    public void CilckQuestWindow()
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

    private void DontReleseObject()
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