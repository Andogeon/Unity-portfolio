using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CBarrier : MonoBehaviour
{
    private Cookie m_pUserCookie = null;

    private Rigidbody2D m_pRigidBody2D = null;

    private CSoundManaged m_pSoundManager = null;

    private Vector3 m_vOrlPosition = Vector3.zero;

    private Quaternion m_RotOrlRotation = Quaternion.identity;

    private void Awake()
    {
        m_pUserCookie = FindObjectOfType<Cookie>();

        m_vOrlPosition = transform.localPosition;
        
        m_RotOrlRotation = transform.rotation;

        m_pSoundManager = CTopManager.GetInstance().GetSoundManaged;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (m_pUserCookie.GetSkill[0] != null || m_pUserCookie.transform.localScale.x >= 1.5f)
        {
            ComponentInit();

            return;
        }

        if (m_pUserCookie.AccessCookieState is CookieBouns || m_pUserCookie.GetSkill[2] != null)
            return;

        m_pUserCookie.Hit();
    }

    private void ComponentInit()
    {
        if(m_pRigidBody2D != null)
        {
            m_pRigidBody2D.AddForce(new Vector2(1500.0f, 500.0f));
            m_pRigidBody2D.angularVelocity = 700.0f;
            
            return;
        }

        m_pRigidBody2D = gameObject.AddComponent<Rigidbody2D>();

        if (m_pRigidBody2D == null)
            m_pRigidBody2D = GetComponent<Rigidbody2D>();

        int _iIndex = UnityEngine.Random.Range(0, 2);

        string _strSoundName = string.Empty;

        switch(_iIndex)
        {
            case 0:
                _strSoundName = "FCrush";
                break;
            case 1:
                _strSoundName = "SCrush";
                break;
            case 2:
                _strSoundName = "TCrush";
                break;
        }

        m_pSoundManager.PlaySound(_strSoundName);

        m_pRigidBody2D.AddForce(new Vector2(1500.0f, 500.0f));
        m_pRigidBody2D.angularVelocity = 700.0f;

    }

    private void OnDisable()
    {
        m_pRigidBody2D = GetComponent<Rigidbody2D>();
        Destroy(m_pRigidBody2D);
        transform.localPosition = m_vOrlPosition;
        transform.rotation = m_RotOrlRotation;
    }
}