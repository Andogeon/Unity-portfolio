using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Exp : MonoBehaviour
{
    [SerializeField] private Player _PlayerObject = null;

    [SerializeField] private Sprite[] _NumberSprite = null;

    [SerializeField] private Image[] _ExpImage = null;

    [SerializeField] private Image[] _MaxExpImage = null;

    [SerializeField] private Image[] _PercentImage = null;

    private int m_iExpCount = 0;

    private int m_iMaxExpCount = 0;

    private int m_iPercent = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_iMaxExpCount = m_iExpCount = 4;

        m_iPercent = 4;
    }

    // Update is called once per frame
    void Update()
    {
        float _PlayerExp = _PlayerObject.AccessExp;

        OnExpSetSprite(_PlayerExp, _ExpImage);

        float _PlayerMaxExp = _PlayerObject.AccessMaxExp;

        OnExpSetSprite(_PlayerMaxExp, _MaxExpImage);

        float _Percent = (_PlayerExp / _PlayerMaxExp) * 100.0f;

        OnPercentSetSprite(_Percent, _PercentImage);
    }

    public void OnExpSetSprite(float _Exp, Image[] _ExpImages)
    {
        if (null == _ExpImages)
            return;

        float _Number = 10000.0f;

        bool _MaxNumberSwitch = false;

        if (_Exp / _Number < 1) // 몫이 없는 경우 최고자리여부
            _ExpImages[m_iExpCount].gameObject.SetActive(false); // 끈다
        else
        {
            if (_ExpImages[m_iExpCount].gameObject.activeSelf == false)
                _ExpImages[m_iExpCount].gameObject.SetActive(true);

            float fIndex = _Exp / _Number;

            if (fIndex > 9)
                fIndex = 9;

            _ExpImages[m_iExpCount].sprite = _NumberSprite[(int)fIndex];

            _MaxNumberSwitch = true;

            _Exp -= (int)fIndex * _Number;
        }

        // 1000

        _Number = 1000.0f;

        if (_MaxNumberSwitch == true)
        {
            float fIndex = _Exp / _Number;

            if (fIndex > 9)
                fIndex = 9;

            _ExpImages[m_iExpCount - 1].sprite = _NumberSprite[(int)fIndex];

            _Exp -= (int)fIndex * _Number;
        }
        else
        {
            if (_Exp / _Number < 1) // 몫이 없는 경우 최고자리여부
                _ExpImages[m_iExpCount - 1].gameObject.SetActive(false); // 끈다
            else
            {
                if (_ExpImages[m_iExpCount - 1].gameObject.activeSelf == false)
                    _ExpImages[m_iExpCount - 1].gameObject.SetActive(true);

                float fIndex = _Exp / _Number;

                if (fIndex > 9)
                    fIndex = 9;

                _ExpImages[m_iExpCount - 1].sprite = _NumberSprite[(int)fIndex];

                _MaxNumberSwitch = true;

                _Exp -= (int)fIndex * _Number;
            }
        }

        // 100

        _Number = 100.0f;

        if (_MaxNumberSwitch == true)
        {
            float fIndex = _Exp / _Number;

            if (fIndex > 9)
                fIndex = 9;

            _ExpImages[m_iExpCount - 2].sprite = _NumberSprite[(int)fIndex];

            _Exp -= (int)fIndex * _Number;
        }
        else
        {
            if (_Exp / _Number < 1) // 몫이 없는 경우 최고자리여부
                _ExpImages[m_iExpCount - 2].gameObject.SetActive(false); // 끈다
            else
            {
                if (_ExpImages[m_iExpCount - 2].gameObject.activeSelf == false)
                    _ExpImages[m_iExpCount - 2].gameObject.SetActive(true);

                float fIndex = _Exp / _Number;

                if (fIndex > 9)
                    fIndex = 9;

                _ExpImages[m_iExpCount - 2].sprite = _NumberSprite[(int)fIndex];

                _MaxNumberSwitch = true;

                _Exp -= (int)fIndex * _Number;
            }
        }

        //10 

        _Number = 10.0f;

        if (_MaxNumberSwitch == true)
        {
            float fIndex = _Exp / _Number;

            if (fIndex > 9)
                fIndex = 9;

            _ExpImages[m_iExpCount - 3].sprite = _NumberSprite[(int)fIndex];

            _Exp -= (int)fIndex * _Number;
        }
        else
        {
            if (_Exp / _Number < 1) // 몫이 없는 경우 최고자리여부
                _ExpImages[m_iExpCount - 3].gameObject.SetActive(false); // 끈다
            else
            {
                if (_ExpImages[m_iExpCount - 3].gameObject.activeSelf == false)
                    _ExpImages[m_iExpCount - 3].gameObject.SetActive(true);

                float fIndex = _Exp / _Number;

                if (fIndex > 9)
                    fIndex = 9;

                _ExpImages[m_iExpCount - 3].sprite = _NumberSprite[(int)fIndex];

                _MaxNumberSwitch = true;

                _Exp -= (int)fIndex * _Number;
            }
        }

        // 1

        _Number = 1;

        float fFinalIndex = _Exp / _Number;

        if (fFinalIndex > 9)
            fFinalIndex = 9;

        _ExpImages[m_iExpCount - 4].sprite = _NumberSprite[(int)fFinalIndex];
    }

    public void OnPercentSetSprite(float _Percent, Image[] _PercentImages)
    {
        if (null == _PercentImages)
            return;

        float _Number = 10.0f;

        //bool _MaxNumberSwitch = false;

        if(_Percent / _Number < 1)
            _PercentImages[m_iPercent - 4].gameObject.SetActive(false); // 끈다
        else
        {
            if (_PercentImages[m_iPercent - 4].gameObject.activeSelf == false)
                _PercentImages[m_iPercent - 4].gameObject.SetActive(true);

            float fIndex = _Percent / _Number;

            if (fIndex > 9)
                fIndex = 9;

            _PercentImages[m_iPercent - 4].sprite = _NumberSprite[(int)fIndex];

            _Percent -= (int)fIndex * _Number;

            //_MaxNumberSwitch = true;
        }

        //// 1 레벨업 할 때 문제다 이거지 ?

        _Number = 1;

        float fFinalIndex = _Percent / _Number;

        if (fFinalIndex > 9)
            fFinalIndex = 9;

        _PercentImages[m_iPercent - 3].sprite = _NumberSprite[(int)fFinalIndex];

        _Percent -= (int)fFinalIndex * _Number;


        float _fNumber = _Percent * 10.0f;

        int _Index = (int)_fNumber;

        if (_Index >= 9)
            _Index = 9;

        _PercentImages[m_iPercent - 2].sprite = _NumberSprite[_Index];

        _fNumber = _Index / 10.0f;

        _Percent -= _fNumber;



        _fNumber = _Percent * 100.0f;

        _Index = (int)_fNumber;

        if (_Index >= 9)
            _Index = 9;

        _PercentImages[m_iPercent - 1].sprite = _NumberSprite[_Index];
    }



    public void OnDestroy()
    {
    }
}
