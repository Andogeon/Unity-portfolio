using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniPlayer : MonoBehaviour
{
    [SerializeField] private GameObject _UserObject = null;

    [SerializeField] private SpriteRenderer _WorldMapRenderer = null;

    [SerializeField] private Vector2 _Offset = Vector2.zero;

    private GameObject m_pMovingObject = null;

    private RectTransform m_pImageTransfrom = null;

    private RectTransform m_pParnentTransform = null;

    private float m_fHeight = 0.0f;

    private float m_fWidth = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        m_pImageTransfrom = GetComponent<RawImage>().rectTransform;

        m_pParnentTransform = GameObject.Find("Mini Map").GetComponent<Image>().rectTransform;

        m_fWidth = _WorldMapRenderer.bounds.max.x;

        m_fHeight = _WorldMapRenderer.bounds.max.y - _WorldMapRenderer.bounds.min.y;

        m_pMovingObject = _UserObject;
    }

    // Update is called once per frame
    void Update()
    {
        MoveingAvatar();
    }

    private void MoveingAvatar()
    {
        if (m_pMovingObject == null)
            m_pMovingObject = GameObject.Find("Player");

        Vector2 _MiniPosition = Vector2.zero;

        _MiniPosition.x = (m_pMovingObject.transform.position.x / m_fWidth) * m_pParnentTransform.rect.xMax;

        _MiniPosition.y = (m_pMovingObject.transform.position.y / m_fHeight) * m_pParnentTransform.rect.height;

        m_pImageTransfrom.localPosition = _MiniPosition + _Offset;
    }
}







//public class MiniPlayer : MonoBehaviour
//{
//    [SerializeField] private GameObject _UserObject = null;

//    //[SerializeField] private SpriteRenderer _Renderer = null;

//    [SerializeField] private BoxCollider2D m_pBoxCollision = null;

//    [SerializeField] private Vector2 _Offset = Vector2.zero;

//    private GameObject m_pMovingObject = null;

//    private RectTransform m_pImageTransfrom = null;

//    private RectTransform m_pParnentTransform = null;

//    private float m_fHeight = 0.0f;

//    private float m_fWidth = 0.0f;

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_pImageTransfrom = GetComponent<RawImage>().rectTransform;

//        m_pParnentTransform = GameObject.Find("Mini Map").GetComponent<Image>().rectTransform;

//        m_fWidth = m_pBoxCollision.bounds.max.x;

//        m_fHeight = m_pBoxCollision.bounds.max.y - m_pBoxCollision.bounds.min.y;

//        m_pMovingObject = _UserObject;
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        MoveingAvatar();
//    }

//    private void MoveingAvatar()
//    {
//        if (m_pMovingObject == null)
//            m_pMovingObject = GameObject.Find("Player");

//        m_fHeight = m_pBoxCollision.bounds.max.y - m_pBoxCollision.bounds.min.y;

//        Vector2 _MiniPosition = Vector2.zero;

//        _MiniPosition.x = (m_pMovingObject.transform.position.x / m_fWidth) * m_pParnentTransform.rect.xMax;

//        _MiniPosition.y = (m_pMovingObject.transform.position.y / m_fHeight) * m_pParnentTransform.rect.height;

//        m_pImageTransfrom.localPosition = _MiniPosition + _Offset;
//    }
//}
