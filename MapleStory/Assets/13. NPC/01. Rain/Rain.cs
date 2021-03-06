using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RAINSTATE
{
    RAIN_IDLE,
    RAIN_CRY,
    RAIN_BEWILDERED
}

// 해당 클래스는 오브젝트 용도로 사용되는 클래스입니다.

public class Rain : NPC // 레인 클래스입니다
{
    [SerializeField] private GameObject _MessageBar = null; // 메세지 박스 오브젝트 

    [SerializeField] private LayerMask _LayerMask = 0;

    private GameobjectManager m_pGameObejctManager = GameobjectManager.GetInstance();

    private RectTransform m_pCanvas = null;

    private Animator m_pAnimator = null; // 애니메이션을 변경할 애니메이터 

    private bool m_bIsClick = false;

    private float m_fTimeAcc = 0.0f; // 애니메이션을 변경할 시간 변수입니다.

    private RAINSTATE m_eRainState = RAINSTATE.RAIN_IDLE; // 레인의 애니메이션을 결정할 열거형 변수입니다.

    // Start is called before the first frame update
    void Start()
    {
        m_pGameObejctManager.OriginalGamgObjectInsert("Message Bar", _MessageBar);

        m_pCanvas = GameObject.Find("Canvas").GetComponent<RectTransform>();

        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationUpdate(); // 애니메이션을 시간값에 따라 변경하기 위해서 호출 되는 함수입니다.

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

        // 마이 클래스와 동일하게 더블클릭시 메세지 박스를 생성합니다.
        if ((null == m_pMessageBar || m_pMessageBar.gameObject.activeSelf == false) && m_bIsOnClick == true)
        {
            m_pMessageBar = m_pGameObejctManager.GameObejctPooling("Message Bar", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas).GetComponent<MessageBox>();

            m_pMessageBar.AccessNpc = this; // NPC 전달 

            m_pMessageBar.DisableButton();

            m_pMessageBar.DisableQuestButton();

            m_bIsClick = m_bIsOnClick = false; // 더블클릭 및 클릭을 비활성화 
        }
    }

    // 애니메이션을 시간값에 따라서 변경을 하기 위해서 호출되는 함수입니다.
    private void AnimationUpdate()
    {
        bool _bIsChange = false;

        switch(m_eRainState)
        {
            case RAINSTATE.RAIN_IDLE: // 대기 애니메이션 
                m_pAnimator.SetBool("CRY", false);
                m_pAnimator.SetBool("BEWILDERED", false);
                _bIsChange = Timeover();
                if (_bIsChange)
                    m_eRainState = RAINSTATE.RAIN_CRY;
                break;
            case RAINSTATE.RAIN_CRY: // 울고 있는 애니메이션 
                m_pAnimator.SetBool("CRY", true);
                m_pAnimator.SetBool("BEWILDERED", false);
                _bIsChange = Timeover();
                if (_bIsChange)
                    m_eRainState = RAINSTATE.RAIN_BEWILDERED;
                break;
            case RAINSTATE.RAIN_BEWILDERED: // 당황하는 애니메이션 
                m_pAnimator.SetBool("CRY", false);
                m_pAnimator.SetBool("BEWILDERED", true);
                _bIsChange = Timeover();
                if (_bIsChange)
                    m_eRainState = RAINSTATE.RAIN_IDLE;
                break;
        }
    }

    // 시간이 1.5초가 지났다면 true를 반환하여 애니메이션을 변경하는 데 결정하는 함수입니다.
    private bool Timeover()
    {
        m_fTimeAcc += Time.deltaTime;

        if (m_fTimeAcc >= 1.5f)
        {
            m_fTimeAcc = 0.0f;

            return true;
        }

        return false;
    }
}