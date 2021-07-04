using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyUIBar : MonoBehaviour
{
    [SerializeField] private GameObject UIBar = null;

    private Image m_pHpImage = null;

    private Image m_pNameImage = null;

    private Image m_pFaceImage = null;

    public Image GetHpImage
    {
        get 
        {
            if (null == m_pHpImage)
            {
                Image[] _GetImages = UIBar.GetComponentsInChildren<Image>();

                m_pHpImage = _GetImages[1];
            }

            return m_pHpImage; 
        }
    }

    public Image GetNameImage
    {
        get
        {
            if (null == m_pNameImage)
            {
                Image[] _GetImages = UIBar.GetComponentsInChildren<Image>();

                m_pNameImage = _GetImages[3];
            }

            return m_pNameImage;
        }
    }

    public Image GetFaceImage
    {
        get
        {
            if (null == m_pFaceImage)
            {
                Image[] _GetImages = UIBar.GetComponentsInChildren<Image>();

                m_pFaceImage = _GetImages[4];
            }

            return m_pFaceImage;
        }
    }

    public void OnEnemyBar()
    {
        UIBar.SetActive(true);
    }

    public void OffEnemyBar()
    {
        UIBar.SetActive(false);
    }
}
