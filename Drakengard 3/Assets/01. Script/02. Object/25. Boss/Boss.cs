using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Rendering;
using UnityEngine.Audio;

public enum BOSS_STATE
{
    BOSS_IDLE,
    BOSS_ATTACK,
    BOSS_RUN,
    BOSS_HIT,
    BOSS_RESET,
    BOSS_DEAD
};

public class Boss : Monter
{
    [SerializeField] private GameObject _Swordobject = null;

    [SerializeField] private Material _NewBodyMaterial = null;

    [SerializeField] private Material _NewWeaponMaterial = null;

    [SerializeField] private Sprite _NameSprite = null;

    [SerializeField] private Sprite _FaceSprite = null;

    [SerializeField] private BoxCollider _SwordCollistion = null;

    private BS_Animation m_pBossAnimation = null;

    private BOSS_STATE m_eState = BOSS_STATE.BOSS_IDLE;

    private Transform _TargetTranform = null;

    private NavMeshAgent _NavMeshAgent = null;

    private TrailRenderer _SwordTrailBuffer = null;

    private SkinnedMeshRenderer _BodyMeshRenderer = null;

    private SkinnedMeshRenderer _WeaponMeshRenderer = null;

    private GameobjectManager _GameobjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private EnemyUIBar m_pEnemyBar = null;

    private Vector2 m_vNamePosition = new Vector2(475.1f, -277.2f);

    private Vector3 m_vNameScale = new Vector3(1.163589f, 0.48762f, 1.0f);

    private int m_iHitCount = 0;

    private float m_fHitTimeAcc = 0.0f;

    private float m_fHp = 50.0f;

    private float m_fMaxHp = 0.0f;

    private float m_fDeleteAcc = 0.0f;

    private bool m_bIsRotation = true;

    private bool m_bIsHitDamege = false;

    private bool m_bIsDelete = false;

    public BOSS_STATE SetState
    {
        set { m_eState = value; }
    }

    private void Start()
    {
        if(null == m_pBossAnimation)
            m_pBossAnimation = new BS_Animation(GetComponent<Animator>());

        _NavMeshAgent = GetComponent<NavMeshAgent>();

        _SwordTrailBuffer = _Swordobject.GetComponent<TrailRenderer>();

        _BodyMeshRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        _WeaponMeshRenderer = _Swordobject.GetComponentInParent<SkinnedMeshRenderer>();

        _TargetTranform = GameObject.Find("Zero").transform;

        m_pEnemyBar = GameObject.Find("Canvas").GetComponent<EnemyUIBar>();

        AudioClip _Sound = Resources.Load("Boss Run Sound") as AudioClip;

        m_pSoundManager.AddSound("Boss Run", _Sound);

        _Sound = Resources.Load("Boss First Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Boss First Attack", _Sound);

        _Sound = Resources.Load("Monter Hit Sound") as AudioClip;

        m_pSoundManager.AddSound("Monter Dead", _Sound);

        m_fMaxHp = m_fHp;

        StartCoroutine("StartBossState");
    }

    private void OnDisable()
    {
        m_fDeleteAcc = 0.0f;

        m_bIsHitDamege = false;

        m_bIsDelete = false;

        StopCoroutine("StartBossState");
    }

    IEnumerator StartBossState()
    {
        while(true)
        {
            if(m_fHp <= 0.0f)
                m_eState = BOSS_STATE.BOSS_DEAD;           

            switch(m_eState)
            {
                case BOSS_STATE.BOSS_IDLE:
                    Idle();
                    break;
                case BOSS_STATE.BOSS_RUN:
                    Run();
                    break;
                case BOSS_STATE.BOSS_ATTACK:
                    Attack();
                    break;
                case BOSS_STATE.BOSS_HIT:
                    Hit();
                    break;
                case BOSS_STATE.BOSS_RESET:
                    BossReset();
                    break;
                case BOSS_STATE.BOSS_DEAD:
                    Dead();
                    break;
            }

            m_pBossAnimation.UpDateAnimation();

            yield return null;
        }
    }

    private void Idle()
    {
        m_bIsRotation = true;

        Object_Rotation();

        m_pBossAnimation.AccessAnimations = Boss_State.BOSS_IDLE;
    }

    private void Run()
    {
        float Length = (_TargetTranform.position - transform.position).magnitude;

        if (Length <= 30.0f)
        {
            _NavMeshAgent.isStopped = true;

            m_eState = BOSS_STATE.BOSS_ATTACK;

            return;
        }
        else
        {
            if (_NavMeshAgent.isStopped == true)
                _NavMeshAgent.isStopped = false;
        }

        m_bIsRotation = true;

        Object_Rotation();

        m_pBossAnimation.AccessAnimations = Boss_State.BOSS_RUN;

        _NavMeshAgent.SetDestination(_TargetTranform.position);
    }

    private void Attack()
    {
        if (m_bIsRotation == true)
            Object_Rotation();

        Collider[] _Collision = Physics.OverlapSphere(transform.position + Vector3.up * 3.0f, 20.0f, 1 << 8);

        if(_Collision.Length == 0)
        {
            if (_NavMeshAgent.isStopped == true)
                _NavMeshAgent.isStopped = false;

            m_eState = BOSS_STATE.BOSS_RUN;

            return;
        }

        m_pBossAnimation.AccessAnimations = Boss_State.BOSS_NORMALATTACK;
    }

    private void Hit()
    {
        if (m_pBossAnimation.AccessAnimations == Boss_State.BOSS_HITING)
        {
            m_fHitTimeAcc += Time.deltaTime;

            if(m_fHitTimeAcc >= 5.0f)
            {
                m_fHitTimeAcc = 0.0f;

                m_eState = BOSS_STATE.BOSS_RESET;
            }
        }

        if(m_pBossAnimation.AccessAnimations != Boss_State.BOSS_HITING)
            m_pBossAnimation.AccessAnimations = Boss_State.BOSS_HIT;
    }

    private void Dead()
    {
        if (m_bIsDelete == true)
        {
            m_fDeleteAcc += Time.deltaTime;

            _BodyMeshRenderer.material.SetFloat("_Cut", m_fDeleteAcc * 0.5f);

            _WeaponMeshRenderer.material.SetFloat("_Cut", m_fDeleteAcc * 0.5f);
        }

        if (m_fDeleteAcc >= 1.0f)
        {
            Destroy(gameObject);

            return;
        }

        m_pBossAnimation.AccessAnimations = Boss_State.BOSS_DEAD;
    }

    private void BossReset()
    {
        m_pBossAnimation.AccessAnimations = Boss_State.BOSS_RESET;
    }

    public void Object_Rotation()
    {
        if (null == _TargetTranform)
            return;

        Vector3 _Direction = (_TargetTranform.position - transform.position).normalized;

        float _Angle = Vector3.Dot(Vector3.forward, _Direction);

        _Angle = Mathf.Acos(_Angle) * Mathf.Rad2Deg;

        Vector3 _CrossVector = Vector3.Cross(Vector3.forward, _Direction.normalized);

        if (_CrossVector.y < 0)
            _Angle *= -1.0f;

        transform.eulerAngles = new Vector3(0.0f, _Angle, 0.0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PlayerWeapon" && m_bIsHitDamege == false)
        {
            m_pSoundManager.PlaySound("Hit", 0.3f);

            if (m_iHitCount == 3)
            {
                m_eState = BOSS_STATE.BOSS_HIT;

                m_iHitCount = 0;
            }
            else if (m_eState != BOSS_STATE.BOSS_HIT && m_eState != BOSS_STATE.BOSS_RESET)
                m_iHitCount += 1;

            _GameobjectManager.GameObjectActivation("Hit Effect", transform.position + Vector3.up * 3.0f);

            m_pEnemyBar.OnEnemyBar();

            Image _HpImage = m_pEnemyBar.GetHpImage;

            if (null == _HpImage)
                return;

            Image NameImage = m_pEnemyBar.GetNameImage;

            if (null == NameImage)
                return;

            Image FaceImage = m_pEnemyBar.GetFaceImage;

            if (null == FaceImage)
                return;

            NameImage.rectTransform.localPosition = m_vNamePosition;

            NameImage.rectTransform.localScale = m_vNameScale;

            _HpImage.fillAmount = m_fHp / m_fMaxHp;

            NameImage.sprite = _NameSprite;

            FaceImage.sprite = _FaceSprite;
        }
        else if (other.gameObject.tag == "PlayerWeapon" && m_bIsHitDamege == true)
        {
            m_pSoundManager.PlaySound("Hit", 0.3f);

            m_fHp -= 10.0f;

            _GameobjectManager.GameObjectActivation("Hit Effect", transform.position + Vector3.up * 3.0f);

            m_pEnemyBar.OnEnemyBar();

            Image _HpImage = m_pEnemyBar.GetHpImage;

            if (null == _HpImage)
                return;

            Image NameImage = m_pEnemyBar.GetNameImage;

            if (null == NameImage)
                return;

            Image FaceImage = m_pEnemyBar.GetFaceImage;

            if (null == FaceImage)
                return;

            NameImage.rectTransform.localPosition = m_vNamePosition;

            NameImage.rectTransform.localScale = m_vNameScale;

            _HpImage.fillAmount = m_fHp / m_fMaxHp;

            NameImage.sprite = _NameSprite;

            FaceImage.sprite = _FaceSprite;
        }
    }

    public void ResetMode()
    {
        m_eState = BOSS_STATE.BOSS_RUN;
    }

    public void Hiting()
    {
        _SwordTrailBuffer.enabled = false;

        m_pBossAnimation.AccessAnimations = Boss_State.BOSS_HITING;

        m_bIsHitDamege = true;
    }

    public void OnRotation()
    {
        _NavMeshAgent.isStopped = true;

        m_bIsRotation = true;
    }

    public void OffRotation()
    {
        _NavMeshAgent.isStopped = true;

        m_bIsRotation = false;
    }

    public void OffSwitchBuffer()
    {
        _SwordTrailBuffer.enabled = false;
    }

    public void OnSwitchBuffer()
    {
        _SwordTrailBuffer.enabled = true;
    }

    public void OffDamege()
    {
        m_bIsHitDamege = false;
    }

    public void OnDelete()
    {
        _BodyMeshRenderer.material = _NewBodyMaterial;

        _WeaponMeshRenderer.material = _NewWeaponMaterial;

        m_bIsDelete = true;
    }

    public void OnBoxCollision()
    {
        _SwordCollistion.enabled = true;
    }

    public void OffBoxCollision()
    {
        _SwordCollistion.enabled = false;
    }

    public void PlayRunSound()
    {
        m_pSoundManager.PlaySound("Boss Run");
    }

    public void PlayFirstAttackSound()
    {
        m_pSoundManager.PlaySound("Boss First Attack");
    }

    
}