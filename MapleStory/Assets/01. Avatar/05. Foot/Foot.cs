using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Foot : PART
{
    [SerializeField] private LayerMask _LayerMask = 0;

    [SerializeField] private Vector3 _UpRayDirection = Vector3.zero;

    [SerializeField] private Vector3 _DownRayDirection = Vector3.zero;

    private Player m_pPlayer = null;

    private BoxCollider2D m_pCollision = null;

    private void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pPlayer = FindObjectOfType<Player>();

        m_pCollision = GetComponent<BoxCollider2D>();
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        m_pPlayer.AccessTerrainLine = true;

        if(m_pPlayer.AccessLadder == true)
            m_pPlayer.AccessLadder = false;

        m_pPlayer.AccessLadderState = LADDERMODE.LADDER_NORMAL;

        if(m_pPlayer.AccessPlayerState == PLAYERSTATE.PLAYER_LOPE)
            m_pPlayer.AccessPlayerState = PLAYERSTATE.PLAYER_IDLE;
    }

    public override Vector3 IDLE()
    {
        return Vector3.zero;
    }

    public override Vector3 RUN()
    {
        return Vector3.zero;
    }

    public override void SetItem(ITEM _Item)
    {
        if (null == _Item)
            return;

        if (m_pOrlItem == null || m_pOrlItem.name != _Item.name)
        {
            GameObject _pCreateObject = GameObject.Instantiate(_Item.gameObject);

            _pCreateObject.transform.SetParent(gameObject.transform);

            _pCreateObject.transform.localScale = new Vector3(1.0f, 1.0f);

            _pCreateObject.transform.localRotation = Quaternion.identity;

            _pCreateObject.transform.localPosition = new Vector3(0.0f, 0.0f);

            ITEM _pItem = _pCreateObject.GetComponent<ITEM>();

            if (m_pItemobject != null)
            {
                m_pItemobject.transform.SetParent(null);

                Destroy(m_pItemobject);

                m_pItemobject = null;

                m_pItem = null;
            }

            _pItem.AccessSetItem = _Item;

            m_pItemobject = _pCreateObject;

            m_pItem = _pItem;

            m_pOrlItem = _Item;
        }
    }
}


//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Foot : PART
//{
//    [SerializeField] private LayerMask _LayerMask = 0;

//    [SerializeField] private Vector3 _UpRayDirection = Vector3.zero;

//    [SerializeField] private Vector3 _DownRayDirection = Vector3.zero;

//    private Player m_pPlayer = null;

//    //private Rigidbody2D m_pPlayerRigid = null;

//    private BoxCollider2D m_pCollision = null;

//    [SerializeField] private float m_fLength = 0.01f;

//    private void Start()
//    {
//        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

//        m_pPlayer = FindObjectOfType<Player>();

//        m_pCollision = GetComponent<BoxCollider2D>();
//    }

//    private void Update()
//    {
//        // 이거 안되면 착지 레이어 쏘면서 착지가 되었을 때 라인이랑 발에 있는 레이랑 충돌여부 확인해야 됨 !! 

//        //Ray2D _Ray2D = new Ray2D(transform.position + _DownRayDirection, Vector3.down);

//        //Debug.DrawRay(_Ray2D.origin, _Ray2D.direction * m_fLength, Color.red);

//        if (null == m_pPlayer)
//            return;

//        if (m_pPlayer.AccessTerrainLine == false)
//            LadderDown();
//    }

//    private void LadderDown()
//    {
//        Ray2D _Ray2D = new Ray2D(transform.position + _DownRayDirection, Vector3.down);

//        RaycastHit2D _Hit2D = Physics2D.Raycast(_Ray2D.origin, _Ray2D.direction, m_fLength, _LayerMask);

//        if (_Hit2D.collider != null)
//            m_pPlayer.AccessTerrainLine = true;
//    }

//    public override Vector3 IDLE()
//    {
//        return Vector3.zero;
//    }

//    public override Vector3 RUN()
//    {
//        return Vector3.zero;
//    }

//    public override void SetItem(ITEM _Item)
//    {
//        if (null == _Item)
//            return;

//        if (m_pOrlItem == null || m_pOrlItem.name != _Item.name)
//        {
//            GameObject _pCreateObject = GameObject.Instantiate(_Item.gameObject);

//            _pCreateObject.transform.SetParent(gameObject.transform);

//            _pCreateObject.transform.localScale = new Vector3(1.0f, 1.0f);

//            _pCreateObject.transform.localRotation = Quaternion.identity;

//            _pCreateObject.transform.localPosition = new Vector3(0.0f, 0.0f);

//            ITEM _pItem = _pCreateObject.GetComponent<ITEM>();

//            if (m_pItemobject != null)
//            {
//                m_pItemobject.transform.SetParent(null);

//                Destroy(m_pItemobject);

//                m_pItemobject = null;

//                m_pItem = null;
//            }

//            _pItem.AccessSetItem = _Item;

//            m_pItemobject = _pCreateObject;

//            m_pItem = _pItem;

//            m_pOrlItem = _Item;
//        }
//    }
//}