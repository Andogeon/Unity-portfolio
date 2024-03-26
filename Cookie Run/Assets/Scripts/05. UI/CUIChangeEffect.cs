using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CUIChangeEffect : CEffect
{
    protected override void Awake()
    {
        m_pAnimator = GetComponent<Animator>();
    }

    protected override void Update()
    {
        if (m_pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            return;

        Destroy(gameObject);
    }

    private Animator m_pAnimator = null;
}
