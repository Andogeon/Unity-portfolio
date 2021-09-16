using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hair : PART
{
    private HAIR m_pHair = null;

    public HAIR AccessHair
    {
        get { return m_pHair; }

        set { m_pHair = value; }
    }

    private void Awake()
    {
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

    public override Sprite GetSprite() // 헤어 스프라이트의 데이터를 넘겨는 함수 
    {
        return m_pSpriteRenderer.sprite;
    }

    public void ChangeLadderHair()
    {
        if (m_pHair == null)
            return;

        m_pHair.LadderChangeHair(m_pSpriteRenderer);
    }

    public void ResetHair()
    {
        if (m_pHair == null)
            return;

        m_pHair.ResetHair(m_pSpriteRenderer);
    }
}
