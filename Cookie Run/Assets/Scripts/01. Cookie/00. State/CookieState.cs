using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Networking;

public abstract class CookieState
{
    // 쿠키의 상태
    public enum COOKSTATE
    {
        COOKIE_RUN,
        COOKIE_JUMP,
        COOKIE_DOUDLE_JUMP,
        COOKIE_SLIDER,
        COOKIE_HIT,
        COOKIE_BOOSTER,
        COOKIE_BOUNS_MODE,
        COOKIE_BOUNS_MOVE,
        COOKIE_BOUNS_RESET
    }

    public CookieState(Cookie _MyCookie)
    {
        m_pCookie = _MyCookie;
    }

    public abstract bool Action();

    public abstract void Reset();

    public void SetStateMode(COOKSTATE _eMode)
    {
        m_eCookieState = _eMode;
    }

    public void ResetAllAnimatorTriggr()
    {
        if (m_pAnimator == null)
            return;

        foreach (string _TriggerName in m_pAnimatorTriggerNames)
            m_pAnimator.ResetTrigger(_TriggerName);
    }

    public void ResetAllAnimator()
    {
        m_pAnimator.SetBool("Slide", false);
        m_pAnimator.SetBool("Booster", false);
    }

#region 프로퍼티
    public COOKSTATE GetCookieState { get => m_eCookieState; }
    public Animator GetAnimator { get => m_pAnimator; }
#endregion

#region 변수들
    protected Cookie m_pCookie = null;

    protected Animator m_pAnimator = null;

    protected string[] m_pAnimatorTriggerNames = { "Jump", "Double Jump", "Landing", "Slide", "Hit", "Run", "Bouns" };

    protected COOKSTATE m_eCookieState = COOKSTATE.COOKIE_RUN;
#endregion
}
