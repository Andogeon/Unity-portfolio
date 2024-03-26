using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CJelly : MonoBehaviour
{
    protected virtual void Start()
    {
        ComponentInitialize();
    }

    private void ComponentInitialize()
    {
        m_pResourceManaged = CTopManager.GetInstance().GetResourceManaged;

        m_pPoolManaged = CTopManager.GetInstance().GetPoolManaged;

        m_pJellyGroup = CTopManager.GetInstance().GetSceneManaged.GetJellyParent;

        m_pCookie = FindObjectOfType<Cookie>();

        m_pAnimator = GetComponent<Animator>();
    }

    protected virtual void OnTriggerStay2D(Collider2D other, Callback_CollisionMessage _pCallback_Message)
    {
        float _fCookieXPosition = other.transform.position.x;
        float _fJellyXPosition = transform.position.x;

        if(m_pCookie.GetCookieEventBox.GetEventType == CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNET || 
           m_pCookie.GetSkill[((int)CGlobalEnum.COOKIE_SKILL.COOKIE_BOOSTER)] != null && m_pCookie.GetCookieEventBox.GetEventType == CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNET)
        {
            Vector2 _vDirection = other.transform.position - transform.position;
            float _fLength = _vDirection.magnitude;

            if (_fLength > 0.3f)
                return;

            string _strJellyEffectname = GetJellyName();

            GameObject _pJellyeffect = m_pPoolManaged.pop(_strJellyEffectname, transform.position, m_pJellyGroup);

            if (m_JellyEffectName.ToString() != "Jelly_Effect")
                SetJellyEffectAnimator(_pJellyeffect);

            if (null != _pCallback_Message)
                _pCallback_Message();

            gameObject.SetActive(false);

            return;
        }

        if(gameObject.name.Contains("Big"))
        {
            Vector2 _vDirection = other.transform.position - transform.position;
            float _fLength = _vDirection.magnitude;

            if (_fLength > 0.68f)
                return;

            string _strJellyEffectname = GetJellyName();

            GameObject _pJellyeffect = m_pPoolManaged.pop(_strJellyEffectname, transform.position, m_pJellyGroup);

            if (m_JellyEffectName.ToString() != "Jelly_Effect")
                SetJellyEffectAnimator(_pJellyeffect);

            if (null != _pCallback_Message)
                _pCallback_Message();

            gameObject.SetActive(false);

            return;
        }


        if (_fCookieXPosition < _fJellyXPosition)
            return;

        string _strJellyEffectName = GetJellyName();

        GameObject _pJellyEffect = m_pPoolManaged.pop(_strJellyEffectName, transform.position, m_pJellyGroup);

        if (m_JellyEffectName.ToString() != "Jelly_Effect")
            SetJellyEffectAnimator(_pJellyEffect);

        if (null != _pCallback_Message)
            _pCallback_Message();

        gameObject.SetActive(false);
    }

    private string GetJellyName()
    {
        return m_JellyEffectName.ToString().Replace("_", " ");
    }

    private void SetJellyEffectAnimator(GameObject _pGameObject)
    {
        if (null == _pGameObject)
            return;

        Animator _pAnimator = _pGameObject.GetComponent<Animator>();

        int _iCilpCount = _pAnimator.runtimeAnimatorController.animationClips.Length;

        if (_iCilpCount <= 1)
            return;

        _pAnimator.SetTrigger(m_JellyEffectName.ToString());
    }

    protected abstract void SetJellyAnimation();
    protected abstract void SetJellyFunction();

    protected abstract void ComponentInit();

    [SerializeField] protected CGlobalEnum.Jelly_Effect_Name m_JellyEffectName = CGlobalEnum.Jelly_Effect_Name.Jelly_Effect;

    protected Cookie m_pCookie = null;

    protected Animator m_pAnimator = null;

    private CPoolManaged m_pPoolManaged = null;

    private CResourceManaged m_pResourceManaged = null;

    private Transform m_pJellyGroup = null;
}


//public class CJelly : MonoBehaviour
//{
//    private void Start()
//    {
//        ComponentInitialize();
//    }

//    private void ComponentInitialize()
//    {
//        m_pResourceManaged = CTopManager.GetInstance().GetResourceManaged;

//        m_pPoolManaged = CTopManager.GetInstance().GetPoolManaged;

//        m_pJellyGroup = CTopManager.GetInstance().GetSceneManaged.GetJellyParent;

//        m_pCookie = FindObjectOfType<Cookie>();

//        m_pBounsTime = FindObjectOfType<CBounsTime>();

//        m_pAnimator = GetComponent<Animator>();

//        SetAnimatorLayer();
//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        float _fCookieXPosition = other.transform.position.x;

//        float _fJellyXPosition = transform.position.x;

//        if (_fCookieXPosition < _fJellyXPosition)
//            return;

//        string _strJellyEffectName = GetJellyName();

//        GameObject _pJellyEffect = m_pPoolManaged.pop(_strJellyEffectName, transform.position, m_pJellyGroup);

//        SetJellyFunction();

//        gameObject.SetActive(false);
//    }

//    private string GetJellyName()
//    {
//        return m_JellyEffectName.ToString().Replace("_", " ");
//    }

//    //----------------
//    // 젤리 효과 분류 
//    //----------------
//    private void SetJellyFunction()
//    {
//        switch (gameObject.name)
//        {
//            case "Booster Jelly":
//                m_pCookie.SetCookieSkillMode(CGlobalEnum.COOKIE_SKILL.COOKIE_BOOSTER);
//                break;
//            case "Bonus Jelly":
//                m_pBounsTime.StartBounsTime();
//                break;
//            case "Big Life Jelly":
//                break;
//            case "Mini Life Jelly":
//                break;
//            case "Power Up Jelly":
//                m_pCookie.SetCookieSkillMode(CGlobalEnum.COOKIE_SKILL.COOKIE_POWER_UP);
//                break;
//        }
//    }

//    private void SetAnimatorLayer()
//    {
//        switch (gameObject.name)
//        {
//            case "Booster Jelly":
//                m_pAnimator.SetTrigger("Booster");
//                break;
//            case "Big Life Jelly":
//                break;
//            case "Mini Life Jelly":
//                break;
//            case "Power Up Jelly":
//                m_pAnimator.SetTrigger("");
//                break;
//        }
//    }

//    [SerializeField] private CGlobalEnum.Jelly_Effect_Name m_JellyEffectName = CGlobalEnum.Jelly_Effect_Name.Jelly_Effect;

//    private CPoolManaged m_pPoolManaged = null;

//    private CResourceManaged m_pResourceManaged = null;

//    private Transform m_pJellyGroup = null;

//    private Cookie m_pCookie = null;

//    private CBounsTime m_pBounsTime = null;

//    private Animator m_pAnimator = null;
//}


















//public class EatItem : MonoBehaviour
//{
//    [SerializeField] private GameObject m_JellyEatEffect = null;

//    private EffectManaged m_pEffectManaged = null;

//    private BoxCollider2D m_pBoxCollider = null;

//    private Cookie m_pUserCookie = null;

//    private float m_fBoxHalfSize = 0.0f;

//    private float m_fBoxSize = 0.0f;

//    private Animator m_pAnimator = null;

//    public float GetBoxSize { get => m_fBoxSize; }

//    public float GetBoxHalfSize { get => m_fBoxHalfSize; }

//    public float GetEatLeftPoint { get => transform.localPosition.x - m_fBoxHalfSize; }

//    public float GetEatRightPoint { get => transform.localPosition.x + m_fBoxHalfSize; }

//    private void Awake()
//    {
//        //m_pEffectManaged = GameObject.Find("EffectGroup").GetComponent<EffectManaged>();

//        m_pAnimator = GetComponent<Animator>();

//        m_pBoxCollider = GetComponent<BoxCollider2D>();

//        m_pUserCookie = FindObjectOfType<Cookie>();

//        m_fBoxSize = m_pBoxCollider.size.x * transform.localScale.x;

//        m_fBoxHalfSize = m_fBoxSize / 2;

//        SetAnimatorLayer();
//    }

//    private void OnTriggerStay2D(Collider2D other)
//    {
//        Vector3 _CollisionPosition = new Vector3(other.transform.position.x, 0.0f, 0.0f);
//        Vector3 _MyPosition = new Vector3(transform.position.x, 0.0f, 0.0f);

//        if (_CollisionPosition.x < _MyPosition.x)
//            return;

//        Vector3 _Position = new Vector3(transform.position.x, transform.position.y + 0.2f, 0.0f);

//        //GameObject _JellyEffect = GameObject.Instantiate(m_JellyEatEffect, _Position, Quaternion.identity, null);

//        //m_pEffectManaged.InsertEffect(_JellyEffect, m_JellyEatEffect.transform.localScale);

//        //if (gameObject.name.Contains("Mini Life Jelly"))
//        //    m_pUserCookie.SetCookieHpDelta(1.0f);
//        //else if (gameObject.name.Contains("Big Life Jelly"))
//        //    m_pUserCookie.SetCookieHpDelta(10.0f);
//        //else
//        //    m_pUserCookie.SetJellyPoint(18);

//        //Destroy(gameObject);

//        gameObject.SetActive(false);
//    }

//    private void SetAnimatorLayer()
//    {
//        if (!m_pAnimator)
//            return;

//        switch (m_pAnimator.runtimeAnimatorController.name)
//        {
//            case "Big Bear Jelly":
//                SetBigBearJellyLayer();
//                break;
//            case "Coin Animator":
//                SetCoinLayer();
//                break;
//            case "Life Jelly Animator":
//                SetLifeLayer();
//                break;
//        }
//    }

//    private void SetBigBearJellyLayer()
//    {
//        switch (gameObject.name)
//        {
//            case "Rainbow Bear Jelly":
//                m_pAnimator.SetTrigger("RainBow");
//                break;
//            case "Pink Wing Bear Jelly":
//                m_pAnimator.SetTrigger("PinkWing");
//                break;
//            case "Yellow Wing Bear Jelly":
//                m_pAnimator.SetTrigger("YellowWing");
//                break;
//        }
//    }

//    private void SetCoinLayer()
//    {
//        switch (gameObject.name)
//        {
//            case "Big Gold Coin":
//                m_pAnimator.SetTrigger("Big Gold");
//                break;
//            case "Big Coin":
//                m_pAnimator.SetTrigger("Big Normal");
//                break;
//            case "Coin":
//                m_pAnimator.SetTrigger("Normal");
//                break;
//        }
//    }

//    private void SetLifeLayer()
//    {
//        if (gameObject.name.Contains("Mini Life Jelly"))
//            m_pAnimator.SetTrigger("Mini Life Jelly");
//    }
//}
