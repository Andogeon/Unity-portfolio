using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class CResultPanel : MonoBehaviour
{
    private CResultPoint m_pResultPoint = null;

    private CSceneManaged m_pSceneManaged = null;

    private CBounsTime m_pBounsTime = null;

    private RectTransform m_pLightTransform = null;

    private Coroutine m_pRotationCorotine = null;

    private Coroutine m_pScoreCorotine = null;

    private Cookie m_pCookie = null;

    private CookieHeartGauge m_pHeartGarge = null;

    private CSoundManaged m_pSoundManaged = null;

    private void Start()
    {
        m_pResultPoint = GetComponentInChildren<CResultPoint>();

        Button[] _pResultBtns = GetComponentsInChildren<Button>();

        int _iIndex = 0;
        _pResultBtns[_iIndex++].onClick.AddListener(new UnityAction(OnClickRestart));
        _pResultBtns[_iIndex].onClick.AddListener(new UnityAction(OnClickGameEnd));

        m_pSceneManaged = CTopManager.GetInstance().GetSceneManaged;

        GameObject _pTextRotation = transform.Find("GameResultText").gameObject;
        m_pLightTransform = (RectTransform)_pTextRotation.transform.Find("Light");
        m_pRotationCorotine = StartCoroutine(LightRotation());

        m_pCookie = FindObjectOfType<Cookie>();
        m_pHeartGarge = FindObjectOfType<CookieHeartGauge>();
        m_pBounsTime = FindObjectOfType<CBounsTime>();
    }

    public void SetGameResultScore(int _iResultScore)
    {
        if(m_pScoreCorotine != null)
            StopCoroutine(m_pScoreCorotine);

        m_pScoreCorotine = StartCoroutine(StartResultScore(_iResultScore));
    }

    private IEnumerator LightRotation()
    {
        while(true)
        {
            Vector3 _vRotation = m_pLightTransform.eulerAngles;
            _vRotation += new Vector3(0.0f, 0.0f, 1.0f) * 5.0f * Time.deltaTime;
            m_pLightTransform.eulerAngles = _vRotation;

            yield return null;
        }
    }

    private IEnumerator StartResultScore(int _iResultScore)
    {
        if (_iResultScore == 0)
            yield break;

        if(m_pResultPoint == null)
            m_pResultPoint = GetComponentInChildren<CResultPoint>();

        int _iScore = 0;

        float _fScore = _iResultScore / 16;

        while(_iScore < _iResultScore)
        {
            _iScore += ((int)_fScore);
            m_pResultPoint.SetEatScoreNumber(_iScore);
            yield return new WaitForSeconds(0.1f);
        }

        if (_iScore >= _iResultScore)
            m_pResultPoint.SetEatScoreNumber(_iResultScore);
    }

    public void OnClickRestart() => GameRestart();

    private void GameRestart()
    {
        if (m_pSceneManaged == null)
            m_pSceneManaged = CTopManager.GetInstance().GetSceneManaged;

        if (m_pSoundManaged == null)
            m_pSoundManaged = CTopManager.GetInstance().GetSoundManaged;

        if (m_pBounsTime == null)
            m_pBounsTime = FindObjectOfType<CBounsTime>();

        m_pSoundManaged.PlaySound("ClickBtn");

        CGlobalValues.m_bIsGameOver = CGlobalValues.m_bIsPause = false;

        m_pSceneManaged.GameRestart();

        if (m_pCookie == null)
        {
            m_pCookie = FindObjectOfType<Cookie>();
            m_pHeartGarge = FindObjectOfType<CookieHeartGauge>();
        }

        m_pCookie.DisableTimer();
        m_pCookie.ResetCookie();
        m_pCookie.CookiePlay();
        m_pBounsTime.ResetBounsTime();
        m_pHeartGarge.PlayHeartGauge();

        if (m_pSoundManaged == null)
            m_pSoundManaged = CTopManager.GetInstance().GetSoundManaged;
        
        m_pSoundManaged.PlayBGM("GameScene");
    }

    private void OnClickGameEnd()
    {
        m_pSoundManaged.PlaySound("ClickBtn");

#if !UNITY_EDITOR
        Application.Quit();
#else
        EditorApplication.isPlaying = false;
#endif
    }

    private void OnEnable()
    {
        m_pSoundManaged = CTopManager.GetInstance().GetSoundManaged;
        m_pSoundManaged.StopBGM();
        m_pSoundManaged.PlaySound("GameResult");
    }

    private void OnDisable()
    {
        if(m_pRotationCorotine != null)
            StopCoroutine(m_pRotationCorotine);

        if(m_pScoreCorotine != null)
            StopCoroutine(m_pScoreCorotine);
        
        m_pLightTransform.eulerAngles = Vector3.zero;

        m_pResultPoint.ResetPoint();
    }
}
