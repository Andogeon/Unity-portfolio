using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Option : MonoBehaviour // 사운드의 음량 조절을 하기 위한 클래스입니다.
{
    [SerializeField] private Slider _BGMSilider = null;

    [SerializeField] private Slider _EffectSilider = null;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private float BGMVolume = 1.0f; // 배경음의 볼륨 

    private float EffectVolume = 1.0f; // 이펙트의 볼륨 

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    // 배경음의 볼륨을 조절시 호출 되는 함수입니다.

    public void BGMSoundValue()
    {
        if (null == _BGMSilider)
            return;

        BGMVolume = _BGMSilider.value;

        m_pSoundManager.SetPlayBGMSound(_BGMSilider.value);
    }

    // 효과음의 볼륨을 조절시 호출 되는 함수입니다.
    public void EffectSoundValue()
    {
        if (null == _BGMSilider)
            return;

        EffectVolume = _EffectSilider.value;

        m_pSoundManager.SetPlayEffectSound(_EffectSilider.value);
    }

    // 옵션 창이 활성화 및 비활성화 시 호출되는 함수입니다.
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

    // 취소 버튼을 누를시 다시 음량을 사운드 매니저가 소유하고 잇는 음량으로 되돌리는 함수입니다.
    public void ResetSoundOption()
    {
        m_pSoundManager.ResetBGMSoundvolume();

        m_pSoundManager.ResetEffectSoundvolume();

        gameObject.SetActive(false);

        _BGMSilider.value = m_pSoundManager.AccessBGMVolume;

        _EffectSilider.value = m_pSoundManager.AccessEffectVolume;
    }
}
