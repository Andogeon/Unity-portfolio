using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageButton : MonoBehaviour
{
    //[SerializeField] private AudioClip _ClickSound = null;

    private UnityEngine.UI.Button m_pButton = null;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private QuestList m_pQuestUI = null;

    private NPC m_pNPC = null;

    private Player m_pPlayer = null;

    private UnityEngine.UI.Button.ButtonClickedEvent m_pClickEvent = null;

    private UnityEngine.UI.Button.ButtonClickedEvent m_pOldClickEvent = null;

    public NPC AccessNpc
    {
        set { m_pNPC = value; }
    }

    private void Start()
    {
        GameObject _GameObject = GameObject.Find("Quest UI");

        if (_GameObject != null)
        {
            _GameObject = _GameObject.transform.Find("Quest Window Box").gameObject;

            m_pQuestUI = _GameObject.transform.Find("Quest List").GetComponent<QuestList>();

            m_pPlayer = GameObject.Find("Player").GetComponent<Player>();
        }

        m_pButton = GetComponent<UnityEngine.UI.Button>();

        m_pClickEvent = new UnityEngine.UI.Button.ButtonClickedEvent();

        m_pClickEvent.AddListener(CompleButton);

        m_pOldClickEvent = m_pButton.onClick;

        //m_pSoundManager.AddSound("Click Sound", _ClickSound, SoundType.Sound_Effect);

        // 현재 온 클릭이 여부를 확인해야 하는데 ??
    }

    public void CloseMessageBotton()
    {
        GameObject _BarObject = transform.parent.gameObject;

        if (null == _BarObject)
            return;

        _BarObject.transform.SetParent(null);

        MessageBox _MessageBox = _BarObject.GetComponent<MessageBox>();

        _MessageBox.ClearText();

        _MessageBox.AccessNpc.AccessMessageIndex = 0;

        _MessageBox.AccessNpc.AccessMessageBox = null;

        _MessageBox.AccessNpc = null;

        m_pNPC = null;

        m_pSoundManager.PlaySound("Click Sound");

        m_pGameObjectManager.Remove("Message Bar", _BarObject);
    }

    public void NextButton()
    {
        //if (m_pNPC.AccessMessage.Length - 1 >= m_pNPC.AccessMessageIndex)
        //    m_pNPC.AccessMessageIndex += 1;

        if (m_pNPC.AccessMessage.Length - 1 > m_pNPC.AccessMessageIndex)
            m_pNPC.AccessMessageIndex += 1;

        m_pNPC.AccessMessageBox.AccessIndex = 0.0f;

        m_pSoundManager.PlaySound("Click Sound");
    }

    public void PrevButton()
    {
        if (0 < m_pNPC.AccessMessageIndex)
            m_pNPC.AccessMessageIndex -= 1;

        m_pNPC.AccessMessageBox.AccessIndex = 0.0f;

        m_pSoundManager.PlaySound("Click Sound");
    }

    public void YesButton() // ?? 여기서 문제인데 ??
    {
        if (null == m_pNPC || null == m_pNPC.AccessQuest)
            return;

        if (m_pNPC.AccessQuest.AccessQuestType == QUESTTYPE.QUEST_MESSAGE)
        {
            if (m_pNPC.AccessNextNPC != null)
            {
                m_pNPC.AccessNextNPC.AccessQuest = m_pNPC.AccessQuest;

                m_pNPC.AccessNextNPC.AccessQuest.AccessQuestComple = true;
            }
        }

        m_pSoundManager.PlaySound("Click Sound");

        m_pNPC.AccessQuest.AccessQuestStart = true;

        m_pQuestUI.InsertQuest(m_pNPC.AccessQuest);

        CloseMessageBotton();
    }

    public void CompleButton()
    {
        if(m_pNPC.AccessQuest.AccessQuestType == QUESTTYPE.QUEST_ITEM)
        {
            ItemQuest _ItemQuest = m_pNPC.AccessQuest as ItemQuest;

            if (null == _ItemQuest)
                return;

            _ItemQuest.RemoveQuestItem();
        }

        m_pNPC.AccessQuest.AccessQuestFinal = true;

        m_pPlayer.AccessExp += m_pNPC.AccessQuest.AccessQuestExp;

        m_pQuestUI.RemoveQuest(m_pNPC.AccessQuest);

        CloseMessageBotton();
    }

    public void ChangeButton()
    {
        if (null == m_pNPC || null == m_pNPC.AccessQuest) // ???
            return;

        if (m_pNPC.AccessQuest.AccessQuestComple == false)
            return;

        if (null == m_pClickEvent)
        {
            m_pClickEvent = new UnityEngine.UI.Button.ButtonClickedEvent();

            m_pClickEvent.AddListener(CompleButton);
        }       

        m_pButton.onClick = m_pClickEvent; // ?? 여기서 nullptr??
    }

    //public void NextCompleButton(NPC _NPC)
    //{
    //    if (_NPC == null)
    //        return;
    //}

    public void ResetButton()
    {
        if(m_pButton.onClick == m_pOldClickEvent)
            return;

        m_pButton.onClick = m_pOldClickEvent;
    }

    private void OnDestroy()
    {
        m_pQuestUI = null;

        m_pButton = null;

        m_pClickEvent = null;

        m_pNPC = null;
    }
}






//public void OnEnable()
//{
//    if (null == m_pNPC || null == m_pNPC.AccessQuest) // ???
//        return;

//    if (null == m_pClickEvent)
//        return;

//    if (m_pNPC.AccessQuest.AccessQuestComple == true
//        && m_pButton.onClick != m_pClickEvent && gameObject.name == "Yes Button")
//        m_pButton.onClick = m_pClickEvent;
//}



//public class MessageButton : MonoBehaviour
//{
//    private UnityEngine.UI.Button m_pButton = null;

//    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

//    private QuestList m_pQuestUI = null;

//    private NPC m_pNPC = null;

//    private UnityEngine.UI.Button.ButtonClickedEvent m_pClickEvent = null;

//    public NPC AccessNpc
//    {
//        set { m_pNPC = value; }
//    }

//    private void Start()
//    {
//        GameObject _GameObject = GameObject.Find("Quest UI");

//        if (_GameObject != null)
//        {
//            _GameObject = _GameObject.transform.Find("Quest Window Box").gameObject;

//            m_pQuestUI = _GameObject.transform.Find("Quest List").GetComponent<QuestList>();
//        }

//        m_pButton = GetComponent<UnityEngine.UI.Button>();

//        m_pClickEvent = new UnityEngine.UI.Button.ButtonClickedEvent();

//        m_pClickEvent.AddListener(CompleButton);

//        // 현재 온 클릭이 여부를 확인해야 하는데 ??
//    }

//    private void FixedUpdate()
//    {
//        if (m_pNPC == null)
//            return;

//        if (m_pNPC.AccessQuest.AccessQuestComple == true && m_pButton.onClick != m_pClickEvent
//            && gameObject.name == "Yes Button")
//            m_pButton.onClick = m_pClickEvent;
//    }

//    public void CloseMessageBotton()
//    {
//        GameObject _BarObject = transform.parent.gameObject;

//        if (null == _BarObject)
//            return;

//        _BarObject.transform.SetParent(null);

//        MessageBox _MessageBox = _BarObject.GetComponent<MessageBox>();

//        _MessageBox.ClearText();

//        _MessageBox.AccessNpc.AccessMessageIndex = 0;

//        _MessageBox.AccessNpc.AccessMessageBox = null;

//        m_pGameObjectManager.Remove("Message Bar", _BarObject);
//    }

//    public void NextButton()
//    {
//        if (m_pNPC.AccessMessage.Length - 1 >= m_pNPC.AccessMessageIndex)
//            m_pNPC.AccessMessageIndex += 1;

//        m_pNPC.AccessMessageBox.AccessIndex = 0.0f;
//    }

//    public void PrevButton()
//    {
//        if (0 < m_pNPC.AccessMessageIndex)
//            m_pNPC.AccessMessageIndex -= 1;

//        m_pNPC.AccessMessageBox.AccessIndex = 0.0f;
//    }

//    public void YesButton()
//    {
//        if (null == m_pNPC || null == m_pNPC.AccessQuest)
//            return;

//        m_pNPC.AccessQuest.AccessQuestStart = true;

//        m_pQuestUI.InsertQuest(m_pNPC.AccessQuest);

//        CloseMessageBotton();
//    }

//    public void CompleButton()
//    {
//        m_pNPC.AccessQuest.AccessQuestFinal = true;

//        m_pNPC.AccessQuest.RemoveQuestItem();

//        m_pQuestUI.RemoveQuest(m_pNPC.AccessQuest);

//        CloseMessageBotton();
//    }

//    private void OnDestroy()
//    {
//        m_pQuestUI = null;

//        m_pButton = null;

//        m_pClickEvent = null;

//        m_pNPC = null;
//    }
//}