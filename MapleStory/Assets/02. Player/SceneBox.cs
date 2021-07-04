using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneBox : MonoBehaviour
{
    [SerializeField] private AudioClip _PotalSound = null;

    [SerializeField] private string _SceneName = string.Empty;

    [SerializeField] private Fade _FadeObject = null;

    [SerializeField] private Player _Player = null;

    [SerializeField] private GameObject _InventoryGameObject = null;

    [SerializeField] private GameObject _StateBarGameObject = null;

    [SerializeField] private GameObject _QuestGameObject = null;

    [SerializeField] private GameObject _NameBarGameObject = null;

    [SerializeField] private Vector3 _MovingPosition = Vector3.zero;

    private bool m_bIsSceneChange = false;

    private SoundManager m_pSoundManger = SoundManager.GetInstance();

    private void Start()
    {
        m_pSoundManger.AddSound("Potal Sound", _PotalSound, SoundType.Sound_Effect);
    }

    private void Update()
    {
        if(null != _FadeObject)
        {
            if(_FadeObject.AccessAlpha >= 1.0f && m_bIsSceneChange == true)
            {
                DontDestroyOnLoad(_Player);

                DontDestroyOnLoad(_InventoryGameObject);

                DontDestroyOnLoad(_StateBarGameObject);

                DontDestroyOnLoad(_QuestGameObject);

                DontDestroyOnLoad(_NameBarGameObject);

                SceneManager.LoadScene(_SceneName);

                _Player.transform.position = _MovingPosition;

                m_bIsSceneChange = false;

                return;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_FadeObject == null)
            return;

        _FadeObject.AccessFaceType = FADETYPE.FADE_IN;

        m_bIsSceneChange = true;

        m_pSoundManger.PlaySound("Potal Sound");
    }
}
