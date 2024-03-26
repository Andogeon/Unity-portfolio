using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CEffect : MonoBehaviour
{
    protected abstract void Awake();

    protected abstract void Update();

    public void SetEffectParent()
    {
        if (transform.parent == null)
        {
            RaycastHit2D _Hit2D;

            if (_Hit2D = Physics2D.Raycast(transform.position, Vector2.down, 100.0f, 1 << 6))
            {
                Transform _pJellyGroup = _Hit2D.collider.gameObject.transform.parent.Find("Jelly Group");

                if (_pJellyGroup != null)
                    transform.SetParent(_pJellyGroup);
                else
                {
                    Transform _pTilePatten = m_pCookie.GetTilePatten;

                    if (_pTilePatten != null && _pTilePatten.name.Contains("Tile Pattern"))
                        transform.SetParent(_pTilePatten.Find("Jelly Group"));
                    else if(_pTilePatten != null && !_pTilePatten.name.Contains("Tile Pattern"))
                    {
                        _pTilePatten = _pTilePatten.parent;
                        transform.SetParent(_pTilePatten.Find("Jelly Group"));
                    }
                }
            }
            else
            {
                Transform _pTilePatten = m_pCookie.GetTilePatten;

                if (_pTilePatten != null && _pTilePatten.name.Contains("Tile Pattern"))
                    transform.SetParent(_pTilePatten.Find("Jelly Group"));
                else if (_pTilePatten != null && !_pTilePatten.name.Contains("Tile Pattern"))
                {
                    _pTilePatten = _pTilePatten.parent;
                    transform.SetParent(_pTilePatten.Find("Jelly Group"));
                }
            }
        }
    }


    protected SpriteRenderer m_pSpriteRenderer = null;

    protected float m_fAlphaSpeed = 3.0f;

    protected Cookie m_pCookie = null;
}


//public void SetEffectParent()
//{
//    if (transform.parent == null)
//    {
//        RaycastHit2D _Hit2D;

//        Transform _pJellyGroup = null;

//        if (_Hit2D = Physics2D.Raycast(transform.position, Vector2.down, 100.0f, 1 << 6))
//        {
//            _pJellyGroup = _Hit2D.collider.gameObject.transform.parent.Find("Jelly Group");

//            if (_pJellyGroup != null)
//                transform.SetParent(_pJellyGroup);
//            else
//            {
//                _pJellyGroup = m_pCookie.GetTilePatten;

//                if (_pJellyGroup != null)
//                    transform.SetParent(_pJellyGroup.Find("Jelly Group"));
//            }
//        }
//        else
//        {
//            _pJellyGroup = m_pCookie.GetTilePatten;

//            if (_pJellyGroup != null)
//                transform.SetParent(_pJellyGroup.Find("Jelly Group"));
//        }
//    }
//}