using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour
{
    [SerializeField] private Sprite[] _Sprites = null;

    private Image m_pImage = null;

    public void LevelUpdate(Player _Player)
    {
        if (null == _Player)
            return;

        if (null == m_pImage)
            m_pImage = GetComponent<Image>();

        int _PlayerLevel = _Player.GetLevel - 1;

        if (_PlayerLevel >= _Sprites.Length - 1)
            _PlayerLevel = _Sprites.Length - 1;
        else if (_PlayerLevel <= 0)
            _PlayerLevel = 0;

        m_pImage.sprite = _Sprites[_PlayerLevel];
    }
}
