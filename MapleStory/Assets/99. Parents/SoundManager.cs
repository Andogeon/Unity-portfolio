using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundType
{
    Sound_BGM,
    Sound_Effect,
    Sound_ChangeBGM
}



public class SoundManager : Singleton<SoundManager>
{
    private Dictionary<string, AudioClip> m_pSoundClips = new Dictionary<string, AudioClip>();

    private List<AudioSource> m_pAudioSource = new List<AudioSource>();

    private GameObject m_pBGMGameObejct = null;

    private AudioSource m_pBGMAudio = null;

    private float m_fBGMVolume = 1.0f;

    private float m_fEffectVolume = 1.0f;

    private float m_fOldVolume = 0.0f;

    private bool m_bIsChangeBGMVolume = false;

    private bool m_bIsChangeEffectVolume = false;

    public float AccessBGMVolume
    {
        get { return m_fBGMVolume; }
    }

    public float AccessEffectVolume
    {
        get { return m_fOldVolume; }
    }

    public bool AccessChangeBGMVolume
    {
        get { return m_bIsChangeBGMVolume; }

        set { m_bIsChangeBGMVolume = value; }
    }

    public bool AccessChangeEffectVolume
    {
        get { return m_bIsChangeEffectVolume; }

        set { m_bIsChangeEffectVolume = value; }
    }

    public void AddSound(string _SoundName, AudioClip _AddSound, SoundType _SoundType)
    {
        if (_AddSound == null)
            return;

        if(m_pBGMGameObejct == null && _SoundType == SoundType.Sound_BGM)
        {
            m_pBGMGameObejct = new GameObject();

            m_pBGMGameObejct.name = "BGM Box";

            m_pBGMAudio = m_pBGMGameObejct.AddComponent<AudioSource>();

            m_pBGMAudio.clip = _AddSound;

            m_pBGMAudio.loop = true;

            m_pBGMAudio.volume = m_fBGMVolume;

            GameObject.DontDestroyOnLoad(m_pBGMGameObejct);
        }

        AudioClip _AudioClips = null;

        if (m_pSoundClips.TryGetValue(_SoundName, out _AudioClips) == true)
            return;

        m_pSoundClips.Add(_SoundName, _AddSound);
    }

    public void PlayBGM()
    {
        if (null == m_pBGMAudio || m_pBGMGameObejct == null)
            return;

        if (m_pBGMAudio.isPlaying == true)
            return;

        m_pBGMAudio.Play();

        m_pBGMAudio.volume = m_fBGMVolume;
    }

    public bool IsPlayingBGM(AudioClip _SoundClip)
    {
        if (null == _SoundClip || m_pBGMAudio == null)
            return false;

        if (m_pBGMAudio.clip != _SoundClip)
            return false;

        return m_pBGMAudio.isPlaying;
    }

    public void PlayBGM(string _SoundName)
    {
        if (null == m_pBGMAudio || m_pBGMGameObejct == null)
            return;

        ChangeBGMPlay(_SoundName);
    }

    public void ChangeBGMPlay(string _SoundName)
    {
        m_pBGMAudio.Stop();

        AudioClip _AudioClips = null;

        if (m_pSoundClips.TryGetValue(_SoundName, out _AudioClips) == false)
            return;

        m_pBGMAudio.clip = _AudioClips;

        m_pBGMAudio.Play();

        m_pBGMAudio.volume = m_fBGMVolume;
    }

    public void PlaySound(string _SoundName)
    {
        if (null == m_pAudioSource || _SoundName == string.Empty)
            return;

        AudioClip _AudioClip = null;

        if (m_pSoundClips.TryGetValue(_SoundName, out _AudioClip) == false)
            return;

        AudioSource _AudioSource = null;

        GameObject _AudioObject = null;

        // 하나도 없다면 만들고 집어넣는다
        if(m_pAudioSource.Count <= 0)
        {
            _AudioObject = new GameObject();

            _AudioSource = _AudioObject.AddComponent<AudioSource>();

            _AudioSource.loop = false;

            _AudioSource.clip = _AudioClip;

            _AudioSource.Play();

            _AudioSource.volume = m_fEffectVolume;

            m_pAudioSource.Add(_AudioSource);

            return;
        }

        for (int i = 0; i < m_pAudioSource.Count; ++i)
        {
            if (null == m_pAudioSource[i])
            {
                _AudioObject = new GameObject();

                _AudioSource = _AudioObject.AddComponent<AudioSource>();

                _AudioSource.loop = false;

                _AudioSource.clip = _AudioClip;

                _AudioSource.Play();

                _AudioSource.volume = m_fEffectVolume;

                m_pAudioSource[i] = _AudioSource;

                return;
            }

            if(m_pAudioSource[i].isPlaying == false)
            {
                _AudioSource = m_pAudioSource[i];

                _AudioSource.clip = _AudioClip;

                _AudioSource.Play();

                _AudioSource.volume = m_fEffectVolume;

                return;
            }
        }

        _AudioObject = new GameObject();

        _AudioSource = _AudioObject.AddComponent<AudioSource>();

        _AudioSource.loop = false;

        _AudioSource.clip = _AudioClip;

        _AudioSource.Play();

        _AudioSource.volume = m_fEffectVolume;

        m_pAudioSource.Add(_AudioSource);
    }

    public bool TimePause(string _SoundName, float _TimePause)
    {
        AudioClip _AudioClip = null;

        if (m_pSoundClips.TryGetValue(_SoundName, out _AudioClip) == false)
            return false;

        for (int i = 0; i < m_pAudioSource.Count; ++i)
        {
            if (m_pAudioSource[i] == null)
                continue;
            else
            {
                if(m_pAudioSource[i].clip == _AudioClip)
                {
                    if (m_pAudioSource[i].isPlaying == true && _AudioClip.length <= _TimePause)
                    {
                        m_pAudioSource[i].Pause();

                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void ReplaySound(string _SoundName)
    {
        AudioClip _AudioClip = null;

        if (m_pSoundClips.TryGetValue(_SoundName, out _AudioClip) == false)
            return;

        for (int i = 0; i < m_pAudioSource.Count; ++i)
        {
            if (m_pAudioSource[i] == null)
                continue;
            else
            {
                if (m_pAudioSource[i].clip == _AudioClip)
                {
                    if (m_pAudioSource[i].isPlaying == false)
                    {
                        m_pAudioSource[i].Play();

                        return;
                    }
                }
            }
        }
    }

    public void SetPlayBGMSound(float _BGMVolume)
    {
        if (null == m_pBGMAudio)
            return;

        if (m_bIsChangeBGMVolume == false)
        {
            m_fBGMVolume = m_pBGMAudio.volume;

            m_bIsChangeBGMVolume = true;
        }

        m_pBGMAudio.volume = _BGMVolume;
    }

    public void SetPlayEffectSound(float _EffectVolume)
    {
        if (null == m_pBGMAudio)
            return;

        if (m_bIsChangeEffectVolume == false)
        {
            m_fOldVolume = m_fEffectVolume;

            m_bIsChangeEffectVolume = true;
        }

        m_fEffectVolume = _EffectVolume;

        for (int i = 0; i < m_pAudioSource.Count; ++i)
        {
            if (m_pAudioSource[i] == m_pBGMAudio)
                continue;

            //if (i == m_pAudioSource.Count - 1 && m_bIsChangeEffectVolume == false && m_pAudioSource[i] != null)
            //{
            //    m_fEffectVolume = m_pAudioSource[i].volume;

            //    m_bIsChangeEffectVolume = true;
            //}

            if (m_pAudioSource[i] != null)
                m_pAudioSource[i].volume = m_fOldVolume;
        }
    }

    public void ResetBGMSoundvolume()
    {
        if (null == m_pBGMAudio)
            return;

        m_pBGMAudio.volume = m_fBGMVolume;

        m_bIsChangeBGMVolume = false;
    }

    public void ResetEffectSoundvolume()
    {
        if (null == m_pBGMAudio)
            return;

        if (null == m_pBGMAudio)
            return;

        for (int i = 0; i < m_pAudioSource.Count; ++i)
        {
            if (m_pAudioSource[i] == m_pBGMAudio)
                continue;

            if(m_pAudioSource[i] != null)
                m_pAudioSource[i].volume = m_fEffectVolume;
        }

        m_bIsChangeEffectVolume = false;
    }
}






//public class SoundManager : Singleton<SoundManager>
//{
//    private Dictionary<string, AudioSource> m_pSounds = new Dictionary<string, AudioSource>();

//    public void AddSound(string _SoundName, AudioClip _AddSound, SoundType _SoundType)
//    {
//        if (_AddSound == null)
//            return;

//        AudioSource _CreateSound = null;

//        if (m_pSounds.TryGetValue(_SoundName, out _CreateSound) == true) // 해당 사운드 존재한다..
//            return; // 등록 X

//        GameObject _SoundObject = new GameObject();

//        _CreateSound = _SoundObject.AddComponent<AudioSource>();

//        _CreateSound.clip = _AddSound;

//        if (_SoundType == SoundType.Sound_BGM)
//        {
//            _SoundObject.name = "BGM BOX";


//        }

//        m_pSounds.Add(_SoundName, _CreateSound);
//    }

//    public void PlayBGM(string _SoundName)
//    {
//        AudioSource _PlayingSound = null;

//        if (m_pSounds.TryGetValue(_SoundName, out _PlayingSound) == false)
//            return;

//        if (_PlayingSound.isPlaying == true)
//            return;

//        _PlayingSound.loop = true;

//        _PlayingSound.playOnAwake = true;

//        _PlayingSound.volume = 0.3f;

//        _PlayingSound.Play();
//    }

//    public void PlayingVolume(string _PlayingName, float _Volume)
//    {
//        if (_Volume < 0.0f)
//            return;

//        AudioSource _PlayingSound = null;

//        if (m_pSounds.TryGetValue(_PlayingName, out _PlayingSound) == false)
//            return;

//        if (_PlayingSound.isPlaying == false)
//            return;

//        _PlayingSound.volume = _Volume;
//    }

//    public void PlaySound(string _PlayingSoundName)
//    {
//        AudioSource _PlayingSound = null;

//        if (m_pSounds.TryGetValue(_PlayingSoundName, out _PlayingSound) == false)
//            return;

//        if (_PlayingSound.isPlaying == true)
//            return;

//        _PlayingSound.volume = 1.0f;

//        _PlayingSound.Play();
//    }

//    public void PlaySound(string _PlayingSoundName, float _Volume = 1.0f)
//    {
//        AudioSource _PlayingSound = null;

//        if (m_pSounds.TryGetValue(_PlayingSoundName, out _PlayingSound) == false)
//            return;

//        _PlayingSound.volume = _Volume;

//        _PlayingSound.Play();
//    }

//    public AudioSource FindSound(string _PlayingSoundName)
//    {
//        AudioSource _PlayingSound = null;

//        if (m_pSounds.TryGetValue(_PlayingSoundName, out _PlayingSound) == false)
//            return null;

//        return _PlayingSound;
//    }