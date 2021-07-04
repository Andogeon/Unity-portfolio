using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameobjectManager : Singleton<GameobjectManager>
{
    private Dictionary<string, GameObject> m_GameObjects = new Dictionary<string, GameObject>();

    private Dictionary<string, List<GameObject>> m_PoolingObjects = new Dictionary<string, List<GameObject>>();

    public void CreateobjectToInsert(string _GameObjectName, GameObject _Object, Vector3 _Position, Quaternion _Rotation, int _CreateCount)
    {
        GameObject _NewCreateObject = null;

        if (m_GameObjects.Count == 0) // 맵 컨테이너에 아무것도 없다면...
        {
            m_GameObjects.Add(_GameObjectName, _Object); // 생성한다..

            _NewCreateObject = _Object;
        }
        else // 맵 컨테이너가 1개 이상 존재한다면 ..
        {
            if (m_GameObjects.TryGetValue(_GameObjectName, out _NewCreateObject) == false)
            {
                m_GameObjects.Add(_GameObjectName, _Object); // 생성한다..

                _NewCreateObject = _Object;
            }
        }

        List<GameObject> m_GameObjectList = null;

        if (m_PoolingObjects.TryGetValue(_GameObjectName, out m_GameObjectList) == false)
        {
            m_GameObjectList = new List<GameObject>();

            m_PoolingObjects.Add(_GameObjectName, m_GameObjectList);
        }

        for (int i = 0; i < _CreateCount; ++i)
        {
            GameObject _CopyObject = GameObject.Instantiate(_NewCreateObject, _Position, _Rotation);

            _CopyObject.SetActive(false);

            m_GameObjectList.Add(_CopyObject);
        }
    }

    public void GameObjectActivation(string _GameObjectName, Vector3 _Position, float Angle = 0.0f)
    {
        List<GameObject> _ActivationList = null;

        if (m_PoolingObjects.TryGetValue(_GameObjectName, out _ActivationList) == false)
            return;

        for (int i = 0; i < _ActivationList.Count; ++i)
        {
            if (_ActivationList[i].activeSelf == false)
            {
                _ActivationList[i].transform.position = _Position;

                _ActivationList[i].SetActive(true);

                if (Angle != 0.0f)
                    _ActivationList[i].transform.eulerAngles = new Vector3(0.0f, Angle, 0.0f);

                return;
            }
        }

        GameObject _NewCreateObject = GameObject.Instantiate(_ActivationList[0], _Position, Quaternion.identity);

        _ActivationList.Add(_NewCreateObject);
    }

    public List<GameObject> GetDontInstacneGameObject(string _PoolingName)
    {
        List<GameObject> _ActivationList = null;

        if (m_PoolingObjects.TryGetValue(_PoolingName, out _ActivationList) == false)
            return null;

        return _ActivationList;
    }

    public GameObject DontInstacneObject(string _PoolingName)
    {
        List<GameObject> _ActivationList = null;

        if (m_PoolingObjects.TryGetValue(_PoolingName, out _ActivationList) == false)
            return null;

        for (int i = 0; i < _ActivationList.Count; ++i)
        {
            if (_ActivationList[i].activeSelf == false)
                return _ActivationList[i];
        }

        return null;
    }

    public List<GameObject> DontInstacneGameObject(string _PoolingName) //할당 문제가 발생하면 이 코드에서 할당해재 코드를 억지로 넣어라 아니면 머리 아프다..
    {
        List<GameObject> _ActivationList = null;

        if (m_PoolingObjects.TryGetValue(_PoolingName, out _ActivationList) == false)
            return null;

        List<GameObject> _DontActivationList = new List<GameObject>();

        for(int i = 0; i < _ActivationList.Count; ++i)
        {
            if (_ActivationList[i].activeSelf == false)
                _DontActivationList.Add(_ActivationList[i]);
        }

        return _DontActivationList;
    }

    public List<GameObject> GetInstacneGameObject(string _PoolingName)
    {
        List<GameObject> _ActivationList = null;

        if (m_PoolingObjects.TryGetValue(_PoolingName, out _ActivationList) == false)
            return null;

        List<GameObject> _DontActivationList = new List<GameObject>();

        for (int i = 0; i < _ActivationList.Count; ++i)
        {
            if (_ActivationList[i].activeSelf == true)
                _DontActivationList.Add(_ActivationList[i]);
        }

        return _DontActivationList;
    }

    public void GameObjectDisabled(string _GameObjectName, GameObject _Object) // 삭제 코드
    {
        if (null == _Object)
            return;

        List<GameObject> _ActivationList = null;

        if (m_PoolingObjects.TryGetValue(_GameObjectName, out _ActivationList) == false)
            return;

        _Object.SetActive(false);

        _ActivationList.Add(_Object);
    }
}