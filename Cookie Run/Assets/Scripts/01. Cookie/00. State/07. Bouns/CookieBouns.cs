using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//------------------------------
// 쿠기 보너스 상태 관리 클래스
//------------------------------
public class CookieBouns : CookieState
{
    public CookieBouns(Cookie _MyCookie)
        : base(_MyCookie)
    {
        m_eCookieState = COOKSTATE.COOKIE_BOUNS_MODE;

        m_pAnimator = _MyCookie.GetComponent<Animator>();

        m_pRigidbody2D = _MyCookie.GetComponent<Rigidbody2D>();

        m_pSoundManaged = CTopManager.GetInstance().GetSoundManaged;
        m_pSoundManaged.PlayBGM("BounsTime");
    }

    public override bool Action()
    {
        ResetAllAnimatorTriggr();

        m_pRigidbody2D.isKinematic = true;
        m_pRigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY | RigidbodyConstraints2D.FreezeRotation;
        m_pCookie.SetCookieControl(true);

        m_pAnimator.SetTrigger("Bouns");

        return true;
    }

    public override void Reset()
    {
        m_pCookie.SetCookieControl(false);

        m_pAnimator.SetTrigger("Landing");

        CookieNomral _NomralState = new CookieNomral(m_pCookie);

        m_pCookie.SetCookieState(_NomralState);
    }

    private Rigidbody2D m_pRigidbody2D = null;

    private CSoundManaged m_pSoundManaged = null;
}