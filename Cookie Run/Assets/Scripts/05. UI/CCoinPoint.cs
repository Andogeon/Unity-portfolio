using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//------------------------------
// 코인 점수판 클래스
//------------------------------
public class CCoinPoint : MonoBehaviour
{
    [SerializeField] private Sprite[] m_NumberSprites = null;

    [SerializeField] private GameObject m_CombaGameObject = null;

    [SerializeField] private RectTransform m_JellyTranform = null;

    private RectTransform[] m_pPointTrasnform = null;

    private Image[] m_pNumberImages = null;

    private List<GameObject> m_pCombaGameObject = new List<GameObject>();

    private int m_iScore = 0;

    private void Awake()
    {
        m_pPointTrasnform = new RectTransform[9];

        m_pNumberImages = new Image[9];

        for (int i = transform.childCount - 1; i >= 0; --i)
        {
            Transform _NumberTransform = transform.GetChild(i);

            m_pPointTrasnform[i] = _NumberTransform.GetComponent<RectTransform>();

            m_pNumberImages[i] = _NumberTransform.GetComponent<Image>();
        }

        SetScoreNumber(m_iScore);

        SetPointPosition();
    }

    public void SetEatScoreNumber(int _Number)
    {
        m_iScore += _Number;

        SetScoreNumber(m_iScore);

        SetPointPosition();
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

            m_pNumberImages[_SpriteIndex].sprite = m_NumberSprites[_Index];

            m_pNumberImages[_SpriteIndex++].SetNativeSize();

            _Score -= _Index * _ScoreUnit;

            _ScoreUnit /= 10;
        }
    }

    public void SetPointPosition()
    {
        SetJellyPosition();

        RectTransform _pHeadTranform = m_pPointTrasnform[m_pPointTrasnform.Length - 1];

        int _iCommaCount = GetCommaCount();

        for (int i = 0; i < _iCommaCount; ++i)
            CreateComma(i);

        bool _bIsMoveCommaPositon = false;

        int _iIndex = 0, _iMaxCount = 3;
        _iCommaCount = 1;

        for (int i = m_pPointTrasnform.Length - 1; i >= 0; --i)
        {
            if (i - 1 < 0 || m_iScore == 0 || !m_pPointTrasnform[i].gameObject.activeSelf)
                break;

            if (_iCommaCount >= _iMaxCount)
            {
                _bIsMoveCommaPositon = true;
                _iCommaCount = 1;
            }
            else
                ++_iCommaCount;

            float _fHalfWidth = GetSpriteHalfWidth(m_pPointTrasnform[i]) * 2;
            float _fOffset = 0.0f;

            if (_bIsMoveCommaPositon && m_pPointTrasnform[i - 1].gameObject.activeSelf)
            {
                RectTransform _pPrevTrasnform = m_pPointTrasnform[i - 1];
                RectTransform _pCommaTransform = (RectTransform)m_pCombaGameObject[_iIndex++].transform;

                _fOffset = (GetSpriteHalfWidth(_pCommaTransform) * 2);

                SetNumberPosition(m_pPointTrasnform[i - 1], m_pPointTrasnform[i], _fHalfWidth, _fOffset);

                float _fXCommaPosition = _pPrevTrasnform.anchoredPosition.x + _fOffset;
                _pCommaTransform.anchoredPosition = new Vector2(_fXCommaPosition, _pCommaTransform.anchoredPosition.y);

                _bIsMoveCommaPositon = false;
            }
            else
                SetNumberPosition(m_pPointTrasnform[i - 1], m_pPointTrasnform[i], _fHalfWidth, _fOffset);
        }
    }

    public void ResetPoint()
    {
        m_iScore = 0;

        SetEatScoreNumber(m_iScore);

        for (int i = 0; i < m_pCombaGameObject.Count; ++i)
            GameObject.Destroy(m_pCombaGameObject[i]);

        m_pCombaGameObject.Clear();
    }

    private int GetCommaCount()
    {
        int _iCount = 0, _iEnableCount = 0;

        for (int i = m_pPointTrasnform.Length - 1; i >= 0; --i)
        {
            if (m_pPointTrasnform[i].gameObject.activeSelf)
                ++_iEnableCount;

            if (_iEnableCount % 4 == 0)
            {
                ++_iCount;
                _iEnableCount = 1;
            }
        }

        return _iCount;
    }

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

    private void CreateComma(int _Index)
    {
        if (m_pCombaGameObject.Count == 0 || m_pCombaGameObject.Count <= _Index)
        {
            GameObject _CombaClone = GameObject.Instantiate(m_CombaGameObject, transform);
            _CombaClone.gameObject.name = "Number Comma";

            _CombaClone.transform.SetAsLastSibling();

            m_pCombaGameObject.Add(_CombaClone);
        }
        else
        {
            if (m_pCombaGameObject[_Index] != null)
                return;

            GameObject _CombaClone = GameObject.Instantiate(m_CombaGameObject, transform);
            _CombaClone.gameObject.name = "Number Comma";
            _CombaClone.transform.SetAsLastSibling();

            m_pCombaGameObject.Add(_CombaClone);
        }
    }

    private void SetJellyPosition()
    {
        float _fOffset = 0;

        // 각 숫자들을 하나씩 순회한다.
        for (int i = 0; i < m_pPointTrasnform.Length; ++i)
        {
            // 숫자가 활성화 되지 않았다면 즉 보이지 않는다면 건너뛴다.
            if (!m_pPointTrasnform[i].gameObject.activeSelf)
                continue;

            // 코인 오브젝트의 가로 길이를 X축 위치로 가져온다.
            float _fCoinXPosition = (GetSpriteHalfWidth(m_JellyTranform) * 2.0f) + 20.0f;

            float _fXPosition = _fCoinXPosition + _fOffset;
            _fOffset += 31.0f;

            // 각 숫자 번호의 위치를 지정한다.
            m_pPointTrasnform[i].anchoredPosition = new Vector2(_fXPosition, m_pPointTrasnform[i].anchoredPosition.y);
        }
    }
}