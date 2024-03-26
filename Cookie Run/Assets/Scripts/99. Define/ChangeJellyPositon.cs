using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ChangeJellyPositon : MonoBehaviour
{
    [SerializeField] private Transform[] m_lhsJellys = null;

    [SerializeField] private Transform[] m_rhsJellys = null;

    // Update is called once per frame
    void Update()
    {
        if (null == m_lhsJellys || null == m_rhsJellys)
            return;

        if (m_lhsJellys.Length != m_rhsJellys.Length)
            return;

        for (int i = 0; i < m_lhsJellys.Length; ++i)
            m_rhsJellys[i].localPosition = m_lhsJellys[i].localPosition;
    }
}
