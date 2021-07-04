using UnityEngine;
using System.Collections;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Audio;

public class Zero : MonoBehaviour
{
    [SerializeField] private float MoveSpeed = 0.0f;
    
    [SerializeField] private string[] _AnimaionsName = null;
    
    [SerializeField] private GameObject _SwordObject = null;

    [SerializeField] private GameObject Taril = null;

    [SerializeField] private GameObject _Dash = null;

    [SerializeField] private GameObject _Gauge = null;

    [SerializeField] private GameObject _UIHPBar = null;

    [SerializeField] private SkinnedMeshRenderer m_pZeroRenderer = null;

    private BoxCollider m_SwordBox = null;

    private TrailRenderer m_TarilRenderer = null;

    private Image m_pHpbar = null;

    private Image m_pGauge = null;

    private MainCamera m_Camera = null;

    private ZeroAnimation m_ZeroAnimations = null;

    private GameobjectManager _GameobjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private Vector3 m_vDirection = Vector3.zero;

    private Vector3 m_vAttackDirection = Vector3.zero;

    private Vector3 m_vOrlVector = Vector3.zero;

    private float m_fAngle = 0.0f;

    private float m_fHp = 100.0f;

    private float m_fGauge = 0.0f;

    private float m_fBlood = 0.0f;

    private bool m_bIsIdle = true;

    private bool m_bIsSubIdle = true;

    public Vector3 GetDirectionVector
    {
        get { return m_vDirection; }
    }

    public float SetGauge
    {
        get { return m_fGauge; }

        set { m_fGauge = value; }
    }

    public float SetBlood
    {
        get { return m_fBlood; }

        set { m_fBlood = value; }
    }

    private void Awake()
    {
        m_ZeroAnimations = new ZeroAnimation(GetComponentInChildren<Animator>());

        m_Camera = GameObject.Find("Main Camera").GetComponent<MainCamera>();

        m_SwordBox = _SwordObject.GetComponent<BoxCollider>();

        GetComponent<NavMeshAgent>().avoidancePriority = 1;

        m_TarilRenderer = Taril.GetComponent<TrailRenderer>();

        m_pHpbar = _UIHPBar.GetComponent<Image>();

        m_pGauge = _Gauge.GetComponent<Image>();

        AddSound();

        _GameobjectManager.CreateobjectToInsert("Dash Effect", _Dash, Vector3.zero, Quaternion.identity, 1);

        m_SwordBox.enabled = false;
    }

    private void Start()
    {
        StartCoroutine("ZeroUpdate");
    }

    IEnumerator ZeroUpdate()
    {
        while (true)
        {
            m_ZeroAnimations.UpDateAnimation(_AnimaionsName);

            ZeroUpdateData();

            yield return null;
        }
    }

    private void KeyInput()
    {
        SetBloodState();

        if (Input.GetMouseButtonDown(0))
            MouseClick();

        if (Input.GetMouseButtonDown(1))
            RightMouseclick();

        ZeroMoveing();
    }

    private void RightMouseclick()
    {
        ZeroAnimations _eZeroAnimations = m_ZeroAnimations.AccessZeroAnimaions;

        m_ZeroAnimations.AccessZeroAnimaions = ZeroAnimations.ZERO_SUBNORMALATTACK;

        if (_eZeroAnimations == ZeroAnimations.ZERO_NORMALATTACK)
            m_ZeroAnimations.AccessSubCombo = 0;

        if (m_ZeroAnimations.AccessSubCombo >= m_ZeroAnimations.GetsubMaxCombo)
            return;

        if (m_ZeroAnimations.AccessClick == true)
        {
            m_ZeroAnimations.AccessSubCombo += 1;

            m_ZeroAnimations.AccessClick = false;
        }

        switch (m_ZeroAnimations.AccessSubCombo)
        {
            case 1:
                m_ZeroAnimations.AccessZeroSubAttackType = ZeroSubAttackType.ZNA_SUBATTACK1;
                break;
            case 2:
                m_ZeroAnimations.AccessZeroSubAttackType = ZeroSubAttackType.ZNA_SUBATTACK2;
                break;
            case 3:
                m_ZeroAnimations.AccessZeroSubAttackType = ZeroSubAttackType.ZNA_SUBATTACK3;
                break;
        }
    }

    private void MouseClick()
    {
        m_ZeroAnimations.AccessZeroAnimaions = ZeroAnimations.ZERO_NORMALATTACK;

        if (m_ZeroAnimations.AccessZeroCombo >= m_ZeroAnimations.GetZeroMaxCombo)
            return;

        if (m_ZeroAnimations.AccessClick == true)
        {
            m_ZeroAnimations.AccessZeroCombo += 1;

            m_ZeroAnimations.AccessClick = false;
        }

        switch (m_ZeroAnimations.AccessZeroCombo)
        {
            case 1:
                m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK1;
                break;
            case 2:
                m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK2;
                break;
            case 3:
                m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK3;
                break;
            case 4:
                m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK4;
                break;
            case 5:
                m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK5;
                break;
            case 6:
                m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK6;
                break;
            case 7:
                m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK7;
                break;
            case 8:
                m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK8;
                break;
        }
    }

    private void ZeroMoveing()
    {
        if (m_bIsIdle == false)
            return;

        if (m_bIsSubIdle == false)
            return;

        if (m_ZeroAnimations.AccessZeroAnimaions != ZeroAnimations.ZERO_NORMALATTACK && m_ZeroAnimations.AccessZeroAnimaions != ZeroAnimations.ZERO_SUBNORMALATTACK)
        {
            if (m_ZeroAnimations.AccessZeroAnimaions == ZeroAnimations.ZERO_HIT)
                return;

            if (m_ZeroAnimations.GetdashMode == true)
                return;

            m_Camera.SetDashCamera = false;

            Vector3 _vLocalLookVector = Vector3.zero;

            Vector3 _vLocalRightVector = Vector3.zero;

            ZeroAnimations _eSelectZeroAnimation = ZeroAnimations.ZERO_IDLE; // 애니메이션의 변경을 위한 임시변수

            if (Input.GetKey(KeyCode.W))
            {
                _vLocalLookVector = transform.forward;

                _eSelectZeroAnimation = ZeroAnimations.ZERO_RUN;

                m_vAttackDirection = Vector3.zero;
            }

            if (Input.GetKey(KeyCode.S))
            {
                _vLocalLookVector = transform.forward * -1.0f;

                _eSelectZeroAnimation = ZeroAnimations.ZERO_RUN;

                m_vAttackDirection = Vector3.zero;
            }

            if (Input.GetKey(KeyCode.A))
            {
                _vLocalRightVector = transform.right * -1.0f;

                _eSelectZeroAnimation = ZeroAnimations.ZERO_RUN;

                m_vAttackDirection = Vector3.zero;
            }

            if (Input.GetKey(KeyCode.D))
            {
                _vLocalRightVector = transform.right;

                _eSelectZeroAnimation = ZeroAnimations.ZERO_RUN;

                m_vAttackDirection = Vector3.zero;
            }

            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Space))
            {
                _vLocalLookVector = transform.forward;

                _eSelectZeroAnimation = ZeroAnimations.ZERO_DASH;

                m_Camera.SetDashCamera = true;
            }

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
            {
                _vLocalLookVector = transform.forward;

                _eSelectZeroAnimation = ZeroAnimations.ZERO_DASH;

                m_Camera.SetDashCamera = true;
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space))
            {
                _vLocalRightVector = transform.forward;

                _eSelectZeroAnimation = ZeroAnimations.ZERO_DASH;

                m_Camera.SetDashCamera = true;
            }

            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space))
            {
                _vLocalRightVector = transform.forward;

                _eSelectZeroAnimation = ZeroAnimations.ZERO_DASH;

                m_Camera.SetDashCamera = true;
            }

            if (_vLocalLookVector.magnitude != 0.0 || _vLocalRightVector.magnitude != 0.0f)
                m_vDirection = (_vLocalLookVector + _vLocalRightVector).normalized;

            if (_eSelectZeroAnimation == ZeroAnimations.ZERO_RUN)
                transform.position += transform.forward * MoveSpeed * Time.deltaTime;

            if (m_ZeroAnimations.AccessZeroAnimaions != ZeroAnimations.ZERO_HIT)
                m_ZeroAnimations.AccessZeroAnimaions = _eSelectZeroAnimation;
        }
        else if(m_ZeroAnimations.AccessZeroAnimaions == ZeroAnimations.ZERO_NORMALATTACK)
        {
            if (m_ZeroAnimations.AccessZeroAnimaions == ZeroAnimations.ZERO_HIT)
                return;

            if (m_ZeroAnimations.GetdashMode == true)
                return;

            m_Camera.SetDashCamera = false;

            Vector3 _vLocalLookVector = Vector3.zero;

            Vector3 _vLocalRightVector = Vector3.zero;


            Vector3 _LookVector = Vector3.zero;

            Vector3 _RightVector = Vector3.zero;


            ZeroAnimations _eSelectZeroAnimation = m_ZeroAnimations.AccessZeroAnimaions;


            if (Input.GetKey(KeyCode.W) && m_ZeroAnimations.AccessZeroAnimaions != ZeroAnimations.ZERO_ATTACKDASH)
                _LookVector = transform.forward;

            if (Input.GetKey(KeyCode.S) && m_ZeroAnimations.AccessZeroAnimaions != ZeroAnimations.ZERO_ATTACKDASH)
                _LookVector = transform.forward * -1.0f;

            if (Input.GetKey(KeyCode.A) && m_ZeroAnimations.AccessZeroAnimaions != ZeroAnimations.ZERO_ATTACKDASH)
                _RightVector = transform.right * -1.0f;

            if (Input.GetKey(KeyCode.D) && m_ZeroAnimations.AccessZeroAnimaions != ZeroAnimations.ZERO_ATTACKDASH)
                _RightVector = transform.right;


            if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.Space))
            {
                _eSelectZeroAnimation = ZeroAnimations.ZERO_ATTACKDASH;

                m_Camera.SetDashCamera = true;
            }

            if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.Space))
            {
                _eSelectZeroAnimation = ZeroAnimations.ZERO_ATTACKDASH;

                m_Camera.SetDashCamera = true;
            }

            if (Input.GetKey(KeyCode.A) && Input.GetKey(KeyCode.Space))
            {
                _eSelectZeroAnimation = ZeroAnimations.ZERO_ATTACKDASH;

                m_Camera.SetDashCamera = true;
            }

            if (Input.GetKey(KeyCode.D) && Input.GetKey(KeyCode.Space))
            {
                _eSelectZeroAnimation = ZeroAnimations.ZERO_ATTACKDASH;

                m_Camera.SetDashCamera = true;
            }

            if (_LookVector.magnitude != 0.0 || _RightVector.magnitude != 0.0f)
                m_vAttackDirection = (_LookVector + _RightVector).normalized;

            m_ZeroAnimations.AccessZeroAnimaions = _eSelectZeroAnimation;
        }
    }

    private void ZeroUpdateData()
    {
        m_pHpbar.fillAmount = m_fHp / 100.0f;

        m_pGauge.fillAmount = m_fGauge / 1.0f;



        float _CameraAngle = Vector3.Angle(m_Camera.GetCameraForwardVector, Vector3.forward);

        Vector3 _CrossVector = Vector3.Cross(m_Camera.GetCameraForwardVector, Vector3.forward);

        if (_CrossVector.y > 0.0f)
            _CameraAngle *= -1.0f; // 카메라 개인적 각도 연산        


        float _fAngle = Vector3.Angle(transform.forward, m_vDirection);

        _CrossVector = Vector3.Cross(transform.forward, m_vDirection);

        if (_CrossVector.y < 0)
            _fAngle *= -1.0f;

        if (m_ZeroAnimations.AccessZeroAnimaions == ZeroAnimations.ZERO_RUN && m_vDirection.magnitude != 0.0f)
            m_fAngle = _fAngle;
        
        else if (m_ZeroAnimations.AccessZeroAnimaions == ZeroAnimations.ZERO_ATTACKDASH)
        {
            if (m_vOrlVector.magnitude == 0.0f)
                m_vOrlVector = transform.forward;

            _fAngle = Vector3.Angle(m_vOrlVector, m_vAttackDirection);

            _CrossVector = Vector3.Cross(m_vOrlVector, m_vAttackDirection);

            if (_CrossVector.y < 0)
                _fAngle *= -1.0f;

            m_fAngle = _fAngle;           
        }

        transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, _CameraAngle + m_fAngle, transform.rotation.eulerAngles.z);

        KeyInput();
    }

    private void SetBloodState()
    {
        if (m_pZeroRenderer == null)
            return;

        Material[] _Materials = m_pZeroRenderer.materials;

        if (null == _Materials)
            return;

        for (int i = 0; i < _Materials.Length; ++i)
            _Materials[i].SetFloat("_Cut", m_fBlood);
    }

    public void ResetIdle()
    {
        m_ZeroAnimations.AccessZeroAnimaions = ZeroAnimations.ZERO_IDLE;

        m_ZeroAnimations.AccessZeroAttackType = ZeroNormalAttackType.ZNA_ATTACK1;

        m_ZeroAnimations.GetdashMode = m_ZeroAnimations.AccessClick = false;

        m_ZeroAnimations.AccessZeroCombo = 1;

        m_ZeroAnimations.AccessSubCombo = 1;

        m_SwordBox.enabled = false;

        m_vOrlVector = m_vAttackDirection = Vector3.zero;
    }

    public void AddSound()
    {
        AudioClip _Sound = Resources.Load("Zero Dash Sound") as AudioClip;

        m_pSoundManager.AddSound("Dash Sound", _Sound);

        _Sound = Resources.Load("Run") as AudioClip;

        m_pSoundManager.AddSound("Run Sound", _Sound);

        _Sound = Resources.Load("First Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("First Attack", _Sound);

        _Sound = Resources.Load("Second Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Second Attack", _Sound);

        _Sound = Resources.Load("Third Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Third Attack", _Sound);

        _Sound = Resources.Load("Fourth Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Fourth Attack", _Sound);

        _Sound = Resources.Load("Fifth Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Fifth Attack", _Sound);

        _Sound = Resources.Load("Sixth Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Sixth Attack", _Sound);

        _Sound = Resources.Load("Seventh Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Seventh Attack", _Sound);

        _Sound = Resources.Load("Eighth Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Eighth Attack", _Sound);

        _Sound = Resources.Load("Hit") as AudioClip;

        m_pSoundManager.AddSound("Hit", _Sound);

        _Sound = Resources.Load("First Sub Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("First Sub Attack", _Sound);

        _Sound = Resources.Load("Second Sub Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Second Sub Attack", _Sound);

        _Sound = Resources.Load("Third Sub Attack Sound") as AudioClip;

        m_pSoundManager.AddSound("Third Sub Attack", _Sound);

        _Sound = Resources.Load("Player Hit Sound") as AudioClip;

        m_pSoundManager.AddSound("Player Hit", _Sound);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "EnemyWeapon")
        {
            m_ZeroAnimations.AccessZeroAnimaions = ZeroAnimations.ZERO_HIT;

            if (m_ZeroAnimations.AccessHit >= 3)
                m_ZeroAnimations.AccessHit = 0;

            if(m_fHp > 0.0f)
                m_fHp -= 10.0f;

            m_ZeroAnimations.AccessHit += 1;

            m_bIsIdle = false;
        }
    }

    public void StartBoxCollision()
    {
        m_SwordBox.enabled = true;
    }

    public void EndBoxCollision()
    {
        m_SwordBox.enabled = false;
    }

    public void StartStateIdle()
    {
        m_bIsIdle = true;
    }

    public void EndStateIdle()
    {
        m_bIsIdle = false;
    }

    public void StartSubStateIdle()
    {
        m_bIsSubIdle = true;
    }

    public void EndSubStateIdle()
    {
        m_bIsSubIdle = false;
    }

    public void OnNextAttack()
    {
        m_ZeroAnimations.AccessClick = true;
    }

    public void OffNextAttack()
    {
        m_ZeroAnimations.AccessClick = false;
    }

    public void OnTrailBuffer()
    {
        m_TarilRenderer.enabled = true;
    }

    public void OffTrailBuffer()
    {
        m_TarilRenderer.enabled = false;
    }

    public void Create_DashEffect()
    {
        _GameobjectManager.GameObjectActivation("Dash Effect", transform.position + Vector3.up);
    }

    public void PlayRunSound()
    {
        m_pSoundManager.PlaySound("Run Sound");
    }

    public void SubAttackPlayingSound()
    {
        switch(m_ZeroAnimations.AccessZeroSubAttackType)
        {
            case ZeroSubAttackType.ZNA_SUBATTACK1:
                m_pSoundManager.PlaySound("First Sub Attack");
                break;
            case ZeroSubAttackType.ZNA_SUBATTACK2:
                m_pSoundManager.PlaySound("Second Sub Attack");
                break;
            case ZeroSubAttackType.ZNA_SUBATTACK3:
                m_pSoundManager.PlaySound("Third Sub Attack");
                break;
        }
    }

    public void AttackPlayingSound()
    {
        switch (m_ZeroAnimations.AccessZeroAttackType)
        {
            case ZeroNormalAttackType.ZNA_ATTACK1:
                m_pSoundManager.PlaySound("First Attack", 1.0f);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK2:
                m_pSoundManager.PlaySound("Second Attack", 1.0f);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK3:
                m_pSoundManager.PlaySound("Third Attack", 1.0f);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK4:
                m_pSoundManager.PlaySound("Fourth Attack", 1.0f);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK5:
                m_pSoundManager.PlaySound("Fifth Attack", 1.0f);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK6:
                m_pSoundManager.PlaySound("Sixth Attack", 1.0f);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK7:
                m_pSoundManager.PlaySound("Seventh Attack", 1.0f);
                break;
            case ZeroNormalAttackType.ZNA_ATTACK8:
                m_pSoundManager.PlaySound("Eighth Attack", 1.0f);
                break;
        }
    }

    public void HitSoundPlay()
    {
        m_pSoundManager.PlaySound("Player Hit");
    }
}