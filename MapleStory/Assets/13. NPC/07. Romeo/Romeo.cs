using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Romeo : NPC // 로미오 클래스입니다.
{
    [SerializeField] private QUEST _OldQuset = null;

    [SerializeField] private GameObject _MessageBar = null;

    [SerializeField] private NPC _ConnectNPCObject = null;

    [SerializeField] private LayerMask _LayerMask = 0;

    [SerializeField] private string _DefaultMessage = string.Empty;

    [SerializeField] private string _QuestStartMessage = string.Empty;

    [SerializeField] private string _QuestCompleMessage = string.Empty;

    [SerializeField] private string _QuestFinalMessage = string.Empty;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private bool m_bIsClick = false;

    private Transform m_pCanvas = null;

    private QuestList m_pQusetList = null;

    private static bool m_bIsStartQuset = false;

    private void Start()
    {
        m_pCanvas = GameObject.Find("Canvas").transform;

        GameObject _QuestWindowObject = GameObject.Find("Quest UI");

        if(null != _QuestWindowObject)
        {
            GameObject _QuestWindow = _QuestWindowObject.transform.Find("Quest Window Box").gameObject;

            m_pQusetList = _QuestWindow.transform.Find("Quest List").GetComponent<QuestList>();
        }

        m_pGameobjectManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        ItemQuest _ItemQuest = _Quest as ItemQuest;

        if (null == _ItemQuest)
            return;

        GameObject _Inventory = GameObject.Find("Inventory UI");

        _Inventory = _Inventory.transform.Find("Inventory Box").gameObject;

        _Inventory = _Inventory.transform.Find("Inventory").gameObject;

        Inventory _QuestInventory = _Inventory.transform.Find("Other Window").GetComponent<Other_Inventory>();

        _ItemQuest.AccessInventory = _QuestInventory;

        _ItemQuest.AccessNPC = this;
    }

    // 본 NPC에 퀘스트가 완료시 다음 퀘스트를 진행하기 위해 호출 되는 함수입니다.
    public override void QuestNextNPC()
    {
        Vasily _VasilyNpc = _ConnectNPCObject as Vasily;

        if (_VasilyNpc != null)
            _VasilyNpc.AccessNextQuest = true;
    }

    private void Update()
    {
        CilckNPC();
    }   

    private void MessageBox()
    {
        // 바실리와 동일하게 루카스의 퀘스트가 완료가 되었다면 해당 NPC의 퀘스트를 진행 할 수 있게 했습니다.

        List<QUEST> _ClearQuestList = m_pQusetList.GetFinalQuestList;

        // 완료된 퀘스트의 목록을 찾아 

        for (int i = 0; i < _ClearQuestList.Count; ++i) // 하나씩 순회한다
        {

            // 아직 퀘스트가 진행하기 않았고 이전 루카스 퀘스트가 완료가 되었다면 

            if (m_bIsStartQuset == false && _ClearQuestList[i].name == _OldQuset.name) 
            {
                m_bIsStartQuset = true; // 퀘스트를 진행 

                break;
            }

            if (_ClearQuestList[i].name == _Quest.name) // 퀘스트 완료했다면
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                m_pMessageBar.AccessMessage = _QuestFinalMessage;

                m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

                m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

                m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

                m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

                return;
            }
        }

        // 루카스의 퀘스트를 완료 했는지 


        if (m_bIsStartQuset == false) // 퀘스트 진행 사항이 없을시 
        {
            m_pMessageBar.AccessMessage = _DefaultMessage;

            m_pMessageBar.DisableButton();

            m_pMessageBar.DisableQuestButton();
        }
        else if (m_bIsStartQuset == true) // 퀘스트를 진행
        {
            if (_Quest.AccessQuestStart == false)
            {
                m_pMessageBar.ResetYesButton(); // 수락 버튼을 초기화 한다.

                if (_Message.Length > 1 && m_iMessageIndex == 0)
                    m_pMessageBar.DisablePrevButton(); // 메세지의 첫번째라면 이전 버튼을 비활성화 
                else
                    m_pMessageBar.EnabledPrevButton(); // 메세지가 2번째 이상으로 넘어갓다면 이전 버튼 활성화 

                if (_Message.Length > 1 && m_iMessageIndex == _Message.Length - 1) // 메세지가 최종 메세지까지 왔다면 
                {
                    m_pMessageBar.DisableNextButton(); // 다음 버튼의 비활성화

                    m_pMessageBar.EnabledQuestButton(); // 퀘스트 버튼 활성화 
                }
                else // 아직 최종메세지까지 오지 않더라면 
                {
                    m_pMessageBar.EnabledNextButton(); // 다음 버튼의 활성화

                    m_pMessageBar.DisableQuestButton(); // 퀘스트 버튼의 비활성화 
                }

                m_pMessageBar.AccessMessage = _Message[m_iMessageIndex]; // 인덱스의 따른 메세지를 실제 적용 
            }
            else if (_Quest.AccessQuestStart == true && _Quest.AccessQuestComple == false) // 해당 NPC의 퀘스트가 시작 되었다면 
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                // 모든 버튼의 비활성화 

                m_pMessageBar.AccessMessage = _QuestStartMessage; // 퀘스트 진행중 메세지를 적용 
            }

            // 퀘스트 수행이 됬고 완료 버튼 누르지 직전이라면 
            else if (_Quest.AccessQuestComple == true && _Quest.AccessQuestFinal == false)
            {
                m_pMessageBar.DisableButton(); // 이전, 다음 버튼 비활성화

                m_pMessageBar.ChangeYesButton(); // 퀘스트 완료 버튼으로 기능 변경 

                m_pMessageBar.EnabledQuestButton(); // 퀘스트 완료버튼 활성화 

                m_pMessageBar.AccessMessage = _QuestCompleMessage; // 메세지를 수행 완료 메세지로 변경 
            }
            else // 퀘스트 완료시 
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                // 모든 버튼의 비활성화 

                m_pMessageBar.AccessMessage = _QuestFinalMessage; // 완료 메세지를 전달 
            }
        }

        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

        m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);
    }

    private void CilckNPC()
    {
        Vector3 _WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D _Ray2D = new Ray2D();

        _Ray2D.origin = _WorldMousePosition;

        _Ray2D.direction = Vector2.zero;

        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _LayerMask); // 여기서 문제네 ??

        if (_Hit2D.collider != null && Input.GetMouseButtonDown(0) && m_bIsClick == false)
        {
            if (_Hit2D.collider.tag != "NPC" || _Hit2D.collider.gameObject.name != gameObject.name)
                return;

            m_bIsClick = true;
        }

        else if (_Hit2D.collider != null && Input.GetMouseButtonDown(0))
        {
            if (_Hit2D.collider.tag != "NPC" || _Hit2D.collider.gameObject.name != gameObject.name)
                return;

            m_bIsOnClick = true;

            m_bIsClick = false;
        }

        // 더블 클릭시 메세지 박스를 생성 

        if ((null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false) && m_bIsOnClick == true)
        {
            m_pMessageBar = m_pGameobjectManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas).GetComponent<MessageBox>();

            m_pMessageBar.AccessNpc = this;

            if (_Message.Length == 1)
                m_pMessageBar.DisableButton();

            m_bIsClick = m_bIsOnClick = false;
        }

        if(null != m_pMessageBar)
            MessageBox();
    }

    private void OnDestroy()
    {
        m_pCanvas = null;

        m_pGameobjectManager = null;
    }
}