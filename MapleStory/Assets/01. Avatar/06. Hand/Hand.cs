using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : PART
{
    private Body m_pBodyObject = null;

    private Animator m_pAnimator = null;

    private SpriteRenderer m_pHandSpriteRenderer = null;

    private void Awake()
    {
        m_pSpriteRenderer = transform.parent.GetComponent<SpriteRenderer>();

        m_pHandSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pBodyObject = transform.parent.GetComponent<Body>();

        m_pAnimator = GetComponent<Animator>();
    }

    //private void LateUpdate()
    private void Update()
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
        }
    }

    public override Vector3 IDLE()
    {
        if (m_pHandSpriteRenderer.color != Color.white)
            m_pHandSpriteRenderer.color = Color.white;

        if (m_pAnimator.enabled == false)
        {
            m_pHandSpriteRenderer.enabled = true;

            m_pAnimator.enabled = true;
        }

        if (m_pItemobject != null && m_pItemobject.activeSelf == false)
            m_pItemobject.SetActive(true);

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "stand1.0.body":
                transform.localPosition = new Vector3(0.105f, 0.0436f);
                break;
            case "stand1.1.body":
                transform.localPosition = new Vector3(0.1263f, 0.0436f);
                break;
            case "stand1.2.body":
                transform.localPosition = new Vector3(0.134f, 0.0436f);
                break;
        }

        return Vector3.zero;
    }

    public override Vector3 RUN()
    {
        if (m_pAnimator.enabled == false)
        {
            m_pHandSpriteRenderer.enabled = true;

            m_pAnimator.enabled = true;
        }

        if (m_pItemobject != null && m_pItemobject.activeSelf == false)
            m_pItemobject.SetActive(true);

        if (m_pHandSpriteRenderer.color != Color.white)
            m_pHandSpriteRenderer.color = Color.white;

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "run0.body":
                transform.localPosition = new Vector3(0.12f, 0.052f);
                break;
            case "run1.body":
                transform.localPosition = new Vector3(0.035f, 0.04f);
                break;
            case "run2.body":
                transform.localPosition = new Vector3(0.139f, 0.05f);
                break;
            case "run3.body":
                transform.localPosition = new Vector3(0.138f, 0.056f);
                break;
        }

        return Vector3.zero;
    }

    public void Attack()
    {
        if (m_pHandSpriteRenderer.color != Color.white)
            m_pHandSpriteRenderer.color = Color.white;

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Attack0.body":
                transform.localPosition = new Vector3(-0.12f, 0.069f);
                break;
            case "Attack1.body":
                transform.localPosition = new Vector3(-0.118f, 0.082f);
                break;
            case "Attack2.body":
                transform.localPosition = new Vector3(0.034f, 0.195f);
                break;
            case "SecondAttack0.body":
                transform.localPosition = new Vector3(-0.103f, 0.168f);
                break;
            case "SecondAttack1.body":
                transform.localPosition = new Vector3(-0.144f, 0.125f);
                break;
            case "SecondAttack2.body":
                transform.localPosition = new Vector3(0.145f, 0.05f);
                break;
            case "ThirdAttack0.body":
                transform.localPosition = new Vector3(0.181f, 0.14f);
                break;
            case "ThirdAttack1.body":
                transform.localPosition = new Vector3(-0.181f, 0.066f);
                break;
            case "ThirdAttack2.body":
                transform.localPosition = new Vector3(-0.115f, 0.068f);
                break;
        }
    }

    public override void Jump()
    {
        if (m_pAnimator.enabled == false)
        {
            m_pHandSpriteRenderer.enabled = true;

            m_pAnimator.enabled = true;
        }

        if (m_pItemobject != null && m_pItemobject.activeSelf == false)
            m_pItemobject.SetActive(true);

        if (m_pHandSpriteRenderer.color != Color.white)
            m_pHandSpriteRenderer.color = Color.white;

        transform.localPosition = new Vector3(0.062f, 0.093f);
    }

    private void Ladder()
    {
        if (m_pAnimator.enabled == false)
            return;

        if (m_pHandSpriteRenderer.color != Color.white)
            m_pHandSpriteRenderer.color = Color.white;

        m_pAnimator.enabled = false;

        m_pHandSpriteRenderer.enabled = false;

        m_pItemobject.SetActive(false); // 무기에 대한 여부 
    }

    public override void SetItem(ITEM _Item)
    {
        if (null == _Item)
            return;

        if (m_pOrlItem == null || m_pOrlItem.name != _Item.name)
        {
            GameObject _pCreateObject = GameObject.Instantiate(_Item.gameObject);

            _pCreateObject.transform.SetParent(gameObject.transform);

            _pCreateObject.transform.localScale = new Vector3(1.0f, 1.0f);

            _pCreateObject.transform.localRotation = Quaternion.identity;

            _pCreateObject.transform.localPosition = new Vector3(0.0f, 0.0f);

            ITEM _pItem = _pCreateObject.GetComponent<ITEM>();

            if (m_pItemobject != null)
            {
                m_pItemobject.transform.SetParent(null);

                Destroy(m_pItemobject);

                m_pItemobject = null;

                m_pItem = null;
            }

            _pItem.AccessSetItem = _Item;

            m_pItemobject = _pCreateObject;

            m_pItem = _pItem;

            m_pOrlItem = _Item;
        }
    }
}
