using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CSceneMap : MonoBehaviour
{
    [Header("이미지 스피드")]
    [SerializeField] protected float m_Speed = 0.0f;

    protected SpriteRenderer m_pSpriteRenerer = null;

    protected BoxCollider2D m_pParentBoxCollider = null;

    protected BoxCollider2D m_pBoxCollider = null;

    protected string m_StrObjectName = string.Empty;

    protected virtual void Awake()
    {
        m_pSpriteRenerer = GetComponent<SpriteRenderer>();

        m_pBoxCollider = GetComponent<BoxCollider2D>();

        Transform _ParentTransform = transform.parent.parent;

        if (_ParentTransform != null)
        {
            Transform _pGameSceneTransform = _ParentTransform.Find("GameBackGround");
            
            m_pParentBoxCollider = _pGameSceneTransform.GetComponent<BoxCollider2D>();
        }
        else
            m_pParentBoxCollider = GetComponent<BoxCollider2D>();

    }

    public void ResetBoxCollder()
    {
        Destroy(m_pBoxCollider);

        m_pBoxCollider = gameObject.AddComponent<BoxCollider2D>();
        m_pBoxCollider.isTrigger = true;
    }
}
