using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seed : NPC
{
    [SerializeField] private ICON[] SellerItemIcons = null;

    [SerializeField] private GameObject _Shop = null;

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

    private void OnClickShop()
    {
        if(m_pShop != null)
            m_pShop.AccessSprire.sprite = m_pSpriteRenderer.sprite;

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

            m_bIsClick = false;

            m_bIsOnClick = true;
        }

        if(m_bIsOnClick == true) // 상점을 생성한다 !! 
        {
            m_bIsOnClick = false;

            if (m_pShop == null)
            {
                GameObject _PoolingObject = m_pGameobjectManager.GameObejctPooling("Shop", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas.transform);

                _PoolingObject.transform.localScale = new Vector3(5.0f, 5.0f);

                m_pShop = _PoolingObject.GetComponent<Shop>();

                if (m_pShop.AccessShop != null)
                    m_pShop.ClearIConList();

                m_pShop.AccessShop = SellerItemIcons;

                m_pShop.AccessNpc = this;
            }
        }
    }

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




//public class Seed : NPC
//{
//    [SerializeField] private ICON[] SellerItemIcons = null;

//    [SerializeField] private GameObject _Shop = null;

//    [SerializeField] private LayerMask _LayerMask = 0;

//    private GameObject m_pCanvas = null;

//    private SpriteRenderer m_pSpriteRenderer = null;

//    private Animator m_pAnimator = null;

//    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

//    private Shop m_pShop = null;

//    private float m_fTimeAcc = 0.0f;

//    private bool m_bIsClick = false;

//    private bool m_bIsOnClick = false;

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

//        m_pCanvas = GameObject.Find("Canvas");

//        m_pGameobjectManager.OriginalGamgObjectInsert("Shop", _Shop);

//        m_pAnimator = GetComponent<Animator>();
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        AnimationUpdate();

//        OnClickShop();
//    }

//    private void OnClickShop()
//    {
//        if (m_pShop != null)
//            m_pShop.AccessSprire.sprite = m_pSpriteRenderer.sprite;

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

//            m_bIsClick = false;

//            m_bIsOnClick = true;
//        }

//        if (m_bIsOnClick == true) // 상점을 생성한다 !! 
//        {
//            m_bIsOnClick = false;

//            if (m_pShop == null)
//            {
//                GameObject _PoolingObject = m_pGameobjectManager.GameObejctPooling("Shop", Vector3.zero, Vector3.zero, Quaternion.identity, m_pCanvas.transform);

//                _PoolingObject.transform.localScale = new Vector3(5.0f, 5.0f);

//                m_pShop = _PoolingObject.GetComponent<Shop>();

//                if (m_pShop.AccessShop != null)
//                    m_pShop.ClearIConList();

//                m_pShop.AccessShop = SellerItemIcons;

//                m_pShop.AccessNpc = this;
//            }
//        }
//    }

//    private void AnimationUpdate()
//    {
//        if (gameObject.name != "Lucy")
//            return;

//        m_fTimeAcc += Time.deltaTime;

//        if (m_fTimeAcc >= 10.0f)
//        {
//            m_pAnimator.SetTrigger("SMILE");

//            m_fTimeAcc = 0.0f;
//        }
//    }

//    public override void ResetButton()
//    {
//        m_pShop = null;
//    }

//    private void OnDestroy()
//    {
//        m_pGameobjectManager = null;

//        m_pCanvas = null;

//        m_pShop = null;

//        m_pAnimator = null;
//    }
//}