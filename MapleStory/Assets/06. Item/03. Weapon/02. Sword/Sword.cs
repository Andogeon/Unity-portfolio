using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Stick 클래스와 기능은 같으며 아이템의 스프라이트만 따로 변경한 클래스입니다.
public class Sword : ITEM // 한 손검 클래스입니다.
{
    [SerializeField] private AudioClip _AttackSound = null;

    [SerializeField] private GameObject[] _SlashEffects = null;

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
        m_eItemType = ITEMTYPE.ITEM_SWORD;

        m_fOldAttack = m_fAttack = 12.0f;

        m_pSoundManager.AddSound("Sword Attack Sound", _AttackSound, SoundType.Sound_Effect);

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

        if (null == m_pPlayer && m_pBodySpriteRenderer.transform.parent.gameObject.name == "Player")
            m_pPlayer = m_pBodySpriteRenderer.transform.parent.GetComponent<Player>();
    }

    private void Update()
    {
        if (null == m_pBodyObejct || m_eItemModeType == ITEMMODETYPE.ITEM_INVENTORY)
        {
            if (transform.parent == null)
                return;

            GameObject _Obejct = transform.parent.gameObject;

            _Obejct = _Obejct.transform.parent.gameObject;

            m_pBodySpriteRenderer = _Obejct.GetComponent<SpriteRenderer>();

            m_pBodyObejct = _Obejct.GetComponent<Body>();

            if (null == m_pPlayer && m_pBodySpriteRenderer.transform.parent.gameObject.name == "Player")
                m_pPlayer = m_pBodySpriteRenderer.transform.parent.GetComponent<Player>();
        }

        if (m_pPlayer != null && m_pPlayer.AccessPlayerLevelUp == true)
        {
            m_fAttack = m_fOldAttack + m_pPlayer.AccessPlayerAttack;

            m_pPlayer.AccessPlayerLevelUp = false;
        }

        m_eAvatarState = m_pBodyObejct.GetAvatarState;

        switch (m_eAvatarState)
        {
            case AVATARSTATES.AVATAR_IDLE:
                m_pSpriteRenderer.sortingLayerName = "2";
                _Animations[0].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                IDLE();
                break;
            case AVATARSTATES.AVATAR_RUN:
                m_pSpriteRenderer.sortingLayerName = "2";
                _Animations[1].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                RUN();
                break;
            case AVATARSTATES.AVATAR_FIRSTNORMALATTACK:
                m_pSpriteRenderer.sortingLayerName = "1";
                _Animations[2].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_SECONDNORMALATTACK:
                m_pSpriteRenderer.sortingLayerName = "1";
                _Animations[3].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_THIRDNORMALATTACK:
                m_pSpriteRenderer.sortingLayerName = "1";
                _Animations[4].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                NormalAttack();
                break;
            case AVATARSTATES.AVATAR_HIT:
            case AVATARSTATES.AVATAR_JUMP:
                m_pSpriteRenderer.sortingLayerName = "2";
                _Animations[5].UpdateAnimation(m_pSpriteRenderer, m_pBodySpriteRenderer);
                Jump();
                break;
        }
    }

    public override Vector3 IDLE()
    {
        if (m_bIsSound == true)
            m_bIsSound = false;

        switch (m_pSpriteRenderer.sprite.name)
        {
            case "Idle0.weapon":
                transform.localPosition = new Vector3(-0.121f, -0.084f);
                break;
        }

        return transform.localPosition;
    }

    public override Vector3 RUN()
    {
        if (m_bIsSound == true)
            m_bIsSound = false;

        switch (m_pBodySpriteRenderer.sprite.name)
        {
            case "run0.body":
                transform.localPosition = new Vector3(-0.106f, -0.043f);
                break;
            case "run1.body":
                transform.localPosition = new Vector3(-0.135f, -0.018f);
                break;
            case "run2.body":
                transform.localPosition = new Vector3(-0.106f, -0.043f);
                break;
            case "run3.body":
                transform.localPosition = new Vector3(-0.095f, -0.061f);
                break;
        }

        return transform.localPosition;
    }

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
                    m_pSoundManager.PlaySound("Sword Attack Sound");

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
                transform.localPosition = new Vector3(0.02f, 0.124f);
                break;
            case "Second Attack1.weapon":
                if (m_bIsSound == false)
                {
                    m_pSoundManager.PlaySound("Sword Attack Sound");

                    m_bIsSound = true;
                }
                m_bIsSlash = false;
                transform.localPosition = new Vector3(-0.013f, 0.122f);
                break;
            case "Second Attack2.weapon":
                transform.localPosition = new Vector3(0.133f, -0.133f);
                m_strInstanceSlashName = "Second Slash";
                m_vEffectLocalPosition = new Vector3(-0.68f, 0.234f);
                m_bIsSound = false;
                CreateEffectSlash();
                break;


            case "Third Attack0.weapon":
                transform.localPosition = new Vector3(0.142f, 0.089f);
                break;
            case "Third Attack1.weapon":
                if (m_bIsSound == false)
                {
                    m_pSoundManager.PlaySound("Sword Attack Sound");

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

    public override void Jump()
    {
        if (m_bIsSound == true)
            m_bIsSound = false;

        transform.localPosition = new Vector3(0.071f, 0.099f);
    }

    private void CreateEffectSlash()
    {
        if (m_bIsSlash == true)
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