using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Junior_StoneBall_Item : ITEM
{
    //private int m_iPortionCount = 1;

    public override bool AddCount()
    {
        if (m_iPortionCount >= 10)
            return false;

        m_iPortionCount += 1;

        return true;
    }
}
