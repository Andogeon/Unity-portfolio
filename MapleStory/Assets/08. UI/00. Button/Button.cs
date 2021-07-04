using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Button : UI
{
    [SerializeField] private AudioClip _Sound = null;

    [SerializeField] private GameObject _Gameobject = null;

    private TEXT m_pText = null;

    private AudioSource m_pAudioSource = null;

    //private bool m_bIsCannel = false;

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

    public void LeftClick()
    {
        if (0 > m_pText.SetIndex)
            return;

        if(m_pText.SetIndex != 0)
            m_pText.SetIndex -= 1;

        m_pAudioSource.Play();
    }

    public void RightClick()
    {
        if (m_pText.GetCount - 1 <= m_pText.SetIndex)
            return;

        if (m_pText.GetCount != m_pText.SetIndex)
            m_pText.SetIndex += 1;

        m_pAudioSource.Play();
    }

    public void OKCharName()
    {
        if (null == _Gameobject)
            return;

        GameObject _GameObjects = gameObject.transform.parent.gameObject;

        _GameObjects.SetActive(false);

        _Gameobject.SetActive(true);
    }

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