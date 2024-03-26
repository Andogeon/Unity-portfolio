using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using UnityEngine;

public class CookieInvin : CookieState
{
    public CookieInvin(Cookie _MyCookie)
        : base(_MyCookie)
    {
        m_pAnimator = m_pCookie.GetComponent<Animator>();

        m_pCookieBox = m_pCookie.GetComponent<BoxCollider2D>();

        m_pSpriteRenderer = m_pCookie.GetComponent<SpriteRenderer>();
    }

    public override bool Action()
    {
        if (m_fTimeAcc >= 3.0f)
            return false;

        float _SwapAlpha = m_bIsSwap ? 0.9f : 0.5f;

        Color _CookieColor = new Color(m_pSpriteRenderer.color.r, m_pSpriteRenderer.color.g, m_pSpriteRenderer.color.b, _SwapAlpha);

        m_pSpriteRenderer.color = _CookieColor;

        m_bIsSwap = !m_bIsSwap;

        m_fTimeAcc += Time.deltaTime;

        return true;
    }

    public override void Reset()
    {
        m_pSpriteRenderer.color = new Color(m_pSpriteRenderer.color.r, m_pSpriteRenderer.color.g, m_pSpriteRenderer.color.b ,1.0f);

        m_fTimeAcc = 0.0f;
    }

    private BoxCollider2D m_pCookieBox = null;

    private SpriteRenderer m_pSpriteRenderer = null;   

    private float m_fTimeAcc = 0.0f;

    private bool m_bIsSwap = false;
}