using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BlueOneLine 클래스처럼 플레이어의 몸체 스프라이트 기준으로 반바지의 스프라이트 및 로컬 위치를 변경했습니다.

public class Halfjeans : ITEM
{
    private SpriteRenderer m_pBodySpriteRenderer = null; // 몸체의 스프라이트를 얻기 위한 참조 변수 

    private Body m_pBodyObejct = null; // 플레이어 몸체의 상태를 얻어오기 위한 참조변수 

    private void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        GameObject _Obejct = transform.parent.gameObject;

        _Obejct = _Obejct.transform.parent.gameObject;

        _Obejct = _Obejct.transform.GetChild(0).gameObject;

        m_pBodySpriteRenderer = _Obejct.GetComponent<SpriteRenderer>();

        m_pBodyObejct = _Obejct.GetComponent<Body>();
    }

    // 플레이어 몸체의 상태를 상시로 갱신하여 로컬 위치와 스프라이트를 변경했습니다.

    private void Update()
    {
        if (m_pBodyObejct == null)
            return;

        m_eAvatarState = m_pBodyObejct.GetAvatarState;

        switch (m_eAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
                IDLE();
                _Animations[0].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_RUN:
                RUN();
                _Animations[1].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                NormalAttack();
                _Animations[2].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                NormalAttack();
                _Animations[3].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                NormalAttack();
                _Animations[4].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                Jump();
                _Animations[5].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
            case AVATARSTATES.AVATAR_LADDER:
            case AVATARSTATES.AVATAR_LADDERRUN:
                Ladder();
                _Animations[6].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                break;
        }
    }

    // 대기 상태에서 호출 되는 함수 
    public override Vector3 IDLE()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Idle0.pants":
                transform.localPosition = new Vector3(-0.001f, 0.015f);
                break;
            case "Idle1.pants":
                transform.localPosition = new Vector3(0.005f, 0.015f);
                break;
            case "Idle2.pants":
                transform.localPosition = new Vector3(0.008f, 0.015f);
                break;
        }

        return transform.localPosition;
    }

    // 이동시 호출 되는 함수 
    public override Vector3 RUN()
    {
        switch (m_pBodySpriteRenderer.sprite.name)
        {
            case "run0.body":
                transform.localPosition = new Vector3(0.01f, 0.0f);
                break;
            case "run1.body":
                transform.localPosition = new Vector3(-0.023f, 0.0f);
                break;
            case "run2.body":
                transform.localPosition = new Vector3(0.022f, 0.0f);
                break;
            case "run3.body":
                transform.localPosition = new Vector3(0.017f, 0.0f);
                break;
        }

        return transform.localPosition;
    }

    // 공격 시 호출 되는 함수 
    public override void NormalAttack()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "First Attack0.pants":
                transform.localPosition = new Vector3(-0.03f, 0.021f);
                break;
            case "First Attack1.pants":
                transform.localPosition = new Vector3(-0.03f, 0.021f);
                break;
            case "First Attack2.pants":
                transform.localPosition = new Vector3(-0.0643f, 0.021f);
                break;

            case "Second Attack0.pants":
                transform.localPosition = new Vector3(0.002f, 0.017f);
                break;
            case "Second Attack1.pants":
                transform.localPosition = new Vector3(-0.021f, 0.017f);
                break;
            case "Second Attack2.pants":
                transform.localPosition = new Vector3(0.012f, 0.017f);
                break;


            case "Third Attack0.pants":
                transform.localPosition = new Vector3(0.057f, 0.01f);
                break;
            case "Third Attack1.pants":
                transform.localPosition = new Vector3(-0.025f, 0.01f);
                break;
            case "Third Attack2.pants":
                transform.localPosition = new Vector3(-0.034f, 0.01f);
                break;
        }
    }

    // 점프 했을 때 호출 되는 함수

    public override void Jump()
    {
        transform.localPosition = new Vector3(-0.001f, 0.004f);
    }

    // 사다리에 올라 탔을 때 호출 되는 함수
    private void Ladder()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "ladder.0.pants":
                transform.localPosition = new Vector3(-0.02f, -0.05f);
                break;
            case "ladder.1.pants":
                transform.localPosition = new Vector3(-0.007f, -0.03f);
                break;
        }
    }

    public override Sprite GetSprite()
    {
        return null;
    }
}
