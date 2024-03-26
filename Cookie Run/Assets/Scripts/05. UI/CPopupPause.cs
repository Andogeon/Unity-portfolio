using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CPopupPause : MonoBehaviour
{
    private Cookie m_pCookie = null;

    private CookieHeartGauge m_pHeartGauge = null;

    private CResultPanel m_pResultPanel = null;

    private Button[] m_pButtons = null;

    private CSoundManaged m_pSoundManager = null;

    private void Awake()
    {
        GameObject _pFindGameObject = GameObject.Find("Cookie");

        if (_pFindGameObject)
            m_pCookie = _pFindGameObject.GetComponent<Cookie>();

        _pFindGameObject = transform.Find("Btn Group")?.gameObject;

        if (_pFindGameObject)
        {
            m_pButtons = _pFindGameObject.GetComponentsInChildren<Button>();

            int _iIndex = 0;
            m_pButtons[_iIndex++].onClick.AddListener(new UnityAction(OnClickRestartGame));
            m_pButtons[_iIndex++].onClick.AddListener(new UnityAction(OnClickResetGame));
            m_pButtons[_iIndex].onClick.AddListener(new UnityAction(OnClickGameEnd));
        }

        m_pHeartGauge = FindObjectOfType<CookieHeartGauge>();

        _pFindGameObject = GameObject.Find("Canvas");

        m_pResultPanel = _pFindGameObject.transform.Find("Result Panel").GetComponent<CResultPanel>();

        m_pSoundManager = CTopManager.GetInstance().GetSoundManaged;
    }

    private void OnClickRestartGame() => m_pCookie.StartCoroutine(StartGameRestart());

    private IEnumerator StartGameRestart()
    {
        m_pSoundManager.PlaySound("ClickBtn");

        gameObject.SetActive(false);

        int _iTimeCount = 3;

        while(_iTimeCount > 0)
        {
            m_pCookie.SetCookieTime(_iTimeCount);

            string _strSoundName = string.Empty;

            switch(_iTimeCount)
            {
                case 3:
                    _strSoundName = "TTime";
                    break;
                case 2:
                    _strSoundName = "STime";
                    break;
                case 1:
                    _strSoundName = "FTime";
                    break;
            }

            --_iTimeCount;

            m_pSoundManager.PlaySound(_strSoundName);

            yield return new WaitForSecondsRealtime(1.0f);
        }

        m_pSoundManager.PlaySound("GameStart");

        GameRestart();
    }

    private void GameRestart()
    {
        CGlobalValues.m_bIsPause = false;

        m_pCookie.CookiePlay();
        m_pCookie.DisableTimer();

        m_pHeartGauge.PlayHeartGauge();
        m_pSoundManager.RestartBGM();
    }

    private void OnClickResetGame()
    {
        m_pSoundManager.StopBGM();

        GameRestart();

        m_pResultPanel.OnClickRestart();

        gameObject.SetActive(false);
    }

    private void OnClickGameEnd()
    {
        m_pSoundManager.PlaySound("ClickBtn");

        m_pCookie.ActiveGameResult();

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        CGlobalValues.m_bIsPause = true;

        m_pHeartGauge.PauseHeartGauge();

        m_pCookie.CookiePause();

        m_pSoundManager.PauseBGM();
    }
}