using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UE_Animation
{
    protected Animator m_ObjectAnimator = null;

    //public abstract void UpDateAnimation(Animator _ObjectAnimator, string[] _AnimationNames = null);

    public abstract void UpDateAnimation(string[] _AnimationNames = null);

    public void SetAnimations(Animator _Animator, string[] _AnimationNames, string _ChangeAnimationName = "")
    {
        if (null == _Animator)
            return;

        for (int i = 0; i < _AnimationNames.Length; ++i)
        {
            if (_AnimationNames[i] == _ChangeAnimationName)
                _Animator.SetBool(_AnimationNames[i], true);
            else
                _Animator.SetBool(_AnimationNames[i], false);
        }
    }

    public bool Animation_Of_End(Animator _ObjectAnimator, string _StrAnimationname, float _fExitTime) // 애니메이션 종료여부를 확인하는 함수
    {
        if (_ObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName(_StrAnimationname) && _ObjectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime > _fExitTime)
            return true;
        else
            return false;
    }

    public bool Animation_Medium(Animator _ObjectAnimator, string _StrAnimationname, float _iFirstTime = 0.0f, float _iExitTime = 0.8f)
    {
        if ((_ObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName(_StrAnimationname) && _ObjectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= _iFirstTime) 
            && (_ObjectAnimator.GetCurrentAnimatorStateInfo(0).IsName(_StrAnimationname) && _ObjectAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime <= _iExitTime))
            return true;
        else
            return false;
    }
}
