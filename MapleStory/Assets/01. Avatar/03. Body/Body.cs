using UnityEngine;

public enum AVATARSTATES
{
    AVATAR_IDLE,
    AVATAR_RUN,
    AVATAR_JUMP,
    AVATAR_LADDER,
    AVATAR_LADDERRUN,
    AVATAR_FIRSTNORMALATTACK,
    AVATAR_SECONDNORMALATTACK,
    AVATAR_THIRDNORMALATTACK,
    AVATAR_HIT,
    AVATAR_DEAD
}

public class Body : PART // 플레이어 및 캐릭터 커스터마이징 캐릭터 몸체의 클래스 입니다.
{
    private Animator m_pAnimator = null;

    private Animator m_pHandAnimator = null;

    private AVATARSTATES m_eAvatarState = AVATARSTATES.AVATAR_IDLE; // 플레이어 상태에 따른 몸체의 상태 변수 

    private string m_strAttackName = string.Empty;

    private Player m_pPlayer = null;

    // 손, 머리, 얼굴, 헤어 스프라이트 랜더러 컴포넌트 변수

    private SpriteRenderer m_pHandRenderer = null;

    private SpriteRenderer m_pHeadRenderer = null;

    private SpriteRenderer m_pFaceRenderer = null;

    private SpriteRenderer m_pHairRenderer = null;

    private LowerBody m_pLowerBody = null;

    private Foot m_pFoot = null;

    // 케릭터 커스터마이징에서 지정한 이름을 게임 시작시 플레이어 이름으로 지정할 변수입니다.

    private string m_PlayerName = string.Empty;

    // 케릭터의 사망 여부 

    private bool m_bIsDead = false;

    private int m_IsUpDown = 0;

    public AVATARSTATES GetAvatarState
    {
        get { return m_eAvatarState; }

        set { m_eAvatarState = value; }
    }

    public string AccessPlayerName
    {
        get { return m_PlayerName; }
        
        set { m_PlayerName = value; }
    }

    private void Awake()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pAnimator = GetComponent<Animator>();

        m_pHandAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>(); // 손 부위 애니메이터 

        m_pHandRenderer = m_pHandAnimator.GetComponent<SpriteRenderer>(); // 손 부위 스프라이트 랜더러

        m_pPlayer = FindObjectOfType<Player>();

        if (m_pPlayer != null)
        {
            m_pHeadRenderer = m_pPlayer.transform.Find("Head").GetComponent<SpriteRenderer>();

            m_pFaceRenderer = m_pHeadRenderer.transform.Find("Face").GetComponent<SpriteRenderer>();

            m_pHairRenderer = m_pHeadRenderer.transform.Find("Hair").GetComponent<SpriteRenderer>();

            m_pLowerBody = m_pPlayer.transform.Find("Lower Body").gameObject.GetComponent<LowerBody>();

            m_pFoot = m_pPlayer.transform.Find("Foot").gameObject.GetComponent<Foot>();
        }
    }

    private void Update()
    {
        if (null != m_pItem) // 
            m_pItem.AvatarState = m_eAvatarState; // 아이템에 현재 플레이어 상태 정보를 넘김

        if (transform.parent.name == "Select Avatar")
        {
            SelectAnimation(); // 상위 오브젝트가 케릭터 커스터마이징시 애니메이션을 실행하기 위한 함수입니다.

            return;
        }

        AnimationUpdate();
    }

    private void AnimationUpdate() // 실제 상위 오브젝트가 플레이어시 실행되는 애니메이션 함수입니다.
    {
        // 애니메이터로 몸체의 애니메이션을 전환을 위해 Setbool로 제어했습니다.

        // m_strAttackName 변수로 공격 모션 애니메이션에서 대기 애니매이션 전환을 위한 변수입니다.

        switch (m_eAvatarState) // 현재 상태값에 따른 분기문입니다.
        {
            case AVATARSTATES.AVATAR_IDLE: // 대기 상태
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                m_pAnimator.SetBool("LADDER", false);
                m_pAnimator.SetBool("LADDER RUN", false);
                if (m_pHandAnimator.gameObject.activeSelf == false)
                    m_pHandAnimator.gameObject.SetActive(true);
                if (m_pSpriteRenderer.color != Color.white)
                    m_pSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
                m_strAttackName = string.Empty;
                break;
            case AVATARSTATES.AVATAR_RUN: // 달리기 
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                m_pAnimator.SetBool("RUN", true);
                m_pHandAnimator.SetBool("RUN", true);
                m_pAnimator.SetBool("LADDER", false);
                m_pAnimator.SetBool("LADDER RUN", false);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                if (m_pHandAnimator.gameObject.activeSelf == false)
                    m_pHandAnimator.gameObject.SetActive(true);
                m_strAttackName = string.Empty;
                break;
            case AVATARSTATES.AVATAR_JUMP: // 점프 모션 
                m_strAttackName = string.Empty;
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("JUMP", true);
                m_pHandAnimator.SetBool("JUMP", true);
                m_pAnimator.SetBool("LADDER", false);
                m_pAnimator.SetBool("LADDER RUN", false);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                if (m_pHandAnimator.gameObject.activeSelf == false)
                    m_pHandAnimator.gameObject.SetActive(true);
                m_strAttackName = string.Empty;
                break;
            case AVATARSTATES.AVATAR_LADDER: // 사다리나 로프에 올라탈 시 대기 상태 
                m_pAnimator.SetBool("LADDER", true);
                m_pAnimator.SetBool("LADDER RUN", false);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                m_strAttackName = string.Empty;
                break;
            case AVATARSTATES.AVATAR_LADDERRUN: // 사다리나 로프에 오르고 내려올 때 애니메이션 
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("LADDER RUN", true);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                m_strAttackName = string.Empty;
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK: // 첫 번째 공격 모션 
                if (m_strAttackName != string.Empty)
                    break;
                m_strAttackName = "FIRST ATTACK"; // 첫 번째 공격 레이어명으로 변경 
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                m_pAnimator.SetBool("ATTACK", true);
                m_pHandAnimator.SetBool("ATTACK", true);
                m_pAnimator.SetInteger("ATTACK MODE", 1);
                m_pHandAnimator.SetInteger("ATTACK MODE", 1);
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK: // 두 번째 공격 모션 
                if (m_strAttackName != string.Empty)
                    break;
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                m_pAnimator.SetBool("ATTACK", true);
                m_pHandAnimator.SetBool("ATTACK", true);
                m_pAnimator.SetInteger("ATTACK MODE", 2);
                m_pHandAnimator.SetInteger("ATTACK MODE", 2);
                m_strAttackName = "SECOND ATTACK"; // 두 번째 공격 레이어명으로 변경
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK: // 세 번째 공격 모션 
                if (m_strAttackName != string.Empty)
                    break;
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                m_pAnimator.SetBool("ATTACK", true);
                m_pHandAnimator.SetBool("ATTACK", true);
                m_pAnimator.SetInteger("ATTACK MODE", 3);
                m_pHandAnimator.SetInteger("ATTACK MODE", 3);
                m_strAttackName = "THIRD ATTACK"; // 세 번째 공격 레이어명으로 변경
                break;
            case AVATARSTATES.AVATAR_HIT: // 몬스터와 충돌시 
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("LADDER", false);
                m_pAnimator.SetBool("LADDER RUN", false);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                m_pAnimator.SetBool("JUMP", true);
                m_pHandAnimator.SetBool("JUMP", true);
                Hit();
                m_strAttackName = string.Empty;
                break;
            case AVATARSTATES.AVATAR_DEAD: // 사망 
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("LADDER", false);
                m_pAnimator.SetBool("LADDER RUN", false);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                Dead();
                m_strAttackName = string.Empty;
                break;
        }

        Chanking(); 
    }

    public override Vector3 IDLE()
    {
        return Vector3.zero;
    }

    public override Vector3 RUN()
    {
        return Vector3.zero;
    }

    private void SelectAnimation() //상위 오브젝트가 케릭터 커스터마이징시 애니메이션을 실행하기 위한 함수입니다.
    {
        // 애니메이터로 몸체의 애니메이션을 전환을 위해 Setbool로 제어했습니다.

        switch(m_eAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                break;
            case AVATARSTATES.AVATAR_RUN:
                m_pAnimator.SetBool("FIRST ATTACK", false);
                m_pHandAnimator.SetBool("FIRST ATTACK", false);
                m_pAnimator.SetBool("RUN", true);
                m_pHandAnimator.SetBool("RUN", true);
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("SECOND ATTACK", false);
                m_pHandAnimator.SetBool("SECOND ATTACK", false);
                if (m_pAnimator.GetBool("FIRST ATTACK") == false)
                {
                    m_pAnimator.SetBool("FIRST ATTACK", true);
                    m_pHandAnimator.SetBool("FIRST ATTACK", true);
                }
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("THIRD ATTACK", false);
                m_pHandAnimator.SetBool("THIRD ATTACK", false);
                if (m_pAnimator.GetBool("SECOND ATTACK") == false)
                {
                    m_pAnimator.SetBool("SECOND ATTACK", true);
                    m_pHandAnimator.SetBool("SECOND ATTACK", true);
                }
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("THIRD ATTACK", true);
                m_pHandAnimator.SetBool("THIRD ATTACK", true);
                break;   
        }
    }

    private bool IsEndAnimation(string _AnimationName) // 애니메이션 끝남 여부를 알려주는 함수입니다.
    {
        bool _AnimationFinal = false;

        _AnimationFinal = m_pAnimator.GetCurrentAnimatorStateInfo(0).IsName(_AnimationName) && m_pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f;

        return _AnimationFinal;
    }

    private void Chanking() // 공격 모션 애니메이션 완료 확인하기 위한 함수입니다.
    {
        // eunm 변수 값을 int형으로 캐스팅하여 첫 번째 공격 모션 이상일 경우 

        if ((int)m_eAvatarState >= (int)AVATARSTATES.AVATAR_FIRSTNORMALATTACK) 
        {
            bool _bIsIdle = IsEndAnimation(m_strAttackName); // 공격 레이어 명으로 애니메이션 종료 확인하여 

            if (true == _bIsIdle) // 종료 되었다면 
            {
                m_eAvatarState = AVATARSTATES.AVATAR_IDLE;

                m_pPlayer.AccessPlayerState = PLAYERSTATE.PLAYER_IDLE; 

                m_pPlayer.AccessAttack = false;

                m_strAttackName = string.Empty;

                // 상태값을 다시 변경 기본 대기 상태로 변경 
            }
        }
    }

    private void Hit() // 피격 시 플레이어의 색상을 변경하는 함수입니다.
    {
        float Color = m_pSpriteRenderer.color.r;

        if (Color == 1.0f)
        {
            m_pHeadRenderer.color = m_pHandRenderer.color = m_pFaceRenderer.color = 
                m_pHairRenderer.color = m_pSpriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
        }
        else if (Color == 0.5f)
        {
            m_pHeadRenderer.color = m_pHandRenderer.color = 
                m_pSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        }
    }

    private void Dead() // 플레이어 사망시 호출하는 함수입니다.
    {
        if (m_bIsDead == false) // 사망 시 유령 애니메이션으로 전환합니다.
        {
            m_pAnimator.SetBool("DEAD", true);

            m_bIsDead = true;
        }

        if(m_bIsDead == true) // 이후 사망시 유령의 이미지를 위 아래로 움직 일 수 있게 합니다.
        {
            Vector3 _Position = transform.localPosition;

            if (m_IsUpDown == 1)
            {
                _Position += Vector3.down * 0.1f * Time.deltaTime;

                if (_Position.y <= 0.0f)
                    m_IsUpDown = 2;
            }
            else if (m_IsUpDown != 1)
            {
                _Position += Vector3.up * 0.1f * Time.deltaTime;

                if (_Position.y >= 0.3f)
                    m_IsUpDown = 1;
            }

            transform.localPosition = _Position;
        }

        // 해당 장비 및 신체 부위 오브젝트를 모두 비활성화합니다.

        if (m_pHandAnimator.gameObject.activeSelf == true)
            m_pHandAnimator.gameObject.SetActive(false);

        if (m_pItemobject.activeSelf == true)
            m_pItemobject.SetActive(false);

        if (m_pLowerBody.AccessEquipItemObject.activeSelf == true)
            m_pLowerBody.AccessEquipItemObject.SetActive(false);

        if (m_pFoot.AccessEquipItemObject.activeSelf == true)
            m_pFoot.AccessEquipItemObject.SetActive(false);
    }

    public void ResetState()
    {
        if (m_pHandAnimator.gameObject.activeSelf == false)
            m_pHandAnimator.gameObject.SetActive(true);

        if (m_pItemobject.activeSelf == false)
            m_pItemobject.SetActive(true);

        if (m_pLowerBody.AccessEquipItemObject.activeSelf == false)
            m_pLowerBody.AccessEquipItemObject.SetActive(true);

        if (m_pFoot.AccessEquipItemObject.activeSelf == false)
            m_pFoot.AccessEquipItemObject.SetActive(true);

        m_bIsDead = false;

        m_IsUpDown = 0;

        m_pAnimator.SetBool("DEAD", false);

        m_eAvatarState = AVATARSTATES.AVATAR_IDLE;

        transform.localPosition = new Vector3(0.005f, -0.084f);
    }

    public override void SetItem(ITEM _Item)
    {
        // 최초에는 장비창에 정보만 받게한다.

        if (null == _Item)
            return;

        if (m_pOrlItem == null || m_pOrlItem.name != _Item.name)
        {
            GameObject _pCreateObject = GameObject.Instantiate(_Item.gameObject);

            _pCreateObject.transform.SetParent(gameObject.transform);

            _pCreateObject.transform.localScale = new Vector3(1.0f, 1.0f);

            _pCreateObject.transform.localRotation = Quaternion.identity;

            _pCreateObject.transform.localPosition = new Vector3(0.0f, 0.0f);

            ITEM _pItem = _pCreateObject.GetComponent<ITEM>();

            if (m_pItemobject != null)
            {
                m_pItemobject.transform.SetParent(null);

                Destroy(m_pItemobject);

                m_pItemobject = null;

                m_pItem = null;
            }
            
            _pItem.AccessSetItem = _Item;

            m_pItemobject = _pCreateObject;

            m_pItem = _pItem;

            m_pOrlItem = _Item;
        }
    }

    private void OnDestroy()
    {
        m_pHandRenderer = null;

        m_pPlayer = null;

        m_pHandAnimator = null;

        m_pAnimator = null;

        m_pHeadRenderer = null;

        m_pFaceRenderer = null;

        m_pHairRenderer = null;

        m_pLowerBody = null;

        m_pFoot = null;
    }
}