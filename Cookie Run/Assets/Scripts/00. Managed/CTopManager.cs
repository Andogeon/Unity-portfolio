using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Data;
using UnityEngine;
using UnityEngine.Networking;

public class CTopManager
{
    public static CTopManager GetInstance()
    {
        if (null == m_pInstance)
            m_pInstance = new CTopManager();

        return m_pInstance;
    }

    public void Initialize()
    {
        if (m_pResourceManaged != null)
            return;

        m_pResourceManaged = new CResourceManaged();
        m_pResourceManaged.Initialize();

        m_pPoolManaged = new CPoolManaged(m_pResourceManaged);
        m_pSceneManaged = new CSceneManaged(m_pPoolManaged);
    }

    public void SetSoundManager(CSoundManaged _pSoundManaged)
    {
        m_pSoundManaged = _pSoundManaged;
    }

    public CResourceManaged GetResourceManaged { get => m_pResourceManaged; }

    public CPoolManaged GetPoolManaged { get => m_pPoolManaged; }

    public CSceneManaged GetSceneManaged { get => m_pSceneManaged; }

    public CSoundManaged GetSoundManaged { get => m_pSoundManaged; }

    private CResourceManaged   m_pResourceManaged = null;
    private CSceneManaged      m_pSceneManaged = null;
    private CPoolManaged       m_pPoolManaged = null;
    private CSoundManaged      m_pSoundManaged = null;

    private static CTopManager m_pInstance = null;
}