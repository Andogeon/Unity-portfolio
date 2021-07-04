using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SW_Animations
{
    SW_IDLE,
    SW_RUN,
    SW_ATTACKREADY,
    SW_FACE_ATTACKREADY,
    SW_NORMALATTACK,
    SW_HIT,
    SW_DEAD
};

public class SW_Animation : UE_Animation
{
    public SW_Animation(Animator _ObjectAnimator)
    {
        m_ObjectAnimator = _ObjectAnimator;
    }

    private SW_Animations m_eSwordAniType = SW_Animations.SW_IDLE;

    private bool m_bIsAttack = false;

    private bool m_bIsAttackReady = false;

    private bool m_bIsHit = false;

    private bool m_bIsDead = false;

    public SW_Animations AccessSwordAnimations
    {
        get { return m_eSwordAniType; }

        set { m_eSwordAniType = value; }
    }

    public bool AccessAttack
    {
        get { return m_bIsAttack; }

        set { m_bIsAttack = value; }
    }

    public bool AccessHit
    {
        get { return m_bIsHit; }

        set { m_bIsHit = value; }
    }

    public bool AccessAttackReady
    {
        get { return m_bIsAttackReady; }

        set { m_bIsAttackReady = value; }
    }

    public override void UpDateAnimation(string[] _AnimationNames = null)
    {
        if (null == m_ObjectAnimator)
            return;       

        switch(m_eSwordAniType)
        {
            case SW_Animations.SW_IDLE:
                m_ObjectAnimator.SetBool("IsIdle", true);
                m_ObjectAnimator.SetBool("IsRun", false);
                m_ObjectAnimator.SetBool("IsAttackReady", false);
                m_ObjectAnimator.SetBool("IsFaceAttackReady", false);
                m_bIsHit = false;
                break;
            case SW_Animations.SW_RUN:
                m_ObjectAnimator.SetBool("IsRun", true);
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsAttackReady", false);
                m_ObjectAnimator.SetBool("IsFaceAttackReady", false);
                break;
            case SW_Animations.SW_ATTACKREADY:
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsRun", false);
                if(m_ObjectAnimator.GetBool("IsAttackReady") == false)
                    m_ObjectAnimator.SetBool("IsAttackReady", true);
                m_ObjectAnimator.SetBool("IsFaceAttackReady", false);
                m_bIsHit = false;
                break;
            case SW_Animations.SW_FACE_ATTACKREADY:
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsRun", false);
                m_ObjectAnimator.SetBool("IsAttackReady", false);
                m_ObjectAnimator.SetBool("IsFaceAttackReady", true);
                m_bIsHit = false;
                break;
            case SW_Animations.SW_NORMALATTACK:
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsRun", false);
                m_ObjectAnimator.SetBool("IsAttackReady", false);
                SelectAttackTpye();
                break;
            case SW_Animations.SW_HIT:
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsRun", false);
                OnTriggerAnimation(m_ObjectAnimator, "One Hit", ref m_bIsHit);
                break;
            case SW_Animations.SW_DEAD:
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsRun", false);
                m_ObjectAnimator.SetBool("IsAttackReady", false);
                m_ObjectAnimator.SetBool("IsFaceAttackReady", false);
                OnTriggerAnimation(m_ObjectAnimator, "IsDead", ref m_bIsDead);
                break;
        }
    }

    private void SelectAttackTpye()
    {
        int _ISelectAttackType = Random.Range(0, 2);

        if (_ISelectAttackType > 1 || m_bIsAttack == true)
            return;

        switch(_ISelectAttackType)
        {
            case 0:
                m_ObjectAnimator.SetTrigger("One Attack");
                m_bIsAttack = true;
                break;
            case 1:
                m_ObjectAnimator.SetTrigger("Two Attack");
                m_bIsAttack = true;
                break;
        }
    }

    private void OnTriggerAnimation(Animator _ObjectAnimator, string _Triggername, ref bool _IsSwitchname)
    {
        if (_IsSwitchname == false)
        {
            _ObjectAnimator.SetTrigger(_Triggername);

            _IsSwitchname = true;
        }
    }
}