using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkySandals : ITEM
{
    private SpriteRenderer m_pBodySpriteRenderer = null;

    private Body m_pBodyObejct = null;

    private void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        GameObject _Obejct = transform.parent.gameObject;

        _Obejct = _Obejct.transform.parent.gameObject;

        _Obejct = _Obejct.transform.GetChild(0).gameObject;

        m_pBodySpriteRenderer = _Obejct.GetComponent<SpriteRenderer>();

        m_pBodyObejct = _Obejct.GetComponent<Body>();
    }

    private void Update()
    {
        m_eAvatarState = m_pBodyObejct.GetAvatarState;

        switch (m_eAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
                _Animations[0].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_RUN:
                _Animations[1].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                _Animations[2].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                _Animations[3].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                _Animations[4].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                _Animations[5].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_LADDER:
            case AVATARSTATES.AVATAR_LADDERRUN:
                _Animations[6].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
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
            case AVATARSTATES.AVATAR_JUMP:
                Jump();
                break;
            case AVATARSTATES.AVATAR_LADDER:
            case AVATARSTATES.AVATAR_LADDERRUN:
                Ladder();
                break;
        }
    }

    public override Vector3 IDLE()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Idle0.shoes":
                transform.localPosition = new Vector3(-0.001f, 0.0f);
                break;
            case "Idle1.shoes":
                transform.localPosition = new Vector3(0.005f, 0.0f);
                break;
            case "Idle2.shoes":
                transform.localPosition = new Vector3(0.012f, 0.0f);
                break;
        }

        return transform.localPosition;
    }

    public override Vector3 RUN()
    {
        switch (m_pBodySpriteRenderer.sprite.name)
        {
            case "run0.body":
                transform.localPosition = new Vector3(0.084f, 0.012f);
                break;
            case "run1.body":
                transform.localPosition = new Vector3(0.0f, 0.012f);
                break;
            case "run2.body":
                transform.localPosition = new Vector3(0.061f, 0.012f);
                break;
            case "run3.body":
                transform.localPosition = new Vector3(0.037f, 0.02f);
                break;
        }

        return transform.localPosition;
    }

    public override void NormalAttack()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "First Attack0.shoes":
                transform.localPosition = new Vector3(-0.002f, 0.033f);
                break;
            case "First Attack1.shoes":
                transform.localPosition = new Vector3(-0.015f, 0.009f);
                break;
            case "First Attack2.shoes":
                transform.localPosition = new Vector3(0.003f, 0.033f);
                break;

            case "Second Attack0.shoes":
                transform.localPosition = new Vector3(0.01f, 0.014f);
                break;
            case "Second Attack1.shoes":
                transform.localPosition = new Vector3(0.005f, 0.012f);
                break;
            case "Second Attack2.shoes":
                transform.localPosition = new Vector3(0.038f, 0.009f);
                break;


            case "Third Attack0.shoes":
                transform.localPosition = new Vector3(0.102f, 0.027f);
                break;
            case "Third Attack1.shoes":
                transform.localPosition = new Vector3(0.015f, 0.018f);
                break;
            case "Third Attack2.shoes":
                transform.localPosition = new Vector3(0.003f, 0.03f);
                break;
        }
    }

    public override void Jump()
    {
        transform.localPosition = new Vector3(0.005f, 0.026f);
    }

    private void Ladder()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "ladder.0.shoes":
                transform.localPosition = new Vector3(-0.008f, -0.036f);
                break;
            case "ladder.1.shoes":
                transform.localPosition = new Vector3(0.005f, -0.034f);
                break;
        }
    }

    public override Sprite GetSprite()
    {
        return null;
    }
}
