using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : Singleton<SoundManager>
{
    private Dictionary<string, AudioSource> m_pSounds = new Dictionary<string, AudioSource>();

    public void AddSound(string _SoundName, AudioClip _AddSound)
    {
        if (_AddSound == null)
            return;

        AudioSource _CreateSound = null;

        if (m_pSounds.TryGetValue(_SoundName, out _CreateSound) == true) // 해당 사운드 존재한다..
            return; // 등록 X

        GameObject _SoundObject = new GameObject();

        _CreateSound = _SoundObject.AddComponent<AudioSource>();

        _CreateSound.clip = _AddSound;

        m_pSounds.Add(_SoundName, _CreateSound);
    }

    public void PlayBGM(string _SoundName)
    {
        AudioSource _PlayingSound = null;

        if (m_pSounds.TryGetValue(_SoundName, out _PlayingSound) == false)
            return;

        if (_PlayingSound.isPlaying == true)
            return;

        _PlayingSound.loop = true;

        _PlayingSound.playOnAwake = true;

        _PlayingSound.volume = 0.3f;

        _PlayingSound.Play();
    }

    public void PlayingVolume(string _PlayingName, float _Volume)
    {
        if (_Volume < 0.0f)
            return;

        AudioSource _PlayingSound = null;

        if (m_pSounds.TryGetValue(_PlayingName, out _PlayingSound) == false)
            return;

        if (_PlayingSound.isPlaying == false)
            return;

        _PlayingSound.volume = _Volume;
    }

    public void PlaySound(string _PlayingSoundName)
    {
        AudioSource _PlayingSound = null;

        if (m_pSounds.TryGetValue(_PlayingSoundName, out _PlayingSound) == false)
            return;

        if (_PlayingSound.isPlaying == true)
            return;

        _PlayingSound.volume = 1.0f;

        _PlayingSound.Play();
    }

    public void PlaySound(string _PlayingSoundName, float _Volume = 1.0f)
    {
        AudioSource _PlayingSound = null;

        if (m_pSounds.TryGetValue(_PlayingSoundName, out _PlayingSound) == false)
            return;

        _PlayingSound.volume = _Volume;

        _PlayingSound.Play();
    }
}
