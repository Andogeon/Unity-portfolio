using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CookieHit : CookieState
{
    public CookieHit(Cookie _MyCookie)
        :base(_MyCookie)
    {
        m_pAnimator = m_pCookie.GetComponent<Animator>();

        m_pRigidBody = m_pCookie.GetComponent<Rigidbody2D>();

        m_eCookieState = COOKSTATE.COOKIE_HIT;

        m_pSoundManager = CTopManager.GetInstance().GetSoundManaged;
        m_pSoundManager.PlaySound("Hit");
    }

    public override bool Action()
    {
        if (m_bIsHit)
            return false;

        m_pCookie.SetCookieHp(-10.0f);

        m_bIsHit = true;

        ResetAllAnimatorTriggr();

        m_pAnimator.SetTrigger("Hit");

        m_pCookie.StartCoroutine(AnimationHitTime());

        return true;
    }

    public override void Reset()
    {
        if (m_pCookie.AccessPrevCookieState != null && m_pCookie.AccessPrevCookieState is CookieJump)
        {
            CookieJump _pCookieJump = m_pCookie.AccessPrevCookieState as CookieJump;
            m_pCookie.SetCookieState(_pCookieJump);

            ResetAllAnimatorTriggr();

            if (_pCookieJump.GetAnimaionName == "Jump")
                m_pAnimator.SetTrigger(_pCookieJump.GetAnimaionName);
            else
            {
                Transform _pTilePatten = m_pCookie.GetTilePatten;

                Vector3 _vDirection = (Vector3)m_pRigidBody.position - _pTilePatten.localPosition;

                float _fLength = _vDirection.magnitude;

                if(_fLength >= 2.0f)
                    m_pAnimator.SetTrigger("Hit Jump");
            }

            return;
        }

        CookieState _CookieNormal = new CookieNomral(m_pCookie);

        m_pCookie.SetCookieState(_CookieNormal);

        _CookieNormal.Action();
    }

    private IEnumerator AnimationHitTime()
    {
        yield return null;

        if (m_pCookie.AccessCookieState.GetCookieState != CookieState.COOKSTATE.COOKIE_HIT)
            yield break;

        AnimatorStateInfo _HitTimeInfo = m_pAnimator.GetCurrentAnimatorStateInfo(0);

        if(!_HitTimeInfo.IsName("Hit"))
        {
            m_bIsHit = false;

            Reset();

            yield break;
        }

        if(_HitTimeInfo.normalizedTime >= 0.5f)
        {
            m_bIsHit = false;

            Reset();
        }
        else
            m_pCookie.StartCoroutine(AnimationHitTime());
    }

    private bool m_bIsHit = false;

    private Rigidbody2D m_pRigidBody = null;

    private CSoundManaged m_pSoundManager = null;
}