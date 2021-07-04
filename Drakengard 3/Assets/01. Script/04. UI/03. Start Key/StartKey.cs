using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class StartKey : MonoBehaviour
{
    [SerializeField] private GameObject _FadeObject = null;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private void Start()
    {
        AudioClip _Sound = Resources.Load("Button Sound") as AudioClip;

        m_pSoundManager.AddSound("Button Sound", _Sound);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            _FadeObject.SetActive(true);

            m_pSoundManager.PlaySound("Button Sound");
        }
    }
}
