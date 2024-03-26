using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ScenePrefabGroup", menuName = "ScenePrefabGroup", order = 0)]
public class ScenePrefab : ScriptableObject
{
    [SerializeField] private Sprite[] m_SceneBackGrounds = null;

    [SerializeField] private GameObject[] m_SceneTiles = null;

    [SerializeField] private GameObject[] m_SceneBounsTiles = null;

    [SerializeField] private string m_strSceneName = string.Empty;

    #region 프로퍼티
    public GameObject[] GetSceneTiles { get => m_SceneTiles; }
    public GameObject[] GetBounsSceneTiles { get => m_SceneBounsTiles; }
    public Sprite[] GetSceneSprite { get => m_SceneBackGrounds; }
    public string GetSceneName { get => m_strSceneName; }
    #endregion
}
