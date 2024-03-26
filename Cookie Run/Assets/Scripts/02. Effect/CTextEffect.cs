using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTextEffect : CEffect
{
    protected override void Awake()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pPoolManaged = CTopManager.GetInstance().GetPoolManaged;

        m_pCookie = FindObjectOfType<Cookie>();
    }

    protected override void Update()
    {
        //SetEffectParent();

        if (m_vOrlPosition == Vector2.zero)
            m_vOrlPosition = transform.localPosition;

        if (m_bIsDead)
        {
            SetEffectParent();

            TextAlphaDelete();

            return;
        }

        transform.localPosition += Vector3.one * m_fSpeed * Time.deltaTime;

        Vector2 _vDirection = (Vector2)transform.localPosition - (Vector2)m_vOrlPosition;
        float _fLength = _vDirection.magnitude;

        if (_fLength >= 1.6f)
            m_bIsDead = true;
    }

    private void TextAlphaDelete()
    {
        Color _SpriteColor = m_pSpriteRenderer.color;

        _SpriteColor.a -= m_fAlphaSpeed * Time.deltaTime;

        m_pSpriteRenderer.color = _SpriteColor;

        if (_SpriteColor.a <= 0.0f)
            gameObject.SetActive(false);
    }


    private void OnDisable()
    {
        Color _SpriteColor = m_pSpriteRenderer.color;
        _SpriteColor.a = 1.0f;

        m_pSpriteRenderer.color = _SpriteColor;

        m_vOrlPosition = Vector2.zero;

        m_bIsDead = false;
    }

    private bool m_bIsDead = false;

    private Vector2 m_vOrlPosition = Vector2.zero;

    private float m_fSpeed = 5.0f;

    private CPoolManaged m_pPoolManaged = null;
}
