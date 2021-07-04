using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BW_Animations
{
    BW_IDLE,
    BW_NORMALATTACK,
    BW_ATTACKREADY,
    BW_HIT,
    BW_DEAD
};

public class BW_Animation : UE_Animation
{
    private BW_Animations m_eBowAnimation = BW_Animations.BW_IDLE;

    private bool m_bIsHit = false;

    private bool m_bIsDead = false;

    public BW_Animations BWAnimations
    {
        get { return m_eBowAnimation; }

        set { m_eBowAnimation = value; }
    }

    public BW_Animation(Animator _ObjectAnimator)
    {
        m_ObjectAnimator = _ObjectAnimator;
    }

    public bool GetHit
    {
        get { return m_bIsHit; }

        set { m_bIsHit = value; }
    }

    public override void UpDateAnimation(string[] _AnimationNames = null)
    {
        switch(m_eBowAnimation)
        {
            case BW_Animations.BW_IDLE:
                m_bIsHit = false;
                m_ObjectAnimator.SetBool("Hit", false);
                m_ObjectAnimator.SetBool("AttackReady", false);
                m_ObjectAnimator.SetBool("Attack", false);
                m_ObjectAnimator.SetBool("Idle", true);
                break;
            case BW_Animations.BW_ATTACKREADY:
                m_bIsHit = false;
                m_ObjectAnimator.SetBool("Hit", false);
                m_ObjectAnimator.SetBool("Idle", false);
                m_ObjectAnimator.SetBool("Attack", false);
                m_ObjectAnimator.SetBool("AttackReady", true);
                break;
            case BW_Animations.BW_NORMALATTACK:
                m_ObjectAnimator.SetBool("AttackReady", false);
                m_ObjectAnimator.SetBool("Attack", true);
                break;
            case BW_Animations.BW_HIT:
                m_ObjectAnimator.SetBool("Idle", false);
                m_ObjectAnimator.SetBool("AttackReady", false);
                m_ObjectAnimator.SetBool("Attack", false);
                if (false == m_bIsHit)
                {
                    m_ObjectAnimator.SetTrigger("Hit");
                    
                    m_bIsHit = true;
                }
                break;
            case BW_Animations.BW_DEAD:
                m_ObjectAnimator.SetBool("Idle", false);
                m_ObjectAnimator.SetBool("AttackReady", false);
                m_ObjectAnimator.SetBool("Attack", false);
                if (false == m_bIsDead)
                {
                    m_ObjectAnimator.SetTrigger("Dead");

                    m_bIsDead = true;
                }
                break;
        }
    }
}
