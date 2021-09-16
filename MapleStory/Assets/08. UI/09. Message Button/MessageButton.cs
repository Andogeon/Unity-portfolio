using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MessageButton : MonoBehaviour // 메세지 박스와 함께하는 메세지 버튼입니다.
{
    private UnityEngine.UI.Button m_pButton = null;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private QuestList m_pQuestUI = null;

    private NPC m_pNPC = null;

    private NPC m_pNextQuestNPC = null;

    private Player m_pPlayer = null;

    private UnityEngine.UI.Button.ButtonClickedEvent m_pClickEvent = null;

    private UnityEngine.UI.Button.ButtonClickedEvent m_pOldClickEvent = null;

    public NPC AccessNpc
    {
        set { m_pNPC = value; }
    }

    public NPC AccessNextQuestNPC
    {
        set { m_pNextQuestNPC = value; }
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

        m_pClickEvent = new UnityEngine.UI.Button.ButtonClickedEvent(); // 퀘스트 완료시 호출될 함수 이벤트를 미리 만들고

        m_pClickEvent.AddListener(CompleButton); // 이벤트를 연결합니다.

        m_pOldClickEvent = m_pButton.onClick; // 그리고 기존 수락할 수 있는 버튼을 미리 만들어 줍니다.
    }

    // 퀘스트 창을 닫을 때 호출 되는 함수입니다.
    public void CloseMessageBotton()
    {
        GameObject _BarObject = transform.parent.gameObject; // 메세지 박스 오브젝트가 존재하는지

        if (null == _BarObject)
            return; // 존재 하지 않는다면 종료 

        _BarObject.transform.SetParent(null); // 부모 연결을 해재하고 

        MessageBox _MessageBox = _BarObject.GetComponent<MessageBox>();

        _MessageBox.ClearText(); // 텍스트를 비우고

        _MessageBox.AccessNpc.AccessMessageIndex = 0; // 텍스트의 인덱스를 0으로 설정

        _MessageBox.AccessNpc.AccessMessageBox = null; // NPC와 연결된 메세지 박스를 null로 초기화

        _MessageBox.AccessNpc = null; // 메세지 박스와 NPC가 연결된 값 또한 null로 초기화 

        m_pNPC = null; // 해당 버튼들도 null로 초기화 

        m_pSoundManager.PlaySound("Click Sound"); 

        m_pGameObjectManager.Remove("Message Bar", _BarObject); // 메세지 박스를 비활성화하여 다시 오브젝트 풀링 객체에 넣어준다
    }

    // 다음으로 넘어가는 UI를 클릭시 호출 되는 함수입니다.
    public void NextButton()
    {
        if (m_pNPC.AccessMessage.Length - 1 > m_pNPC.AccessMessageIndex)
            m_pNPC.AccessMessageIndex += 1;

        m_pNPC.AccessMessageBox.AccessIndex = 0.0f; // 메세지 속도를 0으로 다시 조정

        m_pSoundManager.PlaySound("Click Sound");
    }


    // 이전으로 넘어가는 UI를 클릭시 호출 되는 함수입니다.
    public void PrevButton()
    {
        if (0 < m_pNPC.AccessMessageIndex)
            m_pNPC.AccessMessageIndex -= 1;

        m_pNPC.AccessMessageBox.AccessIndex = 0.0f; // 메세지 속도를 0으로 다시 조정

        m_pSoundManager.PlaySound("Click Sound");
    }

    // 메세지 박스에서 퀘스트를 수락시 해당 퀘스트를 삽입하는 함수입니다.
    public void YesButton()
    {
        if (null == m_pNPC || null == m_pNPC.AccessQuest) // 메세지 박스에서 NPC 설정이나 NPC가 퀘스트를 소유하지 않는다면
            return; // 종료

        m_pSoundManager.PlaySound("Click Sound");

        m_pNPC.AccessQuest.AccessQuestStart = true; // 해당 NPC 퀘스트를 시작하고 

        m_pQuestUI.InsertQuest(m_pNPC.AccessQuest); // 퀘스트목록에 퀘스트를 추가한다

        CloseMessageBotton();
    }

    public void CompleButton() // 퀘스트 완료시 수락 버튼을 누르면 호출 되는 함수입니다.
    {
        if (m_pNPC.AccessQuest.AccessQuestType == QUESTTYPE.QUEST_ITEM) // 해당 퀘스트가 아이템 관련 퀘스트라면
        {
            ItemQuest _ItemQuest = m_pNPC.AccessQuest as ItemQuest;

            if (null == _ItemQuest) // 형변환 했는데 아이템이 아니다
                return; // 종료 

            _ItemQuest.RemoveQuestItem();// 인벤토리에서 아이템을 뺀다
        }

        m_pNPC.AccessQuest.AccessQuestFinal = true; // 퀘스트를 완료

        m_pNPC.QuestNextNPC();

        m_pPlayer.AccessExp += m_pNPC.AccessQuest.AccessQuestExp; // 경험치를 추가한다.

        m_pQuestUI.ClearRemoveQuest(m_pNPC.AccessQuest); // 완료된 퀘스트를 퀘스트 완료 리스트에 추가한다

        CloseMessageBotton(); // 메세지 박스를 종료한다
    }

    public void ChangeButton() // 수락 버튼의 이벤트 함수를 교체할때 호출 되는 함수입니다.
    {
        if (null == m_pNPC || null == m_pNPC.AccessQuest)
            return;

        if (m_pNPC.AccessQuest.AccessQuestComple == false) // 퀘스트가 완료 직전이 아니라면 
            return; // 종료

        if (null == m_pClickEvent) // 혹시 모를 교채해야 될 이벤트 함수값이 존재하지 않는다면 
        {
            m_pClickEvent = new UnityEngine.UI.Button.ButtonClickedEvent();

            m_pClickEvent.AddListener(CompleButton); // 생성하여 이벤트 함수를 추가한다
        }       

        m_pButton.onClick = m_pClickEvent; // 그리고 이벤트 함수를 변경한다
    }

    public void ResetButton() // 다시 퀘스트 수락시 버튼으로 되돌릴 때 호출 되는 함수입니다.
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