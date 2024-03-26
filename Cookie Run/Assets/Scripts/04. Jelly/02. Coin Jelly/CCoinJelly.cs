using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCoinJelly : CJelly
{
    private CCoinPoint m_pCoinPoint = null;

    private CSoundManaged m_pSoundManaged = null;

    protected override void Start()
    {
        base.Start();

        ComponentInit();
    }

    protected override void ComponentInit()
    {
        SetJellyAnimation();

        m_pCoinPoint = FindObjectOfType<CCoinPoint>();

        m_pSoundManaged = CTopManager.GetInstance().GetSoundManaged;
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.name == "Jelly Event Box")
            return;

        base.OnTriggerStay2D(other, SetJellyFunction);
    }

    protected override void SetJellyAnimation()
    {
        if (m_pAnimator == null)
            return;

        switch (gameObject.name)
        {
            case "Gold Coin":
                m_pAnimator.SetTrigger("Gold Coin");
                break;
            case "Big Coin":
                m_pAnimator.SetTrigger("Big Normal");
                break;
            case "Big Gold Coin":
                m_pAnimator.SetTrigger("Big Gold");
                break;
        }
    }

    protected override void SetJellyFunction()
    {
        switch (gameObject.name)
        {
            case "Coin":
                m_pCoinPoint.SetEatScoreNumber(1);
                m_pSoundManaged.PlaySound("GetCoin");
                break;
            case "Gold Coin":
                m_pCoinPoint.SetEatScoreNumber(10);
                m_pSoundManaged.PlaySound("GetCoin");
                break;
            case "Big Coin":
                m_pCoinPoint.SetEatScoreNumber(50);
                m_pSoundManaged.PlaySound("BigCoinJelly");
                break;
            case "Big Gold Coin":
                m_pCoinPoint.SetEatScoreNumber(100);
                m_pSoundManaged.PlaySound("BigCoinJelly");
                break;
        }
    }

    private void OnEnable()
    {
        ComponentInit();
    }
}
