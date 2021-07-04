using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour
{
    [SerializeField] private Slider _BGMSilider = null;

    [SerializeField] private Slider _EffectSilider = null;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private float BGMVolume = 1.0f;

    private float EffectVolume = 1.0f;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void BGMSoundValue()
    {
        if (null == _BGMSilider)
            return;

        BGMVolume = _BGMSilider.value;

        m_pSoundManager.SetPlayBGMSound(_BGMSilider.value);
    }

    public void EffectSoundValue()
    {
        if (null == _BGMSilider)
            return;

        EffectVolume = _EffectSilider.value;

        m_pSoundManager.SetPlayEffectSound(_EffectSilider.value);
    }

    public void OnOption()
    {
        if(gameObject.activeSelf == false)
            gameObject.SetActive(true);
        else
            gameObject.SetActive(false);
    }

    public void OffOption()
    {
        m_pSoundManager.AccessChangeBGMVolume = false;

        m_pSoundManager.AccessChangeEffectVolume = false;

        gameObject.SetActive(false);        
    }

    public void ResetSoundOption()
    {
        m_pSoundManager.ResetBGMSoundvolume();

        m_pSoundManager.ResetEffectSoundvolume();

        gameObject.SetActive(false);

        _BGMSilider.value = m_pSoundManager.AccessBGMVolume;

        _EffectSilider.value = m_pSoundManager.AccessEffectVolume;
    }
}
