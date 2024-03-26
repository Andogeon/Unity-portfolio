using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    protected virtual void Awake()
    {
        ComponentInitialize();
    }

    private void ComponentInitialize()
    {
        m_pBoxCollder = GetComponent<BoxCollider2D>();

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pJellyGroup = transform.parent.Find("Jelly Group");

        if (m_pJellyGroup != null)
        {
            m_iJellyCount = m_pJellyGroup.transform.childCount;

            m_vJellylocalPositions = new Vector3[m_iJellyCount];

            for (int i = 0; i < m_iJellyCount; ++i)
                m_vJellylocalPositions[i] = m_pJellyGroup.transform.GetChild(i).localPosition;
        }
    }
    
    public float GetTileLeftPostion()
    {
        if(m_pBoxCollder == null)
            m_pBoxCollder = GetComponent<BoxCollider2D>();

        return transform.position.x - m_pBoxCollder.bounds.extents.x;
    }

    public float GetTileRightPostion()
    {
        if (m_pBoxCollder == null)
            m_pBoxCollder = GetComponent<BoxCollider2D>();

        return transform.position.x + m_pBoxCollder.bounds.extents.x;
    }

    public void ResetJelly()
    {
        if (m_pJellyGroup == null)
            return;

        for(int i = 0; i < m_pJellyGroup.childCount; ++i)
        {
            Transform _pJelly = m_pJellyGroup.GetChild(i);

            if (i >= m_iJellyCount)
            {
                Destroy(_pJelly.gameObject);
                continue;
            }

            _pJelly.transform.localPosition = m_vJellylocalPositions[i];
            _pJelly.gameObject.SetActive(true);
        }
    }

    public virtual void TileAnimationChecking(BoxCollider2D _pGameSceneBox)
    {

    }

    public virtual void ResetMyAnimation()
    {
    }

    public void ResetAnimation()
    {
        Transform _pParentTrasnform = transform.parent;

        Tile[] _pTiles = _pParentTrasnform.GetComponentsInChildren<Tile>();

        for (int i = 0; i < _pTiles.Length; ++i)
        {
            if (_pTiles[i] == null || _pTiles[i] == this)
                continue;

            _pTiles[i].ResetMyAnimation();
        }
    }

    protected BoxCollider2D     m_pBoxCollder = null;

    protected SpriteRenderer    m_pSpriteRenderer = null;

    protected Transform         m_pJellyGroup = null;

    private int m_iJellyCount = 0;

    private Vector3[] m_vJellylocalPositions = null;
}












//public virtual void ResetAnimation()
//{
//    Transform _pParentTransform = transform.parent;

//    Tile[] _pTiles = _pParentTransform.GetComponentsInChildren<Tile>();

//    for (int i = 0; i < _pTiles.Length; ++i)
//    {
//        if (_pTiles[i] == null || _pTiles[i] == this)
//            continue;

//        _pTiles[i].ResetAnimation();
//    }
//}
















//public class Tile : MonoBehaviour
//{
//    [SerializeField] private TlieEffectType m_eTileEffectType = TlieEffectType.Tile_Nomral;

//    private BoxCollider2D m_pBoxCollder = null;

//    private SpriteRenderer m_pSpriteRenderer = null;

//    private float m_fBoxHalfSize = 0.0f;

//    private float m_fBoxSize = 0.0f;

//    private Cookie m_pUserCookie = null;

//    private CGameScene m_pGameScene = null;

//    private Animator m_pAnimator = null;

//    private bool m_bIsSpeed = false;

//    public float GetTileLeftPoint { get => transform.localPosition.x - m_fBoxHalfSize; }

//    public float GetTileRightPoint { get => transform.position.x + m_fBoxHalfSize; }

//    public float GetTileHalfSize { get => m_fBoxHalfSize; }

//    void Awake()
//    {
//        initialize();
//    }

//    private void initialize()
//    {
//        m_pBoxCollder = GetComponent<BoxCollider2D>();

//        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

//        m_pUserCookie = FindObjectOfType<Cookie>();

//        m_fBoxSize = (m_pBoxCollder.size.x * transform.localScale.x);

//        m_fBoxHalfSize = m_fBoxSize / 2;

//        m_pGameScene = FindObjectOfType<CGameScene>();

//        if (m_eTileEffectType == TlieEffectType.Tile_Nomral)
//        {
//            m_pAnimator = GetComponent<Animator>();

//            if (m_pAnimator != null)
//                Destroy(m_pAnimator);

//            return;
//        }

//        if (m_eTileEffectType != TlieEffectType.Tile_Lava)
//            m_pAnimator = GetComponent<Animator>();
//    }

//    private void FixedUpdate()
//    {
//        if (m_eTileEffectType == TlieEffectType.Tile_Nomral)
//            return;

//        switch (m_eTileEffectType)
//        {
//            case TlieEffectType.Tile_Lava:
//                StartTileEffectMode();
//                break;
//            case TlieEffectType.Tile_Hind:
//                AniTileEffectMode();
//                break;
//            case TlieEffectType.Tile_Move:
//                AniMoveTile();
//                break;
//        }
//    }


//    private void EnableTileEffect()
//    {
//        if (transform.childCount == 0)
//            return;

//        Transform _TileEffectTrasform = transform.GetChild(0);

//        if (_TileEffectTrasform == null)
//            return;

//        if (_TileEffectTrasform.gameObject.activeSelf)
//            return;

//        _TileEffectTrasform.gameObject.SetActive(true);
//    }

//    private void StartTileEffectMode()
//    {
//        //if (transform.localPosition.x >= m_pUserCookie.GetCooikeLeftPosition)
//        //    return;

//        //EnableTileEffect();
//    }

//    private void AniTileEffectMode()
//    {
//        if (7.93f < transform.position.x)
//            return;

//        m_pAnimator.SetTrigger("Move");
//    }

//    private void AniMoveTile()
//    {
//        if (m_bIsSpeed)
//            return;

//        //m_pAnimator.speed = Random.Range(1.0f, 1.1f);

//        m_pAnimator.speed = 1.0f;

//        m_bIsSpeed = true;
//    }
//}