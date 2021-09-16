using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : PART
{
    [SerializeField] Sprite _LadderSprite = null;

    private Body m_pBodyObject = null;

    private Face m_pFaceObject = null;

    private Hair m_pHairObject = null;

    private SpriteRenderer m_pHeadSpriteRenderer = null;

    private SpriteRenderer m_pFaceSpriteRenderer = null;

    private SpriteRenderer m_pHairSpriteRenderer = null;

    private void Awake()
    {
        Transform _Parent = transform.parent;

        m_pSpriteRenderer = _Parent.GetChild(0).GetComponent<SpriteRenderer>();

        m_pBodyObject = _Parent.GetChild(0).GetComponent<Body>();

        m_pFaceObject = GetComponentInChildren<Face>();

        m_pHairObject = GetComponentInChildren<Hair>();

        m_pHeadSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pFaceSpriteRenderer = m_pFaceObject.GetComponent<SpriteRenderer>();

        m_pHairSpriteRenderer = m_pHairObject.GetComponent<SpriteRenderer>();

        m_pSprite = m_pHeadSpriteRenderer.sprite;
    }

    private void LateUpdate()
    {
        switch (m_pBodyObject.GetAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
                IDLE();
                break;
            case AVATARSTATES.AVATAR_RUN:
                RUN();
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                Attack();
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                Attack();
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                Attack();
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                Jump();
                break;
            case AVATARSTATES.AVATAR_LADDER:
            case AVATARSTATES.AVATAR_LADDERRUN:
                Ladder();
                break;
            case AVATARSTATES.AVATAR_DEAD:
                Dead();
                break;
        }
    }

    public override Vector3 IDLE()
    {
        if (m_pHeadSpriteRenderer.color != Color.white)
            m_pHeadSpriteRenderer.color = Color.white;

        if(m_pFaceSpriteRenderer.color != Color.white)
            m_pFaceSpriteRenderer.color = Color.white;

        if (m_pHairSpriteRenderer.color != Color.white)
            m_pHairSpriteRenderer.color = Color.white;


        if (m_pHeadSpriteRenderer.sprite != m_pSprite)
        {
            m_pHeadSpriteRenderer.sprite = m_pSprite;

            m_pFaceObject.gameObject.SetActive(true);

            m_pHairObject.ResetHair();
        }

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "stand1.0.body":
                transform.localPosition = new Vector3(0.025f, 0.23f);
                break;
            case "stand1.1.body":
                transform.localPosition = new Vector3(0.027f, 0.228f);
                break;
            case "stand1.2.body":
                transform.localPosition = new Vector3(0.035f, 0.23f);
                break;
        }

        return Vector3.zero;
    }

    public override Vector3 RUN()
    {
        if(null == m_pSpriteRenderer)
            return Vector3.zero;

        if (m_pHeadSpriteRenderer.sprite != m_pSprite)
        {
            m_pHairObject.ResetHair();

            m_pHeadSpriteRenderer.sprite = m_pSprite;

            m_pFaceObject.gameObject.SetActive(true);
        }

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "run0.body":
                transform.localPosition = new Vector3(0.024f, 0.223f);
                break;
            case "run1.body":
                transform.localPosition = new Vector3(-0.003f, 0.209f);
                break;
            case "run2.body":
                transform.localPosition = new Vector3(0.04f, 0.217f);
                break;
            case "run3.body":
                transform.localPosition = new Vector3(0.033f, 0.212f);
                break;
        }

        return Vector3.zero;
    }

    public void Attack()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Attack0.body":
                transform.localPosition = new Vector3(-0.113f, 0.182f);
                //transform.localPosition = new Vector3(-0.114f, 0.273f);
                break;
            case "Attack1.body":
                transform.localPosition = new Vector3(-0.072f, 0.215f);
                //transform.localPosition = new Vector3(-0.078f, 0.301f);
                break;
            case "Attack2.body":
                transform.localPosition = new Vector3(-0.171f, 0.176f);
                //transform.localPosition = new Vector3(-0.173f, 0.266f);
                break;
            case "SecondAttack0.body":
                transform.localPosition = new Vector3(0.031f, 0.225f);
                //transform.localPosition = new Vector3(0.026f, 0.311f);
                break;
            case "SecondAttack1.body":
                transform.localPosition = new Vector3(-0.048f, 0.213f);
                //transform.localPosition = new Vector3(-0.054f, 0.294f);
                break;
            case "SecondAttack2.body":
                transform.localPosition = new Vector3(-0.043f, 0.19f);
                //transform.localPosition = new Vector3(-0.043f, 0.275f);
                break;
            case "ThirdAttack0.body":
                transform.localPosition = new Vector3(0.026f, 0.195f);
                //transform.localPosition = new Vector3(0.023f, 0.286f);
                break;
            case "ThirdAttack1.body":
                transform.localPosition = new Vector3(-0.121f, 0.195f);
                //transform.localPosition = new Vector3(-0.124f, 0.275f);
                break;
            case "ThirdAttack2.body":
                transform.localPosition = new Vector3(-0.108f, 0.189f);
                //transform.localPosition = new Vector3(-0.115f, 0.272f);
                break;
        }
    }

    public override void Jump()
    {
        if(m_pFaceObject.gameObject.activeSelf == false)
        {
            m_pHeadSpriteRenderer.sprite = m_pSprite;

            m_pFaceObject.gameObject.SetActive(true);

            m_pHairObject.ResetHair();
        }

        transform.localPosition = new Vector3(0.016f, 0.214f);
    }

    private void Ladder()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "ladder.0.body":
                transform.localPosition = new Vector3(-0.004f, 0.145f);
                break;
            case "ladder.1.body":
                transform.localPosition = new Vector3(0.018f, 0.145f);
                break;
        }

        if (m_pHeadSpriteRenderer.sprite == _LadderSprite)
            return;

        m_pHeadSpriteRenderer.sprite = _LadderSprite;

        m_pFaceObject.gameObject.SetActive(false);

        m_pHairObject.ChangeLadderHair();
    }

    private void Dead()
    {
        if (m_pHeadSpriteRenderer.sprite != m_pSprite)
        {
            m_pHairObject.ResetHair();

            m_pHeadSpriteRenderer.sprite = m_pSprite;

            m_pFaceObject.gameObject.SetActive(true);
        }

        transform.localPosition = m_pBodyObject.transform.localPosition + new Vector3(0.0f, 0.28f, 0.0f);
    }
}
