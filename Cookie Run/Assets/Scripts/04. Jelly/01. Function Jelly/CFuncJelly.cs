using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CFuncJelly : CJelly
{
    private CBounsTime m_pBounsTime = null;

    private CPoolManaged m_pPoolManaged = null;

    private CookieEventBox m_pCookieEventBox = null;

    private CSoundManaged m_pSoundManager = null;

    protected override void Start()
    {
        base.Start();

        ComponentInit();

        SetJellyAnimation();

        m_pSoundManager = CTopManager.GetInstance().GetSoundManaged;
    }

    //--------------------------------------------
    // ���� �浹 �� �߻��� �̺�Ʈ �޼���
    //--------------------------------------------
    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("EventBox"))
            return;

        base.OnTriggerStay2D(other, SetJellyFunction);
    }

    //--------------------------------------
    // ��� �������� �ʿ��� ������Ʈ �ʱ�ȭ 
    //--------------------------------------
    protected override void ComponentInit()
    {
        m_pBounsTime = FindObjectOfType<CBounsTime>();

        m_pPoolManaged = CTopManager.GetInstance().GetPoolManaged;

        if (gameObject.name == "Magnet Jelly" || gameObject.name == "Yellow Change Jelly")
        {
            GameObject _pGameScene = GameObject.Find("GameScene");

            if (_pGameScene != null)
                m_pCookieEventBox = _pGameScene.transform.Find("Jelly Event Box").GetComponent<CookieEventBox>();
        }
    }

    //--------------------------------------
    // ��� �������� �ִϸ��̼� ���� �Լ� 
    //--------------------------------------
    protected override void SetJellyAnimation()
    {
        if (m_pAnimator == null)
            return;

        switch (gameObject.name)
        {
            case "Booster Jelly":
                m_pAnimator.SetTrigger("Booster");
                break;
            case "Magnet Jelly":
                m_pAnimator.SetTrigger("Magnet");
                break;
            case "Mini Life Jelly":
                m_pAnimator.SetTrigger("Mini");
                break;
            case "Power Up Jelly":
                m_pAnimator.SetTrigger("Power");
                break;
            case "Yellow Change Jelly":
                m_pAnimator.SetTrigger("Yellow Change");
                break;
        }
    }

    //--------------------------------------
    // ��� �������� ���Ǵ� �ʿ� ��ɵ� 
    //--------------------------------------
    protected override void SetJellyFunction()
    {
        if(gameObject.CompareTag("Bouns") && !(m_pCookie.AccessCookieState is CookieBouns))
        {
            m_pBounsTime.SetBounsItem(gameObject.name);
            m_pSoundManager.PlaySound("GetJelly");
            return;
        }

        switch (gameObject.name)
        {
            case "Booster Jelly":
                m_pPoolManaged.pop("Booster Text Effect", transform.position);
                m_pCookie.SetCookieSkillMode(CGlobalEnum.COOKIE_SKILL.COOKIE_BOOSTER);
                break;
            case "Magnet Jelly":
                m_pPoolManaged.pop("Magnet Text Effect", transform.position);
                StartEvent(CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNET);
                break;
            case "Bonus Jelly":
                m_pPoolManaged.pop("Bouns Text Effect", transform.position);
                m_pBounsTime.StartBounsTime();
                m_pSoundManager.PlaySound("BounsJelly");
                break;
            case "Big Life Jelly":
                m_pCookie.SetCookieHp(20.0f);
                m_pPoolManaged.pop("Hp Text Effect", transform.position);
                m_pSoundManager.PlaySound("HeartJelly");
                break;
            case "Mini Life Jelly":
                m_pCookie.SetCookieHp(10.0f);
                m_pPoolManaged.pop("Hp Text Effect", transform.position);
                m_pSoundManager.PlaySound("HeartJelly");
                break;
            case "Power Up Jelly":
                m_pPoolManaged.pop("Power Up Text Effect", transform.position);
                m_pCookie.SetCookieSkillMode(CGlobalEnum.COOKIE_SKILL.COOKIE_POWER_UP);
                break;
            case "Yellow Change Jelly":
                StartEvent(CGlobalEnum.COOKIE_SKILL.COOKIE_CHANGE_YELLOW_JELLY);
                m_pPoolManaged.pop("Bear Party Text Effect", transform.position);
                m_pSoundManager.PlaySound("HeartJelly");
                break;
        }
    }

    //------------------------------------------------
    // �ڼ�, ������ ��Ƽ �ڽ� Ȱ��ȭ �� Ÿ�� ���� �Լ�
    //------------------------------------------------
    private void StartEvent(CGlobalEnum.COOKIE_SKILL _eCookieSkill)
    {
        if(!m_pCookieEventBox.gameObject.activeSelf)
        {
            m_pCookieEventBox.SetCookieEventType(_eCookieSkill);
            m_pCookieEventBox.gameObject.SetActive(true);

            return;
        }

        if (m_pCookieEventBox.GetEventType == _eCookieSkill)
        {
            switch (_eCookieSkill)
            {
                case CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNET:
                    m_pCookieEventBox.ResetMagnetTime();
                    break;
                case CGlobalEnum.COOKIE_SKILL.COOKIE_CHANGE_YELLOW_JELLY:
                    m_pCookieEventBox.ResetJellyChangeTime();
                    break;
            }
        }
        else if(m_pCookieEventBox.GetEventType == CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNETANDCHANGEJELLY)
        {
            switch (_eCookieSkill)
            {
                case CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNET:
                    m_pCookieEventBox.ResetMagnetTime();
                    break;
                case CGlobalEnum.COOKIE_SKILL.COOKIE_CHANGE_YELLOW_JELLY:
                    m_pCookieEventBox.ResetJellyChangeTime();
                    break;
            }
        }
        else
            m_pCookieEventBox.SetCookieEventType(CGlobalEnum.COOKIE_SKILL.COOKIE_MAGNETANDCHANGEJELLY);
    }

    private void OnEnable()
    {
        ComponentInit();

        SetJellyAnimation();
    }
}
