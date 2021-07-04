using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monter : MonoBehaviour
{
    protected Transform[] m_pPositions = null;

    public Transform[] SetPositions
    {
        set { m_pPositions = value; }
    }
}