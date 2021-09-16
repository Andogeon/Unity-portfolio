using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SceceCamera : MonoBehaviour // 최초 게임 시작씬에서 사용할 카메라 클래스입니다.
{
    [SerializeField] private AudioClip _ReturnSound = null;

    [SerializeField] private Vector3 _StopPosition = Vector3.zero;

    [SerializeField] private float _Speed = 0.0f;

    private bool m_bIsMoving = false;

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if (m_bIsMoving == false)
            {
                AudioSource _AudioSource = gameObject.AddComponent<AudioSource>();

                _AudioSource.clip = _ReturnSound;

                _AudioSource.loop = false;

                _AudioSource.Play();

                m_bIsMoving = true;
            }
        }

        if (m_bIsMoving == true)
        {
            if (transform.position.y < _StopPosition.y)
                transform.position = transform.position + Vector3.up * _Speed * Time.deltaTime;
        }
    }
}
