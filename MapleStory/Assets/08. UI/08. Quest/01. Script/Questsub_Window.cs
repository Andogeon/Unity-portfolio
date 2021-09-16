using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 퀘스트 리스트에서 퀘스트 슬롯을 누를 시 퀘스트 세부사항을 알 수 있게 따로 생성 되는 퀘스트 서브 창 클래스입니다.

public class Questsub_Window : MonoBehaviour // 퀘스트 서브 창 클래스입니다.
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

    public QuestSlot AccessQusetSlot
    {
        get { return m_pQusetSlot; }

        set { m_pQusetSlot = value; }
    }

    private void Update()
    {
        if (m_pQuest == null) // 퀘스트가 존재하지 않는다면 
        {
            if (null == m_pCountText)
                return;

            m_pCountText.text = string.Empty;

            m_pContentsText.text = string.Empty;

            _ExpText.text = string.Empty;

            _QuestNpc.gameObject.SetActive(false);

            // 기존에 있는 텍스트, 스프라이트 비우고 비활성화한다.

            return;
        }

        string _Result = string.Empty; // 출력할 최종 문자열

        string _ItemCount = string.Empty; // 아이템 숫자를 나타낼 문자열

        if(m_pQuest.AccessQuestType == QUESTTYPE.QUEST_ITEM) // 해당 퀘스트가 아이템 타입이라면
            _ItemCount = m_pQuest.AccessQuestItemCount.ToString(); // 현재 가지고 있는 아이템의 개수를 가지고 온다.
        if (m_pQuest.AccessQuestType == QUESTTYPE.QUEST_MONTER) // 몬스터 일시 
        {
            MonterQuest _MonterQuest = m_pQuest as MonterQuest; // 형변환하여 

            if (null != _MonterQuest) //존재 한다면 
                _ItemCount = _MonterQuest.AccessDeleteCount.ToString(); // 몬스터를 사냥한 개수를 가지고 온다.
        }

        string _QuestCount = " / " + m_pQuest.AccessQuestCount.ToString(); // " / 퀘스트의 개수를 표현"

        _Result = _ItemCount + _QuestCount; // "현재 아이템 개수 혹은 몬스터 잡은 개수 / 퀘스트의 개수를 표현" 

        m_pCountText.text = _Result; // 최종 결과를 출력
    }

    private void OnDestroy()
    {
        m_pCountText = null;

        m_pQuest = null;

        m_pText = null;

        m_pImage = null;

        m_pContentsText = null;
    }

    // 서브 퀘스트의 세부 사항 및 
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

        m_pText.text = _Quset.AccessQuestSlotName; // 퀘스트 주 제목 

        m_pContentsText.text = m_pQuest.AccessQuestContents; // 내용 

        _ExpText.text = m_pQuest.AccessQuestExp.ToString(); // 받을 경험치

        if (_QuestNpc.gameObject.activeSelf == false) // 스프라이트가 꺼져 있다면 
            _QuestNpc.gameObject.SetActive(true); // 다시 활성화 

        _QuestNpc.sprite = m_pQuest.AccessQuestNPCSprite; // 퀘스트와 연관 되어 있는 NPC의 스프라이트 

        if (_Quset.AccessQuestType == QUESTTYPE.QUEST_ITEM)
        {
            m_pCountText.gameObject.SetActive(true);

            ItemQuest _ItemQuest = _Quset as ItemQuest;

            if (null == _ItemQuest)
                return;

            m_pImage.sprite = _ItemQuest.AccessQuestICon.GET_ICONIMAGE.sprite; // 아이템의 이미지 출력
        }
        else if (_Quset.AccessQuestType == QUESTTYPE.QUEST_MESSAGE)
        {           
            MessageQuest _MessageQuest = _Quset as MessageQuest;

            if (null == _MessageQuest)
                return;

            if (_MessageQuest.AccessNPC.AccessSprireRenderer == null)
                _MessageQuest.AccessNPC.AccessSprireRenderer = _MessageQuest.AccessNPC.GetComponent<SpriteRenderer>();

            m_pCountText.gameObject.SetActive(false);

            m_pImage.sprite = _MessageQuest.AccessNPC.AccessSprireRenderer.sprite; // 전달 받을 메세지 NPC 출력
        }
        else
        {
            MonterQuest _MonterQuest = _Quset as MonterQuest;

            if (null == _MonterQuest)
                return;

            if (m_pCountText.gameObject.activeSelf == false)
                m_pCountText.gameObject.SetActive(true);

            m_pImage.sprite = _MonterQuest.AccessQuestMonterSprite; // 몬스터 출력 
        }
    }

    public void CancalQuest() // 취소시 호출 후 자동 정렬
    {
        if (null == m_pQuest)
            return;

        m_pQuest.AccessQuestStart = false;

        _QusetList.SortQusetList(m_pQusetSlot);

        gameObject.SetActive(false);
    }
}