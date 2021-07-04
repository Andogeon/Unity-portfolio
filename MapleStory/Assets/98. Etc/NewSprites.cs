using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewSprites : AnimationSprites
{
    [SerializeField] private Sprite[] _Sprites = null;

    [SerializeField] private string[] _Spritesname = null;

    public override void UpdateAnimation(SpriteRenderer _UpdateSpriteRenderer, SpriteRenderer _SpriteRenderer)
    {
        if (null == _UpdateSpriteRenderer || null == _SpriteRenderer)
            return;

        for (int i = 0; i < _Spritesname.Length; ++i)
        {
            if (_Spritesname[i] == _SpriteRenderer.sprite.name)
            {
                _UpdateSpriteRenderer.sprite = _Sprites[i];

                return;
            }
        }
    }
}
