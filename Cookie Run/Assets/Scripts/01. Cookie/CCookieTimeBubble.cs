using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CCookieTimeBubble : MonoBehaviour
{
    [SerializeField] private Sprite[] m_TimeSprites = null;

    private SpriteRenderer m_pSpriteRenderer = null;

    private void OnEnable()
    {
        m_pSpriteRenderer = (m_pSpriteRenderer == null) ? GetComponent<SpriteRenderer>() : m_pSpriteRenderer;
    }

    public void SetCount(int _iCount)
    {
        if (_iCount == 0)
            return;

        int _iIndex = 3 - _iCount;

        m_pSpriteRenderer.sprite = m_TimeSprites[_iIndex];
    }
}
