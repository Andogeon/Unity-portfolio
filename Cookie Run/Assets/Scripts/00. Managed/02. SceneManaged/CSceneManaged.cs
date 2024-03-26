using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

//------------------------------------
// 게임 씬 배경, 타일을 결정하여 갱신
//------------------------------------
public class CSceneManaged
{
    public CSceneManaged(CPoolManaged _pPoolManaged)
    {
        m_pPoolManaged = _pPoolManaged;

        m_pSceneTiles = new List<CKeyValue<GameObject, bool>>();

        m_pSceneExchange = new CSceneExchange(this);
        m_pSceneDisplay = new CSceneDisplay(this);
    }

    //-------------------------------------------
    // 초기화 함수
    //-------------------------------------------
    public void Initialize()
    {
        CResourceManaged _pResourceManaged = m_pPoolManaged.GetResourceManaged;

        m_pScenePrefab = _pResourceManaged.GetResoucre<ScenePrefab>("Stage 1");

        // 게임 스테이지에서 사용하는 타일, 보너스 타일들을 가지고 온다.
        GameObject[] _pSceneTiles = m_pScenePrefab.GetSceneTiles;
        GameObject[] _pSceneBounsTiles = m_pScenePrefab.GetBounsSceneTiles;

        // 게임 스테이지에서 사용하는 타일 이름 삽입
        for (int i = 0; i < _pSceneTiles.Length; ++i)
        {
            string _strGameSceneTileName = m_strCurrentSceneName + "-" + _pSceneTiles[i].gameObject.name;

            _pResourceManaged.Insert(_strGameSceneTileName, _pSceneTiles[i]);
            m_pSceneNames.Enqueue(_strGameSceneTileName);
        }

        // 게임 스테이지에서 사용하는 라이프, 기타 타일들
        for (int i = 0; i < _pSceneBounsTiles.Length; ++i)
        {
            string _strGameBounsTileName = m_strCurrentSceneName + "-" + _pSceneBounsTiles[i].gameObject.name;
            _pResourceManaged.Insert(_strGameBounsTileName, _pSceneBounsTiles[i]);
        }

        const int _iTileIndex = 0;

        string _strTileName = m_strCurrentSceneName + "-" + "Tile Pattern" + " " + _iTileIndex.ToString();
        GameObject _pGameTiles = m_pPoolManaged.pop(_strTileName, new Vector3(2.61f, -4.34f, 0.0f), m_pGameTileParent);

        if (null == _pGameTiles)
            return;

        CKeyValue<GameObject, bool> _pGameObject = new CKeyValue<GameObject, bool>(_pGameTiles, false);
        m_pSceneTiles.Add(_pGameObject);

        m_pSoundManager = CTopManager.GetInstance().GetSoundManaged;
        m_pSoundManager.Initialize();
        m_pSoundManager.PlayBGM("GameScene");
    }

    //-------------------------------------------
    // 게임 씬을 갱신한다.
    //-------------------------------------------
    public void Update(float _fGameSceneSpeed)
    {
        if (CGlobalValues.m_bIsGameOver || CGlobalValues.m_bIsPause)
            return;

        for (int i = 0; i < m_pSceneTiles.Count; ++i)
            UpdateTile(m_pSceneTiles[i], _fGameSceneSpeed);
    }

    //-------------------------------------------
    // 화면 관리자 초기화 함수 
    //-------------------------------------------
    public void InitGameScene(CGameScene _pGameScene, Transform _pGameTileParent)
    {
        m_pGameScene = _pGameScene;

        m_pGameTileParent = _pGameTileParent;

        m_pSceneRenderer = m_pGameScene.GetComponentsInChildren<SpriteRenderer>();

        m_pGameSceneBoxCollider = m_pGameScene.GetComponent<BoxCollider2D>();

        // 보너스 스테이지 박스 제어를 위한 정보를 얻어오는 과정 
        BoxCollider2D[] _pTerrainBoxCollerders = m_pGameScene.GetTerrain.GetComponentsInChildren<BoxCollider2D>();

        m_pSkyBoxCollliders = new BoxCollider2D[2];

        int _iIndex = 0;

        foreach (BoxCollider2D _pBoxCollder in _pTerrainBoxCollerders)
        {
            if (_pBoxCollder.gameObject.name == "SkyBox Collder" || _pBoxCollder.gameObject.name == "LandBox Collder")
            {
                m_pSkyBoxCollliders[_iIndex] = _pBoxCollder;
                m_pSkyBoxCollliders[_iIndex++].gameObject.SetActive(false);
            }
        }
    }

    //-----------------------------
    // 타일 업데이트 함수 
    //-----------------------------
    private void UpdateTile(CKeyValue<GameObject, bool> _pTiles, float _fGameSceneSpeed)
    {
        if (!_pTiles.m_pKey.activeSelf)
            return;

        _pTiles.m_pKey.transform.localPosition += Vector3.left * _fGameSceneSpeed * Time.deltaTime;

        Tile[] _pGameTiles = _pTiles.m_pKey.GetComponentsInChildren<Tile>();

        for (int i = 0; i < _pGameTiles.Length; ++i)
        {
            if (i == _pGameTiles.Length - 1)
                TileEndcheck(_pTiles, _pGameTiles[i]);
            else
                TileTypeCheck(_pGameTiles[i]);
        }
    }

    public void GameRestart()
    {
        ResetTileCycle();

        m_pSceneDisplay.ResetGameScene();

        m_bIsRestart = true;
    }

    private void TileEndcheck(CKeyValue<GameObject, bool> _pTiles, Tile _pTile)
    {
        if (!_pTile)
            return;

        if (m_pGameSceneBoxCollider == null)
            m_pGameSceneBoxCollider = m_pGameScene.GetComponent<BoxCollider2D>();

        float _fGameBGBoxHalfSize = (m_pGameSceneBoxCollider.size.x * m_pGameScene.transform.localScale.x) / 2;
        float _fGameSceneLeft = (m_pGameSceneBoxCollider.transform.localPosition.x - _fGameBGBoxHalfSize) - 10.0f;
        float _fGameSceneRight = m_pGameSceneBoxCollider.transform.localPosition.x + _fGameBGBoxHalfSize;

        Transform _pLastTileTranform = _pTiles.m_pKey.transform.GetChild(_pTiles.m_pKey.transform.childCount - 2);
        Tile _pLastTile = _pLastTileTranform.GetComponent<Tile>();

        float _fLastTileLeftPos = _pLastTile.GetTileLeftPostion();
        float _fLastTileRightPos = _pLastTile.GetTileRightPostion();

        if (_fLastTileRightPos <= _fGameSceneLeft)
        {
            _pLastTile.ResetJelly();
            _pLastTile.ResetAnimation();
            _pTiles.m_pKey.SetActive(false);

            m_pSceneTiles.Remove(_pTiles);

            if (m_strCurrentSceneName != "Bonus")
                ++m_iTileCycle;
        }

        if (_fLastTileRightPos <= _fGameSceneRight && !_pTiles.m_pValue)
        {
            _pTiles.m_pValue = true;

            string _strTileName = GetTileName();
            GameObject _pGameTiles = m_pPoolManaged.pop(_strTileName, m_vCreatePosition, m_pGameTileParent);

            if (null == _pGameTiles)
                return;

            CKeyValue<GameObject, bool> _pNewValue = new CKeyValue<GameObject, bool>(_pGameTiles, false);
            m_pSceneTiles.Add(_pNewValue);
        }
    }

    private void TileTypeCheck(Tile _pTile)
    {
        if (!_pTile)
            return;

        _pTile.TileAnimationChecking(m_pGameSceneBoxCollider);
    }

    private string GetTileName()
    {
        CGlobalEnum.BOUNSTILETYPE _eBounsTileType = GetBounsTile();

        if (m_pSceneNames.Count <= 0)
        {
            if (_eBounsTileType == CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_NONE && !m_bIsRestart)
            {
                int _iTileCount = m_pScenePrefab.GetSceneTiles.Length;

                //---------------------------------------------------------------------------
                // 1개씩 만들면 큐에 타일 패턴 이름을 집어 넣자마자 빼기 때문에 10개씩 삽입
                //---------------------------------------------------------------------------
                for (int i = 0; i < 10; ++i)
                {
                    string _StrTilePatten = m_pScenePrefab.GetSceneName == "Bonus" ? "Bonus Tile Pattern" : "Tile Pattern";
                    string _strGameSceneTileName = _StrTilePatten + " " + UnityEngine.Random.Range(1, _iTileCount).ToString();

                    m_pSceneNames.Enqueue(m_strCurrentSceneName + "-" + _strGameSceneTileName);
                }
            }
            else if(_eBounsTileType == CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_NONE && m_bIsRestart)
            {
                GameObject[] _pSceneTiles = m_pScenePrefab.GetSceneTiles;

                // 게임 스테이지에서 사용하는 타일 이름 삽입
                for (int i = 0; i < _pSceneTiles.Length; ++i)
                {
                    string _strGameSceneTileName = m_strCurrentSceneName + "-" + _pSceneTiles[i].gameObject.name;

                    m_pSceneNames.Enqueue(_strGameSceneTileName);
                }

                m_bIsRestart = false;
            }
            else
                return LoadBounsTile(_eBounsTileType);
        }

        if (_eBounsTileType == CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_NONE)
            return m_pSceneNames.Dequeue();
        else
            return LoadBounsTile(_eBounsTileType);

    }

    private CGlobalEnum.BOUNSTILETYPE GetBounsTile()
    {
        if (m_pScenePrefab.GetSceneName == "Bonus" || m_iTileCycle < m_iTileCycleScore)
            return CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_NONE;

        // 순회 횟수가 10의 배수일 경우 보너스 맵 강제화 
        if (m_iTileCycleScore % 10 == 0)
        {
            m_iTileCycle = 0;
            m_iTileCycleScore += 1;

            return CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_BOUNS;
        }

        m_iTileCycle = 0;
        m_iTileCycleScore += 1;

        CGlobalEnum.BOUNSTILETYPE _eBounsTileType = (CGlobalEnum.BOUNSTILETYPE)UnityEngine.Random.Range(((int)CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_LIFE), ((int)CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_GOOD));

        return _eBounsTileType;
    }

    private string LoadBounsTile(CGlobalEnum.BOUNSTILETYPE _eBounsTileType)
    {
        int _iTileCount = m_pScenePrefab.GetSceneTiles.Length;

        switch (_eBounsTileType)
        {
            case CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_BOUNS:
                return m_strCurrentSceneName + "-" + "Bouns Tile Pattern";
            case CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_GOOD:
                return m_strCurrentSceneName + "-" + "Good Tile Pattern";
            case CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_LIFE:
                return m_strCurrentSceneName + "-" + "Life Pattern";
            case CGlobalEnum.BOUNSTILETYPE.BOUNSTILE_NICE:
                return m_strCurrentSceneName + "-" + "Nice Tile Pattern";
            default:
                return "Tile Pattern" + " " + UnityEngine.Random.Range(0, _iTileCount).ToString();
        }
    }

#region 설정 함수들
    /// <summary>
    /// 젤리 그룹 설정 함수
    /// </summary>
    /// <param name="_pJellyParent"></param>
    public void SetJellyParent(Transform _pJellyParent)
    {
        m_pJellyParent = _pJellyParent;
    }

    /// <summary>
    /// 부스터 이펙트 그룹 설정 함수
    /// </summary>
    /// <param name="_pBoosterParent"></param>
    public void SetBoosterEffect(Transform _pBoosterParent)
    {
        m_pBoosterParent = _pBoosterParent;
    }

    public void SetSceneName(ScenePrefab _pScenePrefab)
    {
        m_strPrevSceneName = m_strCurrentSceneName;

        m_strCurrentSceneName = _pScenePrefab.GetSceneName;

        m_pScenePrefab = _pScenePrefab;
    }

    public void SetActiveGameLineBox(bool _bIsSwitch)
    {
        foreach (BoxCollider2D _pBoxCollder in m_pSkyBoxCollliders)
            _pBoxCollder.gameObject.SetActive(_bIsSwitch);
    }

    public void SetPrevSceneName(string _strSceceName)
    {
        m_strPrevSceneName = _strSceceName;
    }

    public void ResetTileCycle()
    {
        m_iTileCycle = 0;
    }
    #endregion

#region 프로퍼티
    public CGameScene GetGameScene { get => m_pGameScene; }
    public Transform GetJellyParent { get => m_pJellyParent; }
    public Transform GetBoosterParent { get => m_pBoosterParent; }
    public Transform GetTileParent { get => m_pGameTileParent; }
    public Queue<string> GetTileNames { get => m_pSceneNames; }
    public List<CKeyValue<GameObject, bool>> GetTileList { get => m_pSceneTiles; }
    public CPoolManaged GetPoolManaged { get => m_pPoolManaged; }
    public CSceneExchange GetSceneExchange { get => m_pSceneExchange; }
    public string GetSceneName { get => m_strCurrentSceneName; }
    public string GetPrevSceneName { get => m_strPrevSceneName; }
    #endregion

#region 변수들 
    private CGameScene m_pGameScene = null;

    private CPoolManaged m_pPoolManaged = null;

    private ScenePrefab m_pScenePrefab = null;

    private CSceneExchange m_pSceneExchange = null;
    private CSceneDisplay m_pSceneDisplay = null;

    private List<CKeyValue<GameObject, bool>> m_pSceneTiles = null;

    private SpriteRenderer[] m_pSceneRenderer = null;

    private BoxCollider2D[] m_pSkyBoxCollliders = null;

    private BoxCollider2D m_pGameSceneBoxCollider = null;

    private Transform m_pGameTileParent = null;

    private Transform m_pJellyParent = null;

    private Transform m_pBoosterParent = null;

    private Vector3 m_vCreatePosition = new Vector3(20.6f, -4.34f);

    private Queue<string> m_pSceneNames = new Queue<string>();

    private string m_strPrevSceneName = "Stage 1";

    private string m_strCurrentSceneName = "Stage 1";

    private int m_iTileCycle = 0;

    private int m_iTileCycleScore = 5;

    private bool m_bIsRestart = false;

    private CSoundManaged m_pSoundManager = null;

#endregion
}