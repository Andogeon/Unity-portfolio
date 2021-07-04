using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSprites : MonoBehaviour
{
    protected float _AnimationSpeed = 0.0f;

    protected int m_iIndex = 0;

    public float SetAnimationSpeed
    {
        set { _AnimationSpeed = value; }
    }

    public virtual void UpdateAnimation(SpriteRenderer _UpdateSpriteRenderer, SpriteRenderer _SpriteRenderer)
    {
        return;
    }
}
