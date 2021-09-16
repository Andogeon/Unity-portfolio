using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// 포탈의 열거형으로 지정한 이유는 포탈이 진입하고 다음씬의 플레이어가 포탈의 위치를 못찾고 임의적으로 이동하기 때문에 

// 다른 클래스에서 강제적으로 위치 이동을 하기 위해 열거형을 지정했습니다.

public enum PORTALTYPE 
{
    POTAL_NORMAL,
    POTAL_FIRST,
    POTAL_END
}

// 메이플 포탈 클래스입니다.
public class Potal : MonoBehaviour
{
    [SerializeField] private string _SceneName = string.Empty;

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

    // 인스펙터에서 가지고 온 포탈에 진입했다면 페이드 아웃이 완료 됬는지 수시로 확인하여 

    // 완료가 되었다면 다음씬으로 넘어갑니다.

    private void Update()
    {
        if(true == m_bIsChangeScene) // 포탈에 진입 되었다면 
        {
            if(_Fade.AccessAlpha >= 1.0f) // 페이드 아웃이 완료가 되었을때
                SceneManager.LoadScene(_SceneName); // 씬을 이동한다!
        }
    }


    // 실제 포탈 진입시 UI객체들을 비활성화 하고 진입여부를 활성화하는 함수입니다.
    public void MoveScene(List<GameObject> _NotDeleteObject) 
    {
        for (int i = 0; i < _NotDeleteObject.Count; ++i) // 플레이어가 사전에 지정한 객체들을 모두 
            DontDestroyOnLoad(_NotDeleteObject[i]); // 다음씬으로 넘긴다.

        m_bIsChangeScene = true; // 포탈에 진입을 허용
    }
}




//public class Potal : MonoBehaviour
//{
//    [SerializeField] private string _SceneName = string.Empty;

//    //[SerializeField] private string _SceneStartName = string.Empty;

//    [SerializeField] private PORTALTYPE _PotalType = PORTALTYPE.POTAL_FIRST;

//    [SerializeField] private Fade _Fade = null;

//    private SoundManager m_pSoundManager = SoundManager.GetInstance();

//    private bool m_bIsChangeScene = false;

//    public PORTALTYPE GetPortalType
//    {
//        get { return _PotalType; }
//    }

//    public Fade AccessFaceObject
//    {
//        get { return _Fade; }
//    }

//    // 인스펙터에서 가지고 온 포탈에 진입했다면 페이드 아웃이 완료 됬는지 수시로 확인하여 

//    // 완료가 되었다면 다음씬으로 넘어갑니다.

//    private void Update()
//    {
//        if (true == m_bIsChangeScene) // 포탈에 진입 되었다면 
//        {
//            if (_Fade.AccessAlpha >= 1.0f) // 페이드 아웃이 완료가 되었을때
//                SceneManager.LoadScene(_SceneName); // 씬을 이동한다!
//        }
//    }


//    // 실제 포탈 진입시 UI객체들을 비활성화 하고 진입여부를 활성화하는 함수입니다.
//    public void MoveScene(List<GameObject> _NotDeleteObject)
//    {
//        for (int i = 0; i < _NotDeleteObject.Count; ++i) // 플레이어가 사전에 지정한 객체들을 모두 
//            DontDestroyOnLoad(_NotDeleteObject[i]); // 다음씬으로 넘긴다.

//        m_bIsChangeScene = true; // 포탈에 진입을 허용
//    }
//}