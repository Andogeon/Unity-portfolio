using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//---------------------------------
// 게임 화면을 보여줄 클래스 
//---------------------------------

public class CGameScene : MonoBehaviour
{
    //---------------------------------
    // 씬 관리자 초기화
    //---------------------------------
    private void Awake()
    {
        GameObject _pSoundManager = GameObject.Find("SoundManager");

        m_pTopManager.Initialize();
        m_pSoundManager = _pSoundManager.GetComponent<CSoundManaged>();
        m_pTopManager.SetSoundManager(m_pSoundManager);

        Transform _pGameTileParent = transform.parent.Find("GameTiles");
        Transform _pGameJellyParent = transform.parent.Find("GameJellyEffects");
        Transform _pGameBoosterParent = transform.parent.Find("GameBoosterEffects");

        m_pTerrain = transform.parent.Find("Terrain");

        m_pSceneManaged = m_pTopManager.GetSceneManaged;
        m_pSceneManaged.InitGameScene(this, _pGameTileParent);
        m_pSceneManaged.Initialize();

        m_pSceneManaged.SetJellyParent(_pGameJellyParent);
        m_pSceneManaged.SetBoosterEffect(_pGameBoosterParent);

        InitUIGameObject();
    }

    //---------------------------------------
    // 실제 게임 화면 갱신 
    //---------------------------------------
    private void Update()
    {
        m_pSceneManaged.Update(m_Speed);
    }

    private CGlobalEnum.SCENE_NAME GetTerrainType(GameObject _pGameobject)
    {
        CGlobalEnum.SCENE_NAME _eSceneName = CGlobalEnum.SCENE_NAME.SCENE_BACKGROUND;

        switch(_pGameobject.name)
        {
            case "Mountain":
                _eSceneName = CGlobalEnum.SCENE_NAME.SCENE_TERRIN1;
                break;
            case "Mountain EX":
                _eSceneName = CGlobalEnum.SCENE_NAME.SCENE_TERRIN2;
                break;
            case "Could":
                _eSceneName = CGlobalEnum.SCENE_NAME.SCENE_TERRIN3;
                break;
        }

        return _eSceneName;
    }

    public BoxCollider2D ResetBoxCollider()
    {
        BoxCollider2D _pBoxCollider = GetComponent<BoxCollider2D>();
        Destroy(_pBoxCollider);
        return gameObject.AddComponent<BoxCollider2D>();
    }

    private void InitUIGameObject()
    {
        // UI에 최상위 캔버스 객체를 얻어온다.
        GameObject _pCanvas = GameObject.Find("Canvas");

        // 캔버스가 존재하지 않는다면 함수를 종료한다.
        if (null == _pCanvas)
            return;

        // 페이즈 이미지를 씬 교체를 관리하는 객체로 넘겨준다.
        Image _pFadeImage = _pCanvas.transform.Find("Fade").GetComponent<Image>();
        m_pSceneManaged.GetSceneExchange.SetFadeImage(_pFadeImage);
    }

    public void SetSpeed(float _fSpeed)
    {
        m_fOrlSpeed = m_Speed;

        m_Speed = _fSpeed;
    }

    public void ResetSpeed()
    {
        if (m_fOrlSpeed == 0.0f)
            return;

        m_Speed = m_fOrlSpeed;
    }

    public void OnClickPauseGame()
    {
        m_pSoundManager.PlaySound("CloseBtn");

        m_PopupPause.SetActive(true);
    }

#region 프로퍼티
public float GetGameSceneSpeed { get => m_Speed; }
public Transform GetTerrain { get => m_pTerrain; }
#endregion

#region 변수들
    [SerializeField] private float m_Speed = 0.0f;

    [SerializeField] private GameObject m_PopupPause = null;

    private CTopManager m_pTopManager = CTopManager.GetInstance();

    private CSceneManaged m_pSceneManaged = null;

    private CSoundManaged m_pSoundManager = null;

    private Transform m_pTerrain = null;

    private float m_fOrlSpeed = 0.0f;
#endregion
}