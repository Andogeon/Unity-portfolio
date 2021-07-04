using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mai : NPC
{
    [SerializeField] private string QuestingMessage = string.Empty;

    [SerializeField] private string QuestCompleMessage = string.Empty;

    [SerializeField] private string QuestEndMessage = string.Empty;

    [SerializeField] private GameObject _MessageBar = null;

    [SerializeField] private LayerMask _LayerMask = 0;

    private GameobjectManager m_pGameObejctManager = GameobjectManager.GetInstance();

    private RectTransform m_pCanvas = null;

    private bool m_bIsClick = false;

    //public static GameObject m_pMaiObject = null;

    // Start is called before the first frame update
    void Start()
    {
        //if (null == m_pMaiObject)
        //{
        //    m_pMaiObject = this.gameObject;

        //    GameObject.DontDestroyOnLoad(this.gameObject);
        //}

        //UnityEngine.SceneManagement.SceneUtility.

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
        if(_Quest.AccessQuestFinal == true && m_pMessageBar != null)
        {
            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

            m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

            m_pMessageBar.AccessMessage = QuestEndMessage;

            m_pMessageBar.DisablePrevButton();

            m_pMessageBar.DisableNextButton();

            m_pMessageBar.DisableQuestButton();
        }

        // 생성 되면 더 이상의 레이캐스트는 무시한다 

        else if (m_pMessageBar != null && m_pMessageBar.gameObject.activeSelf == true && _Quest.AccessQuestStart == false)
        {
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

            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

            m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

            if (null != m_pMessageBar)
                m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];

            return;
        }
        else if(m_pMessageBar != null && m_pMessageBar.gameObject.activeSelf == true 
            && _Quest.AccessQuestStart == true && _Quest.AccessQuestComple == false)
        {
            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

            m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

            m_pMessageBar.AccessMessage = QuestingMessage;

            m_pMessageBar.DisablePrevButton();

            m_pMessageBar.DisableNextButton();

            m_pMessageBar.DisableQuestButton();

            return;
        }

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

            m_pMessageBar.ChangeYesButton();

            m_pMessageBar.EnabledQuestButton();

            return;
        }

        

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
    }
}




//public class Mai : NPC
//{
//    [SerializeField] private string QuestingMessage = string.Empty;

//    [SerializeField] private string QuestCompleMessage = string.Empty;

//    [SerializeField] private string QuestEndMessage = string.Empty;

//    [SerializeField] private GameObject _MessageBar = null;

//    [SerializeField] private LayerMask _LayerMask = 0;

//    private GameobjectManager m_pGameObejctManager = GameobjectManager.GetInstance();

//    private RectTransform m_pCanvas = null;

//    private SpriteRenderer m_pSpriteRenderer = null;

//    private bool m_bIsClick = false;

//    private bool m_bIsOnClick = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_pGameObejctManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

//        m_pCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

//        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

//        if (_Quest.AccessQuestType == QUESTTYPE.QUEST_ITEM)
//        {
//            ItemQuest _ItemQuest = _Quest as ItemQuest;

//            if (null == _ItemQuest)
//                return;

//            GameObject _Inventory = GameObject.Find("Inventory UI");

//            _Inventory = _Inventory.transform.Find("Inventory Box").gameObject;

//            _Inventory = _Inventory.transform.Find("Inventory").gameObject;

//            Inventory _QuestInventory = _Inventory.transform.Find("Other Window").GetComponent<Other_Inventory>();

//            _ItemQuest.AccessInventory = _QuestInventory;

//            _ItemQuest.AccessNPC = this;
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (_Quest.AccessQuestFinal == true && m_pMessageBar != null)
//        {
//            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

//            m_pMessageBar.AccessMessage = QuestEndMessage;

//            m_pMessageBar.DisablePrevButton();

//            m_pMessageBar.DisableNextButton();

//            m_pMessageBar.DisableQuestButton();
//        }

//        // 생성 되면 더 이상의 레이캐스트는 무시한다 

//        else if (m_pMessageBar != null && m_pMessageBar.gameObject.activeSelf == true && _Quest.AccessQuestStart == false)
//        {
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

//            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

//            if (null != m_pMessageBar)
//                m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];

//            return;
//        }
//        else if (m_pMessageBar != null && m_pMessageBar.gameObject.activeSelf == true
//            && _Quest.AccessQuestStart == true && _Quest.AccessQuestComple == false)
//        {
//            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

//            m_pMessageBar.AccessMessage = QuestingMessage;

//            m_pMessageBar.DisablePrevButton();

//            m_pMessageBar.DisableNextButton();

//            m_pMessageBar.DisableQuestButton();

//            return;
//        }

//        else if (m_pMessageBar != null && m_pMessageBar.gameObject.activeSelf == true &&
//            _Quest.AccessQuestStart == true && _Quest.AccessQuestComple == true)
//        {
//            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

//            m_pMessageBar.AccessMessage = QuestCompleMessage;

//            m_pMessageBar.DisablePrevButton();

//            m_pMessageBar.DisableNextButton();

//            m_pMessageBar.EnabledQuestButton();

//            return;
//        }

//        Vector3 _WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

//        Ray2D _Ray2D = new Ray2D();

//        _Ray2D.origin = _WorldMousePosition;

//        _Ray2D.direction = Vector2.zero;

//        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _LayerMask); // 여기서 문제네 ??

//        if (_Hit2D.collider != null && Input.GetMouseButtonDown(0) && m_bIsClick == false)
//        {
//            if (_Hit2D.collider.tag != "NPC")
//                return;

//            m_bIsClick = true;
//        }

//        else if (_Hit2D.collider != null && Input.GetMouseButtonDown(0))
//        {
//            if (_Hit2D.collider.tag != "NPC")
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
//}