using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터와 슬래쉬 이펙트가 충돌시 받은 피해량을 나타내는 피격 폰트 클래스입니다.

public class HitFont : MonoBehaviour // 피격 폰트 클래스입니다.
{
    [SerializeField] private string _DeleteName = string.Empty; // 오브젝트 풀링에서 사용할 이름 

    [SerializeField] Sprite[] _NumberSprite = null; // 숫자 스프라이트

    private SpriteRenderer[] m_pNumberSpriteRenderer = null; // 실제로 적용될 숫자들의 스프라이트 랜더러 배열 정보들 

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();


    // 인자로 들어오는 데미지로
    public void SelectNumberFont(float _iDamege)
    {
        if(null == m_pNumberSpriteRenderer) // 배열의 정보가 존재하지 않는다면 
        {
            m_pNumberSpriteRenderer = new SpriteRenderer[transform.childCount]; // 배열을 할당후

            for (int i = 0; i < m_pNumberSpriteRenderer.Length; ++i) // 스프라이트 정보를 순회하면서 
                m_pNumberSpriteRenderer[i] = transform.GetChild(i).GetComponent<SpriteRenderer>(); // 정보 삽입
        }

        if (_iDamege <= 0) // 데미지가 0이거나 그 이하로 들어올 경우 
        {
            Destroy(gameObject); // 파괴 

            return;
        }

        // Exp.cs 클래스 OnExpSetSprite 함수 코드와 비슷합니다.

        float _SelectNumber = 100.0f; // 백 단위 먼저 

        int _iIndex = 0;

        if (_iDamege / _SelectNumber < 1)
            m_pNumberSpriteRenderer[0].gameObject.SetActive(false);
        else
        {
            if (m_pNumberSpriteRenderer[0].gameObject.activeSelf == false)
                m_pNumberSpriteRenderer[0].gameObject.SetActive(true);

            _iIndex = (int)_iDamege / (int)_SelectNumber;

            m_pNumberSpriteRenderer[0].sprite = _NumberSprite[_iIndex];

            _iDamege -= _iIndex * (int)_SelectNumber;
        }

        _SelectNumber /= 10;

        if (_iDamege % _SelectNumber < 0 && _iDamege / _SelectNumber <= 0)
            m_pNumberSpriteRenderer[1].gameObject.SetActive(false);
        else
        {
            if (m_pNumberSpriteRenderer[1].gameObject.activeSelf == false)
                m_pNumberSpriteRenderer[1].gameObject.SetActive(true);

            _iIndex = (int)_iDamege / (int)_SelectNumber;

            m_pNumberSpriteRenderer[1].sprite = _NumberSprite[_iIndex];

            _iDamege -= _iIndex * (int)_SelectNumber;
        }

        _SelectNumber /= 10;

        _iIndex = (int)_iDamege / (int)_SelectNumber;

        m_pNumberSpriteRenderer[2].sprite = _NumberSprite[_iIndex];
    }

    public void Update()
    {
        if(m_pNumberSpriteRenderer[m_pNumberSpriteRenderer.Length - 1].color.a <= 0) // 알파값이 투명해질경우 
        {
            m_pGameobjectManager.Remove(_DeleteName, gameObject);

            for (int i = 0; i < m_pNumberSpriteRenderer.Length; ++i) // 각 스프라이트 랜더러 배열을 순회하면서 
            {
                m_pNumberSpriteRenderer[i].color = new Color(m_pNumberSpriteRenderer[i].color.r, m_pNumberSpriteRenderer[i].color.g,
                    m_pNumberSpriteRenderer[i].color.b, 1.0f); // 다시 알파값을 복수 
            }

            return;
        }

        // 폰트들의 숫자가 생성 후 

        for (int i = 0; i < m_pNumberSpriteRenderer.Length; ++i) // 각 폰트들을 순회하면서 
        {
            float _Alpha = m_pNumberSpriteRenderer[i].color.a - 0.5f * Time.deltaTime; // 알파값을 조금씩 제거 후 

            m_pNumberSpriteRenderer[i].color = new Color(m_pNumberSpriteRenderer[i].color.r, m_pNumberSpriteRenderer[i].color.g,
                m_pNumberSpriteRenderer[i].color.b, _Alpha); // 실질적 적용
        }

        transform.position += Vector3.up * 1.0f * Time.deltaTime; // 데미지 폰트 위치를 Y축으로 이동
    }

    private void OnDestroy()
    {
        m_pGameobjectManager = null;
    }
}