using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HAIR : PART
{
    [SerializeField] protected Sprite _LadderHair = null;

    protected Sprite m_pOldSprite = null;

    public override Vector3 IDLE()
    {
        return Vector3.zero;
    }
    public override Vector3 RUN()
    {
        return Vector3.zero;
    }

    public override Sprite GetSprite()
    {
        return null;
    }

    public virtual void LadderChangeHair(SpriteRenderer _SpriteRenderer)
    {
        if (null == _SpriteRenderer)
            return;

        if (null == m_pOldSprite)
        {
            if (null == m_pSpriteRenderer)
                m_pSpriteRenderer = GetComponent<SpriteRenderer>();

            m_pOldSprite = m_pSpriteRenderer.sprite;
        }

        _SpriteRenderer.sprite = _LadderHair;
    }

    public virtual void ResetHair(SpriteRenderer _SpriteRenderer)
    {
        if (null == _SpriteRenderer || null == m_pOldSprite)
            return;

        _SpriteRenderer.sprite = m_pOldSprite;
    }
}
