using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Giftbox : ITEM
{
    public override bool AddCount()
    {
        if (m_iPortionCount >= 10)
            return false;

        m_iPortionCount += 1;

        return true;
    }
}
