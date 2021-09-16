using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : MONTER // 달팽이, 스포어, 파란달팽이에서 사용되는 클래스입니다.
{
    [SerializeField] private AudioClip _HitSound = null;

    [SerializeField] private AudioClip _DeadSound = null;

    [SerializeField] private string DeadSoundName = string.Empty;

    [SerializeField] private LayerMask _BoxLayer = 0;

    [SerializeField] private Vector2 BoxSize = Vector2.zero;

    private Animator m_pAnimator = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private float m_fMaxHp = 0.0f;

    private Player m_pPlayerObject = null;

    private SpriteRenderer m_pSpriteRenderer = null;

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
        m_pAnimator = GetComponent<Animator>();

        m_fAttack = 15.0f;

        m_pPlayerObject = GameObject.Find("Player").GetComponent<Player>();

        m_pSoundManager.AddSound("Snail Ball Hit Sound", _HitSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound(DeadSoundName, _DeadSound, SoundType.Sound_Effect);
    }

    public void Update()
    {
        // 해당 오브젝트의 체력이 0이하로 떨어졌다면 
        
        if (_Hp <= 0.0f)
        {
            // 사망 애니메이션으로 변경 

            m_pAnimator.SetBool("DEAD", true);

            m_pAnimator.SetBool("RUN", false);

            m_pAnimator.SetBool("HIT", false);

            if (m_bIsDeadSound == false)
            {
                m_pSoundManager.PlaySound(DeadSoundName);

                m_bIsDeadSound = true;
            }

            return;
        }
    }

    // 슬레쉬 이펙트와 충돌시 피격이펙트로 변경 
    public override void SetHItAnimation()
    {
        if (_Hp <= 0.0f)
            return;

        if (m_pAnimator.GetBool("HIT") == true)
            return;

        m_pAnimator.SetBool("HIT", true);

        m_pSoundManager.PlaySound("Snail Ball Hit Sound");
    }

    // 재활성화 시 체력 및 변수들의 초기화하는 함수입니다.
    public void OnEnable()
    {
        if (m_fMaxHp == 0.0f)
        {
            m_fMaxHp = _Hp;

            _Hp = m_fMaxHp;
        }
        else
            _Hp = m_fMaxHp;

        if (null == m_pSpriteRenderer)
            m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        m_bIsDeadSound = false;
    }
}