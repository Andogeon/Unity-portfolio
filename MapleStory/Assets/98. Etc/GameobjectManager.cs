using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 게임 오브젝트 풀링 매니저 클래스입니다.
public class GameobjectManager : Singleton<GameobjectManager>
{
    Dictionary<string, GameObject> m_pOriginalGameObject = new Dictionary<string, GameObject>(); // 원본 게임 오브젝트 객체

    Dictionary<string, List<GameObject>> m_pObjectPooling = new Dictionary<string, List<GameObject>>(); // 이름에 맞는 오브젝트 풀링

    public bool OriginalGamgObjectInsert(string _PoolingName, GameObject _OriginalGameObject)
    {
        if (_PoolingName == string.Empty || _OriginalGameObject == null)
            return false;

        GameObject _OutGameObject = null;

        if (m_pOriginalGameObject.TryGetValue(_PoolingName, out _OutGameObject) == true)
            return false;

        m_pOriginalGameObject.Add(_PoolingName, _OriginalGameObject);

        return true;
    }

    public GameObject GameObejctPooling(string _PoolingName, Vector3 _localPosition, Vector3 _WorldPosition, Quaternion _Rotation, Transform Parent = null)
    {
        if (_PoolingName == string.Empty) // 풀링할 이름이 존재 하지 않는다면 
            return null;

        GameObject _OriginalGameobejct = null; // 원본 객체

        if (m_pOriginalGameObject.TryGetValue(_PoolingName, out _OriginalGameobejct) == false) // 원본 객체를 찾앗는데 없다면 
            return null; // 나간다 

        List<GameObject> _PoolingGameobjectList = null;

        if (m_pObjectPooling.TryGetValue(_PoolingName, out _PoolingGameobjectList) == false) // 해당 풀링명에 풀링 그룹이 존재하지 않는다면 
        {
            GameObject _CopyGameObject = GameObject.Instantiate(_OriginalGameobejct);

            //// 크기 -> 자전 -> 이동 -> 공전 -> 부모 // 월드 스페이스 구성하는 순서 
            _CopyGameObject.transform.rotation = _Rotation;

            _CopyGameObject.transform.position = _WorldPosition;

            _PoolingGameobjectList = new List<GameObject>(); // 새로이 만듬 !! 

            _CopyGameObject.transform.SetParent(Parent);

            if (_CopyGameObject.transform.parent != null)
            {
                _CopyGameObject.transform.localPosition = _localPosition;

                _CopyGameObject.transform.localScale = new Vector3(1.0f, 1.0f);
            }

            _PoolingGameobjectList.Add(_CopyGameObject);

            m_pObjectPooling.Add(_PoolingName, _PoolingGameobjectList);

            return _CopyGameObject;
        }

        for(int i = 0; i < _PoolingGameobjectList.Count; ++i)
        {
            // 하나라도 널값이 존재한다면 ..

            if(null == _PoolingGameobjectList[i])
            {
                GameObject _Gameobject = GameObject.Instantiate(_OriginalGameobejct);

                //// 크기 -> 자전 -> 이동 -> 공전 -> 부모 // 월드 스페이스 구성하는 순서 
                _Gameobject.transform.rotation = _Rotation;

                _Gameobject.transform.position = _WorldPosition;

                _Gameobject.transform.SetParent(Parent);

                if (_Gameobject.transform.parent != null) // 이거 조정
                {
                    _Gameobject.transform.localPosition = _localPosition;

                    _Gameobject.transform.localScale = new Vector3(1.0f, 1.0f);
                }

                _PoolingGameobjectList.Insert(i , _Gameobject);

                return _Gameobject;
            }

            if (_PoolingGameobjectList[i].activeSelf == true)
                continue; // 혹시나 문제 생기면 여기서 손 볼것 !! 
            else
            {
                _PoolingGameobjectList[i].SetActive(true);

                _PoolingGameobjectList[i].transform.SetParent(Parent);

                if (_PoolingGameobjectList[i].transform.parent != null)
                    _PoolingGameobjectList[i].transform.localPosition = _localPosition;

                _PoolingGameobjectList[i].transform.rotation = _Rotation;

                return _PoolingGameobjectList[i];
            }
        }

        GameObject _CopyGameobject = GameObject.Instantiate(_OriginalGameobejct);

        //// 크기 -> 자전 -> 이동 -> 공전 -> 부모 // 월드 스페이스 구성하는 순서 
        _CopyGameobject.transform.rotation = _Rotation;

        _CopyGameobject.transform.position = _WorldPosition;

        _CopyGameobject.transform.SetParent(Parent);

        if (_CopyGameobject.transform.parent != null) // 이거 조정
        {
            _CopyGameobject.transform.localPosition = _localPosition;

            _CopyGameobject.transform.localScale = new Vector3(1.0f, 1.0f);
        }

        _PoolingGameobjectList.Add(_CopyGameobject);

        return _CopyGameobject;
    }

    public void Remove(string _PoolingName, GameObject _CopyObject)
    {
        if (_PoolingName == string.Empty || _CopyObject == null)
            return;

        List<GameObject> _PoolingGameobjectList = null;

        if (m_pObjectPooling.TryGetValue(_PoolingName, out _PoolingGameobjectList) == false) // 해당 풀링명에 풀링 그룹이 존재하지 않는다면 
            return;

        for (int i = 0; i < _PoolingGameobjectList.Count; ++i)
        {
            if (_PoolingGameobjectList[i] == _CopyObject)
            {
                _PoolingGameobjectList[i].SetActive(false);

                break;
            }
        }
    }
}
