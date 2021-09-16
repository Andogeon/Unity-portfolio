using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WildFace : FACE // 날카로운 얼굴
{
    public override Sprite GetSprite()
    {
        if (m_pSprite == null)
            m_pSprite = GetComponent<SpriteRenderer>().sprite;

        return m_pSprite;
    }
}
