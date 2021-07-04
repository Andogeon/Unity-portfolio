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

    // Start is called before the first frame update
    void Start()
    {
        m_pGameObejctManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

        m_pCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        _Quest.AccessQuestFinal = _Quest.AccessQuestComple = _Quest.AccessQuestStart = false;

        _Quest.AccessQuestType = QUESTTYPE.QUEST_MESSAGE;

        m_pNextNpc = _CompleNPC;

        //m_pNextNpc = GameObject.Find("Vasily").GetComponent<NPC>();
    }

    // Update is called once per frame
    void Update()
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

        if (_Quest.AccessQuestStart == false) // 최초 시작 
        {
            if (_Message.Length > 1 && m_iMessageIndex == 0) // 처음 메세지 
                m_pMessageBar.DisablePrevButton(); // 이전 버튼 비활성화 
            else
                m_pMessageBar.EnabledPrevButton(); // 다음 버튼 활성화 

            if (_Message.Length > 1 && m_iMessageIndex == _Message.Length - 1) // 마지막일 때 
            {
                m_pMessageBar.DisableNextButton();

                m_pMessageBar.EnabledQuestButton();
            }
            else // 2번째 메세지 이상일시 
            {
                m_pMessageBar.EnabledNextButton();

                m_pMessageBar.DisableQuestButton();
            }

            m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];
        }
        else
        {
            //_Quest.AccessQuestComple = true;

            m_pMessageBar.DisableButton();

            m_pMessageBar.DisableQuestButton();

            m_pMessageBar.AccessMessage = QuestEndMessage;
        }

        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

        m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);
    }
}







//public class Lucas : NPC
//{
//    [SerializeField] private string QuestEndMessage = string.Empty;

//    [SerializeField] private GameObject _MessageBar = null;

//    [SerializeField] private LayerMask _LayerMask = 0;

//    private GameobjectManager m_pGameObejctManager = GameobjectManager.GetInstance();

//    private RectTransform m_pCanvas = null;

//    private bool m_bIsClick = false;

//    private bool m_bIsOnClick = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_pGameObejctManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

//        m_pCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

//        m_pSpriteRenderer = GetComponent<SpriteRenderer>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        CilckNPC();
//    }

//    private void CilckNPC()
//    {
//        MessageBox();

//        Vector3 _WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//        Ray2D _Ray2D = new Ray2D();

//        _Ray2D.origin = _WorldMousePosition;

//        _Ray2D.direction = Vector2.zero;

//        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _LayerMask); // 여기서 문제네 ??

//        if (_Hit2D.collider != null && Input.GetMouseButtonDown(0) && m_bIsClick == false)
//        {
//            if (_Hit2D.collider.tag != "NPC" && _Hit2D.collider.gameObject.name != gameObject.name)
//                return;

//            m_bIsClick = true;
//        }

//        else if (_Hit2D.collider != null && Input.GetMouseButtonDown(0))
//        {
//            if (_Hit2D.collider.tag != "NPC" && _Hit2D.collider.gameObject.name != gameObject.name)
//                return;

//            m_bIsOnClick = true;
//        }

//        if ((null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false) && m_bIsOnClick == true)
//        {
//            m_pMessageBar = m_pGameObejctManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas).GetComponent<MessageBox>();

//            m_pMessageBar.AccessNpc = this;

//            if (_Message.Length == 1)
//                m_pMessageBar.DisableButton();

//            m_bIsClick = m_bIsOnClick = false;
//        }
//    }

//    public void MessageBox()
//    {
//        if (null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false)
//            return;

//        if (_Quest.AccessQuestStart == false) // 최초 시작 
//        {
//            if (_Message.Length > 1 && m_iMessageIndex == 0) // 처음 메세지 
//                m_pMessageBar.DisablePrevButton(); // 이전 버튼 비활성화 
//            else
//                m_pMessageBar.EnabledPrevButton(); // 다음 버튼 활성화 

//            if (_Message.Length > 1 && m_iMessageIndex == _Message.Length - 1) // 마지막일 때 
//            {
//                m_pMessageBar.DisableNextButton();

//                m_pMessageBar.EnabledQuestButton();
//            }
//            else // 2번째 메세지 이상일시 
//            {
//                m_pMessageBar.EnabledNextButton();

//                m_pMessageBar.DisableQuestButton();
//            }

//            m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];
//        }
//        else
//        {
//            m_pMessageBar.DisablePrevButton(); // 이전 버튼 비활성화 

//            m_pMessageBar.EnabledPrevButton(); // 다음 버튼 활성화 

//            m_pMessageBar.DisableQuestButton();

//            m_pMessageBar.AccessMessage = QuestEndMessage;
//        }

//        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);
//    }
//}