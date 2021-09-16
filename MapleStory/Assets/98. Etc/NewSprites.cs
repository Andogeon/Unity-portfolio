using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 아이템의 애니메이션과 몸체의 애니메이션을 맞추기 위한 클래스입니다.
public class NewSprites : AnimationSprites
{
    [SerializeField] private Sprite[] _Sprites = null;

    [SerializeField] private string[] _Spritesname = null;

    // 플레이어 해당 부위에 애니메이션과 아이템의 애니메이션을 맞추기 위해 만든 함수입니다.
    public override void UpdateAnimation(SpriteRenderer _UpdateSpriteRenderer, SpriteRenderer _SpriteRenderer)
    {
        // 2개의 인자중 하나라도 관련 정보가 존재하지 않는다면 종료 

        if (null == _UpdateSpriteRenderer || null == _SpriteRenderer)
            return;

        for (int i = 0; i < _Spritesname.Length; ++i) // 인스펙터에서 받은 몸체의 애니메이션 스프라이트 명을 하나씩 순회하면서
        {
            if (_Spritesname[i] == _SpriteRenderer.sprite.name) // 만약 현재 부위의 스프라이트 명과 스프라이트 이름의 이름이 같다면 
            {
                _UpdateSpriteRenderer.sprite = _Sprites[i]; // 해당 인덱스의 스프라이트를 적용할 스프라이트로 적용하여 

                // 애니메이션을 맞춤

                return;
            }
        }
    }
}
