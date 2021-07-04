using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vasily : NPC // 중간 다리 역할을 수행하겠네 ;;
{
    [SerializeField] private QUEST _NextQuest = null;

    [SerializeField] private GameObject _MessageBar = null;

    [SerializeField] private string _defaultMessage = string.Empty;

    [SerializeField] private string _FirstQuestFinalMessage = string.Empty;

    [SerializeField] private string[] _NextQuestMessage = null;

    [SerializeField] private LayerMask _LayerMask = 0;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

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

        if (_GameObject != null)
        {
            _GameObject = _GameObject.transform.Find("Quest Window Box").gameObject;

            m_pQuestUI = _GameObject.transform.Find("Quest List").GetComponent<QuestList>();
        }

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pGameObjectManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

        if (m_bIsNextQuest == true)
            _Quest = _NextQuest;
        else
            _NextQuest.AccessQuestComple = _NextQuest.AccessQuestStart = _NextQuest.AccessQuestFinal = false;
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
        if (m_pMessageBar != null && m_bIsNextQuest == true && _Quest.AccessQuestStart == false)
            m_pMessageBar.ResetYesButton();

        MessageBox();

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
            m_pMessageBar = m_pGameObjectManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas.transform).GetComponent<MessageBox>();

            m_pMessageBar.AccessNpc = this;

            if (_Message.Length == 1)
                m_pMessageBar.DisableButton();

            if (m_bIsNextQuest == true)
                m_pMessageBar.ResetYesButton();

            m_bIsClick = m_bIsOnClick = false;
        }
    }

    public void MessageBox()
    {
        if (null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false)
            return;

        if(m_bIsNextQuest == false)
        {
            if (null != _Quest && _Quest.AccessQuestFinal == false)
            {
                m_pMessageBar.AccessMessage = _FirstQuestFinalMessage;

                m_pMessageBar.DisableButton();

                m_pMessageBar.ChangeYesButton();

                m_pMessageBar.EnabledQuestButton();
            }
            else if (null != _Quest && _Quest.AccessQuestFinal == true)
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                m_pMessageBar.AccessMessage = _defaultMessage;
            }
            else if(null == _Quest)
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                m_pMessageBar.AccessMessage = _defaultMessage;
            }
        }
        else // 다음 퀘스트를 수행 시 
        {
            if (_Quest != null && _Quest.AccessQuestStart == false && _Quest.AccessQuestFinal == false) // 퀘스트 시작전이라면 
            {
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

                m_pMessageBar.AccessMessage = _NextQuestMessage[m_iMessageIndex]; // 여기서 문제가 되나?
            }
            else if (_Quest != null && _Quest.AccessQuestStart == true && _Quest.AccessQuestComple == false)
            {
                m_pMessageBar.DisableQuestButton();

                m_pMessageBar.DisableButton();

                m_pMessageBar.AccessMessage = "어서가서 마노를 저지하여 원활하게 출항 할 수 있게 도와주게 !!";
            }
            else if (_Quest != null && _Quest.AccessQuestStart == true && _Quest.AccessQuestComple == true)
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.ChangeYesButton();

                m_pMessageBar.EnabledQuestButton();

                m_pMessageBar.AccessMessage = "고생했어 !! 이제 빅토리아 아일랜드로 떠나세";
            }
            else
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                m_pMessageBar.AccessMessage = _defaultMessage;
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

















//public class Vasily : NPC // 중간 다리 역할을 수행하겠네 ;;
//{
//    [SerializeField] private QUEST _NextQuest = null;

//    [SerializeField] private GameObject _MessageBar = null;

//    [SerializeField] private string _defaultMessage = string.Empty;

//    [SerializeField] private string _FirstQuestFinalMessage = string.Empty;

//    [SerializeField] private string[] _NextQuestMessage = null;

//    [SerializeField] private LayerMask _LayerMask = 0;

//    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

//    private GameObject m_pCanvas = null;

//    private QuestList m_pQuestUI = null;

//    private bool m_bIsClick = false;

//    private bool m_bIsNextQuest = false;

//    public bool AccessNextQuest
//    {
//        get { return m_bIsNextQuest; }

//        set
//        {
//            m_bIsNextQuest = value;

//            if (m_bIsNextQuest == true) // 해당 값이 들어온다면 이 때 
//                _Quest = _NextQuest;
//        }
//    }

//    // 해당 퀘스트를 받으면 더블클릭시 
//    private void Start()
//    {
//        m_pCanvas = GameObject.Find("Canvas");

//        GameObject _GameObject = GameObject.Find("Quest UI");

//        if (_GameObject != null)
//        {
//            _GameObject = _GameObject.transform.Find("Quest Window Box").gameObject;

//            m_pQuestUI = _GameObject.transform.Find("Quest List").GetComponent<QuestList>();
//        }

//        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

//        m_pGameObjectManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);
//    }


//    private void Update()
//    {
//        FindMessageBox();
//    }

//    private void OnDestroy()
//    {
//        m_pGameObjectManager = null;

//        m_pSpriteRenderer = null;

//        m_pQuestUI = null;

//        m_pCanvas = null;
//    }

//    private void FindMessageBox()
//    {
//        MessageBox();

//        Vector3 _WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//        Ray2D _Ray2D = new Ray2D();

//        _Ray2D.origin = _WorldMousePosition;

//        _Ray2D.direction = Vector2.zero;

//        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _LayerMask); // 여기서 문제네 ??

//        if (_Hit2D.collider != null && Input.GetMouseButtonDown(0) && m_bIsClick == false)
//        {
//            if (_Hit2D.collider.tag != "NPC" || _Hit2D.collider.gameObject.name != gameObject.name)
//                return;

//            m_bIsClick = true;
//        }

//        else if (_Hit2D.collider != null && Input.GetMouseButtonDown(0))
//        {
//            if (_Hit2D.collider.tag != "NPC" || _Hit2D.collider.gameObject.name != gameObject.name)
//                return;

//            m_bIsOnClick = true;
//        }

//        if ((null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false) && m_bIsOnClick == true)
//        {
//            m_pMessageBar = m_pGameObjectManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas.transform).GetComponent<MessageBox>();

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

//        if (_Quest != null && _Quest.AccessQuestFinal == true)
//        {
//            if (m_bIsNextQuest == true)
//            {

//            }
//            else
//            {

//            }
//        }
//        else if (null != _Quest)
//        {
//            m_pMessageBar.AccessMessage = _FirstQuestFinalMessage;

//            m_pMessageBar.DisableButton();

//            m_pMessageBar.ChangeYesButton();

//            m_pMessageBar.EnabledQuestButton();
//        }
//        else
//        {
//            m_pMessageBar.DisableButton();

//            m_pMessageBar.DisableQuestButton();

//            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

//            m_pMessageBar.AccessMessage = _defaultMessage;

//            return;
//        }

//        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);
//    }

//    private void NextQuestMessage()
//    {
//        if (_Quest.AccessQuestStart == false)
//        {
//            m_pMessageBar.ResetYesButton();

//            if (_Message.Length > 1 && m_iMessageIndex == 0)
//                m_pMessageBar.DisablePrevButton();
//            else
//                m_pMessageBar.EnabledPrevButton();

//            if (_Message.Length > 1 && m_iMessageIndex == _Message.Length - 1)
//            {
//                m_pMessageBar.DisableNextButton();

//                m_pMessageBar.EnabledQuestButton();
//            }
//            else
//            {
//                m_pMessageBar.EnabledNextButton();

//                m_pMessageBar.DisableQuestButton();
//            }

//            m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];
//        }

//        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);
//    }
//}