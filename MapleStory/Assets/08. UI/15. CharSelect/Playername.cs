using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 플레이어 하단 닉네임을 표현하기 위한 클래스입니다.

public class Playername : MonoBehaviour
{
    [SerializeField] private GameObject _FollowObject = null; // 따라갈 오브젝트 

    [SerializeField] private Vector3 _OffsetPosition = Vector3.zero; // 위치 간격을 조정하기 위한 변수 

    private RectTransform m_pRectTransform = null; // UI 위치변환 변수 

    private Text m_pText = null; // 실제 출력될 텍스트 변수 


    // 외부로 부터 텍스트를 전달받기 위한 프로퍼티입니다.
    public Text AccessText
    {
        get { return m_pText; }

        set { m_pText = value; }
    }

    private void Awake()
    {
        m_pRectTransform = GetComponent<RectTransform>();

        m_pText = GetComponentInChildren<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        m_pRectTransform.anchoredPosition = _FollowObject.transform.position + _OffsetPosition; // 따라갈 오브젝트 + 간격으로 

        // UI 이름의 위치를 따라가게 했습니다.
    }
}
