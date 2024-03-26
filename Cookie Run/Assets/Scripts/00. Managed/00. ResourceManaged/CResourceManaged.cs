using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Networking;

//--------------------------------------------------
// 원본 프리팹 게임 오브젝트를 불러오기 위한 클래스 
//--------------------------------------------------
public class CResourceManaged
{
    public CResourceManaged()
    {
        m_pResoucres = new Dictionary<string, object>();
    }

    public void Initialize()
    {
        InsertEffects();

        object _pStageGroup = Resources.Load("Prefab/00. Game Scene/Stage 1");
        m_pResoucres.Add("Stage 1", _pStageGroup);
        
        object _pBounsGroup = Resources.Load("Prefab/00. Game Scene/Bonus Stage");
        m_pResoucres.Add("Bonus Stage", _pBounsGroup);

        InsertJelly();
    }

    public void Insert(string _strResoucreName, GameObject _pOrlGameobject)
    {
        if (m_pResoucres.ContainsKey(_strResoucreName))
            return;

        m_pResoucres.Add(_strResoucreName, _pOrlGameobject);
    }

    public void Insert<T>(string _strResoucreName, T _pOrlGameobject)
    {
        if (m_pResoucres.ContainsKey(_strResoucreName))
            return;

        m_pResoucres.Add(_strResoucreName, _pOrlGameobject);
    }

    public T GetResoucre<T>(string _strResoucreName)
    {
        if (!m_pResoucres.ContainsKey(_strResoucreName))
            return default;

        return (T)m_pResoucres[_strResoucreName];
    }

    private void InsertEffects()
    {
        GameObject _pEffect = Resources.Load<GameObject>("Prefab/05. Effect/01. Skill Effect/Booster Effect");
        m_pResoucres.Add("Booster", _pEffect);

        string[] _strEffectName = { "Jelly Effect", "Pink Jelly Effect", "Purple Jelly Effect", "Red Jelly Effect", "Twinkle Effect", "Big Bear Effect", "Yellow Effect" };

        for (int i = 0; i < _strEffectName.Length; ++i)
        {
            string _strFilePath = "Prefab/05. Effect/00. Jelly Get Effect/" + _strEffectName[i];
            object _pEffectObject = Resources.Load(_strFilePath);
            m_pResoucres.Add(_strEffectName[i], _pEffectObject);
        }

        _pEffect = Resources.Load<GameObject>("Prefab/05. Effect/03. Text Effect/Booster Text Effect");
        m_pResoucres.Add("Booster Text Effect", _pEffect);

        _pEffect = Resources.Load<GameObject>("Prefab/05. Effect/03. Text Effect/Bouns Text Effect");
        m_pResoucres.Add("Bouns Text Effect", _pEffect);

        _pEffect = Resources.Load<GameObject>("Prefab/05. Effect/03. Text Effect/Power Up Text Effect");
        m_pResoucres.Add("Power Up Text Effect", _pEffect);

        _pEffect = Resources.Load<GameObject>("Prefab/05. Effect/03. Text Effect/Hp Text Effect");
        m_pResoucres.Add("Hp Text Effect", _pEffect);

        _pEffect = Resources.Load<GameObject>("Prefab/05. Effect/03. Text Effect/Magnet Text Effect");
        m_pResoucres.Add("Magnet Text Effect", _pEffect);

        _pEffect = Resources.Load<GameObject>("Prefab/05. Effect/03. Text Effect/Bear Party Text Effect");
        m_pResoucres.Add("Bear Party Text Effect", _pEffect);

        _pEffect = Resources.Load<GameObject>("Prefab/05. Effect/04. Jelly Change/Jelly Change Effect");
        m_pResoucres.Add("Jelly Change Effect", _pEffect);
    }

    private void InsertJelly()
    {
        string[] _strEffectName = { "Nomral Bear Jelly", "Pink Bear Jelly", "Pink Wing Bear Jelly", "Light Blue Bear Jelly", "Rainbow Bear Jelly", "Coin", "Gold Coin" };

        GameObject _pJelly = null;

        for (int i = 0; i < _strEffectName.Length; ++i)
        {
            string _strFilePath = "Prefab/04. Game Jelly/" + _strEffectName[i];
            _pJelly = Resources.Load<GameObject>(_strFilePath);
            m_pResoucres.Add(_strEffectName[i], _pJelly);
        }
    }

    //private void InsertSound()
    //{
    //    string _strFilePath = "Sound/BGM/MainBGM";
    //    AudioClip _pSound = Resources.Load<AudioClip>(_strFilePath);
    //    m_pResoucres.Add("MainBGM", _pSound);

    //    _strFilePath = "Sound/BGM/GameScene";
    //    _pSound = Resources.Load<AudioClip>(_strFilePath);
    //    m_pResoucres.Add("GameScene", _pSound);

    //    _strFilePath = "Sound/Etc/JellyGetSound";
    //    _pSound = Resources.Load<AudioClip>(_strFilePath);
    //    m_pResoucres.Add("GetJelly", _pSound);

    //    _strFilePath = "Sound/Etc/GetCoinJelly";
    //    _pSound = Resources.Load<AudioClip>(_strFilePath);
    //    m_pResoucres.Add("GetCoin", _pSound);

    //    _strFilePath = "Sound/Etc/GameResult";
    //    _pSound = Resources.Load<AudioClip>(_strFilePath);
    //    m_pResoucres.Add("GameResult", _pSound);

    //    _strFilePath = "Sound/Etc/GameEnd";
    //    _pSound = Resources.Load<AudioClip>(_strFilePath);
    //    m_pResoucres.Add("GameEnd", _pSound);
    //}

    private Dictionary<string, object> m_pResoucres = null;
}