using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CJellyEffect : CEffect
{
    protected override void Awake()
    {
        m_pJellyAnimator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (AnimationComplete())
        {
            gameObject.SetActive(false);

            return;
        }

        transform.position += Vector3.left * 7.0f * Time.deltaTime;
    }

    private bool AnimationComplete()
    {
        return m_pJellyAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f;
    }

    protected Animator m_pJellyAnimator = null;
}
