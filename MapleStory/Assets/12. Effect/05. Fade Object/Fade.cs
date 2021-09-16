using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FADETYPE
{
    FADE_IN,
    FADE_OUT
}

// 씬전환시 사용되는 페이드인 페이드 아웃 클래스입니다.

public class Fade : MonoBehaviour // 페이드 인 페이드 아웃 클래스입니다.
{
    [SerializeField] private float _InSpeed = 0.0f; // 페이드 인 속도

    [SerializeField] private float _OutSpeed = 0.0f; // 페이드 아웃 속도 

    private Image m_pImage = null; // 페이드 인 아웃을 할 UI 이미지 

    private FADETYPE m_eFadeTpye = FADETYPE.FADE_OUT; // 페이드 인 아웃을 결정할 열거형 변수

    private bool m_bIsOnFade = false; // 현재 페이드 인인지 결정하는 변수 

    private float m_fAlpha = 1.0f; 

    public FADETYPE AccessFaceType
    {
        get { return m_eFadeTpye; }

        set { m_eFadeTpye = value; }
    }

    public bool AccessOnFade
    {
        get { return m_bIsOnFade; }
    }

    public float AccessAlpha
    {
        get { return m_fAlpha; }
    }

    // Start is called before the first frame update
    void Start()
    {
        m_pImage = GetComponent<Image>();

        m_bIsOnFade = false;

        m_pImage.color = new Color(m_pImage.color.r, m_pImage.color.g, m_pImage.color.b, m_fAlpha);
    }

    // Update is called once per frame
    void Update()
    {
        FaceMode();
    }

    // 알파값을 수시로 빼주거나 더하여 페이드 인 혹은 아웃을 하기 위한 함수입니다.
    public void FaceMode()
    {
        if (m_eFadeTpye == FADETYPE.FADE_OUT && m_pImage.color.a <= 0.0f)
            return; // 페이드 아웃중인데 알파값이 모두 빠졋다면 종료 

        Color _SpriteColor = m_pImage.color; // 이미지의 색상 정보를 받아옴 

        switch (m_eFadeTpye)
        {
            case FADETYPE.FADE_IN: // 페이드 인이라면 
                if (m_fAlpha < 1.0f)
                {
                    m_fAlpha = _SpriteColor.a + _InSpeed * Time.deltaTime; // 현재 알파에서 더하여

                    m_pImage.color = new Color(_SpriteColor.r, _SpriteColor.g, _SpriteColor.b, m_fAlpha); // 실제 적용
                }
                else
                    m_bIsOnFade = true; // 페이드 인 완료
                break;
            case FADETYPE.FADE_OUT: // 페이드 아웃이라면 
                if (m_fAlpha > 0.0f)
                {
                    m_fAlpha = _SpriteColor.a - _OutSpeed * Time.deltaTime; // 알파값에서 빼고 

                    m_pImage.color = new Color(_SpriteColor.r, _SpriteColor.g, _SpriteColor.b, m_fAlpha); // 실제 적용
                }
                break;
        }
    }
}
