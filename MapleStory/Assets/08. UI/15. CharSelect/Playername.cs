using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Playername : MonoBehaviour
{
    [SerializeField] private GameObject _FollowObject = null;

    [SerializeField] private Vector3 _OffsetPosition = Vector3.zero;

    private RectTransform m_pRectTransform = null;

    private Text m_pText = null;

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
        m_pRectTransform.anchoredPosition = _FollowObject.transform.position + _OffsetPosition;
    }
}
