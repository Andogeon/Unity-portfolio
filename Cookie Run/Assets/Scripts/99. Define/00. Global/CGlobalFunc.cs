using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using UnityEngine;
using UnityEditor;
using UnityEngine.Networking;


public static class CGlobalFunc
{
    public static GameObject CreateClone(GameObject _pOrlGameobject, Vector2 _Position = default, Transform _pParentTransform = null)
    {
        GameObject _pCloneGameobject = GameObject.Instantiate(_pOrlGameobject, _pParentTransform);
        _pCloneGameobject.transform.localScale = _pOrlGameobject.transform.localScale;
        _pCloneGameobject.transform.localPosition = _Position;

        return _pCloneGameobject;
    }

    public static void GameEditorPause(bool _bPause)
    {
#if UNITY_EDITOR
        EditorApplication.isPaused = _bPause;
#endif
    }
}

