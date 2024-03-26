using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class CMainScene : MonoBehaviour
{
    [SerializeField] private string[] m_LoadingMessages = null;

    [SerializeField] private CSoundManaged m_SoundManaged = null;

    private EventTrigger m_pBtnTrigger = null;

    private GameObject m_pLoadingPanel = null;

    private Button m_pGameStartBtn = null;

    private Image m_pLoadingBar = null;

    private TextMeshProUGUI m_pLoadingMessage = null;

    private Animator m_pBtnAnimator = null;

    private AsyncOperation m_pSceneLoading = null;

    private float m_fTimeAcc = 0.0f;

    private const float m_fTimelimit = 2.0f;

    private int m_iIndex = 0;

    private void Awake()
    {
        Screen.SetResolution(1280, 720, false);
    }

    private void Start()
    {
        Transform _pGameStartBtn = transform.Find("GameStartBtn");

        if(_pGameStartBtn)
        {
            m_pGameStartBtn = _pGameStartBtn.GetComponent<Button>();
            m_pBtnTrigger = _pGameStartBtn.GetComponent<EventTrigger>();
            m_pBtnAnimator = _pGameStartBtn.GetComponent<Animator>();

            EventTrigger.Entry _pEntry = new EventTrigger.Entry();
            _pEntry.eventID = EventTriggerType.PointerUp;
            _pEntry.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
            m_pBtnTrigger.triggers.Add(_pEntry);

            _pEntry = new EventTrigger.Entry();
            _pEntry.eventID = EventTriggerType.PointerDown;
            _pEntry.callback.AddListener((data) => { OnPointerDown((PointerEventData)data); });
            m_pBtnTrigger.triggers.Add(_pEntry);

            m_pGameStartBtn.onClick.AddListener(new UnityAction(OnClickGameStart));
        }

        m_pLoadingMessage = GetComponentInChildren<TextMeshProUGUI>();
        m_pLoadingBar = transform.Find("Loading Bar")?.GetComponent<Image>();

        StartCoroutine(LoadingGameScene());

        _pGameStartBtn.gameObject.SetActive(false);

        m_pLoadingPanel = GameObject.Find("Canvas").transform.GetChild(1).gameObject;
        m_pLoadingPanel.SetActive(false);

        m_SoundManaged.Initialize();
        m_SoundManaged.PlayBGM("MainBGM");
    }

    private void OnPointerUp(PointerEventData eventData)
    {
        if (m_pBtnAnimator == null)
            return;

        m_pBtnAnimator.SetBool("Pressed", false);
    }

    private void OnPointerDown(PointerEventData eventData)
    {
        if (m_pBtnAnimator == null)
            return;

        m_pBtnAnimator.SetBool("Pressed", true);
    }

    private void OnClickGameStart()
    {
        m_SoundManaged.PlaySound("ClickBtn");
        
        if (m_pLoadingPanel == null || m_pLoadingPanel.activeSelf)
            return;

        m_pLoadingPanel.SetActive(true);

        m_SoundManaged.StopBGM();
        m_SoundManaged.PlaySound("GameLoading");

        StartCoroutine(StartChangeGameScene());
    }

    private IEnumerator LoadingGameScene()
    {
        string _strGameSceneName = "GameScene";

        m_pSceneLoading = SceneManager.LoadSceneAsync(_strGameSceneName);
        m_pSceneLoading.allowSceneActivation = false;

        while(true)
        {
            float _fProgress = m_pSceneLoading.progress;

            if(_fProgress >= 0.9f)
            {
                m_pLoadingBar.fillAmount = 1.0f;
                StartCoroutine(FakeLoading());
                yield break;
            }

            m_pLoadingBar.fillAmount = _fProgress;
            yield return null;
        }
    }

    private IEnumerator FakeLoading()
    {
        while(true)
        {
            m_fTimeAcc += Time.deltaTime;

            if (m_fTimeAcc >= m_fTimelimit)
            {
                m_fTimeAcc = 0.0f;

                m_iIndex = m_iIndex >= m_LoadingMessages.Length - 1 ? 0 : m_iIndex += 1;

                m_pLoadingMessage.text = m_LoadingMessages[m_iIndex];

                if(!m_pGameStartBtn.gameObject.activeSelf)
                    m_pGameStartBtn.gameObject.SetActive(true);
            }

            m_pLoadingBar.fillAmount = m_fTimeAcc / m_fTimelimit;

            yield return null;
        }
    }

    private IEnumerator StartChangeGameScene()
    {
        yield return new WaitForSeconds(2.5f);

        m_pSceneLoading.allowSceneActivation = true;
    }
}
