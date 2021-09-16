using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 케릭터 커스터마이징을 위해서 준비한 클래스입니다.

public class Face : PART // 얼굴의 최상위 계층 클래스
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
