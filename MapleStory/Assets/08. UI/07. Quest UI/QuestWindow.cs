using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestWindow : MonoBehaviour
{
    [SerializeField] LayerMask _DivisionLayer = 0;

    private RectTransform m_pRectTransform = null;

    private bool m_bIsMoving = false;

    private void Start()
    {
        m_pRectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 겹쳐서 되는 문제 나중에 해결 할 것 !! 

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
