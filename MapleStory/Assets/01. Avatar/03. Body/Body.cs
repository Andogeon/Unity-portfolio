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

public class Body : PART
{
    private Animator m_pAnimator = null;

    private Animator m_pHandAnimator = null;

    private AVATARSTATES m_eAvatarState = AVATARSTATES.AVATAR_IDLE;

    private string m_strAttackName = string.Empty;

    private Player m_pPlayer = null;

    private SpriteRenderer m_pHandRenderer = null;

    private SpriteRenderer m_pHeadRenderer = null;

    private SpriteRenderer m_pFaceRenderer = null;

    private SpriteRenderer m_pHairRenderer = null;

    private LowerBody m_pLowerBody = null;

    private Foot m_pFoot = null;

    private string m_PlayerName = string.Empty;

    private bool m_bIsDead = false;

    private int m_bIsUpDown = 0;

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

        m_pHandAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();

        m_pHandRenderer = m_pHandAnimator.GetComponent<SpriteRenderer>();

        m_pPlayer = FindObjectOfType<Player>();

        if (m_pPlayer != null)
        {
            m_pHeadRenderer = m_pPlayer.transform.Find("Head").GetComponent<SpriteRenderer>(); // nullptr;

            m_pFaceRenderer = m_pHeadRenderer.transform.Find("Face").GetComponent<SpriteRenderer>();

            m_pHairRenderer = m_pHeadRenderer.transform.Find("Hair").GetComponent<SpriteRenderer>();

            m_pLowerBody = m_pPlayer.transform.Find("Lower Body").gameObject.GetComponent<LowerBody>();

            m_pFoot = m_pPlayer.transform.Find("Foot").gameObject.GetComponent<Foot>();
        }
        // 머리에서 다 가지고 와야 함 !! 
    }

    //private void Start()
    //{
    //    m_pSpriteRenderer = GetComponent<SpriteRenderer>();

    //    m_pAnimator = GetComponent<Animator>();

    //    m_pHandAnimator = transform.GetChild(0).gameObject.GetComponent<Animator>();

    //    m_pHandRenderer = m_pHandAnimator.GetComponent<SpriteRenderer>();

    //    m_pPlayer = FindObjectOfType<Player>();

    //    m_pHeadRenderer = m_pPlayer.transform.Find("Head").GetComponent<SpriteRenderer>(); // nullptr;

    //    // 머리에서 다 가지고 와야 함 !! 
    //}

    private void Update()
    {
        if (null != m_pItem)
            m_pItem.AvatarState = m_eAvatarState; // 이 녀석이 갱신이 안된다 ??

        
        if (transform.parent.name == "Select Avatar")
        {
            SelectAnimation();

            return;
        }

        AnimationUpdate();

        //Debug.Log(m_eAvatarState); // 현재 아이들 

        //Chanking();
    }

    private void AnimationUpdate()
    {
        //switch (m_eAvatarState)
        //{
        //    case AVATARSTATES.AVATAR_IDLE:
        //        m_pAnimator.SetBool("JUMP", false);
        //        m_pHandAnimator.SetBool("JUMP", false);
        //        m_pAnimator.SetBool("RUN", false);
        //        m_pHandAnimator.SetBool("RUN", false);
        //        m_pAnimator.SetBool("ATTACK", false);
        //        m_pHandAnimator.SetBool("ATTACK", false);
        //        m_pAnimator.SetBool("LADDER", false);
        //        m_pAnimator.SetBool("LADDER RUN", false);
        //        if (m_pHandAnimator.gameObject.activeSelf == false)
        //            m_pHandAnimator.gameObject.SetActive(true);
        //        if (m_pSpriteRenderer.color != Color.white)
        //            m_pSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f);
        //        m_strAttackName = string.Empty;
        //        break;
        //    case AVATARSTATES.AVATAR_RUN:

        //        m_pAnimator.SetBool("JUMP", false);
        //        m_pHandAnimator.SetBool("JUMP", false);
        //        m_pAnimator.SetBool("RUN", true);
        //        m_pHandAnimator.SetBool("RUN", true);
        //        m_pAnimator.SetBool("LADDER", false);
        //        m_pAnimator.SetBool("LADDER RUN", false);
        //        m_pAnimator.SetBool("ATTACK", false);
        //        m_pHandAnimator.SetBool("ATTACK", false);
        //        if (m_pHandAnimator.gameObject.activeSelf == false)
        //            m_pHandAnimator.gameObject.SetActive(true);
        //        m_strAttackName = string.Empty;
        //        break;
        //    case AVATARSTATES.AVATAR_JUMP:
        //        m_strAttackName = string.Empty;
        //        m_pAnimator.SetBool("RUN", false);
        //        m_pHandAnimator.SetBool("RUN", false);
        //        m_pAnimator.SetBool("JUMP", true);
        //        m_pHandAnimator.SetBool("JUMP", true);
        //        m_pAnimator.SetBool("LADDER", false);
        //        m_pAnimator.SetBool("LADDER RUN", false);
        //        if (m_pHandAnimator.gameObject.activeSelf == false)
        //            m_pHandAnimator.gameObject.SetActive(true);
        //        m_strAttackName = string.Empty;
        //        break;
        //    case AVATARSTATES.AVATAR_LADDER:
        //        m_pAnimator.SetBool("LADDER", true);
        //        m_pAnimator.SetBool("LADDER RUN", false);
        //        m_pAnimator.SetBool("ATTACK", false);
        //        m_pHandAnimator.SetBool("ATTACK", false);
        //        m_strAttackName = string.Empty;
        //        break;
        //    case AVATARSTATES.AVATAR_LADDERRUN:
        //        m_pAnimator.SetBool("JUMP", false);
        //        m_pHandAnimator.SetBool("JUMP", false);
        //        m_pAnimator.SetBool("RUN", false);
        //        m_pHandAnimator.SetBool("RUN", false);
        //        m_pAnimator.SetBool("LADDER RUN", true);
        //        m_pAnimator.SetBool("ATTACK", false);
        //        m_pHandAnimator.SetBool("ATTACK", false);
        //        m_strAttackName = string.Empty;
        //        break;
        //    case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
        //        m_pAnimator.SetBool("RUN", false);
        //        m_pHandAnimator.SetBool("RUN", false);
        //        m_pAnimator.SetBool("JUMP", false);
        //        m_pHandAnimator.SetBool("JUMP", false);
        //        m_pAnimator.SetBool("ATTACK", true);
        //        m_pHandAnimator.SetBool("ATTACK", true);
        //        m_pAnimator.SetInteger("ATTACK MODE", 1);
        //        m_pHandAnimator.SetInteger("ATTACK MODE", 1);
        //        if (m_strAttackName == string.Empty)
        //            m_strAttackName = "FIRST ATTACK";
        //        break;
        //    case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
        //        m_pAnimator.SetBool("RUN", false);
        //        m_pHandAnimator.SetBool("RUN", false);
        //        m_pAnimator.SetBool("JUMP", false);
        //        m_pHandAnimator.SetBool("JUMP", false);
        //        m_pAnimator.SetBool("ATTACK", true);
        //        m_pHandAnimator.SetBool("ATTACK", true);
        //        m_pAnimator.SetInteger("ATTACK MODE", 2);
        //        m_pHandAnimator.SetInteger("ATTACK MODE", 2);
        //        if (m_strAttackName == string.Empty)
        //            m_strAttackName = "SECOND ATTACK";
        //        break;
        //    case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
        //        m_pAnimator.SetBool("RUN", false);
        //        m_pHandAnimator.SetBool("RUN", false);
        //        m_pAnimator.SetBool("JUMP", false);
        //        m_pHandAnimator.SetBool("JUMP", false);
        //        m_pAnimator.SetBool("ATTACK", true);
        //        m_pHandAnimator.SetBool("ATTACK", true);
        //        m_pAnimator.SetInteger("ATTACK MODE", 3);
        //        m_pHandAnimator.SetInteger("ATTACK MODE", 3);
        //        if (m_strAttackName == string.Empty)
        //            m_strAttackName = "THIRD ATTACK";
        //        break;
        //    case AVATARSTATES.AVATAR_HIT:
        //        m_pAnimator.SetBool("RUN", false);
        //        m_pHandAnimator.SetBool("RUN", false);
        //        m_pAnimator.SetBool("LADDER", false);
        //        m_pAnimator.SetBool("LADDER RUN", false);
        //        m_pAnimator.SetBool("ATTACK", false);
        //        m_pHandAnimator.SetBool("ATTACK", false);
        //        m_pAnimator.SetBool("JUMP", true);
        //        m_pHandAnimator.SetBool("JUMP", true);
        //        Hit();
        //        m_strAttackName = string.Empty;
        //        break;
        //    case AVATARSTATES.AVATAR_DEAD:
        //        m_pAnimator.SetBool("RUN", false);
        //        m_pHandAnimator.SetBool("RUN", false);
        //        m_pAnimator.SetBool("LADDER", false);
        //        m_pAnimator.SetBool("LADDER RUN", false);
        //        m_pAnimator.SetBool("ATTACK", false);
        //        m_pHandAnimator.SetBool("ATTACK", false);
        //        m_pAnimator.SetBool("JUMP", false);
        //        m_pHandAnimator.SetBool("JUMP", false);
        //        Dead();
        //        m_strAttackName = string.Empty;
        //        break;
        //}

        switch (m_eAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
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
            case AVATARSTATES.AVATAR_RUN:
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
            case AVATARSTATES.AVATAR_JUMP:
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
            case AVATARSTATES.AVATAR_LADDER:
                m_pAnimator.SetBool("LADDER", true);
                m_pAnimator.SetBool("LADDER RUN", false);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                m_strAttackName = string.Empty;
                break;
            case AVATARSTATES.AVATAR_LADDERRUN:
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("LADDER RUN", true);
                m_pAnimator.SetBool("ATTACK", false);
                m_pHandAnimator.SetBool("ATTACK", false);
                m_strAttackName = string.Empty;
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                if (m_strAttackName != string.Empty)
                    break;
                m_strAttackName = "FIRST ATTACK";
                m_pAnimator.SetBool("RUN", false);
                m_pHandAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("JUMP", false);
                m_pHandAnimator.SetBool("JUMP", false);
                m_pAnimator.SetBool("ATTACK", true);
                m_pHandAnimator.SetBool("ATTACK", true);
                m_pAnimator.SetInteger("ATTACK MODE", 1);
                m_pHandAnimator.SetInteger("ATTACK MODE", 1);
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
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
                m_strAttackName = "SECOND ATTACK";
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
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
                m_strAttackName = "THIRD ATTACK";
                break;
            case AVATARSTATES.AVATAR_HIT:
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
            case AVATARSTATES.AVATAR_DEAD:
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

    private void SelectAnimation()
    {
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

    private bool IsEndAnimation(string _AnimationName)
    {
        bool _AnimationFinal = false;

        _AnimationFinal = m_pAnimator.GetCurrentAnimatorStateInfo(0).IsName(_AnimationName) && m_pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.99f;

        return _AnimationFinal;
    }

    private void Chanking()
    {
        if((int)m_eAvatarState >= (int)AVATARSTATES.AVATAR_FIRSTNORMALATTACK)
        {
            bool _bIsIdle = IsEndAnimation(m_strAttackName);

            if (true == _bIsIdle)
            {
                m_eAvatarState = AVATARSTATES.AVATAR_IDLE;

                m_pPlayer.AccessPlayerState = PLAYERSTATE.PLAYER_IDLE;

                m_pPlayer.AccessAttack = false;

                m_strAttackName = string.Empty;
            }
        }
    }

    private void Hit() // 함수 호출 까지는 되는데 .. 여기서 색상 정보를 확인 해야 한다 ?!
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

    private void Dead()
    {
        if (m_bIsDead == false)
        {
            m_pAnimator.SetBool("DEAD", true);

            m_bIsDead = true;
        }

        if(m_bIsDead == true)
        {
            Vector3 _Position = transform.localPosition;

            if (m_bIsUpDown == 1)
            {
                _Position += Vector3.down * 0.1f * Time.deltaTime;

                if (_Position.y <= 0.0f)
                    m_bIsUpDown = 2;
            }
            else if (m_bIsUpDown != 1)
            {
                _Position += Vector3.up * 0.1f * Time.deltaTime;

                if (_Position.y >= 0.3f)
                    m_bIsUpDown = 1;
            }

            transform.localPosition = _Position;
        }

        if (m_pHandAnimator.gameObject.activeSelf == true)
            m_pHandAnimator.gameObject.SetActive(false);

        if (m_pItemobject.activeSelf == true)
            m_pItemobject.SetActive(false);

        if (m_pLowerBody.AccessEquipItemObject.activeSelf == true)
            m_pLowerBody.AccessEquipItemObject.SetActive(false);

        //if (m_pLowerBody.AccessEquipItemObject.activeSelf == true)
        //    m_pLowerBody.AccessEquipItemObject.SetActive(false);

        if (m_pFoot.AccessEquipItemObject.activeSelf == true)
            m_pFoot.AccessEquipItemObject.SetActive(false);

        // 여기서 문제생길시 .. 아이템 비활성화 한다 .. 등...
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

        m_bIsUpDown = 0;

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