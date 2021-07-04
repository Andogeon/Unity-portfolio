using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingGround : MonoBehaviour
{
    [SerializeField] private Camera _Camera = null;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 _Position = _Camera.transform.position;

        _Position.z = 0.0f;

        transform.position = _Position;
    }
}
