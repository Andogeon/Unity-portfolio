using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BAR : MonoBehaviour
{
    private Image m_pHPImage = null;

    private Image m_pMPImage = null;

    private Image m_pExpImage = null;

    private Level m_pLevelUI = null;

    private Text m_pHPText = null;

    public float AccessHPBar
    {
        get 
        {
            if (null == m_pHPImage)
            {
                GameObject _Object = transform.GetChild(0).gameObject;

                m_pHPImage = _Object.transform.GetChild(0).GetComponent<Image>();
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

    public float AccessMPBar
    {
        get
        {
            if (null == m_pMPImage)
                m_pMPImage = transform.GetChild(1).GetComponent<Image>();

            return m_pMPImage.fillAmount;
        }

        set { m_pMPImage.fillAmount = value; }
    }

    public float AccessEXPBar
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

    public Level AccessLevelObject
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







//public class BAR : MonoBehaviour
//{
//    private Image m_pHPImage = null;

//    private Image m_pMPImage = null;

//    private Image m_pExpImage = null;

//    private Level m_pLevelUI = null;

//    private Text m_pHPText = null;

//    private Text m_pExpText = null;

//    public float AccessHPBar
//    {
//        get
//        {
//            if (null == m_pHPImage)
//                m_pHPImage = transform.GetChild(0).GetComponent<Image>();

//            return m_pHPImage.fillAmount;
//        }

//        set
//        {
//            if (null == m_pHPImage)
//                m_pHPImage = transform.GetChild(0).GetComponent<Image>();

//            m_pHPImage.fillAmount = value;
//        }
//    }

//    public float AccessMPBar
//    {
//        get
//        {
//            if (null == m_pMPImage)
//                m_pMPImage = transform.GetChild(1).GetComponent<Image>();

//            return m_pMPImage.fillAmount;
//        }

//        set { m_pMPImage.fillAmount = value; }
//    }

//    public float AccessEXPBar
//    {
//        get
//        {
//            if (null == m_pExpImage)
//                m_pExpImage = transform.GetChild(2).GetComponent<Image>();

//            return m_pExpImage.fillAmount;
//        }

//        set
//        {
//            if (null == m_pExpImage)
//                m_pExpImage = transform.GetChild(2).GetComponent<Image>();

//            m_pExpImage.fillAmount = value;
//        }
//    }

//    public Level AccessLevelObject
//    {
//        get
//        {
//            if (null == m_pLevelUI)
//                m_pLevelUI = transform.GetChild(3).GetComponent<Level>();

//            return m_pLevelUI;
//        }

//        set { m_pLevelUI = value; }
//    }

//    public Text AccessHpText
//    {
//        get
//        {
//            if (null == m_pHPText)
//                m_pHPText = transform.GetChild(4).GetComponent<Text>();

//            return m_pHPText;
//        }

//        set { m_pHPText = value; }
//    }

//    public Text AccessExpText
//    {
//        get
//        {
//            if (null == m_pExpText)
//                m_pExpText = transform.GetChild(5).GetComponent<Text>();

//            return m_pExpText;
//        }

//        set { m_pExpText = value; }
//    }
//}