using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BAR : MonoBehaviour // 체력바, MP바 , 경험치 바를 구현하기 위한 클래스입니다.
{
    private Image m_pHPImage = null; // HP bar UI 

    private Image m_pMPImage = null; // MP bar UI 

    private Image m_pExpImage = null; // 경험치 bar UI 

    private Level m_pLevelUI = null; // 레벨

    private Text m_pHPText = null;

    public float AccessHPBar // 실제 플레이어에서 HP를 갱신 후 체력바에 게이지를 적용하는 프로퍼티입니다.
    {
        get 
        {
            if (null == m_pHPImage) // 해당 UI 이미지 정보가 없다면 
            {
                GameObject _Object = transform.GetChild(0).gameObject; // 오브젝트를 검색 

                m_pHPImage = _Object.transform.GetChild(0).GetComponent<Image>(); // 이미지 정보를 탐색 후 대입
            }

            return m_pHPImage.fillAmount;
        }

        set 
        {
            if (null == m_pHPImage)
            {
                GameObject _Object = transform.GetChild(0).gameObject;

                m_pHPImage = _Object.transform.GetChild(0).GetComponent<Image>();
            }

            m_pHPImage.fillAmount = value; 
        }
    }


    public float AccessMPBar // 사용되지 않는 프로퍼티입니다
    {
        get
        {
            if (null == m_pMPImage)
                m_pMPImage = transform.GetChild(1).GetComponent<Image>();

            return m_pMPImage.fillAmount;
        }

        set { m_pMPImage.fillAmount = value; }
    }

    public float AccessEXPBar // 실제 플레이어에서 경험치를 갱신 후 경험치바에 게이지를 적용하는 프로퍼티입니다.
    {
        get
        {
            if (null == m_pExpImage)
            {
                GameObject _Object = transform.GetChild(2).gameObject;

                m_pExpImage = _Object.transform.GetChild(0).GetComponent<Image>();
            }

            return m_pExpImage.fillAmount;
        }

        set 
        {
            if (null == m_pExpImage)
            {
                GameObject _Object = transform.GetChild(2).gameObject;

                m_pExpImage = _Object.transform.GetChild(0).GetComponent<Image>();
            }

            m_pExpImage.fillAmount = value; 
        }
    }

    public Level AccessLevelObject // 레벨 오브젝트 외부로 넘겨주고 데이터를 수정하기 위한 프로퍼티입니다.
    {
        get 
        {
            if (null == m_pLevelUI)
                m_pLevelUI = transform.GetChild(3).GetComponent<Level>();

            return m_pLevelUI;
        }

        set { m_pLevelUI = value; }
    }

    public Text AccessHpText
    {
        get 
        {
            if (null == m_pHPText)
                m_pHPText = transform.GetChild(4).GetComponent<Text>();

            return m_pHPText;
        }

        set { m_pHPText = value; }
    }
}