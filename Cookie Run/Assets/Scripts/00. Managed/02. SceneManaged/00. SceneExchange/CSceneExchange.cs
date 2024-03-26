using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 게임 화면 교체 관리할 클래스
/// </summary>
public class CSceneExchange
{
    public CSceneExchange(CSceneManaged _pSceneManaged)
    {
        m_pSceneManaged = _pSceneManaged; 
    }

    private void GameSceneTileClear()
    {
        m_pSceneManaged.GetTileNames.Clear();

        for (int i = 0; i < m_pSceneManaged.GetTileList.Count; ++i)
        {
            CKeyValue<GameObject, bool> _pTile = m_pSceneManaged.GetTileList[i];

            GameObject.Destroy(_pTile.m_pKey);
        }

        m_pSceneManaged.GetTileList.Clear();
    }

    private CGlobalEnum.SCENE_NAME GetTerrainType(GameObject _pGameobject)
    {
        CGlobalEnum.SCENE_NAME _eSceneName = CGlobalEnum.SCENE_NAME.SCENE_BACKGROUND;

        switch (_pGameobject.name)
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

    public void SetGameScene(ScenePrefab _pGameScenePrefab)
    {
        if (!_pGameScenePrefab)
            return;

        GameSceneTileClear();

        m_pCurrentScenePrefab = _pGameScenePrefab;

        // 게임 화면 백그라운드 그룹 최상위를 불러옴
        Transform _pTerrain = m_pSceneManaged.GetGameScene.GetTerrain;

        if (!_pTerrain)
            return;

        // 게임 화면 이미지들을 교체한다.
        SetGameSceneSprite(_pTerrain);

        // 게임 타일을 교체 한다.
        SetGameTiles();

        // 2개 이상 생성된 오브젝트가 있을 경우 위치를 재조정한다.
        SearchMultiTerrainResetPosition(_pTerrain);
    }

    //----------------------------------------------------
    // 게임 화면에 있는 모든 지형 이미지 변경 함수 
    //----------------------------------------------------
    private void SetGameSceneSprite(Transform _pTerrain)
    {
        if (!_pTerrain)
            return;

        SpriteRenderer[] _pGameSpriteRenderers = _pTerrain.GetComponentsInChildren<SpriteRenderer>();
        SpriteRenderer _pBackGroundRenderer = m_pSceneManaged.GetGameScene.GetComponent<SpriteRenderer>();
        Sprite[] _pGameSceneSprites = m_pCurrentScenePrefab.GetSceneSprite;

        _pBackGroundRenderer.sprite = _pGameSceneSprites[((int)CGlobalEnum.SCENE_NAME.SCENE_BACKGROUND)];
        BoxCollider2D _pBackGroundBox = ResetBoxCollider(m_pSceneManaged.GetGameScene);

        CTerrain[] _pTerrains = _pTerrain.GetComponentsInChildren<CTerrain>();

        for (int i = 0; i < _pGameSpriteRenderers.Length; ++i)
        {
            CGlobalEnum.SCENE_NAME _eSceneName = GetTerrainType(_pGameSpriteRenderers[i].gameObject);

            _pGameSpriteRenderers[i].sprite = _pGameSceneSprites[((int)_eSceneName)];

            _pTerrains[i].ResetBoxCollder();
            _pTerrains[i].ResetPositions(_pBackGroundBox);
        }

        // 지형 X 위치를 0으로 재조정 
        foreach(SpriteRenderer _pTerrainSprite in _pGameSpriteRenderers)
            _pTerrainSprite.transform.localPosition = new Vector3(0.0f, _pTerrainSprite.transform.localPosition.y, _pTerrainSprite.transform.localPosition.z);
    }

    private void SetGameTiles()
    {
        if (m_pCurrentScenePrefab == null)
            return;

        // 게임 화면 이름을 갱신한다.
        m_pSceneManaged.SetSceneName(m_pCurrentScenePrefab);
        
        CResourceManaged _pResourceManaged = m_pSceneManaged.GetPoolManaged.GetResourceManaged;
        GameObject[] _pSceneTiles = m_pCurrentScenePrefab.GetSceneTiles;

        if (m_pSceneManaged.GetPrevSceneName == string.Empty || m_pSceneManaged.GetPrevSceneName != "Bonus")
        {
            for (int i = 0; i < _pSceneTiles.Length; ++i)
            {
                string _strGameSceneTileName = m_pSceneManaged.GetSceneName + "-" + _pSceneTiles[i].gameObject.name;

                _pResourceManaged.Insert(_strGameSceneTileName, _pSceneTiles[i]);
                m_pSceneManaged.GetTileNames.Enqueue(_strGameSceneTileName);
            }
        }
        else
            m_pSceneManaged.GetTileNames.Clear();


        const int _iTileIndex = 0;

        string _StrTilePatten = (m_pCurrentScenePrefab.GetSceneName == "Bonus") ? "Bonus Tile Pattern" : "Tile Pattern";

        string _strTileName = m_pSceneManaged.GetSceneName + "-" + _StrTilePatten + " " + _iTileIndex.ToString();
        GameObject _pGameTiles = m_pSceneManaged.GetPoolManaged.pop(_strTileName, new Vector3(2.61f, -4.34f, 0.0f), m_pSceneManaged.GetTileParent);
        CKeyValue<GameObject, bool> _pGameObject = new CKeyValue<GameObject, bool>(_pGameTiles, false);
        m_pSceneManaged.GetTileList.Add(_pGameObject);
    }

    private void SearchMultiTerrainResetPosition(Transform _pTerrain)
    {
        List<CTerrain> _pSearchTerrains = new List<CTerrain>();

        CTerrain[] _pTerrains = _pTerrain.GetComponentsInChildren<CTerrain>();

        SearchTerrainObjects(_pTerrains, ref _pSearchTerrains, "Mountain");
        SearchTerrainObjects(_pTerrains, ref _pSearchTerrains, "Mountain EX");
        SearchTerrainObjects(_pTerrains, ref _pSearchTerrains, "Could");
    }

    private void SearchTerrainObjects(CTerrain[] _pTerrains, ref List<CTerrain> _pTerrainGroup, string _strSearchName)
    {
        for(int i = 0; i < _pTerrains.Length; ++i)
        {
            if (_pTerrains[i].name == _strSearchName)
                _pTerrainGroup.Add(_pTerrains[i]);
        }

        if (_pTerrains.Length <= 1)
        {
            _pTerrainGroup.Clear();

            return;
        }

        for (int i = 0; i < _pTerrainGroup.Count; ++i)
        {
            BoxCollider2D _pBoxCollision = _pTerrainGroup[i].GetBoxCollision;

            if (i + 1 >= _pTerrainGroup.Count)
                continue;

            BoxCollider2D _pNextBoxCollision = _pTerrainGroup[i + 1].GetBoxCollision;

            float _fNewXPosition = _pBoxCollision.transform.localPosition.x + (_pBoxCollision.bounds.extents.x * 2);

            _pNextBoxCollision.transform.localPosition = new Vector3(_fNewXPosition, _pNextBoxCollision.transform.localPosition.y, 0.0f);
        }

        _pTerrainGroup.Clear();
    }

    private BoxCollider2D ResetBoxCollider(CGameScene _pGameScene)
    {
        BoxCollider2D _pBoxCollider = _pGameScene.GetComponent<BoxCollider2D>();
        GameObject.Destroy(_pBoxCollider);

        return _pGameScene.gameObject.AddComponent<BoxCollider2D>();
    }

    // 페이드 이미지를 얻어오기 위한 함수
    public void SetFadeImage(Image _pImage)
    {
        m_pFadeImage = _pImage;
    }

    public IEnumerator FadeSceceChange(ScenePrefab _pScenePrefab, Cookie _pCookie, string _StrSoundName = "")
    {
        if (null == _pScenePrefab || null == _pCookie)
            yield break;

        while(m_pFadeImage.color.a < 1.0f)
        {
            Color _LocalFadeColor = m_pFadeImage.color;
            _LocalFadeColor.a += m_fFadeSped * Time.deltaTime;
            m_pFadeImage.color = _LocalFadeColor;
            yield return null;
        }

        Color _FadeColor = m_pFadeImage.color;
        _FadeColor.a = 1.0f;
        m_pFadeImage.color = _FadeColor;

        yield return new WaitForSeconds(0.5f);

        SetGameScene(_pScenePrefab);
        _pCookie.MovePosition();

        if (_StrSoundName != string.Empty)
            CTopManager.GetInstance().GetSoundManaged.PlayBGM(_StrSoundName);

        while (m_pFadeImage.color.a > 0.0f)
        {
            Color _LocalFadeColor = m_pFadeImage.color;
            _LocalFadeColor.a -= m_fFadeSped * Time.deltaTime;
            m_pFadeImage.color = _LocalFadeColor;
            yield return null;
        }

        _FadeColor = m_pFadeImage.color;
        _FadeColor.a = 0.0f;
        m_pFadeImage.color = _FadeColor;
    }

    public string GetScenePrefabName { get => m_pCurrentScenePrefab.GetSceneName; }

    private CSceneManaged m_pSceneManaged = null;

    private ScenePrefab m_pCurrentScenePrefab = null;

    private Image m_pFadeImage = null;

    private float m_fFadeSped = 1.0f;
}