using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// NPC는 퀘스트를 수행하는 NPC와 오브젝트 처럼 사용하는 NPC, 그리고 상점 열수 있는 NPC 3가지로 구분하여 클래스를 

// 만들었습니다.

public class Mai : NPC // NPC 마이 클래스입니다.
{
    [SerializeField] private string QuestingMessage = string.Empty;

    [SerializeField] private string QuestCompleMessage = string.Empty;

    [SerializeField] private string QuestEndMessage = string.Empty;

    [SerializeField] private GameObject _MessageBar = null;

    [SerializeField] private LayerMask _LayerMask = 0;

    private GameobjectManager m_pGameObejctManager = GameobjectManager.GetInstance();

    private RectTransform m_pCanvas = null;

    private bool m_bIsClick = false;

    // Start is called before the first frame update
    void Start()
    {
        m_pGameObejctManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

        m_pCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        if (_Quest.AccessQuestType == QUESTTYPE.QUEST_ITEM)
        {
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
    }

    // Update is called once per frame
    void Update()
    {
        if(_Quest.AccessQuestFinal == true && m_pMessageBar != null) // 메세지 박스가 열려있고 퀘스트가 클리어가 된 상태라면
        {
            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite; // 메세지 박스의 현재 동작하고 있는 스프라이트 전달

            m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite); // NPC에 이름 및 이름 받침대 스프라이드 전달 

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

            m_pMessageBar.AccessMessage = QuestEndMessage; // 현재 퀘스트 메세지를 퀘스트 클리어 후 메세지를 전달

            m_pMessageBar.DisablePrevButton(); // 이전으로 넘어가는 버튼을 끈다 

            m_pMessageBar.DisableNextButton(); // 다음으로 넘기는 버튼을 끈다 

            m_pMessageBar.DisableQuestButton(); // 퀘스트 버튼을 끈다
        }

        // 메세지 박스가 열려 있구 아직 퀘스트가 시작되지 않을 시 
        else if (m_pMessageBar != null && m_pMessageBar.gameObject.activeSelf == true && _Quest.AccessQuestStart == false)
        {
            if (_Message.Length > 1 && m_iMessageIndex == 0) // 하나 뿐인 메세지가 아니거나 첫번째 메세지라면 
                m_pMessageBar.DisablePrevButton(); // 이전 메세지버튼을 비활성화한다
            else // 그러지 않을 경우 
                m_pMessageBar.EnabledPrevButton(); // 이전 메세지버튼을 활성화한다 

            // 만약 처음 메세지가 마지막 메세지까지 왔다면 
            if (_Message.Length > 1 && m_iMessageIndex == _Message.Length - 1)
            {
                m_pMessageBar.DisableNextButton(); // 다음 버튼을 비활성화하고 

                m_pMessageBar.EnabledQuestButton(); // 퀘스트 버튼을 활성화한다.
            }
            else // 그러지 않는다면 
            {
                m_pMessageBar.EnabledNextButton(); // 다음 버튼을 활성화한다

                m_pMessageBar.DisableQuestButton(); // 퀘스트 버튼을 비활성화한다
            }

            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

            m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

            if (null != m_pMessageBar) // 메세지 박스가 존재한다면 
                m_pMessageBar.AccessMessage = _Message[m_iMessageIndex]; // 실제 메세지를 인덱스 배열값에 따라 적용한다 !

            return;
        }
        // 메세지 박스가 열려있고 퀘스트가 시작이 된 상태라면 
        else if(m_pMessageBar != null && m_pMessageBar.gameObject.activeSelf == true 
            && _Quest.AccessQuestStart == true && _Quest.AccessQuestComple == false)
        {
            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

            m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

            m_pMessageBar.AccessMessage = QuestingMessage; // 퀘스트 진행중 메세지로 변환

            m_pMessageBar.DisablePrevButton(); // 이전 메세지 버튼을 비활성화한다 

            m_pMessageBar.DisableNextButton(); // 다음 메세지 버튼을 비활성화한다

            m_pMessageBar.DisableQuestButton(); // 퀘스트 버튼들을 비활성화하고 

            return; // 종료한다
        }

        // 퀘스트가 완료 직전이라면 
        else if (m_pMessageBar != null && m_pMessageBar.gameObject.activeSelf == true && 
            _Quest.AccessQuestStart == true && _Quest.AccessQuestComple == true)
        {
            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

            m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

            m_pMessageBar.AccessMessage = QuestCompleMessage;

            m_pMessageBar.DisablePrevButton();

            m_pMessageBar.DisableNextButton();

            m_pMessageBar.ChangeYesButton(); // 퀘스트 수락 버튼을 완료 버튼으로 변경후 

            m_pMessageBar.EnabledQuestButton(); // 퀘스트 버튼을 활성화하고 

            return; // 종료 
        }

        // 마우스 위치값 
        Vector3 _WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D _Ray2D = new Ray2D();

        _Ray2D.origin = _WorldMousePosition;

        _Ray2D.direction = Vector2.zero;

        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _LayerMask); // 여기서 문제네 ??

        if (_Hit2D.collider != null && Input.GetMouseButtonDown(0) && m_bIsClick == false)
        {
            // NPC가 아니구 충돌된 오브젝트 네임 즉 해당 오브젝트와 같은 오브젝트가 아니라면 
            if (_Hit2D.collider.tag != "NPC" || _Hit2D.collider.gameObject.name != gameObject.name)
                return; // 종료 

            m_bIsClick = true; // 그러지 않을 시 한번 클릭 
        }

        else if (_Hit2D.collider != null && Input.GetMouseButtonDown(0))
        {
            if (_Hit2D.collider.tag != "NPC" || _Hit2D.collider.gameObject.name != gameObject.name)
                return;

            m_bIsOnClick = true; // 더블클릭 

            m_bIsClick = false; // 일반 클랙 해재
        }

        // 더블클릭시 메세지 박스를 만든다 
        if ((null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false) && m_bIsOnClick == true)
        {
            m_pMessageBar = m_pGameObejctManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas).GetComponent<MessageBox>();

            m_pMessageBar.AccessNpc = this; // 메세지에 NPC를 해당 오브젝트로 전달

            if (_Message.Length == 1)
                m_pMessageBar.DisableButton();

            m_bIsClick = m_bIsOnClick = false; // 더블클릭을 해재 
        }
    }
}