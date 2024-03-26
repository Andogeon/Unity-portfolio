using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using Mono.Data.Sqlite;
using UnityEngine;
using UnityEngine.Networking;

public class CookieBooster : CookieState
{
    public CookieBooster(Cookie _MyCookie)
        : base(_MyCookie)
    {
        m_pAnimator = _MyCookie.GetComponent<Animator>();

        //CTopManager.GetInstance().Initialize();

        m_pPoolManaged = CTopManager.GetInstance().GetPoolManaged;

        m_pSceneManaged = CTopManager.GetInstance().GetSceneManaged;

        m_vOffset = Vector3.left * 0.5f;

        m_pBoosterEffectParent = _MyCookie.AccessBoosterParent;

        m_pSceneManaged.GetGameScene.SetSpeed(15.0f);

        m_pSoundManaged = CTopManager.GetInstance().GetSoundManaged;
        m_pSoundManaged.PlaySound("Booster Sound");
    }

    public override bool Action()
    {
        m_fTimeAcc += Time.deltaTime;

        if (!m_pAnimator.GetBool("Booster"))
            m_pAnimator.SetBool("Booster", true);

        if (m_fTimeAcc >= 7.0f)
            return false;

        if (m_pPrevSkiil != null)
            m_pPrevSkiil.Action();

        CreateEffect();

        return true;
    }

    public override void Reset()
    {
        m_pAnimator.SetBool("Booster", false);

        m_fTimeAcc = 0.0f;

        CBoosterEffect.ResetSortNumber();

        m_pSceneManaged.GetGameScene.ResetSpeed();

        m_pCookie.SetCookieSkillMode(CGlobalEnum.COOKIE_SKILL.COOKIE_INVIN);
    }

    public void ResetTime()
    {
        m_fTimeAcc = 0.0f;
    }

    private void CreateEffect()
    {
        m_fEffectTimeAcc += Time.deltaTime;

        if (m_fEffectTimeAcc >= 0.05f)
        {
            m_pPoolManaged.pop("Booster", m_pCookie.transform.position + (Vector3.left * 0.5f));

            m_fEffectTimeAcc = 0.0f;
        }
    }

    private float m_fTimeAcc = 0.0f;

    private float m_fEffectTimeAcc = 0.0f;

    private Vector3 m_vOffset = Vector3.zero;

    private CookieState m_pPrevSkiil = null;

    private CPoolManaged m_pPoolManaged = null;

    private CSceneManaged m_pSceneManaged = null;

    private CSoundManaged m_pSoundManaged = null;

    private Transform m_pBoosterEffectParent = null;
}

