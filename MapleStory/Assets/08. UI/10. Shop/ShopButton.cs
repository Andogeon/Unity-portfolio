using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour // 플레이어가 판매할 장비창, 소비창, 기타창의 버튼 관련 클래스입니다.
{
    [SerializeField] private Sprite _EnableSprite = null; // 활성화 시 사용할 스프라이트

    [SerializeField] private Sprite _DisabledSprite = null; // 비활성화 시 사용할 스프라이트

    [SerializeField] private ShopButton[] _DisabledShops = null; // 비활성화할 버튼

    [SerializeField] private GameObject _EnableListSlots = null; // 활성화 할 버튼 

    [SerializeField] private GameObject[] _DisableListSlots = null; // 비활성화 할 판매 슬롯오브젝트

    [SerializeField] private bool m_bIsEnableImage = false; // 활성화 여부

    private Image m_pImage = null; // 버튼의 활성화 및 비활성화 스프라이트를 넣은 이미지 참조 변수 


    // 외부로 부터 버튼의 활성화 여부를 알려주는 프로퍼티입니다.
    public bool AccessEnableImage
    {
        get { return m_bIsEnableImage; }

        set { m_bIsEnableImage = value; }
    }

    private void Start()
    {
        m_pImage = GetComponent<Image>();
    }
    

    // 상점에서 판매자의 인벤토리 버튼 클릭시 이미지 활성화 및 판매슬롯 활성화 그리고 나머지는 비활성화 될 이미지와 판매 

    // 슬롯을 비활성화하는 함수입니다.
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

    public void EnableImage() // 버튼 이미지 활성화하는 함수입니다.
    {
        m_pImage.sprite = _EnableSprite;
        
        m_bIsEnableImage = true;
    }

    public void DisabledImage() // 버튼 이미지를 비활성화하는 함수입니다.
    {
        m_pImage.sprite = _DisabledSprite;

        m_bIsEnableImage = false;
    }
}
