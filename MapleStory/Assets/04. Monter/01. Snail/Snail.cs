using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail : MONTER
{
    [SerializeField] private AudioClip _HitSound = null;

    [SerializeField] private AudioClip _DeadSound = null;

    [SerializeField] private string DeadSoundName = string.Empty;

    [SerializeField] private LayerMask _BoxLayer = 0;

    [SerializeField] private Vector2 BoxSize = Vector2.zero;

    //[SerializeField] private GameObject _FieldItem = null;

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

        //m_pSoundManager.AddSound("Snail Ball Hit Sound", _HitSound, SoundType.Sound_Effect);

        //m_pSoundManager.AddSound("Snail Ball Dead Sound", _DeadSound, SoundType.Sound_Effect);

        //m_pGameobjectManager.OriginalGamgObjectInsert("Snail Etc Item", _FieldItem);
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
                //m_pSoundManager.PlaySound("Snail Ball Dead Sound");

                m_pSoundManager.PlaySound(DeadSoundName);

                m_bIsDeadSound = true;
            }

            return;
        }
    }

    public override void SetHItAnimation()
    {
        if (_Hp <= 0.0f)
            return;

        if (m_pAnimator.GetBool("HIT") == true)
            return;

        m_pAnimator.SetBool("HIT", true);

        m_pSoundManager.PlaySound("Snail Ball Hit Sound");
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

        if (null == m_pSpriteRenderer)
            m_pSpriteRenderer = GetComponent<SpriteRenderer>();

        m_pSpriteRenderer.color = new Color(1.0f, 1.0f, 1.0f, 1.0f);

        m_bIsDeadSound = false;
    }
}







//private void OnDrawGizmos()
//{
//    Gizmos.color = Color.black;

//    Gizmos.DrawWireCube(transform.position, BoxSize);
//}

//private void OnTriggerEnter2D(Collider2D collision)
//{
//    if (MONTER.m_iCollisionCount >= 1 || collision.gameObject.tag != "Attack Effect")
//        return;

//    if (collision.gameObject.tag == "Attack Effect")
//    {
//        m_iCollisionCount = 1;

//        if (_Hp <= 0.0f)
//        {
//            m_pAnimator.SetBool("DEAD", true);

//            m_pAnimator.SetBool("HIT", false);

//            return;
//        }

//        _Hp -= 10.0f;

//        m_pAnimator.SetBool("HIT", true);
//    }
//}

//private void OnTriggerExit2D(Collider2D collision)
//{
//    if (collision.gameObject.tag == "Attack Effect")
//        m_iCollisionCount = 0;
//}