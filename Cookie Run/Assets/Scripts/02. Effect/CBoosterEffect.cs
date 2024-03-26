using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBoosterEffect : CEffect
{
    private CGameScene m_pGameScene = null;

    private static int m_iSortNumber = 0;

    private Vector3 m_vOffset = Vector3.zero;

    protected override void Awake()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pGameScene = GameObject.Find("GameScene").GetComponent<CGameScene>();

        m_pCookie = FindObjectOfType<Cookie>();

        m_fAlphaSpeed = 1.0f;

        m_pSpriteRenderer.sortingOrder = m_iSortNumber--;
    }

    protected override void Update()
    {
        SetEffectParent();

        float _fAlpha = m_pSpriteRenderer.color.a - m_fAlphaSpeed * Time.deltaTime;

        m_pSpriteRenderer.color = new Color(m_pSpriteRenderer.color.r, m_pSpriteRenderer.color.g, m_pSpriteRenderer.color.b, _fAlpha);

        if (_fAlpha <= 0.0f)
            Destroy(gameObject);
    }

    public void SetOffset(Vector3 _vOffset)
    {
        m_vOffset = _vOffset;
    }

    public static void ResetSortNumber()
    {
        m_iSortNumber = 0;
    }
}
