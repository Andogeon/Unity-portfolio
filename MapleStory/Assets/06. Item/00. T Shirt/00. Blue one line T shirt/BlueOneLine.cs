using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlueOneLine : ITEM
{
    private BlueOneLineArm m_pBlueArm = null;

    private SpriteRenderer m_pBodySpriteRenderer = null;

    private void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        BlueOneLineArm _BlueArm = transform.GetChild(0).GetComponent<BlueOneLineArm>();

        _BlueArm.transform.SetParent(null);
        
        GameObject _Instanceobject = GameObject.Instantiate(_BlueArm.gameObject);

        m_pBlueArm = _Instanceobject.GetComponent<BlueOneLineArm>();

        m_pBlueArm.SetHandRenderer = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();

        m_pBlueArm.SetBodyRenderer = m_pBodySpriteRenderer = transform.parent.GetComponentInParent<SpriteRenderer>();

        m_pBlueArm.transform.SetParent(transform);

        Destroy(_BlueArm.gameObject);
    }

    private void Update()
    {
        m_pBlueArm.AvatarState = m_eAvatarState;

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
            case AVATARSTATES.AVATAR_HIT:
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
        if(m_pBlueArm.gameObject.activeSelf == false)
            m_pBlueArm.gameObject.SetActive(true);

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Blue0.mail":
                transform.localPosition = new Vector3(0.02f, 0.084f);
                break;
            case "Blue1.mail":
                transform.localPosition = new Vector3(0.02f, 0.084f);
                break;
            case "Blue2.mail":
                transform.localPosition = new Vector3(0.026f, 0.084f);
                break;
        }

        return transform.localPosition;
    }

    public override Vector3 RUN()
    {
        if (m_pBlueArm.gameObject.activeSelf == false)
            m_pBlueArm.gameObject.SetActive(true);

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
        if (m_pBlueArm.gameObject.activeSelf == false)
            m_pBlueArm.gameObject.SetActive(true);

        transform.localPosition = new Vector3(0.02f, 0.084f);
    }

    public void Ladder()
    {
        if (m_pBlueArm.gameObject.activeSelf == true)
            m_pBlueArm.gameObject.SetActive(false);

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