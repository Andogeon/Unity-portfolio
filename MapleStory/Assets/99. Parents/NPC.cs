using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class NPC : MonoBehaviour
{
    [SerializeField] protected string[] _Message = null; // 추후에 상속할 것 !! 

    [SerializeField] protected QUEST _Quest = null;

    [SerializeField] protected SpriteRenderer _NameSpriteRenderer = null;

    [SerializeField] protected SpriteRenderer _NameBarSpriteRenderer = null;

    //[SerializeField] static protected QUEST _Quest = null;

    protected bool m_bIsQuestStart = false;

    protected int m_iMessageIndex = 0;

    protected MessageBox m_pMessageBar = null;

    protected SpriteRenderer m_pSpriteRenderer = null;

    //protected Sprite m_pNameSprite = null;

    //protected Sprite m_pNameBarSprite = null;

    protected bool m_bIsQusetMode = false;

    protected private bool m_bIsOnClick = false;

    protected NPC m_pNextNpc = null;

    

    public int AccessMessageIndex
    {
        get { return m_iMessageIndex; }

        set { m_iMessageIndex = value; }
    }

    public MessageBox AccessMessageBox
    {
        get { return m_pMessageBar; }

        set { m_pMessageBar = value; }
    }

    public string[] AccessMessage
    {
        get { return _Message; }
    }

    public QUEST AccessQuest
    {
        get { return _Quest; }

        set { _Quest = value; }
    }

    public bool AccessQuestStart
    {
        get { return m_bIsQuestStart; }

        set { m_bIsQuestStart = value; }
    }

    public SpriteRenderer AccessSprireRenderer
    {
        get { return m_pSpriteRenderer; }

        set { m_pSpriteRenderer = value; }
    }

    //public Sprite AccessNameBarSprite
    //{
    //    get { return m_pNameBarSprite; }

    //    set { m_pNameBarSprite = value; }
    //}

    //public Sprite AccessNameSprite
    //{
    //    get { return m_pNameSprite; }

    //    set { m_pNameSprite = value; }
    //}

    public bool AccessQuestMode
    {
        get { return m_bIsQusetMode; }

        set { m_bIsQusetMode = value; }
    }

    public bool AccessQuestCilck
    {
        get { return m_bIsOnClick; }

        set { m_bIsOnClick = value; }
    }

    public NPC AccessNextNPC
    {
        get { return m_pNextNpc; }

        set { m_pNextNpc = value; }
    }

    public virtual void ResetButton()
    {
        return;
    }
}







//public abstract class NPC : MonoBehaviour
//{
//    [SerializeField] protected string[] _Message = null; // 추후에 상속할 것 !! 

//    [SerializeField] protected QUEST _Quest = null;

//    protected bool m_bIsQuestStart = false;

//    protected int m_iMessageIndex = 0;

//    protected MessageBox m_pMessageBar = null;

//    protected SpriteRenderer m_pSpriteRenderer = null;

//    protected bool m_bIsQusetMode = false;

//    protected private bool m_bIsOnClick = false;

//    public int AccessMessageIndex
//    {
//        get { return m_iMessageIndex; }

//        set { m_iMessageIndex = value; }
//    }

//    public MessageBox AccessMessageBox
//    {
//        get { return m_pMessageBar; }

//        set { m_pMessageBar = value; }
//    }

//    public string[] AccessMessage
//    {
//        get { return _Message; }
//    }

//    public QUEST AccessQuest
//    {
//        get { return _Quest; }

//        set { _Quest = value; }
//    }

//    public bool AccessQuestStart
//    {
//        get { return m_bIsQuestStart; }

//        set { m_bIsQuestStart = value; }
//    }

//    public SpriteRenderer AccessSprireRenderer
//    {
//        get { return m_pSpriteRenderer; }

//        set { m_pSpriteRenderer = value; }
//    }

//    public bool AccessQuestMode
//    {
//        get { return m_bIsQusetMode; }

//        set { m_bIsQusetMode = value; }
//    }

//    public bool AccessQuestCilck
//    {
//        get { return m_bIsOnClick; }

//        set { m_bIsOnClick = value; }
//    }

//    public virtual void ResetButton()
//    {
//        return;
//    }
//}