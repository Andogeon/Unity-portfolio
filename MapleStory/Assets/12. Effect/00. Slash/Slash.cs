using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : EFFECT
{
    [SerializeField] private GameObject _DamageFont = null; // 몬스터와 충돌시 생성 될 피격 폰트

    [SerializeField] private GameObject[] _HitEffect = null;

    [SerializeField] string _EffectName = string.Empty;

    private BoxCollider2D m_pBoxCollision = null;

    private SpriteRenderer m_pSpriteRenderer = null;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private Quaternion _Rotation = Quaternion.identity;

    private bool m_bIsCollision = false;

    private ITEM m_pParnentItem = null;

    private int m_iHitCount = 0;

    public ITEM AccessSlashItem
    {
        get { return m_pParnentItem; }

        set { m_pParnentItem = value; }
    }

    private void Start()
    {
        m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pBoxCollision = GetComponent<BoxCollider2D>();

        _Rotation = transform.parent.rotation;

        transform.SetParent(null);

        if(m_pBoxCollision.enabled == false)
            m_pBoxCollision.enabled = true;

        m_pGameObjectManager.OriginalGamgObjectInsert("Sword Hit Effect", _HitEffect[0]);

        m_pGameObjectManager.OriginalGamgObjectInsert("Stick Hit Effect", _HitEffect[1]);

        m_pGameObjectManager.OriginalGamgObjectInsert("Damage Font", _DamageFont);
    }

    // 해당 업데이트 함수에서는 해당 플레이어 공격 애니메이션에 맞추어 생성 후 더 이상의 위치 이동을 막기 위해 제어했으며 

    // 알파값을 조정하여 자연스럽게 사라지게 구현했습니다.
    
    private void Update()
    {
        if (transform.parent != null) // 현재 부모 오브젝트 즉 무기가 존재한다면 
        {
            if (null == m_pParnentItem) // 실제 충돌시 공격력을 받아오기 위한 정보가 없다면 
                m_pParnentItem = transform.parent.GetComponent<ITEM>(); // 정보를 받아온다 

            transform.SetParent(null); // 무기로부터 해당 이펙트가 생성 후 위치 이동을 막기 위해 부모 연결 해재
        }

        if (m_pSpriteRenderer.color.a <= 0.0f) // 해당 이펙트가 완전 투명할시 
        {
            // 다시 불투명하게 만들고 
            m_pSpriteRenderer.color = new Color(m_pSpriteRenderer.color.r, m_pSpriteRenderer.color.g, m_pSpriteRenderer.color.b, 1.0f);

            // 게임 오브젝트 풀링에 삽입
            m_pGameObjectManager.Remove(_EffectName, gameObject);

            // 박스 충돌 비활성화
            m_pBoxCollision.enabled = false;

            return;
        }

        if (m_pBoxCollision.enabled == false && m_bIsCollision == false) // 충돌이 비활성화가 되어잇다면 
            m_pBoxCollision.enabled = true; // 다시 활성화 
    }

    // 해당 이펙트가 다시 활성화가 된다면 
    private void OnEnable()
    {
        if (null == m_pBoxCollision) // 충돌체에 대한 정보가 없다면 
            m_pBoxCollision = GetComponent<BoxCollider2D>(); // 정보를 받아온다

        if (m_pBoxCollision.enabled == false) // 충돌 여부가 비활성화 되어있다면 
            m_pBoxCollision.enabled = true; // 다시 활성화 

        if (transform.parent != null) // 부모값이 존재한다면 
            m_pParnentItem = transform.GetComponentInParent<ITEM>(); // 부모 오브젝트 즉 무기에 대한 정보를 받아온다 
    }

    private void OnDisable()
    {
        m_pParnentItem = null;

        m_iHitCount = 0;
    }

    private void OnDestroy()
    {
        m_pParnentItem = null;
    }

    // 해당 슬러쉬 이펙트가 몬스터와 충돌시 Hit 폰트 생성과 실제로 체력을 감소하게 했습니다.

    private void OnTriggerEnter2D(Collider2D collision) // 충돌 !! 
    {
        if (m_iHitCount != 0)
        {
            m_pBoxCollision.enabled = false;

            return;
        }

        MONTER _Monter = collision.GetComponent<MONTER>();

        // 해당 이펙트가 기존 오브젝트 또한 몬스터로 인식하기 때문에 몬스터의 네임 2가지를 추가하여 

        // 오로지 몬스터와 충돌이 발생 하였을 때 피격 이펙트를 생성하게 했습니다.

        if(null != _Monter && _Monter.name != "Maple Box" && _Monter.name != "Ticket Box") // 오로지 순수 몬스터 오브젝트라면
        {
            _Monter.AccessHp -= m_pParnentItem.AccessAttack; // 몬스터의 체력에서 공격력만큼 빼고 

            _Monter.SetHItAnimation(); // 

            // Hit 폰트 생성

            GameObject _Object = m_pGameObjectManager.GameObejctPooling("Damage Font", Vector3.zero, Vector3.zero, Quaternion.identity);

            _Object.transform.position = _Monter.transform.position; // 폰트의 위치를 몬스터의 위치로 고정

            HitFont _Font = _Object.GetComponent<HitFont>();

            _Font.SelectNumberFont((int)m_pParnentItem.AccessAttack); // 실제 폰트를 만들기 위해서 공격력을 전달

            m_iHitCount = 1; // 중복 공격을 막기 위한 제어
        }
        else
        {
            MapleBox _MapleBox = _Monter as MapleBox; // 충돌된 것이 몬스터가 아니라 단순 박스이었다면 

            if (null != _MapleBox)
            {
                _MapleBox.AccessHit = true; 

                _MapleBox.HitAnimation();

                _MapleBox.AccessHitCount -= 1; // 충돌 카운터를 감소한다

                m_iHitCount = 1; // 중복 충돌을 막기 위해 제어
            }
        }

        m_pBoxCollision.enabled = false; // 충돌의 비활성화
    }

    // 인자로 넘어오는 위치값을 가지고 Hit 이펙트를 만들시 호출 되는 함수입니다.
    public override void CreateHitEffect(Vector3 _Position)
    {
        if (m_pParnentItem == null) // 무기에 대한 정보가 존재하지 않는다면 
            return; // 종료 

        GameObject _HitObject = null;

        switch(m_pParnentItem.AccessItemType) // 해당 무기 타입에 따른 오브젝트 풀링을 나누었습니다.
        {
            case ITEMTYPE.ITEM_STICK: // 몽둥이라면 
                _HitObject = m_pGameObjectManager.GameObejctPooling("Stick Hit Effect", Vector3.zero, Vector3.zero, Quaternion.identity);
                _HitObject.transform.position = _Position;
                break;
            case ITEMTYPE.ITEM_SWORD: // 도끼 및 한손 검일 경우 
                _HitObject = m_pGameObjectManager.GameObejctPooling("Sword Hit Effect", Vector3.zero, Vector3.zero, Quaternion.identity);
                _HitObject.transform.position = _Position;
                break;
        }
    }
}
