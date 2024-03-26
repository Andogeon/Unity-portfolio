using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CResultPoint : MonoBehaviour
{
    [SerializeField] private Sprite[] _NumberSprite = null;

    private RectTransform[] m_pPointTrasnform = null;

    private RectTransform m_pPointParent = null;

    private Image[] m_pNumberImages = null;

    private int m_iScore = 0;

    private void Awake()
    {
        m_pPointTrasnform = new RectTransform[9];

        m_pNumberImages = new Image[9];

        Transform _pResultPoints = transform.GetChild(0);

        for (int i = _pResultPoints.childCount - 1; i >= 0; --i)
        {
            Transform _NumberTransform = _pResultPoints.GetChild(i);

            m_pPointTrasnform[i] = _NumberTransform.GetComponent<RectTransform>();

            m_pNumberImages[i] = _NumberTransform.GetComponent<Image>();
        }

        m_pPointParent = (RectTransform)_pResultPoints;

        SetEatScoreNumber(m_iScore);
    }

    public void SetEatScoreNumber(int _Number)
    {
        SetScoreNumber(_Number);

        SetNumberPosition();

        SetScorePosition();
    }

    public void SetScoreNumber(int _Score)
    {
        int _ScoreUnit = 100000000;

        int _SpriteIndex = 0;

        bool _MaxMoneyUnit = false;

        while (_ScoreUnit >= 1)
        {
            int _Index = _Score / _ScoreUnit;

            if (!_MaxMoneyUnit)
                _MaxMoneyUnit = _Index >= 1 ? true : false;

            if (!_MaxMoneyUnit && _Index <= 0)
                m_pNumberImages[_SpriteIndex].gameObject.SetActive(false);
            else
                m_pNumberImages[_SpriteIndex].gameObject.SetActive(true);

            if (_ScoreUnit == 1)
                m_pNumberImages[_SpriteIndex].gameObject.SetActive(true);

            m_pNumberImages[_SpriteIndex].sprite = _NumberSprite[_Index];

            m_pNumberImages[_SpriteIndex++].SetNativeSize();

            _Score -= _Index * _ScoreUnit;

            _ScoreUnit /= 10;
        }
    }

    //----------------------------------
    // 숫자와 컴마 위치 조정 함수
    //----------------------------------
    private void SetNumberPosition()
    {
        for(int i = m_pPointTrasnform.Length - 1; i >= 0; --i)
        {
            // 내 숫자를 기준으로 앞 숫자가 없거나 비활성화상태라면 반복문을 멈춤
            if (i - 1 < 0 || !m_pPointTrasnform[i - 1].gameObject.activeSelf)
                break;

            float _fWidth = GetSpriteHalfWidth(m_pPointTrasnform[i]) * 2.6f;
            float _fOffest = 0.0f;

            if (m_pPointTrasnform[i - 1].childCount != 0)
            {
                RectTransform _pCommaTransform = (RectTransform)m_pPointTrasnform[i - 1].GetChild(0);
                float _fXPosition = GetSpriteHalfWidth(m_pPointTrasnform[i - 1]) + 11.0f;
                _pCommaTransform.anchoredPosition = new Vector2(_fXPosition, _pCommaTransform.anchoredPosition.y);
                _fOffest = 11.0f;
            }
            
            SetNumberPosition(m_pPointTrasnform[i - 1], m_pPointTrasnform[i], _fWidth, _fOffest);
        }
    }

    public void ResetPoint()
    {
        m_iScore = 0;

        SetEatScoreNumber(0);
    }

    private void SetScorePosition()
    {
        RectTransform _pHeadTransform = null;

        for (int i = m_pPointTrasnform.Length - 1; i >= 0; --i)
        {
            if (i - 1 < 0 || !m_pPointTrasnform[i - 1].gameObject.activeSelf || m_pPointTrasnform[i - 1] == null)
                break;

            _pHeadTransform = m_pPointTrasnform[i - 1];
        }

        Vector2 _vDirection = Vector2.zero;

        if (_pHeadTransform != null)
            _vDirection = _pHeadTransform.transform.localPosition - m_pPointTrasnform[m_pPointTrasnform.Length - 1].localPosition;

        float _fLength = _vDirection.magnitude;

        m_pPointParent.anchoredPosition = new Vector2((_fLength / 2) + 7.0f, m_pPointParent.anchoredPosition.y);
    }

    //-----------------------------------
    // 앞 번호를 뒤 번호에 붙히기 위한 함수
    //-----------------------------------
    private void SetNumberPosition(RectTransform _plhsRectTranform, RectTransform _prhsRectTranform, float _fHalfWidth, float _fOffset)
    {
        if (_plhsRectTranform == null || _plhsRectTranform == null)
            return;

        _plhsRectTranform.anchoredPosition = new Vector2(_prhsRectTranform.anchoredPosition.x - _fHalfWidth - _fOffset, _prhsRectTranform.anchoredPosition.y);
    }

    private float GetSpriteHalfWidth(RectTransform _RectTransform)
    {
        return _RectTransform.rect.width / 2;
    }   
}
