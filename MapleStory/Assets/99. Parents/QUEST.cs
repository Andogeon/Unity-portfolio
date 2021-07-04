using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum QUESTTYPE
{
    QUEST_ITEM,
    QUEST_MESSAGE,
    QUEST_MONTER
}

public abstract class QUEST : MonoBehaviour
{
    [SerializeField] protected string m_strQuestSlotName = string.Empty;

    [SerializeField] protected string m_strQuestContents = string.Empty;

    [SerializeField] protected int _Exp = 0;

    [SerializeField] protected int _Count = 0;

    [SerializeField] protected Sprite _QuestNpcSprite = null;

    protected QUESTTYPE m_eQuestType = QUESTTYPE.QUEST_ITEM;

    protected int m_iCount = 0;

    protected bool m_bIsStart = false;

    protected bool m_bIsComple = false;

    protected bool m_bIsFinal = false;

    public bool AccessQuestStart
    {
        get { return m_bIsStart; }

        set { m_bIsStart = value; }
    }

    public string AccessQuestSlotName
    {
        get { return m_strQuestSlotName; }

        set { m_strQuestSlotName = value; }
    }

    public string AccessQuestContents
    {
        get { return m_strQuestContents; }

        set { m_strQuestContents = value; }
    }

    public QUESTTYPE AccessQuestType
    {
        get { return m_eQuestType; }

        set { m_eQuestType = value; }
    }

    public bool AccessQuestFinal
    {
        get { return m_bIsFinal; }

        set { m_bIsFinal = value; }
    }

    public bool AccessQuestComple
    {
        get { return m_bIsComple; }

        set { m_bIsComple = value; }
    }

    public int AccessQuestItemCount
    {
        get { return m_iCount; }
    }

    public int AccessQuestCount
    {
        get { return _Count; }
    }

    public int AccessQuestExp
    {
        get { return _Exp; }
    }

    public Sprite AccessQuestNPCSprite
    {
        get { return _QuestNpcSprite; }

        set { _QuestNpcSprite = value; }
    }

    public virtual void QuestUpdate()
    {
        return;
    }
}




//[SerializeField] protected string m_strQuestSlotName = string.Empty;

//[SerializeField] protected int _Count = 0;

//protected QUESTTYPE m_eQuestType = QUESTTYPE.QUEST_ITEM;

//protected int m_iCount = 0;

//protected bool m_bIsStart = false;

//protected bool m_bIsComple = false;

//protected bool m_bIsFinal = false;

//public bool AccessQuestStart
//{
//    get { return m_bIsStart; }

//    set { m_bIsStart = value; }
//}

//public string AccessQuestSlotName
//{
//    get { return m_strQuestSlotName; }

//    set { m_strQuestSlotName = value; }
//}

//public QUESTTYPE AccessQuestType
//{
//    get { return m_eQuestType; }

//    set { m_eQuestType = value; }
//}

//public bool AccessQuestFinal
//{
//    get { return m_bIsFinal; }

//    set { m_bIsFinal = value; }
//}

//public bool AccessQuestComple
//{
//    get { return m_bIsComple; }

//    set { m_bIsComple = value; }
//}

//public int AccessQuestItemCount
//{
//    get { return m_iCount; }
//}

//public int AccessQuestCount
//{
//    get { return _Count; }
//}

//public virtual void QuestUpdate()
//{
//    return;
//}
//}
