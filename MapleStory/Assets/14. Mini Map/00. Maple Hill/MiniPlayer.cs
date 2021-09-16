using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniPlayer : MonoBehaviour // 미니맵에서 사용하는 플레이어 클래스입니다.
{
    [SerializeField] private GameObject _UserObject = null;

    [SerializeField] private SpriteRenderer _WorldMapRenderer = null; // 실제 사용될 맵 스프라이트 랜더러

    [SerializeField] private Vector2 _Offset = Vector2.zero; // 각 맵마다 위치값 보정할 변수 

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

        m_fWidth = _WorldMapRenderer.bounds.max.x; // 현재 사용될 스프라이트가 최대 가로 길이 

        m_fHeight = _WorldMapRenderer.bounds.max.y - _WorldMapRenderer.bounds.min.y; // 세로의 길이 

        m_pMovingObject = _UserObject;
    }

    // Update is called once per frame
    void Update()
    {
        MoveingAvatar();
    }

    private void MoveingAvatar()
    {
        if (m_pMovingObject == null) // 미니맵에 사용 될 플레이어의 존재가 없다면 
            m_pMovingObject = GameObject.Find("Player"); // 플레이어를 찾는다 

        Vector2 _MiniPosition = Vector2.zero;

        // 미니맵의 X축 좌표 = 플레이어의 x좌표 / 맵의 가로 크기 = 플레이어가 맵이 어디까지 와있는지 비율 

        // 플레이어가 맵이 어디까지 와있는지 비율 * 현재 미니맵의 X축 최대 크기 = 현재 미니맵에 플레이어의 위치
        
        _MiniPosition.x = (m_pMovingObject.transform.position.x / m_fWidth) * m_pParnentTransform.rect.xMax;

        // 미니맵의 Y축 좌표 = 플레이어의 Y좌표 / 맵의 세로 크기 = 플레이어가 맵이 어디까지 와있는지 비율 

        // 플레이어가 맵이 어디까지 와있는지 비율 * 현재 미니맵의 Y축 최대 크기 = 현재 미니맵에 플레이어의 위치

        _MiniPosition.y = (m_pMovingObject.transform.position.y / m_fHeight) * m_pParnentTransform.rect.height;

        m_pImageTransfrom.localPosition = _MiniPosition + _Offset; // 미니맵의 실제 적용 
    }
}