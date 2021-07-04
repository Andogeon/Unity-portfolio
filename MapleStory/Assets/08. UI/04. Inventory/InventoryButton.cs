using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryButton : MonoBehaviour
{
    //[SerializeField] private InventoryButton _EnableInventory = null;

    [SerializeField] private InventoryButton[] _DisabledInventorys = null;

    [SerializeField] private Sprite _EnableSprite = null;

    [SerializeField] private Sprite _DisabledSprite = null;

    [SerializeField] private Inventory _Inventory = null;

    private Image m_pImage = null;

    // Start is called before the first frame update
    void Start()
    {
        m_pImage = GetComponent<Image>();
    }

    public void OnInventoryButton()
    {
        EnableImage();

        _Inventory.EnableInventory();

        for (int i = 0; i < _DisabledInventorys.Length; ++i)
        {
            _DisabledInventorys[i].DisabledImage();

            _DisabledInventorys[i]._Inventory.DisabledInventory();
        }
    }

    public void EnableImage()
    {
        m_pImage.sprite = _EnableSprite;
    }

    public void DisabledImage()
    {
        m_pImage.sprite = _DisabledSprite;
    }
}
