﻿using System.Collections;
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


    public override void LadderChangeHair(SpriteRenderer _SpriteRenderer)
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

        _SpriteRenderer.transform.localPosition = new Vector3(0.017f, 0.045f);
    }

    public override void ResetHair(SpriteRenderer _SpriteRenderer)
    {
        if (null == _SpriteRenderer || null == m_pOldSprite)
            return;

        _SpriteRenderer.sprite = m_pOldSprite;

        _SpriteRenderer.transform.localPosition = new Vector3(-0.028f, 0.045f);
    }
}