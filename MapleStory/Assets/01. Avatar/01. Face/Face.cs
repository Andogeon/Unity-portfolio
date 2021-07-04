using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Face : PART
{
    private void Awake()
    {
        if(null == m_pSpriteRenderer)
            m_pSpriteRenderer = GetComponent<SpriteRenderer>();
    }

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
        return m_pSpriteRenderer.sprite;
    }

    public void OnDestroy()
    {
        m_pSpriteRenderer = null;
    }
}
