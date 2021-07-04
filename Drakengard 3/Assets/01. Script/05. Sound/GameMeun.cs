using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class GameMeun : MonoBehaviour
{
    [SerializeField] private AudioClip _MeunSound = null;

    //[SerializeField] private float _Volume = 1.0f;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    // Start is called before the first frame update
    void Start()
    {
        m_pSoundManager.AddSound(_MeunSound.name, _MeunSound);

        m_pSoundManager.PlayBGM(_MeunSound.name);
    }
}
