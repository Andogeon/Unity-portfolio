using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 플레이어의 몸체와 상호하여 애니메이션을 적용하기 위해서 클래스를 생성했습니다.

// 또한 티셔츠들이 티셔츠와 티셔츠 어깨 스프라이트가 따로 분리되어 있어서 클래스를 2개로 분리하여 사용했습니다.

public class BlueOneLine : ITEM
{
    private BlueOneLineArm m_pBlueArm = null; // 자식 오브젝트의 스크립트의 참조 변수

    private SpriteRenderer m_pBodySpriteRenderer = null; // 현재 몸체의 애니메이션 스프라이트를 얻기 위한 참조 변수 

    private void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        BlueOneLineArm _BlueArm = transform.GetChild(0).GetComponent<BlueOneLineArm>();

        _BlueArm.transform.SetParent(null); // 원본 객체의 부모 해재 
        
        GameObject _Instanceobject = GameObject.Instantiate(_BlueArm.gameObject); // 복사본을 생성 

        m_pBlueArm = _Instanceobject.GetComponent<BlueOneLineArm>();

        m_pBlueArm.SetHandRenderer = transform.parent.GetChild(0).GetComponent<SpriteRenderer>();

        m_pBodySpriteRenderer = transform.parent.GetComponentInParent<SpriteRenderer>();

        m_pBlueArm.transform.SetParent(transform); // 복사본을 현재 객체의 부모로 지정 

        Destroy(_BlueArm.gameObject);
    } 

    private void Update()
    {
        m_pBlueArm.AvatarState = m_eAvatarState; // 티셔츠 어깨에 현재 플레이어의 상태를 넘김

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


    // 현재 플레이어의 몸체 스프라이트 이름으로 아이템 로컬위치를 강제로 이동시켜 옷을 입은 것처럼 보이고자 구현했습니다.

    public override Vector3 IDLE() // 대기 상태일때
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

    public override Vector3 RUN() // 이동 할 때
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

    public override void NormalAttack() // 공격 할 때
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

    public override void Jump() // 점프 할 때
    {
        if (m_pBlueArm.gameObject.activeSelf == false)
            m_pBlueArm.gameObject.SetActive(true);

        transform.localPosition = new Vector3(0.02f, 0.084f);
    }

    public void Ladder() // 사다리를 탈 때
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