using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CSoundManaged : MonoBehaviour
{
    [SerializeField] private AudioClip[] m_BGMSounds = null;

    [SerializeField] private AudioClip[] m_EffectSounds = null;

    public void Initialize()
    {
        CResourceManaged _pResourceManaged = CTopManager.GetInstance().GetResourceManaged;

        if (_pResourceManaged == null)
        {
            CTopManager.GetInstance().Initialize();
            _pResourceManaged = CTopManager.GetInstance().GetResourceManaged;
        }
        
        foreach (AudioClip _pBGMSound in m_BGMSounds)
            _pResourceManaged.Insert<AudioClip>(_pBGMSound.name, _pBGMSound);

        foreach (AudioClip _pEffectSound in m_EffectSounds)
            _pResourceManaged.Insert<AudioClip>(_pEffectSound.name, _pEffectSound);
    }


    public void PlayBGM(string _strSoundName)
    {
        if (m_pPoolManaged == null)
            m_pPoolManaged = CTopManager.GetInstance().GetPoolManaged;

        AudioClip _pAudioClip = m_pPoolManaged.GetResourceManaged.GetResoucre<AudioClip>(_strSoundName);

        if (!m_pSoundGroup.ContainsKey("BGM"))
        {
            GameObject _pBGMSound = new GameObject("BGM Sound");
            AudioSource _pNewAudioSource = _pBGMSound.AddComponent<AudioSource>();
            _pNewAudioSource.clip = _pAudioClip;
            _pNewAudioSource.loop = true;
            _pNewAudioSource.Play();
            m_pSoundGroup.Add("BGM", _pNewAudioSource);
            _pBGMSound.transform.SetParent(transform);
            _pBGMSound.transform.position = Vector3.zero;
        }
        else
        {
            AudioSource _pNewAudioSource = m_pSoundGroup["BGM"];

            if (null == _pNewAudioSource)
                return;

            _pNewAudioSource.Stop();
            _pNewAudioSource.clip = _pAudioClip;
            _pNewAudioSource.Play();
        }
    }

    public void PlaySound(string _strSoundName)
    {
        AudioClip _pAudioClip = m_pPoolManaged.GetResourceManaged.GetResoucre<AudioClip>(_strSoundName);

        AudioSource _pAudioSource = null;

        for(int i = 0; i < m_pPlaySoundEffects.Count; ++i)
        {
            if (m_pPlaySoundEffects[i].isPlaying)
                continue;

            _pAudioSource = m_pPlaySoundEffects[i];
            break;
        }

        if (null == _pAudioSource)
        {
            _pAudioSource = CreateSource();
            m_pPlaySoundEffects.Add(_pAudioSource);
        }

        _pAudioSource.Stop();
        _pAudioSource.clip = _pAudioClip;
        _pAudioSource.loop = false;
        _pAudioSource.PlayOneShot(_pAudioClip);
    }

    public void StopBGM()
    {
        m_pSoundGroup["BGM"].Stop();
    }

    public void PauseBGM()
    {
        m_pSoundGroup["BGM"].Pause();
    }

    public void RestartBGM()
    {
        m_pSoundGroup["BGM"].Play();
    }

    private AudioSource CreateSource()
    {
        GameObject _pNewGameObject = new GameObject("Effect Sound");
        _pNewGameObject.transform.SetParent(transform);
        _pNewGameObject.transform.position = Vector3.zero;

        return _pNewGameObject.AddComponent<AudioSource>();
    }

    private Dictionary<string, AudioSource> m_pSoundGroup = new Dictionary<string, AudioSource>();

    private List<AudioSource> m_pPlaySoundEffects = new List<AudioSource>();

    private CPoolManaged m_pPoolManaged = null;
}
