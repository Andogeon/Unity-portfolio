using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
