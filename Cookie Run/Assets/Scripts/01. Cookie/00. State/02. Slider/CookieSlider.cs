using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CookieSlider : CookieState
{
    public CookieSlider(Cookie _MyCookie)
        :base(_MyCookie)
    {
        m_pAnimator = m_pCookie.GetComponent<Animator>();

        m_pCookieRigidbody = m_pCookie.GetComponent<Rigidbody2D>();

        m_eCookieState = COOKSTATE.COOKIE_SLIDER;

        m_pSoundManager = CTopManager.GetInstance().GetSoundManaged;
        m_pSoundManager.PlaySound("Slide");
    }

    public override bool Action()
    {
        if (m_pCookieRigidbody.velocity.y > 0)
            return false;

        if (m_pAnimator.GetBool("Slide"))
            return false;

        m_pCookie.SetJumpCount(0);

        ResetAllAnimatorTriggr();

        m_pAnimator.SetBool("Slide", true);

        return true;
    }

    public override void Reset()
    {
        if (!m_pAnimator.GetBool("Slide"))
            return;

        ResetAllAnimatorTriggr();

        m_pAnimator.SetBool("Slide", false);

        CookieState _NomralState = new CookieNomral(m_pCookie);

        m_pCookie.SetCookieState(_NomralState);
    }

    private Rigidbody2D m_pCookieRigidbody = null;

    private CSoundManaged m_pSoundManager = null;
}

