using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using UnityEngine.Rendering;

public enum SwordknightState
{
    IDLE,
    ATTACK_READY,
    FACE_ATTACK_READY,
    NORMAL_ATTACK,
    ATTACK_MOVING,
    REMOVE_POSITION,
    RUN,
    HIT,
    DEAD
};

public class Swordknight : Monter
{
    [SerializeField] private GameObject _SwordObject = null;

    [SerializeField] private float _Speed = 0.0f;

    [SerializeField] private float HP = 0.0f;

    [SerializeField] private float DeadTimeAcc = 0.0f;

    [SerializeField] private GameObject _GameObject = null;

    [SerializeField] private float _YLenght = 0.0f;

    [SerializeField] private float _ZLenght = 0.0f;

    [SerializeField] private Material NewBodyMaterial = null;

    [SerializeField] private Material NewWeaponMaterial = null;

    [SerializeField] private GameObject _HitEffect = null;

    [SerializeField] private Sprite _NameImage = null;

    [SerializeField] private Sprite _FaceSprite = null;

    private SkinnedMeshRenderer m_pSwordMaterial = null;

    private SkinnedMeshRenderer m_pMeshRenderer = null;

    private SwordknightState m_eState = SwordknightState.REMOVE_POSITION;

    private SW_Animation m_SwordAnimation = null;

    private BoxCollider m_SwordObject = null;

    private NavMeshAgent m_pNavigation = null;

    private GameobjectManager _GameobjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private GameObject m_pMoveObject = null;

    private Zero _Targetobject = null;

    private EnemyUIBar m_pEnemyBar = null;

    //private EnemyFOV m_EnemyFov = null;

    private Ray m_EnemyRay;

    private bool m_bIsDead = false;

    private bool m_bIsResetingMaterial = false;

    private float m_AttackTimeAcc = 0.0f;

    private Vector3 m_vDirection = Vector3.zero;

    private Vector3 m_vOrlPosition = Vector3.zero;

    private Vector3 m_vNameScale = new Vector3(1.0f, 0.48762f, 1.0f);

    private Animator m_pAnimator = null;

    private int m_iPositionIndex = 0;

    private float DeadTime = 0.0f;

    private float MaxHp = 0.0f;

    //private float DeleteHp = 0.0f;

    public GameObject SetMoving
    {
        set { m_pMoveObject = value; }
    }

    public SW_Animation GetSwordAnimations
    {
        get { return m_SwordAnimation; }
    }

    public SwordknightState SetSwordknightState
    {
        set { m_eState = value; }
    }

    private void Start()
    {
        m_SwordAnimation = new SW_Animation(GetComponent<Animator>());

        m_SwordObject = _SwordObject.GetComponent<BoxCollider>();

        m_pAnimator = GetComponent<Animator>();

        m_pNavigation = GetComponent<NavMeshAgent>();

        //m_EnemyFov = gameObject.GetComponent<EnemyFOV>();

        m_pMeshRenderer = _GameObject.GetComponent<SkinnedMeshRenderer>();

        m_pSwordMaterial = _SwordObject.GetComponentInChildren<SkinnedMeshRenderer>();

        _Targetobject = GameObject.Find("Zero").GetComponent<Zero>();

        m_vOrlPosition = transform.position;

        m_EnemyRay = new Ray();

        m_SwordObject.enabled = false;

        m_pNavigation.speed = _Speed;

        MaxHp = HP;

        _GameobjectManager.CreateobjectToInsert("Hit Effect", _HitEffect, Vector3.zero, Quaternion.identity, 1);



        m_pEnemyBar = GameObject.Find("Canvas").GetComponent<EnemyUIBar>();       

        StartCoroutine("SwordUpdate");
    }

    private void OnDisable()
    {
        StopCoroutine("SwordUpdate");
    }

    IEnumerator SwordUpdate()
    {
        while (true)
        {
            if(m_bIsDead == true)
            {
                DeadTime += Time.deltaTime;

                ResetMaterial();

                for (int i = 0; i < m_pMeshRenderer.materials.Length; ++i)
                {
                    Material _Material = m_pMeshRenderer.materials[i];

                    _Material.SetFloat("_Cut", DeadTime * 0.15f);
                }

                m_pSwordMaterial.material.SetFloat("_Cut", DeadTime * 0.15f);


                if (DeadTime >= DeadTimeAcc)
                {
                    GameobjectManager.GetInstance().GameObjectDisabled("Sword knight", gameObject);

                    DeadTime = 0.0f;

                    yield break;
                }

                yield return null;
            }

            switch(m_eState)
            {
                case SwordknightState.IDLE:
                    Idle();
                    break;
                case SwordknightState.FACE_ATTACK_READY:
                    FackAttackReady();
                    break;
                case SwordknightState.ATTACK_READY:
                    AttackReady();
                    break;
                case SwordknightState.RUN:
                    Run();
                    break;
                case SwordknightState.NORMAL_ATTACK:
                    NormalAttack();
                    break;
                case SwordknightState.REMOVE_POSITION:
                    RemovingPositions();
                    break;
                case SwordknightState.HIT:
                    Hit();
                    break;
                case SwordknightState.DEAD:
                    Dead();
                    break;
                case SwordknightState.ATTACK_MOVING:
                    AttackMoving();
                    break;
            }

            m_SwordAnimation.UpDateAnimation();

            yield return null;
        }
    }

    private void Idle()
    {
        if (m_pNavigation.isStopped == false)
        {
            m_pNavigation.isStopped = true;

            m_pNavigation.velocity = Vector3.zero;
        }

        if (HP <= 0.0f)
        {
            m_eState = SwordknightState.DEAD;

            m_pEnemyBar.OffEnemyBar();

            return;
        }

        Collider[] _Colliders = Physics.OverlapSphere(transform.position + Vector3.up * 2.0f, 15.0f, 1 << 8);

        if (_Colliders.Length > 0)
        {
            m_pMoveObject = _Colliders[0].gameObject;

            m_eState = SwordknightState.RUN;

            return;
        }

        m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_IDLE;
    }

    private void Run()
    {
        if (m_pNavigation.isStopped == true)
            m_pNavigation.isStopped = false;

        if (HP <= 0.0f)
        {
            m_eState = SwordknightState.DEAD;

            m_pEnemyBar.OffEnemyBar();

            return;
        }

        if (m_pMoveObject == null)
            return;

        Transform _PlayerTransform = m_pMoveObject.transform;

        ObjectRotation(_PlayerTransform);

        m_EnemyRay.origin = transform.position + Vector3.up * _YLenght + transform.forward * _ZLenght;

        m_EnemyRay.direction = transform.forward;

        if (Physics.Raycast(m_EnemyRay, 1.0f, 1 << 9)) // 현재 앞에 적군이 따로 존재한다면
        {
            m_eState = SwordknightState.FACE_ATTACK_READY; // 공격하는 형태만 취하고 끝냄..

            return;
        }

        float _Length = (_PlayerTransform.position - transform.position).magnitude;

        if(_Length <= 4.0f)
        {
            m_eState = SwordknightState.ATTACK_READY;

            return;
        }

        Collider[] _Colliders = Physics.OverlapSphere(transform.position + Vector3.up * 2.0f, 15.0f, 1 << 8);

        if(_Colliders.Length == 0)
        {
            m_eState = SwordknightState.REMOVE_POSITION;

            m_pMoveObject = null;

            return;
        }

        m_pNavigation.SetDestination(m_pMoveObject.transform.position);

        m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_RUN;
    }
    
    private void FackAttackReady()
    {
        if(m_pNavigation.isStopped == false)
        {
            m_pNavigation.isStopped = true;

            m_pNavigation.velocity = Vector3.zero;
        }

        if (HP <= 0.0f)
        {
            m_eState = SwordknightState.DEAD;

            m_pEnemyBar.OffEnemyBar();

            return;
        }

        Collider[] _Colliders = Physics.OverlapSphere(transform.position + Vector3.up * 2.0f, 15.0f, 1 << 8);

        if(_Colliders.Length == 0)
        {
            m_eState = SwordknightState.REMOVE_POSITION;

            return;
        }

        Transform _PlayerTransform = _Colliders[0].gameObject.transform;

        ObjectRotation(_PlayerTransform);

        m_EnemyRay.origin = transform.position + Vector3.up * _YLenght + transform.forward * _ZLenght;

        m_EnemyRay.direction = transform.forward;

        if (Physics.Raycast(m_EnemyRay, 1.0f, 1 << 9)) // 현재 앞에 적군이 따로 존재한다면
        {
            m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_FACE_ATTACKREADY;

            return;
        }

        m_eState = SwordknightState.RUN;
    }
    private void AttackMoving()
    {
        if (m_pNavigation.isStopped == true)
            m_pNavigation.isStopped = false;

        Collider[] _Colliders = Physics.OverlapSphere(transform.position + Vector3.up * 2.0f, 15.0f, 1 << 8);

        if(_Colliders.Length != 0)
        {
            m_eState = SwordknightState.RUN;

            return;
        }

        ObjectRotation(m_pMoveObject.transform);

        m_pNavigation.SetDestination(m_pMoveObject.transform.position);

        m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_RUN;
    }
    private void AttackReady()
    {
        if (m_pNavigation != null && m_pNavigation.isStopped == false)
        {
            m_pNavigation.isStopped = true;

            m_pNavigation.velocity = Vector3.zero;
        }

        if (HP <= 0.0f)
        {
            m_eState = SwordknightState.DEAD;

            m_pEnemyBar.OffEnemyBar();

            return;
        }

        ObjectRotation(m_pMoveObject.transform);   

        Collider[] _Colliders = Physics.OverlapSphere(transform.position + Vector3.up * 2.0f, 15.0f, 1 << 8);

        float _Length = (m_pMoveObject.transform.position - transform.position).magnitude;

        if (_Colliders.Length == 0)
        {
            m_eState = SwordknightState.REMOVE_POSITION;

            m_AttackTimeAcc = 0.0f;

            return;
        }

        if (_Length > 4.0f)
        {
            m_eState = SwordknightState.RUN;

            m_AttackTimeAcc = 0.0f;

            return;
        }

        m_AttackTimeAcc += Time.deltaTime;

        if (m_AttackTimeAcc >= 2.0f)
        {
            m_eState = SwordknightState.NORMAL_ATTACK;

            m_AttackTimeAcc = 0.0f;

            return;
        }

        m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_ATTACKREADY;
    }

    private void NormalAttack()
    {
        if (m_pNavigation.isStopped == false)
        {
            m_pNavigation.isStopped = true;

            m_pNavigation.velocity = Vector3.zero;
        }

        if (HP <= 0.0f)
        {
            m_eState = SwordknightState.DEAD;

            m_pEnemyBar.OffEnemyBar();

            return;
        }

        ObjectRotation(m_pMoveObject.transform);

        Collider[] _Colliders = Physics.OverlapSphere(transform.position + Vector3.up * 2.0f, 15.0f, 1 << 8);

        float _Length = (m_pMoveObject.transform.position - transform.position).magnitude;

        if (_Colliders.Length == 0)
        {
            m_eState = SwordknightState.REMOVE_POSITION;

            m_AttackTimeAcc = 0.0f;

            return;
        }

        if (_Length > 4.0f)
        {
            m_eState = SwordknightState.RUN;

            m_AttackTimeAcc = 0.0f;

            return;
        }

        m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_NORMALATTACK;
    }

    private void Hit()
    {
        if (HP <= 0.0f)
        {
            m_eState = SwordknightState.DEAD;

            m_pEnemyBar.OffEnemyBar();

            return;
        }

        if (m_pNavigation.isStopped == false)
        {
            m_pNavigation.isStopped = true;

            m_pNavigation.velocity = Vector3.zero;
        }

        m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_HIT;
        
        m_eState = SwordknightState.IDLE;
    }

    private void Dead()
    {
        if (m_pNavigation.enabled == true && m_pNavigation.isStopped == false)
        {
            m_pNavigation.isStopped = true;

            m_pNavigation.velocity = Vector3.zero;
        }

        m_bIsDead = true;

        m_pNavigation.enabled = false;

        m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_DEAD;
    }

    private void RemovingPositions() // 이 함수에서 수정할 것 
    {
        if (null == m_pPositions)
            return;

        if (HP <= 0.0f)
        {
            m_eState = SwordknightState.DEAD;

            return;
        }

        if (m_pNavigation.isStopped == true)
            m_pNavigation.isStopped = false;

        Collider[] _Colliders = Physics.OverlapSphere(transform.position + Vector3.up, 15.0f, 1 << 8);

        if (_Colliders.Length != 0)
        {
            m_iPositionIndex = 0;

            m_pMoveObject = _Colliders[0].gameObject;

            m_eState = SwordknightState.FACE_ATTACK_READY;

            return;
        }

        Vector3 _Direction = (m_pPositions[m_iPositionIndex].position - transform.position).normalized;

        float Angle = Vector3.Angle(Vector3.forward, _Direction);

        Vector3 _CrossVector = Vector3.Cross(Vector3.forward, _Direction);

        if (_CrossVector.y < 0)
            Angle *= -1.0f;

        if ((m_pPositions[m_iPositionIndex].position - transform.position).magnitude >= 3.0f)
        {
            transform.rotation = Quaternion.Euler(0.0f, Angle, 0.0f);

            m_pNavigation.SetDestination(m_pPositions[m_iPositionIndex].position);
        }
        else
        {
            m_pNavigation.velocity = Vector3.zero;

            m_iPositionIndex = Random.Range(0, m_pPositions.Length);

            transform.rotation = Quaternion.Euler(0.0f, Angle, 0.0f);

            m_pNavigation.SetDestination(m_pPositions[m_iPositionIndex].position);
        }

        if (m_eState != SwordknightState.FACE_ATTACK_READY)
            m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_RUN;
    }

    private void ObjectRotation(Transform _Transform)
    {
        if (null == _Transform)
        {
            m_vDirection = Vector3.zero;

            return;
        }

        Vector3 _Direction = _Transform.position - transform.position;

        float _Angle = Vector3.Angle(Vector3.forward, _Direction);

        Vector3 _CrossVector = Vector3.Cross(Vector3.forward, _Direction);

        if (_CrossVector.y < 0)
            _Angle *= -1.0f;

        transform.rotation = Quaternion.Euler(transform.rotation.x, _Angle, transform.rotation.z);
    }

    private void ResetMaterial()
    {
        if (m_bIsResetingMaterial == true)
            return;

        if (m_pMeshRenderer.material != NewBodyMaterial)
        {
            m_pMeshRenderer.materials = new Material[] { NewBodyMaterial, NewWeaponMaterial };

            m_pSwordMaterial.material = NewWeaponMaterial;

            m_bIsResetingMaterial = true;
        }
    }

    public void DisabledAttack()
    {
        m_SwordAnimation.AccessAttack = false;

        m_eState = SwordknightState.ATTACK_READY;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_eState == SwordknightState.DEAD)
        {
            m_pMeshRenderer.materials = new Material[] { NewBodyMaterial, NewWeaponMaterial };

            m_pSwordMaterial.material = NewWeaponMaterial;

            m_pMeshRenderer.shadowCastingMode = ShadowCastingMode.Off;

            return;
        }

        if (other.gameObject.tag == "PlayerWeapon")
        {
            m_eState = SwordknightState.HIT;

            _GameobjectManager.GameObjectActivation("Hit Effect", transform.position + Vector3.up * 3.0f);

             _Targetobject.SetGauge += 0.01f;

            if(_Targetobject.SetBlood < 2.0f)
                _Targetobject.SetBlood += 0.02f;

            if (HP > 0.0f)
                HP -= 10.0f;

            m_pEnemyBar.OnEnemyBar();

            m_pSoundManager.PlaySound("Hit", 0.3f);

            Image _HPImage = m_pEnemyBar.GetHpImage;

            if (null == _HPImage)
                return;

            Image NameImage = m_pEnemyBar.GetNameImage;

            if (null == _NameImage)
                return;

            Image FaceImage = m_pEnemyBar.GetFaceImage;

            if (FaceImage == null)
                return;

            _HPImage.fillAmount = HP / MaxHp;

            NameImage.rectTransform.localScale = m_vNameScale;

            NameImage.sprite = _NameImage;

            FaceImage.sprite = _FaceSprite;
        }
    }

    public void ResetIdleReady()
    {
        m_SwordAnimation.AccessSwordAnimations = SW_Animations.SW_IDLE;
    }

    public void OnWeapon()
    {
        m_SwordObject.enabled = true;
    }

    public void OffWeapon()
    {
        m_SwordObject.enabled = false;
    }

    public void OffAttack()
    {
        m_SwordObject.enabled = false;

        m_SwordAnimation.AccessAttack = false;
    }

    public void DeadSound()
    {
        m_pSoundManager.PlaySound("Monter Dead");
    }

    public void SetDead()
    {
        return;
    }
}