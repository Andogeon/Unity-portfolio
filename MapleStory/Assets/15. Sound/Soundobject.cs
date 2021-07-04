using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class Soundobject : MonoBehaviour
{
    [SerializeField] private AudioClip _BgmSound = null;

    [SerializeField] private string _BgmSoundName = string.Empty;

    [SerializeField] private SoundType _SoundType = SoundType.Sound_BGM;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    // Start is called before the first frame update
    void Start()
    {
        m_pSoundManager.AddSound(_BgmSoundName, _BgmSound, _SoundType);

        if (_SoundType == SoundType.Sound_BGM)
        {
            if (m_pSoundManager.IsPlayingBGM(_BgmSound) == true)
                return;

            m_pSoundManager.PlayBGM(_BgmSoundName);
        }
        else
            m_pSoundManager.PlaySound(_BgmSoundName);
    }

    public void OnEnable()
    {
        if (_SoundType != SoundType.Sound_Effect)
            return;
        
        m_pSoundManager.PlaySound(_BgmSoundName);
    }
}
