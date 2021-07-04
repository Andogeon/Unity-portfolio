using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MONTER : MonoBehaviour
{
    [SerializeField] protected float _Hp = 0.0f;

    protected string m_strMonterName = string.Empty;

    protected float m_fAttack = 0.0f;

    //protected int m_iCount = 3;

    public float AccessAttack
    {
        get { return m_fAttack; }
    }

    public string AccessMonterName
    {
        get { return m_strMonterName; }

        set { m_strMonterName = value; }
    }

    public float AccessHp
    {
        get { return _Hp; }

        set { _Hp = value; }
    }

    public virtual SpriteRenderer GetMonterSpriteRenderer()
    {
        return null;
    }

    public virtual void SetHItAnimation()
    {
        return;
    }

    //public int AccessHitCount
    //{
    //    get { return m_iCount; }

    //    set { m_iCount = value; }
    //}

    public static int m_iCollisionCount = 0;
}
