using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBody : PART
{
    private void Awake()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    public override Vector3 IDLE()
    {
        return Vector3.zero;
    }

    public override Vector3 RUN()
    {
        return Vector3.zero;
    }

    public override void SetItem(ITEM _Item)
    {
        if (null == _Item)
            return;

        if (m_pOrlItem == null || m_pOrlItem.name != _Item.name)
        {
            GameObject _pCreateObject = GameObject.Instantiate(_Item.gameObject);

            _pCreateObject.transform.SetParent(gameObject.transform);

            _pCreateObject.transform.localScale = new Vector3(1.0f, 1.0f);

            _pCreateObject.transform.localRotation = Quaternion.identity;

            _pCreateObject.transform.localPosition = new Vector3(0.0f, 0.0f);

            ITEM _pItem = _pCreateObject.GetComponent<ITEM>();

            if (m_pItemobject != null)
            {
                m_pItemobject.transform.SetParent(null);

                Destroy(m_pItemobject);

                m_pItemobject = null;

                m_pItem = null;
            }

            _pItem.AccessSetItem = _Item;

            m_pItemobject = _pCreateObject;

            m_pItem = _pItem;

            m_pOrlItem = _Item;
        }
    }
}
