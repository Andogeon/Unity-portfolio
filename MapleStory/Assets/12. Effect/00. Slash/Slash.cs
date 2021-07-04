using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : EFFECT
{
    //[SerializeField] private AudioClip _HiySound = null;

    [SerializeField] private GameObject _DamageFont = null;

    [SerializeField] private GameObject[] _HitEffect = null;

    [SerializeField] string _EffectName = string.Empty;

    private BoxCollider2D m_pBoxCollision = null;

    private SpriteRenderer m_pSpriteRenderer = null;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private Quaternion _Rotation = Quaternion.identity;

    private bool m_bIsCollision = false;

    private ITEM m_pParnentItem = null;

    private int m_iHitCount = 0;

    public ITEM AccessSlashItem
    {
        get { return m_pParnentItem; }

        set { m_pParnentItem = value; }
    }

    //private Transform m_pParnentTransform = null;

    //private HitFont m_pDamageFont = null;

    private void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pBoxCollision = GetComponent<BoxCollider2D>();

        _Rotation = transform.parent.rotation;

        //m_pParnentTransform = transform.parent;

        transform.SetParent(null);

        if(m_pBoxCollision.enabled == false)
            m_pBoxCollision.enabled = true;

        m_pGameObjectManager.OriginalGamgObjectInsert("Sword Hit Effect", _HitEffect[0]);

        m_pGameObjectManager.OriginalGamgObjectInsert("Stick Hit Effect", _HitEffect[1]);

        m_pGameObjectManager.OriginalGamgObjectInsert("Damage Font", _DamageFont);

        //m_pParnentItem = m_pParnentTransform.GetComponent<ITEM>();

        //if (transform.parent != null) // null
        //    m_pParnentItem = transform.parent.GetComponent<ITEM>();
    }

    private void Update()
    {
        if (transform.parent != null)
        {
            //m_pParnentTransform = transform.parent;

            if (null == m_pParnentItem)
                m_pParnentItem = transform.parent.GetComponent<ITEM>();

            transform.SetParent(null);
        }

        if (m_pSpriteRenderer.color.a <= 0.0f)
        {
            m_pSpriteRenderer.color = new Color(m_pSpriteRenderer.color.r, m_pSpriteRenderer.color.g, m_pSpriteRenderer.color.b, 1.0f);

            m_pGameObjectManager.Remove(_EffectName, gameObject);

            m_pBoxCollision.enabled = false;

            return;
        }

        if (m_pBoxCollision.enabled == false && m_bIsCollision == false)
            m_pBoxCollision.enabled = true;
    }

    private void OnEnable()
    {
        if (null == m_pBoxCollision)
            m_pBoxCollision = GetComponent<BoxCollider2D>();

        if (m_pBoxCollision.enabled == false)
            m_pBoxCollision.enabled = true;

        if (transform.parent != null) // null
            m_pParnentItem = transform.GetComponentInParent<ITEM>();
    }

    private void OnDisable()
    {
        m_pParnentItem = null;

        m_iHitCount = 0;

        //m_pParnentTransform = null;
    }

    private void OnDestroy()
    {
        m_pParnentItem = null;

        //m_pParnentTransform = null;
    }

    private void OnTriggerEnter2D(Collider2D collision) // 충돌 !! 
    {
        if (m_iHitCount != 0)
        {
            m_pBoxCollision.enabled = false;

            return;
        }

        MONTER _Monter = collision.GetComponent<MONTER>();

        if(null != _Monter && _Monter.name != "Maple Box" && _Monter.name != "Ticket Box")
        {
            //if (null == m_pParnentItem)
            //    m_pParnentItem = m_pParnentTransform.GetComponent<ITEM>();

            _Monter.AccessHp -= m_pParnentItem.AccessAttack;

            _Monter.SetHItAnimation();

            GameObject _Object = m_pGameObjectManager.GameObejctPooling("Damage Font", Vector3.zero, Vector3.zero, Quaternion.identity);

            _Object.transform.position = _Monter.transform.position;

            HitFont _Font = _Object.GetComponent<HitFont>();

            _Font.SelectNumberFont((int)m_pParnentItem.AccessAttack);

            m_iHitCount = 1;
        }
        else
        {
            MapleBox _MapleBox = _Monter as MapleBox;

            if (null != _MapleBox)
            {
                _MapleBox.AccessHit = true;

                _MapleBox.HitAnimation();

                _MapleBox.AccessHitCount -= 1;

                m_iHitCount = 1;
            }
        }

        m_pBoxCollision.enabled = false;
    }

    public override void CreateHitEffect(Vector3 _Position)
    {
        if (m_pParnentItem == null)
            return;

        GameObject _HitObject = null;

        switch(m_pParnentItem.AccessItemType)
        {
            case ITEMTYPE.ITEM_STICK:
                _HitObject = m_pGameObjectManager.GameObejctPooling("Stick Hit Effect", Vector3.zero, Vector3.zero, Quaternion.identity);
                _HitObject.transform.position = _Position;
                break;
            case ITEMTYPE.ITEM_SWORD:
                _HitObject = m_pGameObjectManager.GameObejctPooling("Sword Hit Effect", Vector3.zero, Vector3.zero, Quaternion.identity);
                _HitObject.transform.position = _Position;
                break;
        }
    }
}
