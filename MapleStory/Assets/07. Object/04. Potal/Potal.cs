using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum PORTALTYPE
{
    POTAL_NORMAL,
    POTAL_FIRST,
    POTAL_END
}


public class Potal : MonoBehaviour
{
    //[SerializeField] private AudioClip _PotalSound = null;

    [SerializeField] private string _SceneName = string.Empty;

    [SerializeField] private string _SceneStartName = string.Empty;

    [SerializeField] private PORTALTYPE _PotalType = PORTALTYPE.POTAL_FIRST;

    [SerializeField] private Fade _Fade = null;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private bool m_bIsChangeScene = false;

    public PORTALTYPE GetPortalType
    {
        get { return _PotalType; }
    }
    
    public Fade AccessFaceObject
    {
        get { return _Fade; }
    }

    private void Update()
    {
        if(true == m_bIsChangeScene)
        {
            if(_Fade.AccessAlpha >= 1.0f)
                SceneManager.LoadScene(_SceneName);
        }
    }

    public void MoveScene(List<GameObject> _NotDeleteObject)
    {
        for (int i = 0; i < _NotDeleteObject.Count; ++i)
            DontDestroyOnLoad(_NotDeleteObject[i]);

        m_bIsChangeScene = true;

        //LoadSceneParameters _LoadSceneParameters = new LoadSceneParameters();

        //_LoadSceneParameters.loadSceneMode = LoadSceneMode.Single;

        //Scene _NewScene = SceneManager.LoadScene(_SceneName, _LoadSceneParameters);
    }

    //public void MoveScene(GameObject _NotDeleteObejct, GameObject _UIObejct)
    //{
    //    DontDestroyOnLoad(_NotDeleteObejct);

    //    DontDestroyOnLoad(_UIObejct);

    //    LoadSceneParameters _LoadSceneParameters = new LoadSceneParameters();

    //    _LoadSceneParameters.loadSceneMode = LoadSceneMode.Single;

    //    Scene _NewScene = SceneManager.LoadScene(_SceneName, _LoadSceneParameters);
    //}
}