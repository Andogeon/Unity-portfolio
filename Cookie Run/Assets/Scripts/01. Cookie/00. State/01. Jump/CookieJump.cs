using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class CookieJump : CookieState
{
    public CookieJump(Cookie _MyCookie)
        :base(_MyCookie)
    {
        m_pCookieRigidbody = m_pCookie.GetComponent<Rigidbody2D>();

        m_pAnimator = m_pCookie.GetComponent<Animator>();

        m_eCookieState = COOKSTATE.COOKIE_JUMP;

        m_pSoundManager = CTopManager.GetInstance().GetSoundManaged;
    }

    public override bool Action()
    {
        if (m_pCookie.AccessCookieInfo.m_iJumpCount > 1)
            return false;

        ResetAllAnimatorTriggr();

        if (m_pCookie.AccessPrevCookieState == null || m_pCookie.AccessPrevCookieState.GetCookieState != COOKSTATE.COOKIE_BOUNS_MODE)
        {
            m_pCookieRigidbody.isKinematic = false;
            m_pCookieRigidbody.velocity = Vector2.up * m_pCookie.AccessCookieInfo.m_JumpPower;

            int _iJumpCount = (m_pCookie.AccessCookieInfo.m_iJumpCount >= 2) ? 0 : m_pCookie.AccessCookieInfo.m_iJumpCount;
            m_pCookie.SetJumpCount(_iJumpCount + 1);

            m_strAnimationTriggrName = (m_pCookie.AccessCookieInfo.m_iJumpCount > 1) ? "Double Jump" : "Jump";
            m_pSoundManager.PlaySound("Jump");
            m_eCookieState = (m_strAnimationTriggrName == "Jump") ? COOKSTATE.COOKIE_JUMP : COOKSTATE.COOKIE_DOUDLE_JUMP;
            m_pAnimator.SetTrigger(m_strAnimationTriggrName);
        }
        else
            m_pCookieRigidbody.velocity = Vector2.up * m_pCookie.AccessCookieInfo.m_fBounsMovePower;

        return true;
    }

    public override void Reset()
    {
        CookieNomral _NomralState = new CookieNomral(m_pCookie);

        m_pCookie.SetCookieState(_NomralState);
        m_pCookie.SetCookieControl(false);

        m_pAnimator.SetTrigger("Landing");
    }

    public void ExchangeJumpMode()
    {
        if (m_eCookieState == COOKSTATE.COOKIE_BOUNS_MOVE)
            return;

        m_eCookieState = COOKSTATE.COOKIE_BOUNS_MOVE;

        m_pCookie.SetJumpCount(0);
    }

    public string GetAnimaionName { get => m_strAnimationTriggrName; }

    private Rigidbody2D m_pCookieRigidbody = null;

    private string m_strAnimationTriggrName = string.Empty;

    private CSoundManaged m_pSoundManager = null;
}


//public override void Reset()
//{
//    //if (m_eCookieState == COOKSTATE.COOKIE_JUMP && m_eCookieState == COOKSTATE.COOKIE_DOUDLE_JUMP &&
//    //    m_pCookie.AccessCookieInfo.m_iJumpCount == 0)
//    //    return;

//    m_pAnimator.SetTrigger("Landing");

//    CookieNomral _NomralState = new CookieNomral(m_pCookie);

//    m_pCookie.SetCookieState(_NomralState);
//    m_pCookie.SetCookieControl(false);
//}