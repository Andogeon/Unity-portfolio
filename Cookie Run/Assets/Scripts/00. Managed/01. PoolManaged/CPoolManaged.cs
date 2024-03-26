using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;


// -----------------------------------
// 오브젝트 풀링 관리하기 위한 클래스 
// -----------------------------------
public class CPoolManaged
{
    public CPoolManaged(CResourceManaged _pResourceManaged)
    {
        m_pPoolobjects = new Dictionary<string, List<GameObject>>();

        m_pResourceManaged = _pResourceManaged;
    }

    public GameObject pop(string _strPoolname, Vector3 _vPosition, Transform _pParentTransform = null)
    {
        if(!m_pPoolobjects.ContainsKey(_strPoolname))
        {
            GameObject _pOrlGameobject = m_pResourceManaged.GetResoucre<GameObject>(_strPoolname);

            if (null == _pOrlGameobject)
                return null;

            List<GameObject> _pCloneList = new List<GameObject>();

            GameObject _pCloneGameobject = CGlobalFunc.CreateClone(_pOrlGameobject, _vPosition, _pParentTransform);
            _pCloneGameobject.name = _pCloneGameobject.name.Replace("(Clone)", "");

            _pCloneList.Add(_pCloneGameobject);

            m_pPoolobjects.Add(_strPoolname, _pCloneList);

            return _pCloneGameobject;
        }

        GameObject _pUseGameobject = SearchUseGameobject(m_pPoolobjects[_strPoolname]);

        if (_pUseGameobject == null)
        {
            GameObject _pOrlGameobject = m_pResourceManaged.GetResoucre<GameObject>(_strPoolname);

            List<GameObject> _pCloneList = m_pPoolobjects[_strPoolname];

            GameObject _pCloneGameobject = CGlobalFunc.CreateClone(_pOrlGameobject, _vPosition, _pParentTransform);

            _pCloneList.Add(_pCloneGameobject);

            _pUseGameobject = _pCloneGameobject;
        }
        else
        {
            _pUseGameobject.transform.SetParent(_pParentTransform);
            
            _pUseGameobject.transform.localPosition = _vPosition;
            
            _pUseGameobject.SetActive(true);
        }

        _pUseGameobject.name = _pUseGameobject.name.Replace("(Clone)", "");

        return _pUseGameobject;
    }

    private  GameObject SearchUseGameobject(List<GameObject> _pSeacthGameobjects)
    {
        if (null == _pSeacthGameobjects)
            return null;

        GameObject _pUseGameobject = null;

        for(int i = 0; i < _pSeacthGameobjects.Count; ++i)
        {
            if(_pSeacthGameobjects[i] == null)
            {
                _pSeacthGameobjects.Remove(_pSeacthGameobjects[i]);

                continue;
            }
            else
            {
                if (_pSeacthGameobjects[i].activeSelf)
                    continue;

                _pUseGameobject = _pSeacthGameobjects[i];

                break;
            }
        }

        return _pUseGameobject;
    }

    public CResourceManaged GetResourceManaged { get => m_pResourceManaged; }

    private CResourceManaged m_pResourceManaged = null;

    private Dictionary<string, List<GameObject>> m_pPoolobjects = null;
}

