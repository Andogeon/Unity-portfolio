using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP_Portion : ITEM
{
    [SerializeField] private float m_fRecovery = 0.0f;

    public override void UsePotion(Player _Player, ICON _CloneICon)
    {
        //m_iPortionCount -= 1;

        _CloneICon.AccessIconCount -= 1;

        if (_Player.AccessPlayerHP < _Player.AccessPlayerMaxHP)
        {
            _Player.AccessPlayerHP += m_fRecovery;

            if (_Player.AccessPlayerHP > _Player.AccessPlayerMaxHP)
                _Player.AccessPlayerHP = _Player.AccessPlayerMaxHP;
        }

        if (_CloneICon.AccessIconCount <= 0)
        {
            // 정렬한다 !! 

            m_pInventoryIcon.AccessMySlot.AccessICon = null;

            Destroy(m_pInventoryIcon.gameObject);

            return;
        }
    }

    public override bool AddCount()
    {
        if (m_iPortionCount >= 10)
            return false;

        m_iPortionCount += 1;

        return true;
    }
}

//public override void UsePotion(Player _Player, ICON _CloneICon)
//{
//    if (null == _CloneICon)
//        return;

//    m_iPortionCount -= 1;

//    // 회복한다..

//    if (m_iPortionCount <= 0)
//        Destroy(_CloneICon.gameObject);
//}
