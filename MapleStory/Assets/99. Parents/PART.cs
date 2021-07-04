using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PART : MonoBehaviour
{
    protected GameObject m_pItemobject = null;

    protected Sprite m_pSprite = null;

    protected SpriteRenderer m_pSpriteRenderer = null;

    protected ITEM m_pOrlItem = null;

    protected ITEM m_pItem = null;

    public abstract Vector3 IDLE();
    public abstract Vector3 RUN();

    public virtual void Jump()
    {
        return;
    }

    public ITEM AccessSetItem
    {
        set { m_pOrlItem = value; }
    }

    public ITEM AccessItem
    {
        get { return m_pOrlItem; }

        set 
        {
            m_pOrlItem = value;

            SettingItem();
        }
    }

    public GameObject AccessEquipItemObject
    {
        get { return m_pItemobject; }

        set { m_pItemobject = value; }
    }

    public virtual Sprite GetSprite()
    {
        return null;
    }

    public virtual void SetSprite(Sprite _SettingSprite)
    {
        if (null == _SettingSprite)
            return;

        if (m_pSpriteRenderer == null)
            m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pSpriteRenderer.sprite = _SettingSprite;
    }

    public virtual void NormalAttack()
    {

    }

    public virtual void SetItem(ITEM _Item)
    {

    }

    private void SettingItem() // 이거 수정 봐야 되네 ;;
    {
        if (null == m_pOrlItem)
            return;

        m_pOrlItem.transform.SetParent(gameObject.transform);

        m_pOrlItem.transform.localScale = new Vector3(1.0f, 1.0f);

        m_pOrlItem.transform.localRotation = Quaternion.identity;

        m_pOrlItem.transform.localPosition = new Vector3(0.0f, 0.0f);

        m_pItem = m_pOrlItem;

        m_pItemobject = m_pOrlItem.gameObject;

        //GameObject _pCreateObject = GameObject.Instantiate(m_pOrlItem.gameObject);

        //_pCreateObject.transform.SetParent(gameObject.transform);

        //_pCreateObject.transform.localScale = new Vector3(1.0f, 1.0f);

        //_pCreateObject.transform.localRotation = Quaternion.identity;

        //_pCreateObject.transform.localPosition = new Vector3(0.0f, 0.0f);

        //m_pItem = _pCreateObject.GetComponent<ITEM>();

        //m_pItemobject = _pCreateObject;
    }

    public void DestoryItem()
    {
        m_pItem = null;

        Destroy(m_pItemobject);

        m_pItemobject = null;
    }
}
