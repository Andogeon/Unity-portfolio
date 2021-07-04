using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SelectScene : MonoBehaviour
{
    //[SerializeField] private GameObject _Gameobject = null;

    [SerializeField] private GameObject _Gameobject = null;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    public void NextScene()
    {
        Body _Body = _Gameobject.GetComponentInChildren<Body>();

        InputField _InputField = gameObject.transform.parent.GetComponentInChildren<InputField>();

        _Body.AccessPlayerName = _InputField.text;

        DontDestroyOnLoad(_Gameobject);

        //SceneManager.LoadScene("Maple Hill");

        SceneManager.LoadScene("Maple Start Scene");
    }
}