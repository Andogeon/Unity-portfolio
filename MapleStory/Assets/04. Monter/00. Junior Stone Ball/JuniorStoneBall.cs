using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuniorStoneBall : MONTER
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
        if (_Hp <= 0.0f)
        {
            m_pAnimator.SetBool("DEAD", true);

            m_pAnimator.SetBool("RUN", false);

            m_pAnimator.SetBool("HIT", false);

            if (m_bIsDeadSound == false)
            {
                m_pSoundManager.PlaySound("Stone Ball Dead Sound");

                m_bIsDeadSound = true;
            }

            return;
        }
    }

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

//private void OnTriggerExit2D(Collider2D collision)
//{
//    MONTER.m_iCollisionCount = 0;
//}


//private void OnTriggerEnter2D(Collider2D collision)
//{
//    EFFECT _Effect = collision.GetComponent<EFFECT>();

//    if (null == _Effect)
//        return;

//    if (MONTER.m_iCollisionCount >= 1)
//        return;

//    if (collision.gameObject.tag == "Attack Effect")
//    {
//        m_iCollisionCount = 1;

//        _Effect.CreateHitEffect(collision.transform.position); // ? 충돌된 좌표인지 ?? 아니면 충돌 받은 객체의 좌표인지 ?? 애메하다 ../

//        if (_Hp <= 0.0f)
//        {
//            m_pAnimator.SetBool("DEAD", true);

//            m_pAnimator.SetBool("RUN", false);

//            m_pAnimator.SetBool("HIT", false);

//            //m_pPlayerObject.AccessExp += 10.0f;

//            return;
//        }

//        m_pAnimator.SetBool("HIT", true);
//    }
//}
