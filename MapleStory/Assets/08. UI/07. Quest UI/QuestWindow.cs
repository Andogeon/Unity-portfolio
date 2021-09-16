using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 퀘스트 윈도우 창도 인벤토리 창처럼 창을 이동시키기 위해 만든 클래스입니다.

public class QuestWindow : MonoBehaviour // 퀘스트 윈도우 클래스입니다.
{
    [SerializeField] LayerMask _DivisionLayer = 0;

    private RectTransform m_pRectTransform = null;

    private bool m_bIsMoving = false; // 창의 이동을 허용하는 변수 

    private void Start()
    {
        m_pRectTransform = GetComponent<RectTransform>();
    }

    // 인벤토리창과 기능은 비슷하게 레이케스트로 충돌 검사를 확인 후 창을 이동 할 수 있게 구현했습니다.

    private void Update()
    {
        Ray2D _Ray2D = new Ray2D(Input.mousePosition, Vector2.zero);

        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _DivisionLayer);

        if (_Hit2D.collider != null && Input.GetMouseButton(0))
        {
            if(_Hit2D.collider.CompareTag("Quest") == true)
                m_bIsMoving = true;
        }

        if (m_bIsMoving == true)
        {
            if (Input.GetMouseButtonUp(0))
            {
                m_bIsMoving = false;

                return;
            }

            m_pRectTransform.position = Input.mousePosition;
        }
    }

    private void OnDestroy()
    {
        m_pRectTransform = null;
    }
}
