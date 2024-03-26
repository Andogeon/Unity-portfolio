using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-------------------------------------------
// ���ʽ� Ÿ�� ������ Ŭ���� 
//-------------------------------------------
public class CBounsTime : MonoBehaviour
{
    [SerializeField] private float m_BounsTimeAcc = 20.0f;

    [SerializeField] private Transform[] m_BounsObjects = null;

    [SerializeField] private GameObject m_JellyEffect = null;

    private Image m_pBounsGauge = null;

    private Cookie m_pCookie = null;

    private CSceneManaged m_pSceneManaged = null;

    private JellyPoint m_pJellyPoint = null;

    private Dictionary<string, int> m_pBounsValue = new Dictionary<string, int>();

    private Coroutine m_pBounsTimerCoroutine = null;

    public Image GetBounsGauge { get => m_pBounsGauge; }
    public float GetBounsTime { get => m_BounsTimeAcc; }

    //---------------
    // ���� �ʱ�ȭ 
    //---------------
    private void Start()
    {
        m_pSceneManaged = CTopManager.GetInstance().GetSceneManaged;

        m_pBounsGauge = transform.GetChild(1).GetComponent<Image>();

        GameObject _pCookie = GameObject.Find("Cookie");

        if (null != _pCookie)
            m_pCookie = _pCookie.GetComponent<Cookie>();

        string[] _strBounsName = { "B Jelly", "O Jelly", "N Jelly", "U Jelly", "S Jelly", "T Jelly", "I Jelly", "M Jelly", "E Jelly" };

        for (int i = 0; i < _strBounsName.Length; ++i)
            m_pBounsValue.Add(_strBounsName[i], i);

        m_pJellyPoint = FindObjectOfType<JellyPoint>();
    }

    //-------------------------
    // ���ʽ� ������ ȹ�� �Լ�
    //-------------------------
    public void SetBounsItem(string _strJellyName)
    {
        int _iIndex = m_pBounsValue[_strJellyName];

        if (!m_BounsObjects[_iIndex].gameObject.activeSelf)
        {
            CGlobalFunc.CreateClone(m_JellyEffect, m_BounsObjects[_iIndex].localPosition, m_BounsObjects[_iIndex].parent);

            m_BounsObjects[_iIndex].gameObject.SetActive(true);
        }
        else
        {
            m_pJellyPoint.SetEatScoreNumber(1200);
            return;
        }

        if (CheckingBounsItem())
            StartBounsTime();
    }

    //------------------------------------------
    // ���ʽ� �������� ��� �𿴴��� Ȯ�� �Լ�
    //------------------------------------------
    private bool CheckingBounsItem()
    {
        foreach (Transform _pBounsIcon in m_BounsObjects)
        {
            if (_pBounsIcon == null || !_pBounsIcon.gameObject.activeSelf)
                return false;
        }

        return true;
    }

    //-------------------------------------
    // ���ʽ� ���� �ߺ� ���� Ȯ���ϴ� �Լ�
    //-------------------------------------
    public bool DuplicationBounsJelly(GameObject _pBounsJelly)
    {
        foreach (Transform _pBounsIcon in m_BounsObjects)
        {
            if(_pBounsIcon.gameObject.name == _pBounsJelly.name)
                return true;
        }

        return false;
    }

    //--------------------------
    // ���ʽ� Ÿ�� ��ü �Լ�
    //--------------------------
    public void StartBounsTime()
    {
        foreach (Transform _pBounsIcon in m_BounsObjects)
            _pBounsIcon.gameObject.SetActive(false);

        m_pBounsGauge.gameObject.SetActive(true);

        m_pCookie.ExchangeBounsMode();

        StartCoroutine(ExchangeBounsMap());
    }

    //------------------------------------------------
    // ������ ���� �� �������� ������ ���� ���� �Լ�
    //------------------------------------------------
    private ScenePrefab GetScenePrefab(string _StrSceneName)
    {
        if (m_pSceneManaged == null || _StrSceneName == string.Empty)
            return null;

        CResourceManaged _pResourceManaged = m_pSceneManaged.GetPoolManaged.GetResourceManaged;

        return _pResourceManaged.GetResoucre<ScenePrefab>(_StrSceneName);
    }

    //------------------------------------------
    // ���� ���� ���ʽ� �� ��ü�ϱ� ���� �ڷ�ƾ 
    //------------------------------------------
    private IEnumerator ExchangeBounsMap()
    {
        yield return null;

        Animator _pCookieAnimator = m_pCookie.AccessCookieState.GetAnimator;
        bool _bSceneExchange = false;

        while(_pCookieAnimator.GetCurrentAnimatorStateInfo(0).IsName("Bouns Change"))
        {
            Color _EffectColorp = m_pCookie.GetSpriteRenderer.color;

            if (_EffectColorp.r == 0.0f && _EffectColorp.g == 0
                && _EffectColorp.b == 0.0f && _EffectColorp.a >= 0.8f &&
                !_bSceneExchange)
            {
                _bSceneExchange = true;

                string _strBounsPrefabName = "Bonus Stage";

                ScenePrefab _pBounsPrefab = GetScenePrefab(_strBounsPrefabName);

                CSceneExchange _pSceneExchange = m_pSceneManaged.GetSceneExchange;
                _pSceneExchange.SetGameScene(_pBounsPrefab);
                m_pSceneManaged.SetActiveGameLineBox(true);
                m_pSceneManaged.ResetTileCycle();
            }

            yield return null;
        }

        Vector2 _vPosition = new Vector2(m_pCookie.GetRigidbody2D.position.x, 0.8f);

        m_pCookie.SetRigidbodyKinematic(false);
        m_pCookie.GetRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        m_pCookie.SetRigidBodyPositionAndDrag(_vPosition, 10);
        m_pCookie.SetCookieControl(false);

        m_pBounsTimerCoroutine = StartCoroutine(StartBounsTimer());
    }
    //--------------------------------------
    // ���� ���ʽ��� ���ư��� �ִ� �ڷ�ƾ
    //--------------------------------------
    private IEnumerator StartBounsTimer()
    {
        //foreach (Transform _pBounsIcon in m_BounsObjects)
        //    _pBounsIcon.gameObject.SetActive(false);

        float _fResetTimeAcc = m_BounsTimeAcc;

        while(_fResetTimeAcc > 0.0f)
        {
            _fResetTimeAcc -= Time.deltaTime;

            m_pBounsGauge.fillAmount = _fResetTimeAcc / m_BounsTimeAcc;

            yield return null;
        }

        m_pCookie.SetCookieControl(true);
        m_pCookie.AccessCookieState.SetStateMode(CookieState.COOKSTATE.COOKIE_BOUNS_RESET);

        Animator _pCookieAnimator = m_pCookie.AccessCookieState.GetAnimator;
        _pCookieAnimator.SetTrigger("Bouns Land");
        m_pCookie.GetRigidbody2D.drag = 0;

        m_pSceneManaged.SetActiveGameLineBox(false);
        ScenePrefab _pGameScenePrefab = GetScenePrefab("Stage 1");

        StartCoroutine(m_pSceneManaged.GetSceneExchange.FadeSceceChange(_pGameScenePrefab, m_pCookie, "GameScene"));
    }

    //----------------------------------
    // ���ʽ� Ÿ�� �ʱ�ȭ
    //----------------------------------
    public void ResetBounsTime()
    {
        foreach (Transform _pBounsIcon in m_BounsObjects)
            _pBounsIcon.gameObject.SetActive(false);

        m_pBounsGauge.fillAmount = 1.0f;
        m_pBounsGauge.gameObject.SetActive(false);

        m_pSceneManaged.SetActiveGameLineBox(false);

        StopAllCoroutines();

        m_pCookie.GetRigidbody2D.drag = 0;
    }
}
