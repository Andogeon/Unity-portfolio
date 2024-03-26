using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CSceneDisplay
{
    public CSceneDisplay(CSceneManaged _pSceneManaged)
    {
        m_pSceneManaged = _pSceneManaged;

        m_pSceneExchange = m_pSceneManaged.GetSceneExchange;
    }

    public void ResetGameScene()
    {
        CResourceManaged _pResourceManaged = m_pSceneManaged.GetPoolManaged.GetResourceManaged;
        ScenePrefab _pScenePrefab = _pResourceManaged.GetResoucre<ScenePrefab>("Stage 1");
        m_pSceneExchange.SetGameScene(_pScenePrefab);
        m_pSceneManaged.SetSceneName(_pScenePrefab);
        m_pSceneManaged.SetPrevSceneName(string.Empty);
        m_pSceneManaged.ResetTileCycle();
        m_pSceneManaged.GetGameScene.ResetSpeed();

        CGlobalValues.m_bIsGameOver = false;
    }

    private CSceneManaged m_pSceneManaged = null;
    private CSceneExchange m_pSceneExchange = null;
}

