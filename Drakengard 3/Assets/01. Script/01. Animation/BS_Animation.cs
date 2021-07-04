using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public enum Boss_State
{
    BOSS_IDLE,
    BOSS_RUN,
    BOSS_NORMALATTACK,
    BOSS_HIT,
    BOSS_HITING,
    BOSS_RESET,
    BOSS_DEAD
}

public class BS_Animation : UE_Animation
{
    private Boss_State m_eBossState = Boss_State.BOSS_IDLE;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private bool m_bIsHit = false;

    private bool m_bIsHiting = false;

    private bool m_bIsDead = false;

    public Boss_State AccessAnimations
    {
        get { return m_eBossState; }

        set { m_eBossState = value; }
    }

    public BS_Animation(Animator _ObjectAniamtor)
    {
        m_ObjectAnimator = _ObjectAniamtor;
    }

    public override void UpDateAnimation(string[] _AnimationNames = null)
    {
        switch(m_eBossState)
        {
            case Boss_State.BOSS_IDLE:
                m_ObjectAnimator.SetBool("RUN", false);
                m_ObjectAnimator.SetBool("ATTACK", false);
                break;
            case Boss_State.BOSS_RUN:
                m_ObjectAnimator.SetBool("RUN", true);
                m_ObjectAnimator.SetBool("ATTACK", false);
                break;
            case Boss_State.BOSS_NORMALATTACK:
                m_ObjectAnimator.SetBool("RUN", false);
                m_ObjectAnimator.SetBool("ATTACK", true);
                break;
            case Boss_State.BOSS_HIT:
                if (m_bIsHit == false)
                {
                    m_ObjectAnimator.SetBool("RUN", false);
                    m_ObjectAnimator.SetBool("ATTACK", false);
                    m_ObjectAnimator.SetTrigger("HIT");
                    m_bIsHit = true;
                    m_bIsHiting = false;
                }
                break;
            case Boss_State.BOSS_RESET:
                if (m_bIsHiting == false)
                {
                    m_ObjectAnimator.SetTrigger("RESET");
                    m_bIsHiting = true;
                    m_bIsHit = false;
                }
                break;
            case Boss_State.BOSS_DEAD:
                if (m_bIsDead == false)
                {
                    m_ObjectAnimator.SetTrigger("DEAD");
                    m_bIsDead = true;
                }
                break;
        }
    }
}
