using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stone : MonoBehaviour
{
    [SerializeField] private AudioClip _CreateSound = null;

    [SerializeField] private float _SoundPauseTime = 0.0f;

    [SerializeField] private LayerMask _LayerMask = 0;

    [SerializeField] private Vector3 _Size = Vector3.zero;

    private Animator m_pAnimator = null;

    private BoxCollider2D m_pBoxCollision = null;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private bool m_bIsStop = false;

    private bool m_bIsSound = false;

    // Start is called before the first frame update
    void Start()
    {
        m_pAnimator = GetComponent<Animator>();

        m_pBoxCollision = GetComponent<BoxCollider2D>();

        m_pBoxCollision.enabled = false;

        m_pSoundManager.AddSound("Dead Stone Create Sound", _CreateSound, SoundType.Sound_Effect);

        m_pSoundManager.PlaySound("Dead Stone Create Sound");
    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(transform.position, _Size);
    }

    // Update is called once per frame
    void Update()
    {
        if(m_bIsSound == false)
            m_bIsSound = m_pSoundManager.TimePause("Dead Stone Create Sound", _SoundPauseTime);

        Collider2D _Collision = Physics2D.OverlapBox(transform.position, _Size, 0.0f, _LayerMask);

        if(_Collision != null && _Collision.gameObject.name == "Player")
            m_pBoxCollision.enabled = true;

        if (m_bIsStop == false)
            transform.position += Vector3.down * 3.0f * Time.deltaTime;
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        m_bIsStop = true;

        m_pAnimator.SetBool("BOMB", true);

        m_pSoundManager.ReplaySound("Dead Stone Create Sound");
    }

    public void OnEnable()
    {
        m_bIsSound = false;

        m_bIsStop = false;
    }
}
