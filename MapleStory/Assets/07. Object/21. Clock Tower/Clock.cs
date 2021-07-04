using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Sprite[] _NumberSprites = null;

    [SerializeField] private SpriteRenderer[] _SpriteRenderers = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        string _Timestring = System.DateTime.Now.ToString("HH");

        int Number = 0;

        int.TryParse(_Timestring, out Number);

        int _Index = Number / 10;

        _SpriteRenderers[0].sprite = _NumberSprites[_Index];

        _Index = Number % 10;

        _SpriteRenderers[1].sprite = _NumberSprites[_Index];

       _Timestring = System.DateTime.Now.ToString("mm");

       int.TryParse(_Timestring, out Number);

        _Index = Number / 10;

        _SpriteRenderers[2].sprite = _NumberSprites[_Index];

        _Index = Number % 10;

        _SpriteRenderers[3].sprite = _NumberSprites[_Index];
    }
}
