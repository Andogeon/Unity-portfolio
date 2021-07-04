using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Questsub_Window : MonoBehaviour
{
    [SerializeField] private Image _QuestNpc = null;

    [SerializeField] private Text _ExpText = null;

    [SerializeField] private QuestList _QusetList = null;

    private QUEST m_pQuest = null;

    private Text m_pText = null;

    private Text m_pCountText = null;

    private Text m_pContentsText = null;

    private Image m_pImage = null;

    private QuestSlot m_pQusetSlot = null;

    //private Image m_pQusetNpcImage = null;

    public QuestSlot AccessQusetSlot
    {
        get { return m_pQusetSlot; }

        set { m_pQusetSlot = value; }
    }

    private void Update()
    {
        if (m_pQuest == null)
        {
            if (null == m_pCountText)
                return;

            m_pCountText.text = string.Empty;

            m_pContentsText.text = string.Empty;

            _ExpText.text = string.Empty;

            _QuestNpc.gameObject.SetActive(false);

            return;
        }

        string _Result = string.Empty;

        string _ItemCount = string.Empty;

        if(m_pQuest.AccessQuestType == QUESTTYPE.QUEST_ITEM)
            _ItemCount = m_pQuest.AccessQuestItemCount.ToString();
        if (m_pQuest.AccessQuestType == QUESTTYPE.QUEST_MONTER)
        {
            MonterQuest _MonterQuest = m_pQuest as MonterQuest;

            if (null != _MonterQuest)
                _ItemCount = _MonterQuest.AccessDeleteCount.ToString();
        }

        string _QuestCount = " / " + m_pQuest.AccessQuestCount.ToString();

        _Result = _ItemCount + _QuestCount;

        m_pCountText.text = _Result;
    }

    private void OnDestroy()
    {
        m_pCountText = null;

        m_pQuest = null;

        m_pText = null;

        m_pImage = null;

        m_pContentsText = null;
    }

    public void QuestWindowMessage(QUEST _Quset)
    {
        if (null == _Quset)
            return;

        m_pQuest = _Quset;

        if (null == m_pText)
        {
            m_pText = transform.Find("Quest Main Text").GetComponent<Text>();

            m_pCountText = transform.Find("Quest Item Result count").GetComponent<Text>();

            m_pImage = transform.Find("Quest Item Sprite").GetComponent<Image>();

            m_pContentsText = transform.Find("Quest Contents Text").GetComponent<Text>();
        }

        m_pText.text = _Quset.AccessQuestSlotName;

        m_pContentsText.text = m_pQuest.AccessQuestContents;

        _ExpText.text = m_pQuest.AccessQuestExp.ToString();

        if (_QuestNpc.gameObject.activeSelf == false)
            _QuestNpc.gameObject.SetActive(true);

        _QuestNpc.sprite = m_pQuest.AccessQuestNPCSprite;

        if (_Quset.AccessQuestType == QUESTTYPE.QUEST_ITEM)
        {
            m_pCountText.gameObject.SetActive(true);

            ItemQuest _ItemQuest = _Quset as ItemQuest;

            if (null == _ItemQuest)
                return;

            m_pImage.sprite = _ItemQuest.AccessQuestICon.GET_ICONIMAGE.sprite;
        }
        else if (_Quset.AccessQuestType == QUESTTYPE.QUEST_MESSAGE)
        {
            // 이미지 스프라이트의 대화하려고자 하는 NPC 스트라이트를 넣어 줄것 

            MessageQuest _MessageQuest = _Quset as MessageQuest;

            if (null == _MessageQuest)
                return;

            if (_MessageQuest.AccessNPC.AccessSprireRenderer == null)
                _MessageQuest.AccessNPC.AccessSprireRenderer = _MessageQuest.AccessNPC.GetComponent<SpriteRenderer>();

            m_pCountText.gameObject.SetActive(false);

            m_pImage.sprite = _MessageQuest.AccessNPC.AccessSprireRenderer.sprite;
        }
        else
        {
            // 몬스터 스프라이트 이미지를 넣어줄것 !! 

            MonterQuest _MonterQuest = _Quset as MonterQuest;

            if (null == _MonterQuest)
                return;

            if (m_pCountText.gameObject.activeSelf == false)
                m_pCountText.gameObject.SetActive(true);

            m_pImage.sprite = _MonterQuest.AccessQuestMonterSprite;
        }
    }

    public void CancalQuest() // 정렬 !!
    {
        if (null == m_pQuest)
            return;

        m_pQuest.AccessQuestStart = false;

        _QusetList.SortQusetList(m_pQusetSlot);

        gameObject.SetActive(false);
    }
}








//public class Questsub_Window : MonoBehaviour
//{
//    private QUEST m_pQuest = null;

//    private Text m_pText = null;

//    private Text m_pCountText = null;

//    private Image m_pImage = null;

//    private QuestSlot m_pQusetSlot = null;

//    public QuestSlot AccessQusetSlot
//    {
//        get { return m_pQusetSlot; }

//        set { m_pQusetSlot = value; }
//    }

//    public QUEST AccessQuset
//    {
//        get { return m_pQuest; }

//        set
//        {
//            m_pQuest = value;

//            if (null == m_pText)
//            {
//                m_pText = transform.Find("Quest Main Text").GetComponent<Text>();

//                m_pCountText = transform.Find("Quest Item Result count").GetComponent<Text>();

//                m_pImage = transform.Find("Quest Item Sprite").GetComponent<Image>();
//            }

//            if (null == m_pQuest)
//            {
//                m_pText.text = string.Empty;

//                m_pImage.sprite = null;
//            }
//            else
//            {
//                m_pText.text = m_pQuest.AccessQuestSlotName;

//                if (m_pQuest.AccessQuestType == QUESTTYPE.QUEST_ITEM)
//                {
//                    ItemQuest _ItemQuest = m_pQuest as ItemQuest;

//                    if (null == _ItemQuest)
//                        return;

//                    m_pImage.sprite = _ItemQuest.AccessQuestICon.GET_ICONIMAGE.sprite;
//                }
//                else if (m_pQuest.AccessQuestType == QUESTTYPE.QUEST_MESSAGE)
//                {
//                    // 이미지 스프라이트의 대화하려고자 하는 NPC 스트라이트를 넣어 줄것 
//                }
//                else
//                {
//                    // 몬스터 스프라이트 이미지를 넣어줄것 !! 
//                }
//            }
//        }
//    }

//    private void Update()
//    {
//        if (m_pQuest == null)
//        {
//            if (null == m_pCountText)
//                return;

//            m_pCountText.text = string.Empty;

//            return;
//        }

//        string _Result = string.Empty;

//        string _ItemCount = m_pQuest.AccessQuestItemCount.ToString();

//        string _QuestCount = " / " + m_pQuest.AccessQuestCount.ToString();

//        _Result = _ItemCount + _QuestCount;

//        m_pCountText.text = _Result;
//    }

//    private void OnDestroy()
//    {
//        m_pCountText = null;

//        m_pQuest = null;

//        m_pText = null;

//        m_pImage = null;
//    }

//    public void QuestWindowMessage(QUEST _Quset)
//    {
//        if (null == _Quset)
//            return;

//        m_pText.text = _Quset.AccessQuestSlotName;

//        if (_Quset.AccessQuestType == QUESTTYPE.QUEST_ITEM)
//        {
//            ItemQuest _ItemQuest = _Quset as ItemQuest;

//            if (null == _ItemQuest)
//                return;

//            m_pImage.sprite = _ItemQuest.AccessQuestICon.GET_ICONIMAGE.sprite;
//        }
//        else if (_Quset.AccessQuestType == QUESTTYPE.QUEST_MESSAGE)
//        {
//            // 이미지 스프라이트의 대화하려고자 하는 NPC 스트라이트를 넣어 줄것 
//        }
//        else
//        {
//            // 몬스터 스프라이트 이미지를 넣어줄것 !! 
//        }
//    }
//}



















//public class Questsub_Window : MonoBehaviour
//{
//    private Quest m_pQuest = null;

//    private Text m_pText = null;

//    private Text m_pCountText = null;

//    private Image m_pImage = null;

//    public Quest AccessQuset
//    {
//        get { return m_pQuest; }

//        set
//        {
//            m_pQuest = value;

//            if (null == m_pText)
//            {
//                m_pText = transform.Find("Quest Main Text").GetComponent<Text>();

//                m_pCountText = transform.Find("Quest Item Result count").GetComponent<Text>();

//                m_pImage = transform.Find("Quest Item Sprite").GetComponent<Image>();
//            }

//            if (null == m_pQuest)
//            {
//                m_pText.text = string.Empty;

//                m_pImage.sprite = null;
//            }
//            else
//            {
//                m_pText.text = m_pQuest.AccessQuestSlotName;

//                m_pImage.sprite = m_pQuest.AccessQuestItem.AccessIcon.GET_ICONIMAGE.sprite;
//            }
//        }
//    }

//    private void Update()
//    {
//        if (m_pQuest == null)
//        {
//            m_pCountText.text = string.Empty;

//            return;
//        }

//        string _Result = string.Empty;

//        string _ItemCount = m_pQuest.AccessQuestItemCount.ToString();

//        string _QuestCount = " / " + m_pQuest.AccessQuestCount.ToString();

//        _Result = _ItemCount + _QuestCount;

//        m_pCountText.text = _Result;
//    }

//    private void OnDestroy()
//    {
//        m_pCountText = null;

//        m_pQuest = null;

//        m_pText = null;

//        m_pImage = null;
//    }
//}