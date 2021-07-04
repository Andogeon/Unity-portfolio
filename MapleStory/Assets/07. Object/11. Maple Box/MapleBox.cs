using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleBox : MONTER
{
    [SerializeField] private AudioClip _DeadSound = null;

    [SerializeField] private AudioClip _HitSound = null;

    [SerializeField] private string _FieldItemName = string.Empty;

    [SerializeField] private GameObject _FieldItem = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private Animator m_pAnimator = null;

    private int m_iCount = 3;

    private bool m_bisHit = false;

    public int AccessHitCount
    {
        get { return m_iCount; }

        set { m_iCount = value; }
    }

    public bool AccessHit
    {
        get { return m_bisHit; }

        set { m_bisHit = value; }
    }

    public string AccessFieldName
    {
        get { return _FieldItemName; }
    }

    private void Awake()
    {
        m_pAnimator = GetComponent<Animator>();

        m_pGameobjectManager.OriginalGamgObjectInsert(_FieldItemName, _FieldItem);

        m_pSoundManager.AddSound("Box Dead Sound", _DeadSound, SoundType.Sound_Effect);

        m_pSoundManager.AddSound("Box Hit Sound", _HitSound, SoundType.Sound_Effect);

        //m_pGameobjectManager.OriginalGamgObjectInsert("Box Etc Item", _FieldItem);
    }

    private void Update()
    {
        //if (m_pAnimator.GetBool("HIT") == false)
        //    m_bisHit = false;

        //if(m_bisHit == true)
        //    m_pAnimator.SetBool("HIT", true);

        if (m_iCount <= 0 && m_pAnimator.GetBool("DEAD") == false)
        {
            m_pAnimator.SetBool("DEAD", true);

            m_pAnimator.SetBool("HIT", false);

            m_pSoundManager.PlaySound("Box Dead Sound");
        }
    }

    public void HitAnimation()
    {
        if (null == m_pAnimator)
            m_pAnimator = GetComponent<Animator>();

        m_pAnimator.SetBool("HIT", true);

        m_pSoundManager.PlaySound("Box Hit Sound");
    }

    private void OnDestroy()
    {
        m_pAnimator = null;

        m_pGameobjectManager = null;
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (m_bisHit == true || m_iCount <= 0)
    //        return;

    //    if (collision.gameObject.tag == "Attack Effect" && m_pAnimator.GetBool("HIT") == false)
    //    {
    //        //m_pAnimator.SetTrigger("HIT");

    //        m_pAnimator.SetBool("HIT", true);

    //        //m_iCount -= 1;

    //        m_bisHit = true;
    //    }
    //}

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Attack Effect")
    //    {
    //        m_bisHit = false;

    //        m_pAnimator.SetBool("HIT", false);
    //    }
    //}

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (m_pAnimator.GetBool("HIT") == true)
    //        return;

    //    if (m_iCount <= 0)
    //    {
    //        m_iCount = 0;

    //        return;
    //    }

    //    if (collision.gameObject.tag == "Attack Effect" && m_bisHit == false)
    //    {
    //        m_pAnimator.SetBool("HIT", true);

    //        m_iCount -= 1;

    //        m_bisHit = true;
    //    }
    //}


}
