using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucas : NPC
{
    [SerializeField] private NPC _CompleNPC = null; 

    [SerializeField] private string QuestEndMessage = string.Empty;

    [SerializeField] private GameObject _MessageBar = null;

    [SerializeField] private LayerMask _LayerMask = 0;

    private GameobjectManager m_pGameObejctManager = GameobjectManager.GetInstance();

    private RectTransform m_pCanvas = null;

    private bool m_bIsClick = false;

    private QuestList m_pQusetList = null;

    // Start is called before the first frame update
    void Start()
    {
        m_pGameObejctManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

        m_pCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        _Quest.AccessQuestFinal = _Quest.AccessQuestComple = _Quest.AccessQuestStart = false;

        _Quest.AccessQuestType = QUESTTYPE.QUEST_MESSAGE;

        m_pNextNpc = _CompleNPC;


        GameObject _QuestWindowObject = GameObject.Find("Quest UI");

        if (null != _QuestWindowObject)
        {
            GameObject _QuestWindow = _QuestWindowObject.transform.Find("Quest Window Box").gameObject;

            m_pQusetList = _QuestWindow.transform.Find("Quest List").GetComponent<QuestList>();
        }
    }

    private void LateUpdate()
    {
        CilckNPC();
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

        if ((null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false) && m_bIsOnClick == true)
        {
            m_pMessageBar = m_pGameObejctManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas).GetComponent<MessageBox>();

            m_pMessageBar.AccessNpc = this;

            if (_Message.Length == 1)
                m_pMessageBar.DisableButton();

            m_bIsClick = m_bIsOnClick = false;
        }

        MessageBox();
    }

    public void MessageBox()
    {
        if (null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false)
            return;

        List<QUEST> _ClearQuestList = m_pQusetList.GetFinalQuestList; // 퀘스트 리스트 중 이미 완료가 된 리스트를 가지고 온다

        for (int i = 0; i < _ClearQuestList.Count; ++i) // 클리어 된 리스트를 하나씩
        {
            if(_ClearQuestList[i].name == _Quest.name) // 해당 퀘스트가 퀘스트 클리어 된 목록에 존재한다면 
            {
                m_pMessageBar.DisableButton(); // 이전, 다음 버튼을 비활성화 

                m_pMessageBar.DisableQuestButton(); // 퀘스트 수락 버튼 비활성화 

                m_pMessageBar.AccessMessage = QuestEndMessage; // 완료 메세지 출력 

                m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite; // NPC 스프라이트의 전달 

                m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite); // 이름 바 및 이름 전달 

                m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

                m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

                return; // 종료 
            }
        }

        if (_Quest.AccessQuestStart == false) // 최초 퀘스트가 시작이 아직 안되었을때 
        {
            if (_Message.Length > 1 && m_iMessageIndex == 0) // 처음 메세지라면 
                m_pMessageBar.DisablePrevButton(); // 이전 버튼 비활성화 
            else
                m_pMessageBar.EnabledPrevButton(); // 다음 버튼 활성화 

            if (_Message.Length > 1 && m_iMessageIndex == _Message.Length - 1) // 마지막 메세지 일때
            {
                m_pMessageBar.DisableNextButton();

                m_pMessageBar.EnabledQuestButton();
            }
            else // 2번째 메세지 이상일시 
            {
                m_pMessageBar.EnabledNextButton();

                m_pMessageBar.DisableQuestButton();
            }

            m_pMessageBar.AccessMessage = _Message[m_iMessageIndex]; // 실제 메시지를 전달
        }
        else // 퀘스트가 종료 되었다면 
        {
            m_pMessageBar.DisableButton();

            m_pMessageBar.DisableQuestButton();

            m_pMessageBar.AccessMessage = QuestEndMessage;
        }

        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

        m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);


        //if (_Quest.AccessQuestStart == false) // 최초 시작 
        //{
        //    if (_Message.Length > 1 && m_iMessageIndex == 0) // 처음 메세지 
        //        m_pMessageBar.DisablePrevButton(); // 이전 버튼 비활성화 
        //    else
        //        m_pMessageBar.EnabledPrevButton(); // 다음 버튼 활성화 

        //    if (_Message.Length > 1 && m_iMessageIndex == _Message.Length - 1) // 마지막일 때 
        //    {
        //        m_pMessageBar.DisableNextButton();

        //        m_pMessageBar.EnabledQuestButton();
        //    }
        //    else // 2번째 메세지 이상일시 
        //    {
        //        m_pMessageBar.EnabledNextButton();

        //        m_pMessageBar.DisableQuestButton();
        //    }

        //    m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];
        //}
        //else
        //{
        //    m_pMessageBar.DisableButton();

        //    m_pMessageBar.DisableQuestButton();

        //    m_pMessageBar.AccessMessage = QuestEndMessage;
        //}


    }
}