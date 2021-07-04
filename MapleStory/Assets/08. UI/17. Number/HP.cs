using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

enum HPMODE
{
    HP_NORMAL,
    HP_MAX
};

public class HP : MonoBehaviour
{
    [SerializeField] private Player _Player = null;

    [SerializeField] private Sprite[] _NumberImage = null;

    [SerializeField] private HPMODE _HPMode = HPMODE.HP_NORMAL;

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
        if(_HPMode == HPMODE.HP_NORMAL)
            OnExpSetSprite(_Player.AccessPlayerHP, m_pImages);
        else
            OnExpSetSprite(_Player.AccessPlayerMaxHP, m_pImages);
    }

    public void OnExpSetSprite(float _Exp, Image[] _ExpImages)
    {
        if (null == _ExpImages || _Exp < 0.0f)
            return;

        float _Number = 100.0f;

        bool _MaxNumberSwitch = false;

        if (_Exp / _Number < 1) // 몫이 없는 경우 최고자리여부
            _ExpImages[m_iExpCount - 3].gameObject.SetActive(false); // 끈다
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
