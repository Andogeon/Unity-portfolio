using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENESTATE
{
    SCENE_FIELD,
    SCENE_TOWN
}

public class SceneManaged : MonoBehaviour
{
    [SerializeField] private SCENESTATE _SceneState = SCENESTATE.SCENE_FIELD;

    [SerializeField] private string _SceneName = string.Empty;

    [SerializeField] Fade _FadeObject = null;

    private void Awake()
    {
        List<GameObject> _DeleteObejcts = new List<GameObject>();

        GameObject _PlayerObject = GameObject.Find("Player");

        if (null == _PlayerObject)
        {
            _DeleteObejcts = null;

            return;
        }

        _DeleteObejcts.Add(_PlayerObject);

        GameObject _Inventory = GameObject.Find("Inventory UI");

        if (null == _Inventory)
        {
            _DeleteObejcts = null;

            return;
        }

        _DeleteObejcts.Add(_Inventory);

        GameObject _Quest = GameObject.Find("Quest UI");

        if (null == _Quest)
        {
            _DeleteObejcts = null;

            return;
        }

        _DeleteObejcts.Add(_Quest);

        GameObject _StateBar = GameObject.Find("State Bar");

        if (null == _StateBar)
        {
            _DeleteObejcts = null;

            return;
        }

        _DeleteObejcts.Add(_StateBar);


        GameObject _PlayerName = GameObject.Find("Player Name");

        if (null == _PlayerName)
        {
            _DeleteObejcts = null;

            return;
        }

        _DeleteObejcts.Add(_PlayerName);


        Scene _NewScene = SceneManager.GetActiveScene();

        OverlapObject(_NewScene, ref _DeleteObejcts); // 중복 여부 확인 

        Player _ScenePlayer = _PlayerObject.GetComponent<Player>();

        if (null == _ScenePlayer)
            return;

        if (_SceneState == SCENESTATE.SCENE_TOWN)
        {
            _ScenePlayer.AccessSceneName = _SceneName;

            _ScenePlayer.AccessResetPosition = transform.position;
        }

        _ScenePlayer.AccessSceneManaged = this;

        PORTALTYPE _PortalType = _ScenePlayer.AccessPotal;

        GameObject[] _SceneGameObejcts =  _NewScene.GetRootGameObjects();

        if (null == _SceneGameObejcts)
            return;

        List<Potal> _SelectPortalObjects = new List<Potal>();

        for(int i = 0; i < _SceneGameObejcts.Length; ++i)
        {
            if (_SceneGameObejcts[i].name == "Portal")
                _SelectPortalObjects.Add(_SceneGameObejcts[i].GetComponent<Potal>());
        }

        if (_SelectPortalObjects.Count == 0)
            return;

        for(int i = 0; i < _SelectPortalObjects.Count; ++i)
        {
            // 여기네 ㅡㅡ

            if(_PortalType == PORTALTYPE.POTAL_FIRST && _SelectPortalObjects[i].GetPortalType == PORTALTYPE.POTAL_END)
            {
                _PlayerObject.transform.position = _SelectPortalObjects[i].transform.GetChild(0).transform.position;

                break;
            }
            else if(_PortalType == PORTALTYPE.POTAL_END && _SelectPortalObjects[i].GetPortalType == PORTALTYPE.POTAL_FIRST)
            {
                _PlayerObject.transform.position = _SelectPortalObjects[i].transform.GetChild(0).transform.position;

                break;
            }
        }

        GameObject _Camera = GameObject.Find("Main Camera");

        if (null == _Camera)
        {
            _DeleteObejcts = null;

            return;
        }

        FollowCamera _FollowCamera = _Camera.GetComponent<FollowCamera>();

        _FollowCamera.transform.position = _PlayerObject.transform.position;
    }

    private void OverlapObject(Scene _NewScene, ref List<GameObject> _GameobjectList)
    {
        GameObject[] _GameObjects = _NewScene.GetRootGameObjects(); // 해당 씬에서 목록 확인 

        List<GameObject> _Overlaps = new List<GameObject>();

        if (null != _GameObjects)
        {
            for (int i = 0; i < _GameObjects.Length; ++i)
            {
                if (_GameObjects[i].name == "Player")
                {
                    if (_GameObjects[i] != _GameobjectList[0])
                        _Overlaps.Add(_GameObjects[i]);
                }
                if (_GameObjects[i].name == "Inventory UI")
                {
                    if (_GameObjects[i] != _GameobjectList[1])
                        _Overlaps.Add(_GameObjects[i]);
                }
                if (_GameObjects[i].name == "Quest UI")
                {
                    if (_GameObjects[i] != _GameobjectList[2])
                        _Overlaps.Add(_GameObjects[i]);
                }
                if (_GameObjects[i].name == "State Bar")
                {
                    if (_GameObjects[i] != _GameobjectList[3])
                        _Overlaps.Add(_GameObjects[i]);
                }
                if (_GameObjects[i].name == "Player Name")
                {
                    if (_GameObjects[i] != _GameobjectList[4])
                        _Overlaps.Add(_GameObjects[i]);
                }
            }
        }

        if (_Overlaps.Count != 0)
        {
            for (int i = 0; i < _Overlaps.Count; ++i)
            {
                Destroy(_Overlaps[i]);

                _GameobjectList[i] = null;
            }

            _GameobjectList.Clear();

            _GameobjectList = null;
        }
    }

    public void FadeOut()
    {
        if (null == _FadeObject)
            return;

        _FadeObject.AccessFaceType = FADETYPE.FADE_IN;
    }
}
