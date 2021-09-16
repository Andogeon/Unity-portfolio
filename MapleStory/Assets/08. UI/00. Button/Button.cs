using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

// 커스터마이징시 헤어, 얼굴, 아이템, 확인 클릭 버튼에서 사용되는 클래스입니다.

public class Button : UI
{
    [SerializeField] private AudioClip _Sound = null; // 클릭 사운드 변수 

    [SerializeField] private GameObject _Gameobject = null; // 헤어, 얼굴, 아이템의 텍스트 정보를 얻어올 게임오브젝트 변수

    private TEXT m_pText = null; // // 헤어, 얼굴, 아이템의 텍스트

    private AudioSource m_pAudioSource = null; // 재생할 사운드 리소스 

    // Start is called before the first frame update
    private void Awake()
    {
        if (_Gameobject != null)
            m_pText = _Gameobject.GetComponent<TEXT>();

        if (null != _Sound && m_pAudioSource == null)
        {
            m_pAudioSource = gameObject.AddComponent<AudioSource>();

            m_pAudioSource.clip = _Sound;

            m_pAudioSource.loop = false;
        }
    }

    // 왼쪽 버튼 클릭시 헤어, 얼굴, 아이템의 대한 텍스트 정보를 변경하는 함수입니다.

    public void LeftClick()
    {
        if (0 > m_pText.SetIndex) // 텍스트에서 넘어온 인덱스 값이 0보다 작을 경우 
            return; // 종료한다 

        if(m_pText.SetIndex != 0) // 인덱스의 값이 0이 아닐경우만 
            m_pText.SetIndex -= 1; // 인덱스 값을 감소한다 

        m_pAudioSource.Play(); // 클릭시 사운드 재생 
    }

    // 오른쪽 버튼 클릭시 헤어, 얼굴, 아이템의 대한 텍스트 정보를 변경하는 함수입니다.

    public void RightClick()
    {
        if (m_pText.GetCount - 1 <= m_pText.SetIndex) // 인스펙터에서 지정한 문장들의 길이보다 인덱스의 길이가 클 경우 
            return; // 종료 

        if (m_pText.GetCount != m_pText.SetIndex) // 인스펙터에서 지정한 문장들의 길이보다 인덱스의 길이가 같지 않을 경우만 
            m_pText.SetIndex += 1; // 인덱스 증가 

        m_pAudioSource.Play(); // 사운드 재생 
    }

    // 케릭터 머리, 얼굴, 아이템을 결정되고 케릭터의 닉네임 UI를 활성화 할 때 호출하는 함수입니다.

    public void OKCharName()
    {
        if (null == _Gameobject)
            return;

        GameObject _GameObjects = gameObject.transform.parent.gameObject;

        _GameObjects.SetActive(false);

        _Gameobject.SetActive(true);
    }

    // 다시 얼굴, 헤어, 아이템을 선택창을 활성화 할 때 호출 되는 함수입니다.

    // 이름 결정 UI는 비활성화 됩니다.

    public void CancelCharName()
    {
        if (null == _Gameobject)
            return;

        GameObject _GameObjects = gameObject.transform.parent.gameObject;

        _GameObjects.SetActive(false);

        _Gameobject.SetActive(true);
    }

    private void OnEnable()
    {
        if(null != m_pAudioSource)
        {
            if (m_pAudioSource.isPlaying == true)
                m_pAudioSource.Stop();
        }
    }
}