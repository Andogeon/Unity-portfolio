using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ITEMMODETYPE
{
    ITEM_INVENTORY,
    ITEM_EQUIP,
    ITEM_SELETEMODE
}

public enum ITEMTYPE
{
    ITEM_STICK,
    ITEM_SWORD,
}

public class ITEM : PART
{
    // 아이템 타입을 지정하고 거기로 보내주어야 한다 !! 

    [SerializeField] protected AnimationSprites[] _Animations = null;

    [SerializeField] protected ICON _OrlIcon = null;

    protected ICON m_pInventoryIcon = null;

    protected int m_iPortionCount = 1;

    protected float m_fOldAttack = 0;

    protected float m_fAttack = 0;

    protected SpriteRenderer m_pPartSpriteRenderer = null;

    protected AVATARSTATES m_eAvatarState = AVATARSTATES.AVATAR_IDLE;

    protected ITEMMODETYPE m_eItemModeType = ITEMMODETYPE.ITEM_EQUIP;

    protected ITEMTYPE m_eItemType = ITEMTYPE.ITEM_STICK;

    public SpriteRenderer SpriteRenderer
    {
        get { return m_pPartSpriteRenderer; }

        set { m_pPartSpriteRenderer = value; }
    }

    public AVATARSTATES AvatarState
    {
        get { return m_eAvatarState; }

        set { m_eAvatarState = value; }
    }

    public ITEMMODETYPE AccessItemModeType
    {
        get { return m_eItemModeType; }

        set { m_eItemModeType = value; }
    }

    public ITEMTYPE AccessItemType
    {
        get { return m_eItemType; }
    }

    public ICON AccessIcon
    {
        get 
        {
            if(null == m_pInventoryIcon)
                return _OrlIcon;

            return m_pInventoryIcon; 
        }

        set { m_pInventoryIcon = value; }
    }

    public int AccessItemCount
    {
        get { return m_iPortionCount; }

        set { m_iPortionCount = value; }
    }

    public float AccessAttack
    {
        get { return m_fAttack; }

        set { m_fAttack = value; }
    }

    public override Vector3 IDLE()
    {
        return Vector3.zero;
    }

    public override Vector3 RUN()
    {
        return Vector3.zero;
    }

    public override Sprite GetSprite()
    {
        return null;
    }

    public virtual void UsePotion(Player _Player, ICON _CloneICon)
    {
        return;
    }

    public virtual void UsePotion(Player _Player)
    {
        return;
    }

    public virtual void UsePotion(Player _Player, ref int _iCount)
    {
        return;
    }

    public virtual void UsePotion()
    {
        return;
    }

    public virtual bool AddCount()
    {
        return false;
    }
}
