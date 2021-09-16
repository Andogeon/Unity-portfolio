using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 몬스터들의 동작들은 애니메이터에서 별도로 애니메이션 동작마다 스크립트를 구성하게 했습니다.

public class JuniorStoneBall : MONTER // 주니어 스톤볼 클래스입니다.
{
    [SerializeField] private AudioClip _HitSound = null;

    [SerializeField] private AudioClip _DeadSound = null;

    [SerializeField] private LayerMask _BoxLayer = 0;

    [SerializeField] private Vector2 BoxSize = Vector2.zero;

    [SerializeField] private GameObject _FieldItem = null;

    private Animator m_pAnimator = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private float m_fMaxHp = 0.0f;

    private Player m_pPlayerObject = null;

    private bool m_bIsDeadSound = false;

    public Vector2 AccessBoxSize
    {
        get { return BoxSize; }
    }

    public LayerMask AccessLayer
    {
        get { return _BoxLayer; }
    }

    public Player AccessPlayerObject
    {
        get { return m_pPlayerObject; }

        set { m_pPlayerObject = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(null == m_pAnimator)
            m_pAnimator = GetComponent<Animator>();

        if (null == m_pPlayerObject)
            m_pPlayerObject = GameObject.Find("Player").GetComponent<Player>();

        m_pGameobjectManager.OriginalGamgObjectInsert("Stone Etc Item", _FieldItem);

        m_pSoundManager.AddSound("Stone Ball Hit Sound", _HitSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound("Stone Ball Dead Sound", _DeadSound, SoundType.Sound_Effect);

        m_fAttack = 10.0f;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.black;

        Gizmos.DrawWireCube(transform.position, BoxSize);
    }

    // 이펙트와 충돌시 애니메이션을 변경하는 함수입니다.
    public override void SetHItAnimation()
    {
        if (_Hp <= 0.0f)
            return;

        if (m_pAnimator.GetBool("HIT") == true)
            return;

        m_pAnimator.SetBool("HIT", true);

        m_pSoundManager.PlaySound("Stone Ball Hit Sound");
    }

    public void Update()
    {
        if (_Hp <= 0.0f) // 해당 몬스터가 사망했다면 
        {
            m_pAnimator.SetBool("DEAD", true);

            m_pAnimator.SetBool("RUN", false);

            m_pAnimator.SetBool("HIT", false);

            // 애니메이션의 변경 

            if (m_bIsDeadSound == false)
            {
                m_pSoundManager.PlaySound("Stone Ball Dead Sound");

                m_bIsDeadSound = true;
            }

            return;
        }
    }

    // 다시 활성화 함수 호출 시 체력을 회복하게 했습니다.
    public void OnEnable()
    {
        if (m_fMaxHp == 0.0f)
        {
            m_fMaxHp = _Hp;

            _Hp = m_fMaxHp;
        }
        else
            _Hp = m_fMaxHp;

        m_bIsDeadSound = false;
    }

    private void OnDestroy()
    {
        m_pPlayerObject = null;

        m_pAnimator = null;
    }
}
