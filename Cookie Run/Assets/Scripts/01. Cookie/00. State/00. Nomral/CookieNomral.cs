using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CookieNomral : CookieState
{
    public CookieNomral(Cookie _MyCookie)
        :base(_MyCookie)
    {
        m_pAnimator = m_pCookie.GetComponent<Animator>();

        m_eCookieState = COOKSTATE.COOKIE_RUN;

        _MyCookie.SetJumpCount(0);
    }

    public override bool Action()
    {
        ResetAllAnimatorTriggr();

        return true;
    }

    public override void Reset()
    {
        
    }
}