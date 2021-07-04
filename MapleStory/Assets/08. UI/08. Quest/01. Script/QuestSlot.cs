using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuestSlot : MonoBehaviour
{
    private QUEST m_pQuest = null;

    private Text m_pText = null;

    private Questsub_Window m_pSubWindows = null;

    public Questsub_Window AccessQuestSubWindow // null??
    {
        get { return m_pSubWindows; }

        set { m_pSubWindows = value; }
    }

    public QUEST AccessQuest
    {
        get { return m_pQuest; }

        set { m_pQuest = value;}
    }

    public Text AccessText
    {
        get 
        {
            if (null == m_pText)
                m_pText = GetComponentInChildren<Text>();

            return m_pText; 
        }

        set { m_pText = value; }
    }

    public void ChangeQuestSubWindows()
    {
        if (null == m_pQuest || null == m_pSubWindows)
            return;

        if (m_pSubWindows.gameObject.activeSelf == true)
        {
            if (m_pSubWindows.AccessQusetSlot == this)
                m_pSubWindows.gameObject.SetActive(false);
            else
            {
                m_pSubWindows.AccessQusetSlot = this;

                m_pSubWindows.QuestWindowMessage(m_pQuest);
            }
        }
        else
        {
            m_pSubWindows.AccessQusetSlot = this;

            m_pSubWindows.gameObject.SetActive(true);

            m_pSubWindows.QuestWindowMessage(m_pQuest);
        }
    }

    public void OnDestroy()
    {
        m_pQuest = null;

        m_pSubWindows = null;

        m_pText = null;
    }
}







//public class QuestSlot : MonoBehaviour
//{
//    private QUEST m_pQuest = null;

//    private Text m_pText = null;

//    private Questsub_Window m_pSubWindows = null;

//    public Questsub_Window AccessQuestSubWindow // null??
//    {
//        get { return m_pSubWindows; }

//        set { m_pSubWindows = value; }
//    }

//    public QUEST AccessQuest
//    {
//        get { return m_pQuest; }

//        set
//        {
//            m_pQuest = value;

//            if (null != m_pSubWindows)
//                m_pSubWindows.AccessQuset = AccessQuest;
//        }
//    }

//    public Text AccessText
//    {
//        get
//        {
//            if (null == m_pText)
//                m_pText = GetComponentInChildren<Text>();

//            return m_pText;
//        }

//        set { m_pText = value; }
//    }

//    public void EnableSubQuestWindow()
//    {
//        if (null == m_pSubWindows)
//            return;

//        // 윈도우창이 이미 열려있고 다른 퀘스트 슬롯을 열려고 할때.. 퀘스트 내용을 변경한다 !! 

//        //if (m_pSubWindows.gameObject.activeSelf == true)
//        //    m_pSubWindows.gameObject.SetActive(false);
//        //else
//        //{
//        //    m_pSubWindows.gameObject.SetActive(true);

//        //    m_pSubWindows.QuestWindowMessage(m_pQuest);
//        //}

//        //if (m_pSubWindows.gameObject.activeSelf == true)
//        //    m_pSubWindows.gameObject.SetActive(false);
//        //else
//        //{
//        //    m_pSubWindows.gameObject.SetActive(true);

//        //    m_pSubWindows.QuestWindowMessage(m_pQuest);
//        //}


//        // 여기서 손을 봐야 하네 ??
//    }

//    public void ChangeQuestSubWindows()
//    {
//        if (null == m_pQuest || null == m_pSubWindows)
//            return;

//        if (m_pSubWindows.gameObject.activeSelf == true)
//            m_pSubWindows.gameObject.SetActive(false);
//        else
//        {
//            m_pSubWindows.QuestWindowMessage(m_pQuest);
//        }
//    }

//    public void OnDestroy()
//    {
//        m_pQuest = null;

//        m_pSubWindows = null;

//        m_pText = null;
//    }
//}


























//public void EnableSubQuestWindow()
//{
//    if (null == m_pSubWindows)
//        return;

//    if (m_pSubWindows.gameObject.activeSelf == true)
//        m_pSubWindows.gameObject.SetActive(false);
//    else
//        m_pSubWindows.gameObject.SetActive(true);

//    //if (m_pSubWindows.gameObject.activeSelf == true || m_pSubWindows == null)
//    //    return;

//    //m_pSubWindows.gameObject.SetActive(true);
//}