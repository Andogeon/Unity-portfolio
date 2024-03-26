using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CookieDead : CookieState
{
    public CookieDead(Cookie _MyCookie)
       : base(_MyCookie)
    {
        m_eCookieState = COOKSTATE.COOKIE_RUN;

        m_pAnimator = _MyCookie.GetComponent<Animator>();

        m_pRigidbody2D = _MyCookie.GetComponent<Rigidbody2D>();

        m_pSpriteRenderder = _MyCookie.GetSpriteRenderer;
    }

    public override bool Action()
    {
        m_pRigidbody2D.bodyType = RigidbodyType2D.Static;

        m_pCookie.ResetCookieSkill(CGlobalEnum.COOKIE_SKILL.COOKIE_INVIN);

        ResetAllAnimatorTriggr();

        m_pAnimator.SetTrigger("Dead");

        return true;
    }

    public override void Reset()
    {

    }

    private Rigidbody2D m_pRigidbody2D = null;

    private SpriteRenderer m_pSpriteRenderder = null;
}
