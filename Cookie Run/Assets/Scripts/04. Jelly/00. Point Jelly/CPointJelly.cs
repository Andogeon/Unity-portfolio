using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPointJelly : CJelly
{
    private JellyPoint m_pJellyPoint = null;

    private CSoundManaged m_pSoundManaged = null;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        ComponentInit();
    }

    protected override void ComponentInit()
    {
        if(m_pJellyPoint == null)
            m_pJellyPoint = FindObjectOfType<JellyPoint>();

        SetJellyAnimation();

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
            case "Yellow Wing Bear Jelly":
                m_pAnimator.SetTrigger("YellowWing");
                break;
            case "Pink Wing Bear Jelly":
                m_pAnimator.SetTrigger("PinkWing");
                break;
            case "Rainbow Bear Jelly":
                m_pAnimator.SetTrigger("RainBow");
                break;
        }
    }

    protected override void SetJellyFunction()
    {
        switch (gameObject.name)
        {
            case "Blue Jelly":
                m_pJellyPoint.SetEatScoreNumber(1000);
                m_pSoundManaged.PlaySound("GetJelly");
                break;
            case "Nomral Bear Jelly":
                m_pJellyPoint.SetEatScoreNumber(1200);
                m_pSoundManaged.PlaySound("GetJelly");
                break;
            case "Pink Bear Jelly":
                m_pJellyPoint.SetEatScoreNumber(1100);
                m_pSoundManaged.PlaySound("GetJelly");
                break;
            case "Yellow Wing Bear Jelly":
                m_pJellyPoint.SetEatScoreNumber(1600);
                m_pSoundManaged.PlaySound("GetJelly");
                break;
            case "Pink Wing Bear Jelly":
                m_pJellyPoint.SetEatScoreNumber(2100);
                m_pSoundManaged.PlaySound("GetJelly");
                break;
            case "Big Bear Jelly":
                m_pJellyPoint.SetEatScoreNumber(5100);
                m_pSoundManaged.PlaySound("BigBearJelly");
                break;
            case "Rainbow Bear Jelly":
                m_pJellyPoint.SetEatScoreNumber(3100);
                m_pSoundManaged.PlaySound("BigBearJelly");
                break;
            case "Light Blue Bear Jelly":
                m_pJellyPoint.SetEatScoreNumber(1300);
                m_pSoundManaged.PlaySound("GetJelly");
                break;
        }

        
    }

    private void OnEnable()
    {
        ComponentInit();
    }
}
