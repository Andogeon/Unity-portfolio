using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitFont : MonoBehaviour
{
    [SerializeField] private string _DeleteName = string.Empty;

    [SerializeField] Sprite[] _NumberSprite = null;

    private SpriteRenderer[] m_pNumberSpriteRenderer = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    public void SelectNumberFont(float _iDamege)
    {
        if(null == m_pNumberSpriteRenderer)
        {
            m_pNumberSpriteRenderer = new SpriteRenderer[transform.childCount];

            for (int i = 0; i < m_pNumberSpriteRenderer.Length; ++i)
                m_pNumberSpriteRenderer[i] = transform.GetChild(i).GetComponent<SpriteRenderer>();
        }

        if (_iDamege <= 0)
        {
            Destroy(gameObject);

            return;
        }

        float _SelectNumber = 100.0f; // 백 단위 먼저 

        int _iIndex = 0;

        if (_iDamege / _SelectNumber < 1)
            m_pNumberSpriteRenderer[0].gameObject.SetActive(false);
        else
        {
            if (m_pNumberSpriteRenderer[0].gameObject.activeSelf == false)
                m_pNumberSpriteRenderer[0].gameObject.SetActive(true);

            _iIndex = (int)_iDamege / (int)_SelectNumber;

            m_pNumberSpriteRenderer[0].sprite = _NumberSprite[_iIndex];

            _iDamege -= _iIndex * (int)_SelectNumber;
        }

        _SelectNumber /= 10;

        if (_iDamege % _SelectNumber < 0 && _iDamege / _SelectNumber <= 0)
            m_pNumberSpriteRenderer[1].gameObject.SetActive(false);
        else
        {
            if (m_pNumberSpriteRenderer[1].gameObject.activeSelf == false)
                m_pNumberSpriteRenderer[1].gameObject.SetActive(true);

            _iIndex = (int)_iDamege / (int)_SelectNumber;

            m_pNumberSpriteRenderer[1].sprite = _NumberSprite[_iIndex];

            _iDamege -= _iIndex * (int)_SelectNumber;
        }

        _SelectNumber /= 10;

        _iIndex = (int)_iDamege / (int)_SelectNumber;

        m_pNumberSpriteRenderer[2].sprite = _NumberSprite[_iIndex];


        //if (_iDamege / _SelectNumber < 1)
        //    m_pNumberSpriteRenderer[1].gameObject.SetActive(false);
        //else
        //{
        //    if (m_pNumberSpriteRenderer[1].gameObject.activeSelf == false)
        //        m_pNumberSpriteRenderer[1].gameObject.SetActive(true);

        //    _iIndex = (int)_iDamege / (int)_SelectNumber;

        //    m_pNumberSpriteRenderer[1].sprite = _NumberSprite[_iIndex];

        //    _iDamege -= _iIndex * (int)_SelectNumber;
        //}

        //_SelectNumber /= 10;

        //_iIndex = (int)_iDamege / (int)_SelectNumber;

        //m_pNumberSpriteRenderer[2].sprite = _NumberSprite[_iIndex];
    }

    public void Update()
    {
        if(m_pNumberSpriteRenderer[m_pNumberSpriteRenderer.Length - 1].color.a <= 0)
        {
            //m_pGameobjectManager.Remove("Hit Damege Font", gameObject);

            m_pGameobjectManager.Remove(_DeleteName, gameObject);

            for (int i = 0; i < m_pNumberSpriteRenderer.Length; ++i)
            {
                m_pNumberSpriteRenderer[i].color = new Color(m_pNumberSpriteRenderer[i].color.r, m_pNumberSpriteRenderer[i].color.g,
                    m_pNumberSpriteRenderer[i].color.b, 1.0f);
            }

            return;
        }

        for (int i = 0; i < m_pNumberSpriteRenderer.Length; ++i)
        {
            float _Alpha = m_pNumberSpriteRenderer[i].color.a - 0.5f * Time.deltaTime;

            m_pNumberSpriteRenderer[i].color = new Color(m_pNumberSpriteRenderer[i].color.r, m_pNumberSpriteRenderer[i].color.g,
                m_pNumberSpriteRenderer[i].color.b, _Alpha);
        }

        transform.position += Vector3.up * 1.0f * Time.deltaTime;
    }

    private void OnDestroy()
    {
        m_pGameobjectManager = null;
    }
}



//if (_iDamege / _SelectNumber< 1)
//            m_pNumberSpriteRenderer[0].gameObject.SetActive(false);
//        else
//        {
//            if(m_pNumberSpriteRenderer[0].gameObject.activeSelf == false)
//                m_pNumberSpriteRenderer[0].gameObject.SetActive(true);

//            int _iIndex = _iDamege / _SelectNumber;

//m_pNumberSpriteRenderer[0].sprite = _NumberSprite[_iIndex];
//        }

//        _SelectNumber /= 10; // 십의 단위 

//        if (_iDamege / _SelectNumber< 1)
//            m_pNumberSpriteRenderer[1].gameObject.SetActive(false);
//        else
//        {
//            if (m_pNumberSpriteRenderer[1].gameObject.activeSelf == false)
//                m_pNumberSpriteRenderer[1].gameObject.SetActive(true);

//            int _iIndex = _iDamege / _SelectNumber;

//m_pNumberSpriteRenderer[1].sprite = _NumberSprite[_iIndex];
//        }

//        _SelectNumber /= 10; // 십의 단위 

//        if (_iDamege / _SelectNumber< 1)
//            m_pNumberSpriteRenderer[2].gameObject.SetActive(false);
//        else
//        {
//            if (m_pNumberSpriteRenderer[2].gameObject.activeSelf == false)
//                m_pNumberSpriteRenderer[2].gameObject.SetActive(true);

//            int _iIndex = _iDamege / _SelectNumber;

//            if (_iIndex > _NumberSprite.Length)
//                _iIndex = _NumberSprite.Length;

//            m_pNumberSpriteRenderer[2].sprite = _NumberSprite[_iIndex];
//        }