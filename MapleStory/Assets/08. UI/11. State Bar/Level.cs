using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level : MonoBehaviour // 플레이어의 레벨 이미지를 담당하는 클래스입니다.
{
    [SerializeField] private Sprite[] _Sprites = null; // 레벨 숫자들을 가지고 잇는 스프라이트 변수입니다.

    private Image m_pImage = null; // 실제 출력될 UI 이미지 변수 

    public void LevelUpdate(Player _Player) // 인자로 받는 플레이어의 레벨을 갱신하여 UI 이미지로 출력하기 위한 함수입니다.
    {
        if (null == _Player) // 플레이어 정보가 존재하지 않는다면 
            return; // 종료 

        if (null == m_pImage) // UI 이미지 정보가 존재하지 않는다면 
            m_pImage = GetComponent<Image>(); // 찾는다 

        int _PlayerLevel = _Player.GetLevel - 1; // 현재 레벨에서 -1하여 인덱스를 조정

        if (_PlayerLevel >= _Sprites.Length - 1) // 인덱스가 스프라이트 이미지 개수보다 크다면 
            _PlayerLevel = _Sprites.Length - 1; // 강제로 조정
        else if (_PlayerLevel <= 0) // 0보다 작거나 같다면 
            _PlayerLevel = 0; // 레벨을 1로 표현하기 위한 인덱스 조정

        m_pImage.sprite = _Sprites[_PlayerLevel]; // 최종 스프라이트 인덱스 값으로 출력할 UI 이미지에 스프라이트 전달
    }
}
