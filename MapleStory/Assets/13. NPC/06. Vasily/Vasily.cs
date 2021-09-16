using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vasily : NPC // 바실리 클래스입니다.
{
    [SerializeField] private QUEST _RucasQuest = null;

    [SerializeField] private QUEST _NextQuest = null;

    [SerializeField] private GameObject _MessageBar = null;

    [SerializeField] private string _defaultMessage = string.Empty;

    [SerializeField] private string _FirstQuestFinalMessage = string.Empty;

    [SerializeField] private string[] _NextQuestMessage = null;

    [SerializeField] private LayerMask _LayerMask = 0;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private NPC m_pRomeoObject = null;

    private GameObject m_pCanvas = null;

    private QuestList m_pQuestUI = null;

    private bool m_bIsClick = false;

    private static bool m_bIsNextQuest = false;

    public bool AccessNextQuest
    {
        get { return m_bIsNextQuest; }

        set
        {
            m_bIsNextQuest = value;

            if (m_bIsNextQuest == true) // 해당 값이 들어온다면 이 때 
            {
                _NextQuest.AccessQuestType = QUESTTYPE.QUEST_MONTER;

                _Quest = _NextQuest;
            }

            m_iMessageIndex = 0;
        }
    }

    // 해당 퀘스트를 받으면 더블클릭시 
    private void Start()
    {
        m_pCanvas = GameObject.Find("Canvas");

        GameObject _GameObject = GameObject.Find("Quest UI");

        m_pRomeoObject = GameObject.Find("Romeo").GetComponent<NPC>();

        if (_GameObject != null)
        {
            _GameObject = _GameObject.transform.Find("Quest Window Box").gameObject;

            m_pQuestUI = _GameObject.transform.Find("Quest List").GetComponent<QuestList>();
        }

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pGameObjectManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

        QUEST RucasQuest = m_pQuestUI.FindQuest(_RucasQuest);

        if (RucasQuest != null)
        {
            RucasQuest.AccessQuestComple = true;

            _Quest = RucasQuest;
        }
    }


    private void Update()
    {
        FindMessageBox();
    }
    
    private void OnDestroy()
    {
        m_pGameObjectManager = null;

        m_pSpriteRenderer = null;

        m_pQuestUI = null;

        m_pCanvas = null;
    }

    private void FindMessageBox()
    {
        bool _bIsQuest = false;

        // 씬 전환시 해당 오브젝트가 해당 퀘스트가 시작했는지 알 수 없기에 퀘스트 목록에서 찾아 

        // 있다면 강제로 시작하게 했습니다.

        QUEST _NanoQuest = m_pQuestUI.FindQuest(_NextQuest); // 현제 퀘스트 리스트 중에서 해당 퀘스트가 있는지 ?

        if (_NanoQuest != null) // 존재한다면 
            _bIsQuest = true; // 퀘스트를 강제로 시작 

        if (m_pMessageBar != null && m_bIsNextQuest == true && _bIsQuest == false)
            m_pMessageBar.ResetYesButton(); // 다음 퀘스트가 진행 되었다면 버튼을 리셋

        MessageBox(_bIsQuest); // 퀘스트의 강제 여부를 메세지박스 함수로 전달

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

        // 더블클릭시 메세지 박스의 생성
        if ((null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false) && m_bIsOnClick == true)
        {
            m_pMessageBar = m_pGameObjectManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas.transform).GetComponent<MessageBox>();

            m_pMessageBar.AccessNpc = this;

            if (_Message.Length == 1)
                m_pMessageBar.DisableButton();

            if (m_bIsNextQuest == true) // 다음 퀘스트가 진행 되었다면 
                m_pMessageBar.ResetYesButton(); // 수락 버튼을 초기화 

            m_bIsClick = m_bIsOnClick = false;
        }
    }

    public void MessageBox(bool _bIsQuset)
    {
        if (null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false)
            return;

        if (_bIsQuset == true) // 퀘스트를 강제로 시작하기 위해서 
            _Quest = _NextQuest; // 최종 퀘스트를 기존 퀘스트로 이동

        bool _bIsQuestClear = false;

        List<QUEST> _ClearQuestList = m_pQuestUI.GetFinalQuestList;

        // 퀘스트 완료 목록을 가지고 와서 

        for(int i = 0; i < _ClearQuestList.Count; ++i) // 하나씩 순회한다 
        {
            // 완료 된 퀘스트 목록 중 루카스 퀘스트가 있거나 최종 마노 퀘스트가 존재한다면 
            if(_ClearQuestList[i].name == "Lucas Quest" && m_bIsNextQuest == false || _ClearQuestList[i].name == _NextQuest.name && m_bIsNextQuest == true)
            {
                _bIsQuestClear = true;

                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                // 모든 버튼을 비활성화

                m_pMessageBar.AccessMessage = _defaultMessage; // 기존 메세지를 출력한다.

                m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

                m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

                m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

                m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

                return;
            }
        }

        // 루카스로 받은 퀘스트를 진행중 이라면 
        if (null != _Quest && _Quest.name == "Lucas Quest" && _bIsQuestClear == false && m_bIsNextQuest == false) // 루카스 퀘스트 완료 직전 
        {
            m_pMessageBar.AccessMessage = _FirstQuestFinalMessage; // 완료 메세지를 출력 

            m_pMessageBar.DisableButton(); // 이전, 다음 메세지 버튼 비활성화 후 

            m_pMessageBar.ChangeYesButton(); // 클리어 수락으로 변경

            m_pMessageBar.EnabledQuestButton(); // 퀘스트 수락 버튼을 활성화한다 
        }

        QUEST _QUEST = m_pQuestUI.FindQuest(_Quest);

        MonterQuest _MonterQuset = null;

        if (_QUEST != null) // 퀘스트가 존재한다면 
            _MonterQuset = _QUEST as MonterQuest; // 몬스터 퀘스트로 형변환 후 

        if (null != _MonterQuset) // 형변환한 객체의 정보가 맞다면 
        {
            if (_MonterQuset.AccessDeleteCount >= _Quest.AccessQuestCount) // 퀘스트의 완료 여부 검사 
                _Quest.AccessQuestComple = true; // 완료시 해당 퀘스트의 완료 
        }

        if (m_bIsNextQuest == true) // 최종 마노 퀘스트를 받기 위한 조건이 완성 되었다면 
        {
            if (_Quest != null && _Quest.AccessQuestStart == false && _Quest.AccessQuestFinal == false) // 해당 퀘스트가 시작전 
            {
                // 메세지의 인덱스 값에 따른 이전, 다음, 수락 버튼의 대한 버튼 활성화 및 비활성화

                if (m_iMessageIndex == 0)
                {
                    m_pMessageBar.DisablePrevButton();

                    m_pMessageBar.EnabledNextButton();

                    m_pMessageBar.DisableQuestButton();
                }
                else if (m_iMessageIndex == 1)
                {
                    m_pMessageBar.DisableNextButton();

                    m_pMessageBar.EnabledQuestButton();
                }

                m_pMessageBar.AccessMessage = _NextQuestMessage[m_iMessageIndex]; // 인덱스의 따른 메세지의 전달 
            }

            else if (_bIsQuset == true && _Quest.AccessQuestComple == false) // 퀘스트를 수락 받았지만 아직 완료하지 못할경우
            {
                m_pMessageBar.DisableQuestButton();

                m_pMessageBar.DisableButton();

                m_pMessageBar.AccessMessage = "어서가서 마노를 저지하여 원활하게 출항 할 수 있게 도와주게 !!";
            } 
            else if (_bIsQuset == true && _Quest.AccessQuestComple == true) // 퀘스트의 클리어 직전 
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.ChangeYesButton();

                m_pMessageBar.EnabledQuestButton();

                m_pMessageBar.AccessMessage = "고생했어 !! 이제 빅토리아 아일랜드로 떠나세";
            }
        }

        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

        m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);
    }

    private void NextQuestMessage()
    {
        if (_Quest.AccessQuestStart == false)
        {
            m_pMessageBar.ResetYesButton();

            if (_Message.Length > 1 && m_iMessageIndex == 0)
                m_pMessageBar.DisablePrevButton();
            else
                m_pMessageBar.EnabledPrevButton();

            if (_Message.Length > 1 && m_iMessageIndex == _Message.Length - 1)
            {
                m_pMessageBar.DisableNextButton();

                m_pMessageBar.EnabledQuestButton();
            }
            else
            {
                m_pMessageBar.EnabledNextButton();

                m_pMessageBar.DisableQuestButton();
            }

            m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];
        }

        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);
    }
}