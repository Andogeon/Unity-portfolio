using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    [SerializeField] private Transform Target = null;

    [SerializeField] private float Dist = 10.0f;

    [SerializeField] private float AngleSpeed = 0.0f;

    [SerializeField] private float ZoomSpeed = 0.0f;

    [SerializeField] private float MinLength = 0.0f;

    [SerializeField] private float MaxLength = 0.0f;

    private Vector3 m_vCameraLook = Vector3.zero;

    private Vector3 m_vPosition = Vector3.zero;

    private float m_fXAngle = 0.0f;

    private float m_fYAngle = 0.0f;

    private bool m_bIsDash = false;

    public bool SetDashCamera
    {
        set { m_bIsDash = value; }
    }

    public Vector3 GetCameraForwardVector
    {
        get { return m_vCameraLook; }
    }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Cursor.visible = false;

        m_fXAngle = transform.eulerAngles.y;
    }

    private void LateUpdate()
    {
        m_vCameraLook = Vector3.Cross(transform.right, Vector3.up); // 카메라의 룩 백터를 계산

        m_fXAngle += Input.GetAxis("Mouse X") * AngleSpeed * Time.deltaTime;

        m_fYAngle += Input.GetAxis("Mouse Y") * AngleSpeed * Time.deltaTime; // 마우스로 회전

        m_fYAngle = Mathf.Clamp(m_fYAngle, -45.0f, 90.0f);

        transform.eulerAngles = new Vector3(m_fYAngle, m_fXAngle, 0.0f);
        
        Dist += Input.GetAxis("Mouse ScrollWheel") * ZoomSpeed;

        if (Physics.Raycast(Target.position, -transform.forward, 10.0f, 1 << 10)) // 오브젝트와 카메라 충돌 시 
            Dist = Mathf.Lerp(Dist, MinLength, Time.deltaTime * 3.0f); // 줌 인 
        else
            Dist = Mathf.Lerp(Dist, MaxLength, Time.deltaTime * 3.0f); // 줌 아웃

        transform.position = Target.position - transform.forward * Dist;
    }
}









//if (Input.GetKeyDown(KeyCode.DownArrow))
//            AngleSpeed -= 10.0f;
//        if (Input.GetKeyDown(KeyCode.UpArrow))
//            AngleSpeed += 10.0f;