using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 인게임에서 마우스 포인터가 아닌 스프라이트로 사용하고자 했으나 유니티 프로젝트 셋팅 -> 플레이어 -> 디폴트 커서에서 

// 마우스 이미지를 결정 지을수 있다는 것 찾아 해당 클래스를 현재 사용을 하지 않습니다.

public class Mouse : MonoBehaviour
{
    private RectTransform m_pRectTransform = null;

    // Start is called before the first frame update
    void Start()
    {
        m_pRectTransform = GetComponent<RectTransform>();

        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        m_pRectTransform.anchoredPosition = Camera.main.ViewportToWorldPoint(Input.mousePosition);
    }

    private void OnDestroy()
    {
        m_pRectTransform = null;
    }
}
