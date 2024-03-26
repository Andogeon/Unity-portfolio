using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CTerrain : CSceneMap
{
    [SerializeField] private float m_Offset = 0.0f;

    private float m_fInitPosition = 0.0f;

    private float m_fDeletePosition = 0.0f;

    private float m_fCreatePosition = 0.0f;

    private bool m_bCreate = false;

    public BoxCollider2D GetBoxCollision { get => m_pBoxCollider; }

    protected override void Awake()
    {
        base.Awake();

        if (transform.parent != null)
        {
            m_fInitPosition = transform.parent.position.x + m_pParentBoxCollider.bounds.extents.x;

            m_fCreatePosition = m_fInitPosition + m_pBoxCollider.bounds.extents.x;

            m_fDeletePosition = transform.parent.position.x - m_pParentBoxCollider.bounds.extents.x;

            m_fCreatePosition += m_Offset;
        }
        else
        {
            m_fInitPosition = transform.position.x + m_pParentBoxCollider.size.x / 2;

            m_fCreatePosition = m_fInitPosition + m_pBoxCollider.size.x / 2;

            m_fDeletePosition = transform.position.x - m_pParentBoxCollider.size.x / 2;

            m_fCreatePosition += m_Offset;
        }
    }

    private void Update()
    {
        if (CGlobalValues.m_bIsGameOver || CGlobalValues.m_bIsPause)
            return;

        if (m_pBoxCollider == null)
            m_pBoxCollider = GetComponent<BoxCollider2D>();

        if(!m_pBoxCollider.enabled)
        {
            Destroy(m_pBoxCollider);
            m_pBoxCollider = GetComponent<BoxCollider2D>();
        }

        transform.localPosition += Vector3.left * m_Speed * Time.deltaTime;

        float _fPositionX = transform.localPosition.x + m_pBoxCollider.bounds.extents.x;

        if (_fPositionX <= m_fInitPosition && m_bCreate == false)
        {
            m_fCreatePosition = transform.localPosition.x + (m_pBoxCollider.bounds.extents.x * 2);

            GameObject _pCloneGameobject = CGlobalFunc.CreateClone(gameObject, new Vector3(m_fCreatePosition, gameObject.transform.localPosition.y), transform.parent);

            _pCloneGameobject.name = _pCloneGameobject.name.Replace("(Clone)", "");

            m_bCreate = true;
        }
        else
        {
            if (m_bCreate == true)
            {
                if (_fPositionX <= m_fDeletePosition)
                    Destroy(gameObject);
            }
        }
    }

    public void ResetPositions(BoxCollider2D _pBoxCollider)
    {
        m_pParentBoxCollider = _pBoxCollider;

        m_fInitPosition = transform.parent.position.x + _pBoxCollider.bounds.extents.x;

        m_fDeletePosition = transform.parent.position.x - _pBoxCollider.bounds.extents.x;

        m_fCreatePosition = transform.localPosition.x + (m_pBoxCollider.bounds.extents.x * 2);

        m_fCreatePosition += m_Offset;
    }
}