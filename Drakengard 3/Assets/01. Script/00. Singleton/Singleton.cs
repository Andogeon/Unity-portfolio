using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> where T : class
{
    public static T GetInstance()
    {
        if (null == m_pInstance)
            m_pInstance = System.Activator.CreateInstance(typeof(T)) as T;

        return m_pInstance;
    }

    private static T m_pInstance;
}