using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//BlueOneLine 클래스와 변수 및 기능 똑같으며 스프라이트 이미지만 다릅니다.

public class Orange : ITEM
{
    private OrangeArm m_pOrangeArm = null;

    private SpriteRenderer m_pBodySpriteRenderer = null;

    private void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        OrangeArm _OrangeArm = transform.GetChild(0).GetComponent<OrangeArm>();

        _OrangeArm.transform.SetParent(null);

        GameObject _Instanceobject = GameObject.Instantiate(_OrangeArm.gameObject);

        m_pOrangeArm = _Instanceobject.GetComponent<OrangeArm>();

        m_pOrangeArm.SetHandRenderer = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();

        m_pBodySpriteRenderer = transform.parent.GetComponentInParent<SpriteRenderer>();

        m_pOrangeArm.transform.SetParent(transform);

        Destroy(_OrangeArm.gameObject);
    }

    private void LateUpdate()
    {
        m_pOrangeArm.AvatarState = m_eAvatarState;

        switch (m_eAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
                _Animations[0].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                IDLE();
                break;
            case AVATARSTATES.AVATAR_RUN:
                _Animations[1].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                RUN();
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                _Animations[2].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                _Animations[3].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                _Animations[4].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                _Animations[5].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                Jump();
                break;
            case AVATARSTATES.AVATAR_LADDER:
            case AVATARSTATES.AVATAR_LADDERRUN:
                _Animations[6].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                Ladder();
                break;
        }
    }

    public override Vector3 IDLE()
    {
        if (m_pOrangeArm.gameObject.activeSelf == false)
            m_pOrangeArm.gameObject.SetActive(true);

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Idle0.mail":
                transform.localPosition = new Vector3(0.02f, 0.084f);
                break;
            case "Idle1.mail":
                transform.localPosition = new Vector3(0.02f, 0.084f);
                break;
            case "Idle2.mail":
                transform.localPosition = new Vector3(0.026f, 0.084f);
                break;
        }

        return transform.localPosition;
    }

    public override Vector3 RUN()
    {
        if (m_pOrangeArm.gameObject.activeSelf == false)
            m_pOrangeArm.gameObject.SetActive(true);

        switch (m_pBodySpriteRenderer.sprite.name)
        {
            case "run0.body":
                transform.localPosition = new Vector3(0.021f, 0.085f);
                break;
            case "run1.body":
                transform.localPosition = new Vector3(-0.004f, 0.074f);
                break;
            case "run2.body":
                transform.localPosition = new Vector3(0.038f, 0.091f);
                break;
            case "run3.body":
                transform.localPosition = new Vector3(0.03f, 0.079f);
                break;
        }

        return transform.localPosition;
    }

    public override void NormalAttack()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "First Attack0.mail":
                transform.localPosition = new Vector3(-0.0285f, 0.0595f);
                break;
            case "First Attack1.mail":
                transform.localPosition = new Vector3(-0.0228f, 0.075f);
                break;
            case "First Attack2.mail":
                transform.localPosition = new Vector3(-0.0643f, 0.0693f);
                break;

            case "Second Attack0.mail":
                transform.localPosition = new Vector3(0.022f, 0.078f);
                break;
            case "Second Attack1.mail":
                transform.localPosition = new Vector3(0.003f, 0.078f);
                break;
            case "Second Attack2.mail":
                transform.localPosition = new Vector3(0.026f, 0.063f);
                break;


            case "Third Attack0.mail":
                transform.localPosition = new Vector3(0.042f, 0.063f);
                break;
            case "Third Attack1.mail":
                transform.localPosition = new Vector3(-0.025f, 0.068f);
                break;
            case "Third Attack2.mail":
                transform.localPosition = new Vector3(-0.025f, 0.059f);
                break;
        }
    }

    public override void Jump()
    {
        if (m_pOrangeArm.gameObject.activeSelf == false)
            m_pOrangeArm.gameObject.SetActive(true);

        transform.localPosition = new Vector3(0.02f, 0.084f);
    }

    public void Ladder()
    {
        if (m_pOrangeArm.gameObject.activeSelf == true)
            m_pOrangeArm.gameObject.SetActive(false);

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "ladder.0.mail":
                transform.localPosition = new Vector3(-0.0136f, 0.0144f);
                break;
            case "ladder.1.mail":
                transform.localPosition = new Vector3(0.017f, 0.0202f);
                break;
        }
    }

    public override Sprite GetSprite()
    {
        return null;
    }
}
