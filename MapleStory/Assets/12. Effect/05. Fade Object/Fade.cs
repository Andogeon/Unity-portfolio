using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum FADETYPE
{
    FADE_IN,
    FADE_OUT
}

public class Fade : MonoBehaviour
{
    [SerializeField] private float _InSpeed = 0.0f;

    [SerializeField] private float _OutSpeed = 0.0f;

    private Image m_pImage = null;

    private FADETYPE m_eFadeTpye = FADETYPE.FADE_OUT;

    private bool m_bIsOnFade = false;

    private float m_fAlpha = 1.0f;

    //private Player m_pPlayer = null;

    //private FollowCamera m_pCamera = null;

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

        //m_pPlayer = GameObject.Find("Player").GetComponent<Player>();

        //m_pCamera = GameObject.Find("Main Camera").GetComponent<FollowCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        FaceMode();
    }

    public void FaceMode()
    {
        if (m_eFadeTpye == FADETYPE.FADE_OUT && m_pImage.color.a <= 0.0f)
            return;

        Color _SpriteColor = m_pImage.color;

        //float Length = (m_pPlayer.transform.position - m_pCamera.transform.position).magnitude;

        switch (m_eFadeTpye)
        {
            case FADETYPE.FADE_IN:
                if (m_fAlpha < 1.0f)
                {
                    m_fAlpha = _SpriteColor.a + _InSpeed * Time.deltaTime;

                    m_pImage.color = new Color(_SpriteColor.r, _SpriteColor.g, _SpriteColor.b, m_fAlpha);
                }
                else
                    m_bIsOnFade = true;
                break;
            case FADETYPE.FADE_OUT:
                if (m_fAlpha > 0.0f)
                {
                    //m_fAlpha = _SpriteColor.a - _OutSpeed * Time.deltaTime;

                    m_fAlpha = _SpriteColor.a - _OutSpeed * Time.deltaTime;

                    m_pImage.color = new Color(_SpriteColor.r, _SpriteColor.g, _SpriteColor.b, m_fAlpha);
                }
                break;
        }
    }
}
