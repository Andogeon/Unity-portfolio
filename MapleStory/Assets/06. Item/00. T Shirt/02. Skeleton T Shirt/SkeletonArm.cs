using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonArm : ITEM
{
    private SpriteRenderer m_pBodySpriteRenderer = null;

    private SpriteRenderer m_pTargetRenderer = null;

    public SpriteRenderer SetBodyRenderer
    {
        set { m_pBodySpriteRenderer = value; }
    }

    public SpriteRenderer SetHandRenderer
    {
        set { m_pTargetRenderer = value; }
    }

    private void Awake()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        switch (m_eAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
                _Animations[0].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_RUN:
                _Animations[1].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                _Animations[2].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                _Animations[3].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                _Animations[4].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                _Animations[5].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
        }
    }

    private void LateUpdate()
    {
        switch (m_eAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
                IDLE();
                break;
            case AVATARSTATES.AVATAR_RUN:
                RUN();
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                Jump();
                break;
        }
    }

    public override Vector3 IDLE()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Idle0.mailArm":
                transform.localPosition = new Vector3(0.058f, 0.045f);
                break;
            case "Idle1.mailArm":
                transform.localPosition = new Vector3(0.075f, 0.053f);
                break;
            case "Idle2.mailArm":
                transform.localPosition = new Vector3(0.074f, 0.057f);
                break;
        }

        return transform.localPosition;
    }

    public override Vector3 RUN() // 옷이랑 어깨 소팅해야 됨 
    {
        if (m_pBodySpriteRenderer == null)
            return Vector3.zero;

        switch (m_pBodySpriteRenderer.sprite.name)
        {
            case "run0.body":
                transform.localPosition = new Vector3(0.064f, 0.056f);
                break;
            case "run1.body":
                transform.localPosition = new Vector3(0.056f, 0.051f);
                break;
            case "run2.body":
                transform.localPosition = new Vector3(0.06f, 0.051f);
                break;
            case "run3.body":
                transform.localPosition = new Vector3(0.062f, 0.055f);
                break;
        }

        return Vector3.zero;
    }

    public override void NormalAttack()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "First Attack0.mailArm":
                transform.localPosition = new Vector3(-0.0706f, 0.0151f);
                break;
            case "First Attack1.mailArm":
                transform.localPosition = new Vector3(-0.0519f, 0.0638f);
                break;
            case "First Attack2.mailArm":
                transform.localPosition = new Vector3(0.0204f, 0.0638f);
                break;
            ///////////////////////////////////////////////////////////////
            case "Second Attack0.mailArm":
                transform.localPosition = new Vector3(-0.0319f, 0.0488f);
                break;
            case "Second Attack1.mailArm":
                transform.localPosition = new Vector3(-0.066f, 0.026f);
                break;
            case "Second Attack2.mailArm":
                transform.localPosition = new Vector3(0.059f, 0.036f);
                break;
            ///////////////////////////////////////////////////////////////
            case "Third Attack0.mailArm":
                transform.localPosition = new Vector3(0.059f, 0.042f);
                break;
            case "Third Attack1.mailArm":
                transform.localPosition = new Vector3(-0.065f, 0.033f);
                break;
            case "Third Attack2.mailArm":
                transform.localPosition = new Vector3(-0.069f, 0.017f);
                break;
        }
    }

    public override void Jump()
    {
        transform.localPosition = new Vector3(0.066f, 0.046f);
    }

    public override Sprite GetSprite()
    {
        return null;
    }
}
