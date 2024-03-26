using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Networking;
using UnityEditor;

public class Cookie : MonoBehaviour
{
    private CGlobalStruct.COOKIEINFO m_CookieInfo = default;

    private CookieState m_pCookieState = null;

    private CookieState m_pPrevState = null;

    private CookieState[] m_pCookieSkills = new CookieState[3];

    private Animator m_pAnimator = null;

    private SpriteRenderer m_pEffectBackGround = null;

    private Rigidbody2D m_pRigidbody = null;

    private Transform m_pBoosterEffectParent = null;

    private bool m_bIsNoControl = false;

    private float m_fTimeAcc = 0.0f;

    private Coroutine m_pDamageCorotine = null;

    private float m_fDamage = 0.0f;

    private Transform m_pTilePatten = null;

    private BoxCollider2D m_pBoxCollder = null;

    private CookieEventBox m_pCookieEventBox = null;

    private JellyPoint m_pJellyPoint = null;

    private CCoinPoint m_pCoinPoint = null;

    private CBounsTime m_pBounsTime = null;
    
    private CResultPanel m_pResultPanel = null;

    private CCookieTimeBubble m_pTimeBubble = null;

    private CSoundManaged m_pSoundManaged = null;

    private float m_fDeadTimeAcc = 0.0f;

    private RigidbodyType2D m_RigidbodyType = RigidbodyType2D.Static;


#region 프로퍼티    
    public CGlobalStruct.COOKIEINFO AccessCookieInfo { get => m_CookieInfo; }
    public CookieState AccessCookieState { get => m_pCookieState; }
    public CookieState AccessPrevCookieState { get => m_pPrevState; }
    
    public CookieEventBox GetCookieEventBox { get => m_pCookieEventBox; }
    public Transform AccessBoosterParent { get => m_pBoosterEffectParent; }

    public Transform GetTilePatten { get => m_pTilePatten; }
    public SpriteRenderer GetSpriteRenderer { get => m_pEffectBackGround; }
    public Rigidbody2D GetRigidbody2D { get => m_pRigidbody; }
    public CookieState[] GetSkill { get => m_pCookieSkills; }
    public float CookieHpGauge { get => m_CookieInfo.m_fHp / m_CookieInfo.m_fMaxHp; }
#endregion

#region 쿠키에서 사용되는 유니티 이벤트 메세지들
    private void Awake()
    {
        m_CookieInfo.m_fHp = 100.0f;
        m_CookieInfo.m_fMaxHp = m_CookieInfo.m_fHp;
        m_CookieInfo.m_JumpPower = 13.5f;
        m_CookieInfo.m_fBounsMovePower = 5.0f;

        m_pAnimator = GetComponent<Animator>();
        m_pRigidbody = GetComponent<Rigidbody2D>();
        m_pBoxCollder = GetComponent<BoxCollider2D>();

        Transform _pEffectTransform = transform.GetChild(1);

        if (_pEffectTransform != null)
            m_pEffectBackGround = _pEffectTransform.GetComponent<SpriteRenderer>();

        m_pBoosterEffectParent = transform.Find("Booster Effect Parent");

        m_pCookieEventBox = GameObject.Find("GameScene").transform.GetChild(5).GetComponent<CookieEventBox>();

        GameObject _pCanvas = GameObject.Find("Canvas");

        if (_pCanvas != null)
        {
            m_pResultPanel = _pCanvas.transform.GetChild(4).GetComponent<CResultPanel>();

            m_pJellyPoint = FindObjectOfType<JellyPoint>();

            m_pCoinPoint = FindObjectOfType<CCoinPoint>();

            m_pBounsTime = FindObjectOfType<CBounsTime>();
        }

        m_pTimeBubble = GetComponentInChildren<CCookieTimeBubble>();
        m_pTimeBubble.gameObject.SetActive(false);
    }

    private void Start()
    {
        m_pSoundManaged = CTopManager.GetInstance().GetSoundManaged;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.W))
            m_pBounsTime.StartBounsTime();
        if (Input.GetKeyDown(KeyCode.E))
            m_CookieInfo.m_fHp = m_CookieInfo.m_fMaxHp;

        if (CGlobalValues.m_bIsPause)
        {
            CookiePause();

            return;
        }

        if (CookieDead()) 
        {
            m_fDeadTimeAcc += Time.deltaTime;

            if (m_fDeadTimeAcc >= 1.0f)
                ActiveGameResult();

            return;
        }

        RaycastHit2D _Hit2D;

        if (_Hit2D = Physics2D.Raycast(transform.position, Vector2.down, Mathf.Infinity, 1 << 6))
            m_pTilePatten = _Hit2D.collider.transform.parent;

        m_fTimeAcc += Time.deltaTime;

        if (m_fTimeAcc >= 1.0f)
        {
            SetCookieHp(-1.0f);

            m_fTimeAcc = 0.0f;
        }

        CookieSkii();

        if (m_bIsNoControl)
            return;

        if (Input.GetKey(KeyCode.S))
        {
            if (m_pPrevState != null && m_pPrevState.GetCookieState == CookieState.COOKSTATE.COOKIE_BOUNS_MODE)
                Jump(1);
            else
            {
                if(Input.GetKeyDown(KeyCode.S))
                    Jump();
            }
        }

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKey(KeyCode.A))
            Slider();

        if (Input.GetKeyUp(KeyCode.A))
            SliderReset();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Tile"))
            return;

        if (m_pCookieState == null)
            return;

        //---------------------------------------------------------------
        // 쿠키 발에 위치한 위치와 충돌된 타일과 방향백터를 구한다.
        //---------------------------------------------------------------
        Vector2 _vPosition = m_pRigidbody.position + m_pBoxCollder.offset + (Vector2.down * 0.2f);
        Vector2 _vDirection = _vPosition - (Vector2)collision.transform.position;
        _vDirection.Normalize();

        //---------------------------------------------------------------------------------------------------
        // 쿠기가 타일 아래에서 충돌을 시도하고 있을 경우, 그리고 점프 중일 때 충돌 이벤트 함수를 종료 한다.
        //---------------------------------------------------------------------------------------------------
        if (_vDirection.y < 0.0f || m_pRigidbody.velocity.y > 0.0f)
            return;

        if (m_pCookieState.GetCookieState == CookieState.COOKSTATE.COOKIE_JUMP ||
            m_pCookieState.GetCookieState == CookieState.COOKSTATE.COOKIE_DOUDLE_JUMP ||
            m_pCookieState.GetCookieState == CookieState.COOKSTATE.COOKIE_BOUNS_RESET ||
            m_pAnimator.GetCurrentAnimatorStateInfo(0).IsName("Hit Jump"))
        {
            m_pCookieState.Reset();
        }
    }
    #endregion

#region 쿠기 상태 전환 함수들
    /// <summary>
    /// 보너스 맵 교체
    /// </summary>
    public void ExchangeBounsMode()
    {
        if (null == m_pCookieState || !(m_pCookieState is CookieBouns))
        {
            m_pPrevState = m_pCookieState;

            m_pCookieState = new CookieBouns(this);
        }

        m_pCookieState.Action();
    }
    /// <summary>
    /// 쿠키 점프
    /// </summary>
    public void Jump(int _iMode = -1)
    {
        if (null == m_pCookieState || !(m_pCookieState is CookieJump))
        {
            m_pPrevState = m_pCookieState;
            
            m_pCookieState = new CookieJump(this);   
        }

        if (_iMode != -1)
        {
            CookieJump _pJumpState = m_pCookieState as CookieJump;

            _pJumpState.ExchangeJumpMode();
        }

        m_pCookieState.Action();
    }

    /// <summary>
    /// 슬라이드
    /// </summary>
    public void Slider()
    {
        if (m_pCookieState is CookieJump || m_pCookieState is CookieHit)
            return;

        if (null == m_pCookieState || !(m_pCookieState is CookieSlider))
        {
            m_pPrevState = m_pCookieState;

            m_pCookieState = new CookieSlider(this);
        }

        m_pCookieState.Action();
    }

    /// <summary>
    /// 슬라이드 초기화
    /// </summary>
    public void SliderReset()
    {
        if (null == m_pCookieState || !(m_pCookieState is CookieSlider))
            return;

        m_pCookieState.Reset();
    }

    /// <summary>
    /// 피격
    /// </summary>
    public void Hit()
    {
        if (m_pCookieState is CookieHit)
            return;

        if (null == m_pCookieState || !(m_pCookieState is CookieHit))
        {
            m_pPrevState = m_pCookieState;
            m_pCookieState = new CookieHit(this);
        }

        bool _bAction = m_pCookieState.Action();
        m_pCookieSkills[2] = new CookieInvin(this);
    }
    /// <summary>
    /// 사이즈 업, 부스터, 무적 등... 존재할 경우 사용
    /// </summary>
    private void CookieSkii()
    {
        for (int i = 0; i < m_pCookieSkills.Length; ++i)
        {
            if (null == m_pCookieSkills[i])
                continue;

            bool _bIsAction = m_pCookieSkills[i].Action();

            if (!_bIsAction)
            {
                m_pCookieSkills[i].Reset();
                m_pCookieSkills[i] = null;
            }
        }
    }

    //-----------------------
    // 쿠기 젤리 기능 활성화
    //-----------------------
    public void SetCookieSkillMode(CGlobalEnum.COOKIE_SKILL _eCookieSkill)
    {
        if (m_pCookieSkills[((int)_eCookieSkill)] != null)
        {
            if (m_pCookieSkills[((int)_eCookieSkill)] is CookieBooster)
            {
                CookieBooster _pBooster = m_pCookieSkills[((int)_eCookieSkill)] as CookieBooster;
                _pBooster.ResetTime();
            }
            else if(m_pCookieSkills[((int)_eCookieSkill)] is CCookieScale)
            {
                CCookieScale _pScale = m_pCookieSkills[((int)_eCookieSkill)] as CCookieScale;
                _pScale.ResetTime();
            }

            return;
        }

        switch(_eCookieSkill)
        {
            case CGlobalEnum.COOKIE_SKILL.COOKIE_BOOSTER:
                m_pCookieSkills[((int)_eCookieSkill)] = new CookieBooster(this);
                break;
            case CGlobalEnum.COOKIE_SKILL.COOKIE_POWER_UP:
                m_pCookieSkills[((int)_eCookieSkill)] = new CCookieScale(this);
                break;
            case CGlobalEnum.COOKIE_SKILL.COOKIE_INVIN:
                m_pCookieSkills[((int)_eCookieSkill)] = new CookieInvin(this);
                break;
        }
    }
    
    //-----------------------
    // 쿠키 사망 함수
    //-----------------------
    private bool CookieDead()
    {
        if (m_pCookieState != null &&
            (m_pCookieState.GetCookieState == CookieState.COOKSTATE.COOKIE_BOUNS_MODE || 
            m_pCookieState.GetCookieState == CookieState.COOKSTATE.COOKIE_BOUNS_MOVE ||
            m_pCookieState.GetCookieState == CookieState.COOKSTATE.COOKIE_BOUNS_RESET))
            return false;

        if (CGlobalValues.m_bIsGameOver)
            return true;

        if (m_CookieInfo.m_fHp <= 0.0f || transform.position.y < -4.455683f)
        {
            if(m_CookieInfo.m_fHp <= 0.0f)
            {
                m_pCookieState = new CookieDead(this);
                m_pCookieState.Action();
            }

            m_pSoundManaged.PlaySound("GameEnd");


            CGlobalValues.m_bIsGameOver = true;

            SetCookieHp(-m_CookieInfo.m_fMaxHp);
        }

        return false;
    }

    //-----------------------
    // 최종 결과 등장 함수
    //-----------------------
    public void ActiveGameResult()
    {
        if (m_pJellyPoint == null || m_pResultPanel == null || m_pResultPanel.gameObject.activeSelf)
            return;

        m_pResultPanel.gameObject.SetActive(true);
        m_pResultPanel.SetGameResultScore(m_pJellyPoint.GetScore);
    }

    #endregion

#region 기타 설정 함수
    public void SetCookieState(CookieState _CookieState)
    {
        m_pPrevState = m_pCookieState;

        m_pCookieState = _CookieState;
    }

    public void SetJumpCount(int _Count)
    {
        m_CookieInfo.m_iJumpCount = _Count;
    }

    public void SetCookieControl(bool _bIsControl)
    {
        m_bIsNoControl = _bIsControl;
    }

    public void MovePosition()
    {
        m_pRigidbody.position = new Vector3(transform.position.x, 6.46f, 0.0f);
    }

    public void SetRigidbodyKinematic(bool _bIsKinematic)
    {
        m_pRigidbody.isKinematic = _bIsKinematic;
    }

    public void SetRigidBodyPositionAndDrag(Vector2 _vPosition, int _iDrag)
    {
        m_pRigidbody.position = _vPosition;

        if (_iDrag > 0)
            m_pRigidbody.drag = _iDrag;
    }

    public void SetCookieHp(float _fHp)
    {
        m_fDamage += _fHp;

        if (m_pDamageCorotine == null)
            m_pDamageCorotine = StartCoroutine(StartSetHp(_fHp));
    }

    public void ResetCookieSkill(CGlobalEnum.COOKIE_SKILL _eCookieSkill)
    {
        if (m_pCookieSkills[((int)_eCookieSkill)] == null)
            return;

        m_pCookieSkills[((int)_eCookieSkill)].Reset();

        m_pCookieSkills[((int)_eCookieSkill)] = null;
    }

    //-------------------------
    // 쿠키 일시정지
    //-------------------------
    public void CookiePause()
    {
        if (!m_pAnimator.enabled)
            return;

        m_pAnimator.enabled = false;
        m_RigidbodyType = m_pRigidbody.bodyType;
        m_pRigidbody.bodyType = RigidbodyType2D.Static;
    }

    //-------------------------
    // 쿠키 일시정지 해제
    //-------------------------
    public void CookiePlay()
    {
        if (m_pAnimator.enabled)
            return;

        m_pAnimator.enabled = true;
        m_pRigidbody.bodyType = m_RigidbodyType;
    }

    //--------------------------
    // 쿠키 계속하기 할 경우 카운터
    //--------------------------
    public void SetCookieTime(int _iCount)
    {
        m_pTimeBubble.gameObject.SetActive(true);

        m_pTimeBubble.SetCount(_iCount);
    }

    public void DisableTimer() => m_pTimeBubble.gameObject.SetActive(false);

    //----------------------------------------------
    // 다시하기 버튼 누를 경우 쿠키 정보 초기화
    //----------------------------------------------
    public void ResetCookie()
    {
        m_fDeadTimeAcc = m_fTimeAcc = 0.0f;
        m_pResultPanel.gameObject.SetActive(false);
        m_pJellyPoint.ResetPoint();
        m_pCoinPoint.ResetPoint();
        m_pBounsTime.ResetBounsTime();

        for (int i = 0; i < m_pCookieSkills.Length; ++i)
        {
            if(m_pCookieSkills[i] != null)
                m_pCookieSkills[i].Reset();

            m_pCookieSkills[i] = null;
        }

        m_pCookieState = new CookieNomral(this);
        m_pCookieState.Action();
        m_pCookieState.ResetAllAnimator();
        m_pAnimator.SetTrigger("GameReset");

        m_CookieInfo.m_fHp = m_CookieInfo.m_fMaxHp;
        transform.localScale = Vector3.one * 1.25f;
        transform.position = new Vector3(-4.0f, -2.654568f);
    }

    private IEnumerator StartSetHp(float _fHp)
    {
        float m_fHp = m_CookieInfo.m_fHp;

        float _fTimeAcc = 0.0f, _fOrldHp = m_CookieInfo.m_fHp;

        while (_fTimeAcc < 1.0f)
        {
            _fTimeAcc += (Time.deltaTime * 4.0f);

            m_CookieInfo.m_fHp = Mathf.Lerp(_fOrldHp, m_fHp + m_fDamage, _fTimeAcc);

            yield return null;
        }

        m_fDamage = 0.0f;

        m_pDamageCorotine = null;

        if (m_CookieInfo.m_fHp < 0.0f)
            m_CookieInfo.m_fHp = 0.0f;
        else if (m_CookieInfo.m_fHp >= m_CookieInfo.m_fMaxHp)
            m_CookieInfo.m_fHp = m_CookieInfo.m_fMaxHp;

    }
    #endregion

#region 쿠키 유니티 버튼 이벤트 함수
    public void OnClickJump()
    {
        if (m_pPrevState != null && m_pPrevState.GetCookieState == CookieState.COOKSTATE.COOKIE_BOUNS_MODE)
            Jump(1);
        else
            Jump();
    }

    public void OnClickSlider()
    {
        Slider();
    }

#endregion
}