using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum HPMODE // 플레이어의 체력 및 최대 체력을 구분하기 위한 열거형입니다.
{
    HP_NORMAL,
    HP_MAX
};

// Exp 클래스와 동일한 함수의 기능을 가지고 있는 HP 클래스입니다.

public class HP : MonoBehaviour // HP 클래스입니다.
{
    [SerializeField] private Player _Player = null; // 플레이어의 참조변수 

    [SerializeField] private Sprite[] _NumberImage = null; // 숫자들의 스프라이트 배열 변수 

    [SerializeField] private HPMODE _HPMode = HPMODE.HP_NORMAL; // 플레이어의 체력과 최대 체력을 구분 짓기 위한 열거형 변수입니다.

    private Image[] m_pImages = null;

    private int m_iExpCount = 0;

    // Start is called before the first frame update
    void Start()
    {
        m_pImages = GetComponentsInChildren<Image>();

        m_iExpCount = transform.childCount;
    }

    // Update is called once per frame
    void Update()
    {
        if(_HPMode == HPMODE.HP_NORMAL) // 플레이어의 체력이라면 
            OnExpSetSprite(_Player.AccessPlayerHP, m_pImages);
        else // 플레이어의 최대 체력일 경우 
            OnExpSetSprite(_Player.AccessPlayerMaxHP, m_pImages);
    }


    // 함수의 기능은 Exp 클래스의 있는 OnExpSetSprite 함수와 같습니다.
    public void OnExpSetSprite(float _Exp, Image[] _ExpImages)
    {
        if (null == _ExpImages || _Exp < 0.0f)
            return;

        float _Number = 100.0f;

        bool _MaxNumberSwitch = false;

        if (_Exp / _Number < 1) 
            _ExpImages[m_iExpCount - 3].gameObject.SetActive(false);
        else
        {
            if (_ExpImages[m_iExpCount - 3].gameObject.activeSelf == false)
                _ExpImages[m_iExpCount - 3].gameObject.SetActive(true);

            float fIndex = _Exp / _Number;

            if (fIndex > 9)
                fIndex = 9;

            _ExpImages[m_iExpCount - 3].sprite = _NumberImage[(int)fIndex];

            _MaxNumberSwitch = true;

            _Exp -= (int)fIndex * _Number;
        }

        //10 

        _Number = 10.0f;

        if (_MaxNumberSwitch == true)
        {
            if (_ExpImages[m_iExpCount - 2].gameObject.activeSelf == false)
                _ExpImages[m_iExpCount - 2].gameObject.SetActive(true); // 끈다

            float fIndex = _Exp / _Number;

            if (fIndex > 9)
                fIndex = 9;

            _ExpImages[m_iExpCount - 2].sprite = _NumberImage[(int)fIndex];

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

                _ExpImages[m_iExpCount - 2].sprite = _NumberImage[(int)fIndex];

                _MaxNumberSwitch = true;

                _Exp -= (int)fIndex * _Number;
            }
        }

        // 1

        _Number = 1;

        float fFinalIndex = _Exp / _Number;

        if (fFinalIndex > 9)
            fFinalIndex = 9;

        _ExpImages[m_iExpCount - 1].sprite = _NumberImage[(int)fFinalIndex];
    }
}
