using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class FollowCameraPositions
{
    private static Vector3 m_vPositions = Vector3.zero;

    public static Vector3 AccessOldPosition
    {
        get { return m_vPositions; }

        set { m_vPositions = value; }
    }
}


public class FollowCamera : MonoBehaviour
{
    [SerializeField] private Vector2 _Center = Vector2.zero;

    [SerializeField] private Vector2 _Size = Vector2.zero;

    [SerializeField] private Rigidbody2D _FollowGameobject = null;

    private float m_fWidth = 0.0f;

    private float m_fheight = 0.0f;

    private GameObject m_pFollowObject = null;

    //private static Vector3 m_vPosition = Vector3.zero;

    //public Vector3 AccessOldPosition
    //{
    //    get { return m_vPosition; }

    //    set { m_vPosition = value; }
    //}

    //private static FollowCameraPositions m_FollowCameraPositions;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        
        Gizmos.DrawWireCube(_Center, _Size);
    }

    private void Start()
    {
        m_fheight = Camera.main.orthographicSize;

        m_fWidth = m_fheight * Screen.width / Screen.height;

        if (null == _FollowGameobject)
        {
            GameObject _PlayerObject = GameObject.Find("Player");

            if (null == _PlayerObject)
                return;

            m_pFollowObject = _PlayerObject;
        }
        else
            m_pFollowObject = _FollowGameobject.gameObject;

        //if (m_vPosition != Vector3.zero)
        //    transform.position = m_vPosition;
    }

    private void FixedUpdate()
    {
        if (null == m_pFollowObject)
            m_pFollowObject = GameObject.Find("Player");

        Vector3 _Positions = new Vector3(m_pFollowObject.transform.position.x, m_pFollowObject.transform.position.y, -10.0f);

        Vector3 _Position = Vector3.Lerp(transform.position, _Positions, Time.deltaTime * 2.0f);

        float _fX = _Size.x * 0.5f - m_fWidth;

        float _fY = _Size.y * 0.5f - m_fheight;

        _fX = Mathf.Clamp(_Position.x, -_fX + _Center.x, _fX + _Center.x);

        _fY = Mathf.Clamp(_Position.y, -_fY + _Center.y, _fY + _Center.y);

        transform.position = new Vector3(_fX, _fY, -10.0f);
    }
}
