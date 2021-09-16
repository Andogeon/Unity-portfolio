using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalHair : HAIR
{
    public override Sprite GetSprite()
    {
        if (m_pSprite == null)
            m_pSprite = GetComponent<SpriteRenderer>().sprite;

        return m_pSprite;
    }


    // 사다리나 로프에 올라갈 때 스프라이트 변경을 위한 함수입니다.
    public override void LadderChangeHair(SpriteRenderer _SpriteRenderer)
    {
        if (null == _SpriteRenderer)
            return;

        if (null == m_pOldSprite) // 기존 사다리나 로프 타지 않을 때 헤어 스프라이트가 존재 하지 않을 경우 
        {
            if (null == m_pSpriteRenderer)
                m_pSpriteRenderer = GetComponent<SpriteRenderer>();

            m_pOldSprite = m_pSpriteRenderer.sprite; // 기존 헤어 스프라이트를 변수로 따로 보관 
        }

        _SpriteRenderer.sprite = _LadderHair; // 사다리나 로프헤어로 변경 후 

        _SpriteRenderer.transform.localPosition = new Vector3(0.017f, 0.045f); // 로컬 좌표를 변경 
    }


    // 다시 사다리나 로프에 내려왔을 때 기존 헤어 스프라이트 변경을 위한 함수입니다.
    public override void ResetHair(SpriteRenderer _SpriteRenderer)
    {
        if (null == _SpriteRenderer || null == m_pOldSprite)
            return;

        _SpriteRenderer.sprite = m_pOldSprite; // 다시 기존 헤어 스프라이트로 변경 후 

        _SpriteRenderer.transform.localPosition = new Vector3(-0.028f, 0.045f); // 로컬 좌표를 변경 
    }
}