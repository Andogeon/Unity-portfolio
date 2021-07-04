using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowSky : MonoBehaviour
{
    [SerializeField] private Vector3 _Offset = Vector3.zero;

    private GameObject m_pCameraObejct = null;

    // Start is called before the first frame update
    void Start()
    {
        m_pCameraObejct = GameObject.Find("Main Camera");
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _MovingPosition = new Vector3(m_pCameraObejct.transform.position.x, m_pCameraObejct.transform.position.y, 1.0f);

        _MovingPosition += _Offset;

        transform.position = _MovingPosition;
    }
}
