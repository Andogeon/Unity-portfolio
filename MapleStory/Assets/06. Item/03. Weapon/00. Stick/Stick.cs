using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stick : ITEM // 몽둥이 클래스
{
    [SerializeField] private AudioClip _AttackSound = null;

    [SerializeField] GameObject[] _SlashEffects = null;

    private GameObject m_pEffectSlash = null;

    private SpriteRenderer m_pBodySpriteRenderer = null;

    private Body m_pBodyObejct = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private Vector3 m_vEffectLocalPosition = Vector3.zero;

    private string m_strInstanceSlashName = string.Empty;

    private bool m_bIsSlash = false;

    private bool m_bIsSound = false;

    private Player m_pPlayer = null;

    private void Start()
    {
        m_fOldAttack = m_fAttack = 100.0f;

        m_pSoundManager.AddSound("Mace Attack Sound", _AttackSound, SoundType.Sound_Effect);

        m_pGameobjectManager.OriginalGamgObjectInsert("First Slash", _SlashEffects[0]);

        m_pGameobjectManager.OriginalGamgObjectInsert("Second Slash", _SlashEffects[1]);

        m_pGameobjectManager.OriginalGamgObjectInsert("Third Slash", _SlashEffects[2]);

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        GameObject _Obejct = null;

        if (transform.parent == null)
            return;

        _Obejct = transform.parent.gameObject;

        _Obejct = _Obejct.transform.parent.gameObject;

        m_pBodySpriteRenderer = _Obejct.GetComponent<SpriteRenderer>();

        m_pBodyObejct = _Obejct.GetComponent<Body>();

        if(null == m_pPlayer && m_pBodySpriteRenderer.transform.parent.gameObject.name == "Player")
            m_pPlayer = m_pBodySpriteRenderer.transform.parent.GetComponent<Player>();
    }

    // 무기는 다르게 각 상태마다 레이어 소팅을 별도로 제어하여 무기를 들게 했습니다.

    // 레이어 소팅을 제어하지 않을 시 손 위에 무기가 있거나 점프 할 때 무기가 머리카락에 파묻히는 경우, 옷에 파묻히는 경우 등..

    // 이러한 문제를 해결하기 위해 레이어 소팅을 제어하게 했습니다.

    public void Update()
    {
        if (null == m_pBodyObejct || m_eItemModeType == ITEMMODETYPE.ITEM_INVENTORY) // 몸체 오브젝트나 해당 아이템이 인벤토리에 있다면 
        {
            if (transform.parent == null) // 핸들이 부모가 아니라면 
                return; // 종료 

            GameObject _Obejct = transform.parent.gameObject;

            _Obejct = _Obejct.transform.parent.gameObject;

            m_pBodySpriteRenderer = _Obejct.GetComponent<SpriteRenderer>();

            m_pBodyObejct = _Obejct.GetComponent<Body>();

            if (null == m_pPlayer && m_pBodySpriteRenderer.transform.parent.gameObject.name == "Player")
                m_pPlayer = m_pBodySpriteRenderer.transform.parent.GetComponent<Player>();
        }

        if (m_pPlayer != null && m_pPlayer.AccessPlayerLevelUp == true) // 플레이어가 레벨업을 할시 
        {
            m_fAttack = m_fOldAttack + m_pPlayer.AccessPlayerAttack; // 무기 공격력을 갱신한다.

            m_pPlayer.AccessPlayerLevelUp = false; 
        }

        m_eAvatarState = m_pBodyObejct.GetAvatarState; // 몸체 오브젝트의 상태 값을 받아온다.

        switch (m_eAvatarState) // 각 상태 별로 레이어 소팅, 로컬 좌표 변경, 스프라이트 변경하게 분기문을 구성 
        {
            case AVATARSTATES.AVATAR_IDLE:
                IDLE();
                _Animations[0].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                m_pSpriteRenderer.sortingLayerName = "2";
                break;
            case AVATARSTATES.AVATAR_RUN:
                RUN();
                _Animations[1].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                m_pSpriteRenderer.sortingLayerName = "2";
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                NormalAttack();
                _Animations[2].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                m_pSpriteRenderer.sortingLayerName = "1";
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                NormalAttack();
                _Animations[3].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                m_pSpriteRenderer.sortingLayerName = "1";
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                NormalAttack();
                _Animations[4].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                m_pSpriteRenderer.sortingLayerName = "1";
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                Jump();
                _Animations[5].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                m_pSpriteRenderer.sortingLayerName = "2";
                break;
        }
    }

    // 플레이어가 대기 일때 호출 되는 함수입니다.

    public override Vector3 IDLE()
    {
        if (m_bIsSound == true)
            m_bIsSound = false;

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Idle0.weapon":
                transform.localPosition = new Vector3(-0.143f, -0.099f);
                break;
        }

        return transform.localPosition;
    }

    // 플레이어가 이동시 호출 되는 함수입니다.

    public override Vector3 RUN()
    {
        if (m_bIsSound == true)
            m_bIsSound = false;

        switch (m_pBodySpriteRenderer.sprite.name)
        {
            case "run0.body":
                transform.localPosition = new Vector3(-0.125f, -0.04f);
                break;
            case "run1.body":
                transform.localPosition = new Vector3(-0.15f, 0.005f);
                break;
            case "run2.body":
                transform.localPosition = new Vector3(-0.125f, -0.04f);
                break;
            case "run3.body":
                transform.localPosition = new Vector3(-0.106f, -0.08f);
                break;
        }

        return transform.localPosition;
    }

    // 플레이어가 공격시 호출 되는 함수입니다.

    public override void NormalAttack()
    {
        switch (m_pSpriteRenderer.sprite.name)
        {
            case "First Attack0.weapon":
                transform.localPosition = new Vector3(0.173f, -0.018f);
                break;
            case "First Attack1.weapon":
                if (m_bIsSound == false)
                {
                    m_pSoundManager.PlaySound("Mace Attack Sound");

                    m_bIsSound = true;
                }
                m_bIsSlash = false;
                transform.localPosition = new Vector3(0.102f, -0.075f);
                break;
            case "First Attack2.weapon":
                transform.localPosition = new Vector3(0.153f, 0.132f);
                m_strInstanceSlashName = "First Slash";
                m_vEffectLocalPosition = new Vector3(-0.669f, -0.194f);
                m_bIsSound = false;
                CreateEffectSlash();
                break;
            case "Second Attack0.weapon":
                transform.localPosition = new Vector3(0.028f, 0.131f);
                break;
            case "Second Attack1.weapon":
                if (m_bIsSound == false)
                {
                    m_pSoundManager.PlaySound("Mace Attack Sound");

                    m_bIsSound = true;
                }
                m_bIsSlash = false;
                transform.localPosition = new Vector3(-0.01f, 0.136f);
                break;
            case "Second Attack2.weapon":
                transform.localPosition = new Vector3(0.133f, -0.133f);
                m_strInstanceSlashName = "Second Slash";
                m_vEffectLocalPosition = new Vector3(-0.68f, 0.234f);
                m_bIsSound = false;
                CreateEffectSlash();
                break;
            case "Third Attack0.weapon":
                transform.localPosition = new Vector3(0.102f, 0.129f);
                break;
            case "Third Attack1.weapon":
                if (m_bIsSound == false)
                {
                    m_pSoundManager.PlaySound("Mace Attack Sound");

                    m_bIsSound = true;
                }
                m_bIsSlash = false;
                transform.localPosition = new Vector3(-0.196f, -0.019f);
                break;
            case "Third Attack2.weapon":
                transform.localPosition = new Vector3(0.211f, -0.003f);
                m_strInstanceSlashName = "Third Slash";
                m_vEffectLocalPosition = new Vector3(-0.725f, 0.007f);
                m_bIsSound = false;
                CreateEffectSlash();
                break;
        }
    }

    // 플레이어가 점프 할때 호출 되는 함수입니다.

    public override void Jump()
    {
        if (m_bIsSound == true)
            m_bIsSound = false;

        transform.localPosition = new Vector3(0.0264f, 0.1319f);
    }

    // 공격시 이펙트를 생성하게 하는 함수입니다.

    private void CreateEffectSlash()
    {
        if (m_bIsSlash == true) // 추가 이펙트가 생성되지 않게 제어
            return;

        m_pEffectSlash = m_pGameobjectManager.GameObejctPooling(m_strInstanceSlashName, m_vEffectLocalPosition, Vector3.zero, transform.rotation, transform);

        Slash _Slash = m_pEffectSlash.GetComponent<Slash>();

        _Slash.AccessSlashItem = this;

        m_bIsSlash = true;
    }

    public override Sprite GetSprite()
    {
        return null;
    }

    public void OnDestroy()
    {
        m_pBodyObejct = null;

        m_pBodySpriteRenderer = null;

        m_pInventoryIcon = null;

        m_pItem = null;

        m_pItemobject = null;

        m_pOrlItem = null;

        m_pPartSpriteRenderer = null;

        m_pSprite = null;

        m_pSpriteRenderer = null;

        for (int i = 0; i < _Animations.Length; ++i)
            _Animations[i] = null;

        m_pGameobjectManager = null;

        Resources.UnloadUnusedAssets();
    }
}