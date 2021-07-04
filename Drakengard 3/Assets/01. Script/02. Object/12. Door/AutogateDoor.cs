using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AutogateDoor : MonoBehaviour
{
    [SerializeField] float _Speed = 0.0f;

    private Transform _DoorTransform = null;

    private NavMeshObstacle _NaviMesh = null;

    private bool m_bIsSwitchDoor = false;

    public bool SetSwitchDoor
    {
        set { m_bIsSwitchDoor = value; }
    }

    void Start()
    {
        _DoorTransform = gameObject.transform;

        _NaviMesh = GetComponent<NavMeshObstacle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (m_bIsSwitchDoor == false)
            return;

        if (gameObject.tag == "LeftDoor" && _DoorTransform.eulerAngles.y < 90.0f)
            _DoorTransform.eulerAngles = new Vector3(_DoorTransform.eulerAngles.x, _DoorTransform.eulerAngles.y + _Speed * Time.deltaTime, _DoorTransform.eulerAngles.z);
        else if (gameObject.tag == "LeftDoor" && _DoorTransform.eulerAngles.y >= 90.0f)
        {
            if(_NaviMesh.carving == false)
                _NaviMesh.carving = true;
        }

        if (gameObject.tag == "RightDoor")
        {
            float _Angle = _DoorTransform.eulerAngles.y - 360.0f;

            if (_Angle <= -360.0f)
                _Angle = 0.0f;

            if (_Angle > -90.0f || _Angle == 0.0f)
                _DoorTransform.eulerAngles = new Vector3(_DoorTransform.eulerAngles.x, _DoorTransform.eulerAngles.y - _Speed * Time.deltaTime, _DoorTransform.eulerAngles.z);
            else if(_Angle < -90.0f)
            {
                if (_NaviMesh.carving == false)
                    _NaviMesh.carving = true;
            }
        }
    }
}
