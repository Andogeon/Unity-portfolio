using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandSomeFace : FACE
{
    public override Sprite GetSprite()
    {
        if (m_pSprite == null)
            m_pSprite = GetComponent<SpriteRenderer>().sprite;

        return m_pSprite;
    }
}
