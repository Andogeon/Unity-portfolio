using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CookieHeartGauge : MonoBehaviour
{
    [SerializeField] private RectTransform m_pGaugeTransform = null;

    [SerializeField] private Animator m_pHeartCircle = null;

    private Image m_pHpGaugeSprite = null;

    private Animator m_pLightAnimator = null;

    private Cookie m_pUserCookie = null;

    private float m_fXPosition = 0.0f;

    private void Awake()
    {
        m_pHpGaugeSprite = GetComponent<Image>();

        m_pUserCookie = GameObject.Find("Cookie").GetComponent<Cookie>();

        m_fXPosition = m_pGaugeTransform.anchoredPosition.x;

        Transform _pParentGauge = transform.parent;

        m_pLightAnimator = _pParentGauge.Find("Heart Light Effect").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_pHpGaugeSprite.fillAmount = m_pUserCookie.CookieHpGauge;

        float _XPosition = (m_pHpGaugeSprite.fillAmount >= 1.0f) ? (m_fXPosition * m_pHpGaugeSprite.fillAmount) : (m_fXPosition * m_pHpGaugeSprite.fillAmount) + 2.0f;

        if (m_pHpGaugeSprite.fillAmount <= 0.15f)
        {
            m_pHeartCircle.gameObject.SetActive(true);

            m_pHeartCircle.speed = m_pHpGaugeSprite.fillAmount <= 0.1f ? 1.5f : 1.0f;
        }
        else
        {
            if(m_pHeartCircle.gameObject.activeSelf)
                m_pHeartCircle.gameObject.SetActive(false);
        }

        Vector2 _WorldPosition = new Vector2(_XPosition, m_pHpGaugeSprite.rectTransform.anchoredPosition.y);

        m_pGaugeTransform.anchoredPosition = _WorldPosition;
    }

    public void PauseHeartGauge()
    {
        if (m_pHeartCircle.gameObject.activeSelf)
            m_pHeartCircle.enabled = false;

        m_pLightAnimator.enabled = false;
    }

    public void PlayHeartGauge()
    {
        if (m_pHeartCircle.gameObject.activeSelf)
            m_pHeartCircle.enabled = true;

        m_pLightAnimator.enabled = true;
    }
}
