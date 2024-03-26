using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Networking;

public class CCookieScale : CookieState
{
    public CCookieScale(Cookie _MyCookie) 
        : base(_MyCookie)
    {
        m_pSoundManager = CTopManager.GetInstance().GetSoundManaged;
        m_pSoundManager.PlaySound("Power Up");
    }

    public override bool Action()
    {
        m_fCoolTime += Time.deltaTime;

        if (m_fCoolTime >= 10.0f && m_bIsScale)
        {
            m_bIsScale = false;

            m_fTimeAcc = 0.0f;

            m_pSoundManager.PlaySound("Power Down");
        }

        if(m_bIsScale)
        {
            if (m_fTimeAcc >= 1.0f)
                return true;

            m_fTimeAcc += (Time.deltaTime * 0.05f);

            Vector3 _vScale = Vector3.Lerp(m_pCookie.transform.localScale, Vector3.one * 2.0f, m_fTimeAcc);

            m_pCookie.transform.localScale = _vScale;            

            return true;
        }
        else
        {
            m_fTimeAcc += (Time.deltaTime * 1.6f);

            Vector3 _vScale = Vector3.Lerp(Vector3.one * 2.0f, Vector3.one * 1.25f, m_fTimeAcc);

            m_pCookie.transform.localScale = _vScale;

            if (m_fTimeAcc >= 1.0f || m_pCookie.transform.localScale.x <= 1.25f)
            {
                m_fTimeAcc = 0.0f;

                return false;
            }
        }

        return true;
    }

    public override void Reset()
    {
        m_fCoolTime = 0.0f;

        m_bIsScale = false;

        m_pCookie.SetCookieSkillMode(CGlobalEnum.COOKIE_SKILL.COOKIE_INVIN);
    }

    public void ResetTime()
    {
        m_fCoolTime = 0.0f;
    }

    private bool m_bIsScale = true;

    private float m_fTimeAcc = 0.0f;

    private float m_fCoolTime = 0.0f;

    private CSoundManaged m_pSoundManager = null;
}


