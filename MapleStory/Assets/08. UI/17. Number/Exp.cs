using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 경험치를 표현하기 위해서 유니티에서 지원하는 텍스트 컴포넌트가 아닌 스프라이트로 숫자를 표현하기 위해 

// 해당 클래스를 제작했습니다. 

public class Exp : MonoBehaviour // 경험치 스프라이트 텍스트 클래스입니다.
{
    [SerializeField] private Player _PlayerObject = null; // 플레이어의 경험치를 얻기 위한 플레이어 참조 변수 

    [SerializeField] private Sprite[] _NumberSprite = null; // 숫자들의 스프라이트를 모은 배열 변수 

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

    // 인자로 플레이어의 경험치를 통해서 숫자 10000자리부터 1에 자리까지 게임오브젝트를 활성화 및 비활성화 후 

    // 인덱스를 구해 실제 게임에서 적용될 스프라이트를 구하는 함수입니다.

    public void OnExpSetSprite(float _Exp, Image[] _ExpImages)
    {
        if (null == _ExpImages) // 숫자 이미지가 존재하지 않는다면 
            return;

        float _Number = 10000.0f; // 숫자 10000부터 나누기 시작 

        bool _MaxNumberSwitch = false; // 숫자 첫번째 자리여부에 대한 변수 

        if (_Exp / _Number < 1) // 10000자리 나누었는데 정수값이 아닌 소수점 자리로 경험치에 10000자리 숫자가 존재 여부 확인
            _ExpImages[m_iExpCount].gameObject.SetActive(false); // 소수점이 나왔다면 없는 것이므로 10000자리의 숫자 오브젝트를 비활성화
        else // 정수 값이 나왔다면 10000자리에 해당하는 숫자가 존재하니 
        {
            if (_ExpImages[m_iExpCount].gameObject.activeSelf == false) // 비활성화가 되어있다면 
                _ExpImages[m_iExpCount].gameObject.SetActive(true); // 활성화 한다.

            float fIndex = _Exp / _Number; // 인덱스 값을 구하고 

            if (fIndex > 9) // 혹시 인덱스가 9보다 초과한다면
                fIndex = 9; //9로 고정

            _ExpImages[m_iExpCount].sprite = _NumberSprite[(int)fIndex]; // 실제 인덱스를 적용

            _MaxNumberSwitch = true; // 숫자 마지막 자리 여부를 활성화한다

            _Exp -= (int)fIndex * _Number; // 경험치량에서 빼서 나머지 자리 숫자를 계산한다!
        }

        // 1000

        _Number = 1000.0f;

        if (_MaxNumberSwitch == true) // 숫자 첫번째 자리여부가 존재한다면 
        {
            float fIndex = _Exp / _Number; // 바로 인덱스 계산

            if (fIndex > 9)
                fIndex = 9;

            _ExpImages[m_iExpCount - 1].sprite = _NumberSprite[(int)fIndex]; // 적용 후 

            _Exp -= (int)fIndex * _Number; // 경험치에서 뺀다
        }
        else // 숫자 10000자리와 비슷한 코드입니다.
        {
            if (_Exp / _Number < 1)
                _ExpImages[m_iExpCount - 1].gameObject.SetActive(false);
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

        // 숫자 1자리까지 모두 코드의 기능은 비슷합니다.

        _Number = 1;

        float fFinalIndex = _Exp / _Number;

        if (fFinalIndex > 9)
            fFinalIndex = 9;

        _ExpImages[m_iExpCount - 4].sprite = _NumberSprite[(int)fFinalIndex]; // 최종적 숫자 1자리 숫자 스프라이트의 전달
    }


    // 플레이어가 얻은 경험치의 퍼센트로 표현하기 위해 호출 되는 함수입니다.
    public void OnPercentSetSprite(float _Percent, Image[] _PercentImages)
    {
        if (null == _PercentImages) // 숫자 이미지가 존재하지 않는다면 
            return; // 종료 

        float _Number = 10.0f;

        if(_Percent / _Number < 1) // 10에 자리의 표현 
            _PercentImages[m_iPercent - 4].gameObject.SetActive(false);// 10의 자리 숫자 오브젝트를 끈다
        else // 그러지 않는다면 
        {
            if (_PercentImages[m_iPercent - 4].gameObject.activeSelf == false)
                _PercentImages[m_iPercent - 4].gameObject.SetActive(true); // 활성화 한다 

            float fIndex = _Percent / _Number; // 인덱스 연산 

            if (fIndex > 9)
                fIndex = 9;

            _PercentImages[m_iPercent - 4].sprite = _NumberSprite[(int)fIndex]; // 스프라이트 적용 

            _Percent -= (int)fIndex * _Number; // 퍼센트에서 빼서 다시 연산 
        }

        _Number = 1;

        float fFinalIndex = _Percent / _Number; // 1의 자리 연산 

        if (fFinalIndex > 9)
            fFinalIndex = 9;

        _PercentImages[m_iPercent - 3].sprite = _NumberSprite[(int)fFinalIndex]; // 자리수 적용

        _Percent -= (int)fFinalIndex * _Number;


        // 소수점 자리를 구하는 코드입니다.

        float _fNumber = _Percent * 10.0f; // x.0 자리 구하는 연산 

        int _Index = (int)_fNumber;

        if (_Index >= 9)
            _Index = 9;

        _PercentImages[m_iPercent - 2].sprite = _NumberSprite[_Index]; // 스프라이트 적용

        _fNumber = _Index / 10.0f;

        _Percent -= _fNumber;


        _fNumber = _Percent * 100.0f; // x.x0자리 구하는 연산 

        _Index = (int)_fNumber;

        if (_Index >= 9)
            _Index = 9;

        _PercentImages[m_iPercent - 1].sprite = _NumberSprite[_Index]; // 스프라이트 적용 
    }
}
