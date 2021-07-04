using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Romeo : NPC
{
    [SerializeField] private GameObject _MessageBar = null;

    [SerializeField] private NPC _ConnectNPCObject = null;

    [SerializeField] private LayerMask _LayerMask = 0;

    [SerializeField] private string _DefaultMessage = string.Empty;

    [SerializeField] private string _QuestStartMessage = string.Empty;

    [SerializeField] private string _QuestCompleMessage = string.Empty;

    [SerializeField] private string _QuestFinalMessage = string.Empty;

    private QUEST m_pOldQuest = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private bool m_bIsClick = false;

    private Transform m_pCanvas = null;

    private void Start()
    {
        m_pOldQuest = _ConnectNPCObject.AccessQuest;

        m_pCanvas = GameObject.Find("Canvas").transform;

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

    private void Update()
    {
        CilckNPC();
    }

    private void MessageBox()
    {
        if (m_pOldQuest == null || m_pOldQuest.AccessQuestFinal == false)
        {
            m_pMessageBar.AccessMessage = _DefaultMessage;

            m_pMessageBar.DisableButton();

            m_pMessageBar.DisableQuestButton();
        }
        else if (m_pOldQuest.AccessQuestFinal == true) // 기존 퀘스트가 완료시 
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
            else if (_Quest.AccessQuestStart == true && _Quest.AccessQuestComple == false)
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                m_pMessageBar.AccessMessage = _QuestStartMessage;
            }
            else if (_Quest.AccessQuestComple == true && _Quest.AccessQuestFinal == false)
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.ChangeYesButton();

                m_pMessageBar.EnabledQuestButton();

                m_pMessageBar.AccessMessage = _QuestCompleMessage;
            }
            else
            {
                m_pMessageBar.DisableButton();

                m_pMessageBar.DisableQuestButton();

                m_pMessageBar.AccessMessage = _QuestFinalMessage;
            }
        }

        m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

        m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

        m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);
    }

    private void CilckNPC()
    {
        if(_Quest.AccessQuestFinal == true && m_pOldQuest != null)
        {
            Vasily _VasilyNpc = _ConnectNPCObject as Vasily;

            if (_VasilyNpc != null)
            {
                _VasilyNpc.AccessNextQuest = true;

                m_pOldQuest = null;
            }
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
        m_pOldQuest = null;

        m_pCanvas = null;

        m_pGameobjectManager = null;
    }
}


//else if(_Quest.AccessQuestFinal == true && m_pOldQuest != null)
//{
//    Vasily _NextNpc = _ConnectNPCObject as Vasily;

//    if (null != _NextNpc)
//    {
//        _NextNpc.AccessNextQuest = true;

//        m_pOldQuest = null;
//    }
//}