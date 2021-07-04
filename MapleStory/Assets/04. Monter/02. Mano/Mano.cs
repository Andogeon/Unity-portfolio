using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 피격 사운드 제어 

public enum MANOSTATE
{
    MANO_IDLE,
    MANO_RUN,
    MANO_ATTACK,
    MANO_HIT,
    MANO_DEAD,
    MANO_ATTACKMOVING
}


public class Mano : MONTER
{
    [SerializeField] private AudioClip _HitSound = null;

    [SerializeField] private AudioClip _DeadSound = null;

    [SerializeField] private MonterQuest _QusetObject = null;

    [SerializeField] private Vector3 _CollisionSize = Vector3.zero;

    [SerializeField] private LayerMask _LayerMask = 0;

    [SerializeField] private float _Speed = 0.0f;

    private Vector3 m_vOldDirection = Vector3.zero;

    private Animator m_pAnimator = null;

    private SpriteRenderer m_pSpriteRenderer = null;

    private BoxCollider2D m_pBoxCollision = null;

    private GameObject m_pPlayer = null;

    private MANOSTATE m_eManoState = MANOSTATE.MANO_RUN;

    private bool m_bIsCollision = false;

    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private float m_fTimeAcc = 0.0f;

    private bool m_bIsDead = false;

    public MANOSTATE AccessManoState
    {
        get { return m_eManoState; }

        set { m_eManoState = value; }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;

    //    Gizmos.DrawWireCube(transform.position, _CollisionSize);
    //}

    // Start is called before the first frame update
    void Start()
    {
        m_pAnimator = GetComponent<Animator>();

        m_pBoxCollision = GetComponent<BoxCollider2D>();

        if (null == m_pSpriteRenderer)
            m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pPlayer = GameObject.Find("Player");

        m_fAttack = 30.0f;

        m_pSoundManager.AddSound("Mano Hit Sound", _HitSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound("Mano Dead Sound", _DeadSound, SoundType.Sound_Effect);
    }

    // Update is called once per frame
    void Update()
    {
        AnimationUpdate();
    }

    private void OnDestroy()
    {
        m_pAnimator = null;

        m_pGameObjectManager = null;

        m_pSoundManager = null;
    }

    public override SpriteRenderer GetMonterSpriteRenderer()
    {
        if (null == m_pSpriteRenderer)
            m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        return m_pSpriteRenderer;
    }

    private void AnimationUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            _Hp = 0.0f;

        if (_Hp <= 0.0f)
            m_eManoState = MANOSTATE.MANO_DEAD;

        switch(m_eManoState)
        {
            case MANOSTATE.MANO_IDLE:
                m_pAnimator.SetBool("RUN", false);
                Idle();
                break;
            case MANOSTATE.MANO_RUN:
                m_pAnimator.SetBool("RUN", true);
                Run();
                break;
            case MANOSTATE.MANO_ATTACK:
                break;
            case MANOSTATE.MANO_DEAD:
                Dead();
                break;
            case MANOSTATE.MANO_HIT:
                m_pAnimator.SetBool("RUN", false);
                m_pAnimator.SetTrigger("HIT");
                Hit();
                break;
            case MANOSTATE.MANO_ATTACKMOVING:
                m_pAnimator.SetBool("RUN", true);
                AttackMoving();
                break;
        }
    }

    private void Idle()
    {
        m_fTimeAcc += Time.deltaTime;

        if(m_fTimeAcc >= 2.0f)
        {
            m_fTimeAcc = 0.0f;

            m_eManoState = MANOSTATE.MANO_RUN;
        }
    }

    private void Run()
    {
        Collider2D _Collision = Physics2D.OverlapBox(transform.position, _CollisionSize, 0.0f, _LayerMask);

        if (_Collision != null && m_bIsCollision == false)
        {
            m_bIsCollision = true;

            if (transform.eulerAngles.y == 0.0f)
                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
            else
                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        }
        else
            m_bIsCollision = false;

        transform.position += transform.right * -1.0f * _Speed * Time.deltaTime;

        m_fTimeAcc += Time.deltaTime;

        if(m_fTimeAcc >= 5.0f)
        {
            m_fTimeAcc = 0.0f;

            m_eManoState = MANOSTATE.MANO_IDLE;
        }
    }

    private void Hit()
    {
        m_pSoundManager.PlaySound("Mano Hit Sound");

        m_eManoState = MANOSTATE.MANO_ATTACKMOVING;
    }

    private void AttackMoving()
    {
        if (null == m_pPlayer)
            return;

        Vector3 _Direction = m_pPlayer.transform.position - transform.position;

        _Direction.y = 0.0f;

        float Length = _Direction.magnitude;

        float fAngle = Vector3.Angle(Vector3.left, _Direction.normalized);

        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, fAngle, transform.eulerAngles.z);

        transform.position += _Direction * 0.5f * Time.deltaTime;
    }

    public override void SetHItAnimation()
    {
        m_eManoState = MANOSTATE.MANO_HIT;
    }

    private void Dead()
    {
        if (m_bIsDead == false)
        {
            m_pAnimator.SetTrigger("DEAD");

            m_bIsDead = true;

            m_pSoundManager.PlaySound("Mano Dead Sound");

            m_pBoxCollision.enabled = false;
        }

        if (m_pAnimator.GetCurrentAnimatorStateInfo(0).IsName("DEAD") == true
            && m_pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            _QusetObject.AccessDeleteCount += 1;

            Destroy(gameObject);
        }
    }
}
















//public enum MANOSTATE
//{
//    MANO_IDLE,
//    MANO_RUN,
//    MANO_ATTACK,
//    MANO_HIT,
//    MANO_DEAD,
//    MANO_ATTACKMOVING
//}


//public class Mano : MONTER
//{
//    [SerializeField] private AudioClip _HitSound = null;

//    [SerializeField] private AudioClip _DeadSound = null;

//    [SerializeField] private MonterQuest _QusetObject = null;

//    [SerializeField] private Vector3 _CollisionSize = Vector3.zero;

//    [SerializeField] private LayerMask _LayerMask = 0;

//    [SerializeField] private float _Speed = 0.0f;

//    private Vector3 m_vOldDirection = Vector3.zero;

//    private Animator m_pAnimator = null;

//    private GameObject m_pPlayer = null;

//    private MANOSTATE m_eManoState = MANOSTATE.MANO_RUN;

//    private bool m_bIsCollision = false;

//    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

//    private SoundManager m_pSoundManager = SoundManager.GetInstance();

//    private float m_fTimeAcc = 0.0f;

//    private bool m_bIsDead = false;

//    private bool m_bIsHitSound = false;

//    public MANOSTATE AccessManoState
//    {
//        get { return m_eManoState; }

//        set { m_eManoState = value; }
//    }

//    //public QUEST AccessMonterQuest
//    //{
//    //    get { return _QusetObject; }

//    //    set { _QusetObject = value; }
//    //}

//    //private void OnDrawGizmos()
//    //{
//    //    Gizmos.color = Color.red;

//    //    Gizmos.DrawWireCube(transform.position, _CollisionSize);
//    //}

//    // Start is called before the first frame update
//    void Start()
//    {
//        m_pAnimator = GetComponent<Animator>();

//        m_pPlayer = GameObject.Find("Player");

//        m_fAttack = 30.0f;

//        m_pSoundManager.AddSound("Mano Hit Sound", _HitSound, SoundType.Sound_Effect);

//        m_pSoundManager.AddSound("Mano Dead Sound", _DeadSound, SoundType.Sound_Effect);
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        AnimationUpdate();
//    }

//    private void OnDestroy()
//    {
//        m_pAnimator = null;

//        m_pGameObjectManager = null;
//    }

//    private void AnimationUpdate()
//    {
//        if (Input.GetKeyDown(KeyCode.Alpha0))
//            _Hp = 0.0f;

//        if (_Hp <= 0.0f)
//            m_eManoState = MANOSTATE.MANO_DEAD;

//        switch (m_eManoState)
//        {
//            case MANOSTATE.MANO_IDLE:
//                m_pAnimator.SetBool("RUN", false);
//                Idle();
//                break;
//            case MANOSTATE.MANO_RUN:
//                m_pAnimator.SetBool("RUN", true);
//                Run();
//                break;
//            case MANOSTATE.MANO_ATTACK:
//                break;
//            case MANOSTATE.MANO_DEAD:
//                Dead();
//                break;
//            case MANOSTATE.MANO_HIT:
//                m_pAnimator.SetBool("RUN", false);
//                m_pAnimator.SetTrigger("HIT");
//                Hit();
//                break;
//            case MANOSTATE.MANO_ATTACKMOVING:
//                m_pAnimator.SetBool("RUN", true);
//                AttackMoving();
//                break;
//        }
//    }

//    private void Idle()
//    {
//        m_fTimeAcc += Time.deltaTime;

//        if (m_fTimeAcc >= 2.0f)
//        {
//            m_fTimeAcc = 0.0f;

//            m_eManoState = MANOSTATE.MANO_RUN;
//        }

//        if (true == m_bIsHitSound)
//            m_bIsHitSound = false;
//    }

//    private void Run()
//    {
//        if (true == m_bIsHitSound)
//            m_bIsHitSound = false;

//        Collider2D _Collision = Physics2D.OverlapBox(transform.position, _CollisionSize, 0.0f, _LayerMask);

//        if (_Collision != null && m_bIsCollision == false)
//        {
//            m_bIsCollision = true;

//            if (transform.eulerAngles.y == 0.0f)
//                transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
//            else
//                transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
//        }
//        else
//            m_bIsCollision = false;

//        transform.position += transform.right * -1.0f * _Speed * Time.deltaTime;

//        m_fTimeAcc += Time.deltaTime;

//        if (m_fTimeAcc >= 5.0f)
//        {
//            m_fTimeAcc = 0.0f;

//            m_eManoState = MANOSTATE.MANO_IDLE;
//        }
//    }

//    private void Hit()
//    {
//        if (m_bIsHitSound == false)
//        {
//            m_pSoundManager.PlaySound("Mano Hit Sound");

//            m_bIsHitSound = true;
//        }

//        m_eManoState = MANOSTATE.MANO_ATTACKMOVING;
//    }

//    private void AttackMoving()
//    {
//        if (null == m_pPlayer)
//            return;

//        Vector3 _Direction = m_pPlayer.transform.position - transform.position;

//        _Direction.y = 0.0f;

//        float Length = _Direction.magnitude;

//        float fAngle = Vector3.Angle(Vector3.left, _Direction.normalized);

//        transform.rotation = Quaternion.Euler(transform.eulerAngles.x, fAngle, transform.eulerAngles.z);

//        transform.position += _Direction * 0.5f * Time.deltaTime;
//    }

//    //private void Attack()
//    //{
//    //if (m_pAnimator.GetCurrentAnimatorStateInfo(0).IsName("ATTACK") != false)
//    //    return;

//    //if(m_pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
//    //{

//    //}
//    //}

//    //private void OnTriggerEnter2D(Collider2D collision)
//    //{
//    //    if (collision.gameObject.tag == "Attack Effect")
//    //    {
//    //        //m_iCollisionCount = 1;

//    //        if (_Hp <= 0.0f)
//    //        {
//    //            m_eManoState = MANOSTATE.MANO_DEAD;

//    //            m_pSoundManager.PlaySound("Mano Dead Sound");

//    //            return;
//    //        }

//    //        m_eManoState = MANOSTATE.MANO_HIT;
//    //    }
//    //}

//    private void Dead()
//    {
//        if (m_bIsDead == false)
//        {
//            m_pAnimator.SetTrigger("DEAD");

//            m_bIsDead = true;
//        }

//        if (m_pAnimator.GetCurrentAnimatorStateInfo(0).IsName("DEAD") == true
//            && m_pAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
//        {
//            _QusetObject.AccessDeleteCount += 1;

//            Destroy(gameObject);
//        }
//    }