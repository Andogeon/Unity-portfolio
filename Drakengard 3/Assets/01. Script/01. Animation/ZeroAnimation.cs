using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ZeroAnimations
{
    ZERO_IDLE,
    ZERO_RUN,
    ZERO_NORMALATTACK,
    ZERO_SUBNORMALATTACK,
    ZERO_HIT,
    ZERO_ATTACKDASH,
    ZERO_DASH,
    ZERO_JUMP
};

public enum ZeroNormalAttackType
{
    ZNA_ATTACK1,
    ZNA_ATTACK2,
    ZNA_ATTACK3,
    ZNA_ATTACK4,
    ZNA_ATTACK5,
    ZNA_ATTACK6,
    ZNA_ATTACK7,
    ZNA_ATTACK8,
};

public enum ZeroSubAttackType
{
    ZNA_SUBATTACK1,
    ZNA_SUBATTACK2,
    ZNA_SUBATTACK3
}

public class ZeroAnimation : UE_Animation
{
    private Zero _Zero = null;

    public ZeroAnimation(Animator _ObjectAniamtor)
    {
        m_ObjectAnimator = _ObjectAniamtor;

        GameObject _ZeroObject = _ObjectAniamtor.gameObject;

        _Zero = _ZeroObject.GetComponent<Zero>();
    }

    private ZeroAnimations m_eZeroAnimations = ZeroAnimations.ZERO_IDLE;

    private ZeroNormalAttackType m_eZeroNormalAttackType = ZeroNormalAttackType.ZNA_ATTACK1;

    private ZeroSubAttackType m_eZerosubAttackType = ZeroSubAttackType.ZNA_SUBATTACK1;

    private int m_iZeroCombo = 1; // 현재 콤보수 

    private const int m_iZeroMaxCombo = 8; // 최대 콤보수

    private int m_isubcombo = 1;

    private const int m_iSubMaxCombo = 3; // 최대 콤보수

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private int m_iZeroHit = 1; // 맞는 횟수

    private bool m_bOnNextAttack = false;

    private bool m_bIsDash = false;

    private bool m_bIsHit = false;

    public bool GetdashMode
    {
        get { return m_bIsDash; }

        set { m_bIsDash = value; }
    }

    public int AccessHit
    {
        get { return m_iZeroHit; }

        set { m_iZeroHit = value; }
    }

    public bool AccessOnHit
    {
        get { return m_bIsHit; }

        set { m_bIsHit = value; }
    }

    public bool AccessClick
    {
        get { return m_bOnNextAttack; }

        set { m_bOnNextAttack = value; }
    }

    public ZeroAnimations AccessZeroAnimaions
    {
        get { return m_eZeroAnimations; }

        set { m_eZeroAnimations = value; }
    }
    public ZeroNormalAttackType AccessZeroAttackType
    {
        get { return m_eZeroNormalAttackType; }

        set { m_eZeroNormalAttackType = value; }
    }

    public ZeroSubAttackType AccessZeroSubAttackType
    {
        get { return m_eZerosubAttackType; }

        set { m_eZerosubAttackType = value; }
    }
    public int AccessZeroCombo
    {
        get { return m_iZeroCombo; }

        set { m_iZeroCombo = value; }
    }

    public int AccessSubCombo
    {
        get { return m_isubcombo; }

        set { m_isubcombo = value; }
    }

    public int GetsubMaxCombo
    {
        get { return m_iSubMaxCombo; }
    }

    public int GetZeroMaxCombo
    {
        get { return m_iZeroMaxCombo; }
    }

    public override void UpDateAnimation(string[] _AnimationNames = null)
    {
        switch (m_eZeroAnimations)
        {
            case ZeroAnimations.ZERO_IDLE: // 대기상태
                m_bIsHit = false;
                SetAnimations(m_ObjectAnimator, _AnimationNames, "IsIdle");
                m_ObjectAnimator.SetInteger("Attack State", 0);
                m_ObjectAnimator.SetInteger("Attack sub State", 0);
                break;
            case ZeroAnimations.ZERO_RUN: // 달리기
                m_bIsHit = false;
                SetAnimations(m_ObjectAnimator, _AnimationNames, "IsRun");
                break;
            case ZeroAnimations.ZERO_NORMALATTACK:
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsRun", false);
                ZeroNormalAttack(m_ObjectAnimator);
                m_bIsHit = false;
                break;
            case ZeroAnimations.ZERO_SUBNORMALATTACK:
                m_bIsHit = false;
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsRun", false);
                ZeroSubNormalAttack(m_ObjectAnimator);
                break;
            case ZeroAnimations.ZERO_HIT:
                m_ObjectAnimator.SetBool("IsIdle", false);
                m_ObjectAnimator.SetBool("IsRun", false);
                SelectHitMotion(m_ObjectAnimator);
                break;
            case ZeroAnimations.ZERO_DASH:
                m_bIsHit = false;
                ZeroDashMoving(m_ObjectAnimator);
                break;
            case ZeroAnimations.ZERO_ATTACKDASH:
                m_bIsHit = false;
                ZeroDashMoving(m_ObjectAnimator);
                break;
        }
    }

    //private void UpdateAttackAnimaion(Animator _ZeroAnimator)
    //{
    //    switch (m_eZeroAnimations)
    //    {
    //        case ZeroAnimations.ZERO_NORMALATTACK:
    //            ZeroNormalAttack(_ZeroAnimator);               
    //            break;
    //        case ZeroAnimations.ZERO_SUBNORMALATTACK:
    //            break;
    //    }
    //}

    private void ZeroNormalAttack(Animator _ZeroAnimator)
    {
        if (null == _ZeroAnimator) // 현재 애니메이터를 알 수 없다면 
            return; //종료

        switch (m_eZeroNormalAttackType) // 상태값에 따른 애니메이션의 분기
        {
            case ZeroNormalAttackType.ZNA_ATTACK1:
                _ZeroAnimator.SetInteger("Attack State", 1);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK2:
                _ZeroAnimator.SetInteger("Attack State", 2);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK3:
                _ZeroAnimator.SetInteger("Attack State", 3);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK4:
                _ZeroAnimator.SetInteger("Attack State", 4);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK5:
                _ZeroAnimator.SetInteger("Attack State", 5);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK6:
                _ZeroAnimator.SetInteger("Attack State", 6);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK7:
                _ZeroAnimator.SetInteger("Attack State", 7);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK8:
                _ZeroAnimator.SetInteger("Attack State", 8);
                break;
        }
    }

    private void ZeroSubNormalAttack(Animator _ZeroAnimator)
    {
        if (null == _ZeroAnimator)
            return;

        switch(m_eZerosubAttackType)
        {
            case ZeroSubAttackType.ZNA_SUBATTACK1:
                _ZeroAnimator.SetInteger("Attack sub State", 1);
                break;
            case ZeroSubAttackType.ZNA_SUBATTACK2:
                _ZeroAnimator.SetInteger("Attack sub State", 2);
                break;
            case ZeroSubAttackType.ZNA_SUBATTACK3:
                _ZeroAnimator.SetInteger("Attack sub State", 3);
                break;
        }
    }

    private void ZeroDashMoving(Animator _ZeroAnimator)
    {
        if (null == _ZeroAnimator)
            return;

        if (m_bIsDash == false)
        {
            _ZeroAnimator.SetTrigger("Dash");

            m_pSoundManager.PlaySound("Dash Sound");

            m_bIsDash = true;
        }

        Transform _ZeroTranform = _ZeroAnimator.gameObject.transform;

        if (null == _ZeroTranform)
            return;

        _ZeroTranform.transform.position += _Zero.transform.forward * 20.0f * Time.deltaTime;
    }

    private void SelectHitMotion(Animator _ZeroAnimation)
    {
        if (null == _ZeroAnimation || m_bIsHit == true)
            return;

        switch(m_iZeroHit)
        {
            case 1:
                _ZeroAnimation.SetTrigger("IsOneHit");
                m_bIsHit = true;
                break;
            case 2:
                _ZeroAnimation.SetTrigger("IsTwoHit");
                m_bIsHit = true;
                break;
            case 3:
                _ZeroAnimation.SetTrigger("IsThreeHit");
                m_bIsHit = true;
                break;
        }
    }   
}