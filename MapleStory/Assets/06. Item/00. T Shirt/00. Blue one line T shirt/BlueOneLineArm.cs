using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// BlueOneLine 클래스와 동일하게 

public class BlueOneLineArm : ITEM
{
    private SpriteRenderer m_pTargetRenderer = null;

    public SpriteRenderer SetHandRenderer // 티셔츠에서 핸들의 정보를 얻어옴 
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
                IDLE();
                _Animations[0].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_RUN:
                RUN();
                _Animations[1].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                NormalAttack();
                _Animations[2].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                NormalAttack();
                _Animations[3].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                NormalAttack();
                _Animations[4].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                Jump();
                _Animations[5].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
                break;
        }
    }



    // 대기 상태 스프라이트로 로컬 좌표를 맞추는 함수입니다.
    public override Vector3 IDLE()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Blue0.mailArm":
                transform.localPosition = new Vector3(0.073f, 0.019f);
                break;
            case "Blue1.mailArm": 
                transform.localPosition = new Vector3(0.075f, 0.027f);
                break;
            case "Blue2.mailArm":
                transform.localPosition = new Vector3(0.066f, 0.029f);
                break;
        }

        return transform.localPosition;
    }

    // 플레이어의 팔의 스프라이트 네임을 참조하여 이동시 스프라이트 위치를 맞추기 위한 함수입니다.

    public override Vector3 RUN()
    {
        // 기존 티셔츠 어깨 스프라이트 개수와 이동할때 스프라이트 개수가 맞지가 않아서 플레이어의 팔을 기준으로 

        // 로컬 좌표를 맞추었습니다.

        if (m_pTargetRenderer == null)
            return Vector3.zero;

        switch (m_pTargetRenderer.sprite.name)
        {
            case "Run0.arm":
                transform.localPosition = new Vector3(0.054f, 0.032f);
                break;
            case "Run1.arm":
                transform.localPosition = new Vector3(0.051f, 0.025f);
                break;
            case "Run2.arm":
                transform.localPosition = new Vector3(0.069f, 0.018f);
                break;
            case "Run3.arm":
                transform.localPosition = new Vector3(0.062f, 0.031f);
                break;
        }

        return Vector3.zero;
    }


    // 공격 상태 스프라이트로 로컬 좌표를 맞추는 함수입니다.
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

            case "Second Attack0.mailArm":
                transform.localPosition = new Vector3(-0.0319f, 0.0488f);
                break;
            case "Second Attack1.mailArm":
                transform.localPosition = new Vector3(-0.066f, 0.026f);
                break;
            case "Second Attack2.mailArm":
                transform.localPosition = new Vector3(0.059f, 0.036f);
                break;

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

    // 점프 상태 스프라이트로 로컬 좌표를 맞추는 함수입니다.
    public override void Jump()
    {
        transform.localPosition = new Vector3(0.0773f, 0.0245f);
    }

    public override Sprite GetSprite()
    {
        return null;
    }
}





























//public class BlueOneLineArm : ITEM
//{
//    //private SpriteRenderer m_pBodySpriteRenderer = null;

//    private SpriteRenderer m_pTargetRenderer = null;

//    //public SpriteRenderer SetBodyRenderer
//    //{
//    //    set { m_pBodySpriteRenderer = value; }
//    //}

//    public SpriteRenderer SetHandRenderer
//    {
//        set { m_pTargetRenderer = value; }
//    }

//    private void Awake()
//    {
//        m_pSpriteRenderer = GetComponent<SpriteRenderer>();
//    }

//    private void Update()
//    {
//        switch (m_eAvatarState)
//        {
//            case AVATARSTATES.AVATAR_IDLE:
//                IDLE();
//                _Animations[0].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
//                break;
//            case AVATARSTATES.AVATAR_RUN:
//                RUN();
//                _Animations[1].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
//                break;
//            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
//                NormalAttack();
//                _Animations[2].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
//                break;
//            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
//                NormalAttack();
//                _Animations[3].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
//                break;
//            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
//                NormalAttack();
//                _Animations[4].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
//                break;
//            case AVATARSTATES.AVATAR_HIT:
//            case AVATARSTATES.AVATAR_JUMP:
//                Jump();
//                _Animations[5].UpdateAnimation(m_pSpriteRenderer, m_pTargetRenderer);
//                break;
//        }
//    }

//    public override Vector3 IDLE()
//    {
//        switch (m_pSpriteRenderer.sprite.name)
//        {
//            case "Blue0.mailArm":
//                transform.localPosition = new Vector3(0.073f, 0.019f);
//                break;
//            case "Blue1.mailArm":
//                transform.localPosition = new Vector3(0.075f, 0.027f);
//                break;
//            case "Blue2.mailArm":
//                transform.localPosition = new Vector3(0.066f, 0.029f);
//                break;
//        }

//        return transform.localPosition;
//    }

//    public override Vector3 RUN()
//    {
//        if (m_pTargetRenderer == null)
//            return Vector3.zero;

//        switch (m_pTargetRenderer.sprite.name)
//        {
//            case "Run0.arm":
//                transform.localPosition = new Vector3(0.054f, 0.032f);
//                break;
//            case "Run1.arm":
//                transform.localPosition = new Vector3(0.051f, 0.025f);
//                break;
//            case "Run2.arm":
//                transform.localPosition = new Vector3(0.069f, 0.018f);
//                break;
//            case "Run3.arm":
//                transform.localPosition = new Vector3(0.062f, 0.031f);
//                break;
//        }

//        return Vector3.zero;


//        //if (m_pBodySpriteRenderer == null)
//        //    return Vector3.zero;

//        //switch (m_pBodySpriteRenderer.sprite.name)
//        //{
//        //    case "run0.body":               
//        //        transform.localPosition = new Vector3(0.054f, 0.032f);
//        //        break;
//        //    case "run1.body":
//        //        transform.localPosition = new Vector3(0.051f, 0.025f);
//        //        break;
//        //    case "run2.body": 
//        //        transform.localPosition = new Vector3(0.069f, 0.018f);
//        //        break;
//        //    case "run3.body": 
//        //        transform.localPosition = new Vector3(0.062f, 0.031f);
//        //        break;
//        //}

//        //return Vector3.zero;
//    }

//    public override void NormalAttack()
//    {
//        switch (m_pSpriteRenderer.sprite.name)
//        {
//            case "First Attack0.mailArm":
//                transform.localPosition = new Vector3(-0.0706f, 0.0151f);
//                break;
//            case "First Attack1.mailArm":
//                transform.localPosition = new Vector3(-0.0519f, 0.0638f);
//                break;
//            case "First Attack2.mailArm":
//                transform.localPosition = new Vector3(0.0204f, 0.0638f);
//                break;

//            case "Second Attack0.mailArm":
//                transform.localPosition = new Vector3(-0.0319f, 0.0488f);
//                break;
//            case "Second Attack1.mailArm":
//                transform.localPosition = new Vector3(-0.066f, 0.026f);
//                break;
//            case "Second Attack2.mailArm":
//                transform.localPosition = new Vector3(0.059f, 0.036f);
//                break;

//            case "Third Attack0.mailArm":
//                transform.localPosition = new Vector3(0.059f, 0.042f);
//                break;
//            case "Third Attack1.mailArm":
//                transform.localPosition = new Vector3(-0.065f, 0.033f);
//                break;
//            case "Third Attack2.mailArm":
//                transform.localPosition = new Vector3(-0.069f, 0.017f);
//                break;
//        }
//    }

//    public override void Jump()
//    {
//        transform.localPosition = new Vector3(0.0773f, 0.0245f);
//    }

//    public override Sprite GetSprite()
//    {
//        return null;
//    }
//}