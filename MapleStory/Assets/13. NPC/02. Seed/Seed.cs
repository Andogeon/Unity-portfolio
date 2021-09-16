using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 해당 NPC는 무기 상점까지 소유하는 클래스입니다.

public class Seed : NPC // 시드 및 루시 NPC 클래스입니다.
{
    [SerializeField] private ICON[] SellerItemIcons = null; // 판매를 할 아이콘 배열 변수 

    [SerializeField] private GameObject _Shop = null; // 더블클릭시 오픈 할 원본 상점 오브젝트 

    [SerializeField] private LayerMask _LayerMask = 0;

    private GameObject m_pCanvas = null;

    private Animator m_pAnimator = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private Shop m_pShop = null;

    private float m_fTimeAcc = 0.0f;

    private bool m_bIsClick = false;

    // Start is called before the first frame update
    void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pCanvas = GameObject.Find("Canvas");

        m_pGameobjectManager.OriginalGamgObjectInsert("Shop", _Shop);

        m_pAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        AnimationUpdate();

        OnClickShop();
    }

    // 더블 클릭시 해당 상점을 만들어 주는 함수입니다.
    private void OnClickShop()
    {
        if(m_pShop != null) // 장점이 존재한다면 
            m_pShop.AccessSprire.sprite = m_pSpriteRenderer.sprite; // 해당 NPC 스프라이트를 상점에게 전달

        Vector3 _WorldMousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Ray2D _Ray2D = new Ray2D();

        _Ray2D.origin = _WorldMousePosition;

        _Ray2D.direction = Vector2.zero;

        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, 1.0f, _LayerMask);

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

            m_bIsClick = false;

            m_bIsOnClick = true;
        }

        if(m_bIsOnClick == true) // 더블클릭시 
        {
            m_bIsOnClick = false;

            if (m_pShop == null) // 상점이 존재하지 않는다면 
            {
                //상점을 생성한다 !!

                GameObject _PoolingObject = m_pGameobjectManager.GameObejctPooling("Shop", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas.transform);

                _PoolingObject.transform.localScale = new Vector3(5.0f, 5.0f);

                m_pShop = _PoolingObject.GetComponent<Shop>();

                if (m_pShop.AccessShop != null)
                    m_pShop.ClearIConList(); // 상점의 대한 초기화

                m_pShop.AccessShop = SellerItemIcons; // 판매할 아이콘의 전달

                m_pShop.AccessNpc = this; // 상점의 대한 NPC 정보 전달
            }
        }
    }
    
    // 해당 오브젝트가 루시라면 10초 간격으로 애니메이션을 변경하는 함수입니다.

    private void AnimationUpdate()
    {
        if (gameObject.name != "Lucy")
            return;

        m_fTimeAcc += Time.deltaTime;

        if(m_fTimeAcc >= 10.0f)
        {
            m_pAnimator.SetTrigger("SMILE");

            m_fTimeAcc = 0.0f;
        }
    }

    // 상점의 대한 초기화하는 함수입니다.
    public override void ResetButton()
    {
        m_pShop = null;
    }

    private void OnDestroy()
    {
        m_pGameobjectManager = null;

        m_pCanvas = null;

        m_pShop = null;

        m_pAnimator = null;
    }
}