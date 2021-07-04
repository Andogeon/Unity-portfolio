using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour
{
    [SerializeField] private Sprite _EnableSprite = null;

    [SerializeField] private Sprite _DisabledSprite = null;

    [SerializeField] private ShopButton[] _DisabledShops = null;

    [SerializeField] private GameObject _EnableListSlots = null;

    [SerializeField] private GameObject[] _DisableListSlots = null;

    [SerializeField] private bool m_bIsEnableImage = false;

    private Image m_pImage = null;

    public bool AccessEnableImage
    {
        get { return m_bIsEnableImage; }

        set { m_bIsEnableImage = value; }
    }

    private void Start()
    {
        m_pImage = GetComponent<Image>();
    }

    public void OnShopButton()
    {
        EnableImage();

        _EnableListSlots.SetActive(true);

        for (int i = 0; i < _DisabledShops.Length; ++i)
        {
            _DisabledShops[i].DisabledImage();

            _DisableListSlots[i].SetActive(false);
        }
    }

    public void EnableImage()
    {
        m_pImage.sprite = _EnableSprite;
        
        m_bIsEnableImage = true;
    }

    public void DisabledImage()
    {
        m_pImage.sprite = _DisabledSprite;

        m_bIsEnableImage = false;
    }
}
