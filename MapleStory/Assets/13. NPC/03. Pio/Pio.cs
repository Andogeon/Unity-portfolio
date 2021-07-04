using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PIOSTATE
{
    PIO_IDLE,
    PIO_LEFT_RUN,
    PIO_RIGHT_RUN,
    PIO_HAIR
}


// 2020.10.11 Pio class

public class Pio : NPC
{
    [SerializeField] private GameObject _MessageBar = null;

    [SerializeField] private LayerMask _LayerMask = 0;

    [SerializeField] string _MessageValue = string.Empty;

    [SerializeField] private Vector2 _LeftMovingPosition = Vector2.zero;

    [SerializeField] private Vector2 _RightMovingPosition = Vector2.zero;

    private PIOSTATE m_ePioState = PIOSTATE.PIO_LEFT_RUN;

    private PIOSTATE m_eOldPioState = PIOSTATE.PIO_LEFT_RUN;

    private Animator m_pAnimator = null;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private GameObject m_pCanvas = null;

    private float m_fTimeAcc = 0.0f;

    private bool m_bIsClick = false;

    private GameObject m_pNameBar = null;

    private void Start()
    {
        m_pAnimator = GetComponent<Animator>();

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pCanvas = GameObject.Find("Canvas");

        m_pGameObjectManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

        m_pNameBar = transform.GetChild(0).gameObject;
    }

    private void Update()
    {
        AnimationUpdate();

        if (m_pMessageBar != null)
        {
            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

            m_pMessageBar.SetNameImage(_NameSpriteRenderer.sprite, _NameBarSpriteRenderer.sprite);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

            m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];

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
            m_pMessageBar = m_pGameObjectManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas.transform).GetComponent<MessageBox>();

            m_pMessageBar.AccessNpc = this;

            m_pMessageBar.DisableButton();

            m_pMessageBar.DisableQuestButton();

            m_bIsClick = m_bIsOnClick = false;
        }
    }

    private void AnimationUpdate()
    {
        m_pNameBar.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);

        switch (m_ePioState)
        {
            case PIOSTATE.PIO_IDLE:
                m_pAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("HAIR", false);
                m_fTimeAcc += Time.deltaTime;
                if (m_fTimeAcc >= 1.0f)
                {
                    m_ePioState = m_eOldPioState;

                    m_fTimeAcc = 0.0f;
                }
                break;
            case PIOSTATE.PIO_HAIR:
                m_pAnimator.SetBool("RUN", false);
                m_pAnimator.SetBool("HAIR", true);
                m_fTimeAcc += Time.deltaTime;
                if (m_fTimeAcc >= 1.0f)
                {
                    m_ePioState = PIOSTATE.PIO_IDLE;

                    m_fTimeAcc = 0.0f;
                }
                break;
            case PIOSTATE.PIO_LEFT_RUN:
            case PIOSTATE.PIO_RIGHT_RUN:
                MovingRun();
                break;
        }
    }

    private void MovingRun()
    {
        Vector3 _Direction = Vector3.zero;

        m_pAnimator.SetBool("RUN", true);
        m_pAnimator.SetBool("HAIR", false);

        switch (m_ePioState)
        {
            case PIOSTATE.PIO_LEFT_RUN:
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                transform.position += Vector3.left * 3.0f * Time.deltaTime;
                _Direction = transform.position - (Vector3)_LeftMovingPosition;
                if (_Direction.magnitude <= 6.2f)
                    m_eOldPioState = m_ePioState = PIOSTATE.PIO_RIGHT_RUN;
                break;
            case PIOSTATE.PIO_RIGHT_RUN:
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                transform.position += Vector3.right * 3.0f * Time.deltaTime;
                _Direction = transform.position - (Vector3)_RightMovingPosition;
                if (_Direction.magnitude <= 6.2f)
                    m_eOldPioState = m_ePioState = PIOSTATE.PIO_LEFT_RUN;
                break;
        }

        m_fTimeAcc += Time.deltaTime;

        if (m_fTimeAcc >= 2.5f)
        {
            m_ePioState = PIOSTATE.PIO_HAIR;

            m_fTimeAcc = 0.0f;
        }
    }

    private void OnDestroy()
    {
        m_pGameObjectManager = null;

        m_pAnimator = null;

        _MessageValue = string.Empty;

        m_pSpriteRenderer = null;
    }
}




//public class Pio : NPC
//{
//    [SerializeField] private GameObject _MessageBar = null;

//    [SerializeField] private LayerMask _LayerMask = 0;

//    [SerializeField] string _MessageValue = string.Empty;

//    [SerializeField] private Vector2 _LeftMovingPosition = Vector2.zero;

//    [SerializeField] private Vector2 _RightMovingPosition = Vector2.zero;

//    private PIOSTATE m_ePioState = PIOSTATE.PIO_LEFT_RUN;

//    private PIOSTATE m_eOldPioState = PIOSTATE.PIO_LEFT_RUN;

//    private SpriteRenderer m_pSpriteRenderer = null;

//    private Animator m_pAnimator = null;

//    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

//    private GameObject m_pCanvas = null;

//    private float m_fTimeAcc = 0.0f;

//    private bool m_bIsClick = false;

//    private bool m_bIsOnClick = false;

//    private void Start()
//    {
//        m_pAnimator = GetComponent<Animator>();

//        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

//        m_pCanvas = GameObject.Find("Canvas");
//    }

//    private void Update()
//    {
//        AnimationUpdate();

//        if (m_pMessageBar != null)
//        {
//            m_pMessageBar.AccessMainImage.sprite = m_pSpriteRenderer.sprite;

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, m_pSpriteRenderer.sprite.rect.width);

//            m_pMessageBar.AccessMainImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, m_pSpriteRenderer.sprite.rect.height);

//            m_pMessageBar.AccessMessage = _Message[m_iMessageIndex];

//            return;
//        }

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
//            if (_Hit2D.collider.tag != "NPC")
//                return;

//            m_bIsOnClick = true;
//        }

//        if ((null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false) && m_bIsOnClick == true)
//        {
//            m_pMessageBar = m_pGameObjectManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas.transform).GetComponent<MessageBox>();

//            m_pMessageBar.AccessNpc = this;

//            m_pMessageBar.DisableButton();

//            m_pMessageBar.DisableQuestButton();

//            m_bIsClick = m_bIsOnClick = false;
//        }
//    }

//    private void AnimationUpdate()
//    {
//        switch (m_ePioState)
//        {
//            case PIOSTATE.PIO_IDLE:
//                m_pAnimator.SetBool("RUN", false);
//                m_pAnimator.SetBool("HAIR", false);
//                m_fTimeAcc += Time.deltaTime;
//                if (m_fTimeAcc >= 1.0f)
//                {
//                    m_ePioState = m_eOldPioState;

//                    m_fTimeAcc = 0.0f;
//                }
//                break;
//            case PIOSTATE.PIO_HAIR:
//                m_pAnimator.SetBool("RUN", false);
//                m_pAnimator.SetBool("HAIR", true);
//                m_fTimeAcc += Time.deltaTime;
//                if (m_fTimeAcc >= 1.0f)
//                {
//                    m_ePioState = PIOSTATE.PIO_IDLE;

//                    m_fTimeAcc = 0.0f;
//                }
//                break;
//            case PIOSTATE.PIO_LEFT_RUN:
//            case PIOSTATE.PIO_RIGHT_RUN:
//                MovingRun();
//                break;
//        }
//    }

//    private void MovingRun()
//    {
//        Vector3 _Direction = Vector3.zero;

//        m_pAnimator.SetBool("RUN", true);
//        m_pAnimator.SetBool("HAIR", false);

//        switch (m_ePioState)
//        {
//            case PIOSTATE.PIO_LEFT_RUN:
//                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
//                transform.position += Vector3.left * 3.0f * Time.deltaTime;
//                _Direction = transform.position - (Vector3)_LeftMovingPosition;
//                if (_Direction.magnitude <= 6.2f)
//                    m_eOldPioState = m_ePioState = PIOSTATE.PIO_RIGHT_RUN;
//                break;
//            case PIOSTATE.PIO_RIGHT_RUN:
//                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
//                transform.position += Vector3.right * 3.0f * Time.deltaTime;
//                _Direction = transform.position - (Vector3)_RightMovingPosition;
//                if (_Direction.magnitude <= 6.2f)
//                    m_eOldPioState = m_ePioState = PIOSTATE.PIO_LEFT_RUN;
//                break;
//        }

//        m_fTimeAcc += Time.deltaTime;

//        if (m_fTimeAcc >= 2.5f)
//        {
//            m_ePioState = PIOSTATE.PIO_HAIR;

//            m_fTimeAcc = 0.0f;
//        }
//    }

//    private void OnDestroy()
//    {
//        m_pGameObjectManager = null;

//        m_pAnimator = null;

//        _MessageValue = string.Empty;

//        m_pSpriteRenderer = null;
//    }
//}