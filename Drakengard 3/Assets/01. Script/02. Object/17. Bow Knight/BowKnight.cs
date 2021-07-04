using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering;
public enum BW_State
{
    BW_IDLE,
    BW_ATTACK,
    BW_ATTACKREADY,
    BW_HIT,
    BW_DEAD
};

public class BowKnight : Monter
{
    [SerializeField] private GameObject _Arrow = null;

    [SerializeField] private GameObject _BowMesh = null;

    [SerializeField] private float AttackModeTime = 0.0f;

    [SerializeField] private float Hp = 0.0f;

    [SerializeField] private Transform _ParentGameobject = null;

    [SerializeField] private Material _NewBodyMaterial = null;

    [SerializeField] private Material _NewWeaponMaterial = null;

    [SerializeField] private Sprite _NameImage = null;

    [SerializeField] private Sprite _FaceSprite = null;

    private GameobjectManager gameobjectManager = null;

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private SkinnedMeshRenderer m_pBodyRenderer = null;

    private SkinnedMeshRenderer m_pBowRenderer = null;

    private BW_Animation m_pBWAnimation = null;

    private Arrow _InstanceArrow = null;

    private GameObject _ArrowObject = null;

    private EnemyUIBar m_pEnemyBar = null;

    private GameObject _Zeroobejct = null;

    private Quaternion _BowRotation = Quaternion.identity;

    private BW_State m_eBW_State = BW_State.BW_IDLE;

    private Vector2 m_vNamePosition = new Vector2(517.2f, -276.8f);

    private Vector2 m_vNameScale = new Vector2(1.8621f, 0.48762f);

    private float m_fAttackTimeAcc = 0.0f;

    private float m_fDeadTimeAcc = 0.0f;

    private float m_fMaxHp = 0.0f;

    private bool m_bIsDead = false;

    public BW_State SetState
    {
        set { m_eBW_State = value; }
    }

    private void Start()
    {
        if (null == m_pBWAnimation)
            m_pBWAnimation = new BW_Animation(GetComponent<Animator>());

        if (null == gameobjectManager)
            gameobjectManager = GameobjectManager.GetInstance();

        m_pBodyRenderer = GetComponentInChildren<SkinnedMeshRenderer>();

        m_pBowRenderer = _BowMesh.GetComponentInChildren<SkinnedMeshRenderer>();

        _Zeroobejct = GameObject.Find("Zero");

        gameobjectManager.CreateobjectToInsert("Arrow", _Arrow, Vector3.zero, Quaternion.identity, 1);

        m_pEnemyBar = GameObject.Find("Canvas").GetComponent<EnemyUIBar>();

        m_fMaxHp = Hp;

        StartCoroutine("StartEnemy");
    }

    private void OnDisable()
    {
        m_fAttackTimeAcc = 0.0f;

        m_bIsDead = false;

        StopCoroutine("StartEnemy");
    }

    IEnumerator StartEnemy()
    {
        while(!m_bIsDead)
        {
            switch(m_eBW_State)
            {
                case BW_State.BW_IDLE:
                    Idle();
                    break;
                case BW_State.BW_ATTACKREADY:
                    AttackReady();
                    break;
                case BW_State.BW_ATTACK:
                    Attack();
                    break;
                case BW_State.BW_HIT:
                    m_pBWAnimation.BWAnimations = BW_Animations.BW_HIT;
                    break;
                case BW_State.BW_DEAD:
                    m_pBWAnimation.BWAnimations = BW_Animations.BW_DEAD;
                    DeadShaderMode();
                    break;
            }

            m_pBWAnimation.UpDateAnimation();

            yield return null;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (m_eBW_State == BW_State.BW_DEAD)
            return;

        if (other.gameObject.tag == "PlayerWeapon")
        {
            bool _bIsHit = m_pBWAnimation.GetHit;

            if (_bIsHit == true)
                m_pBWAnimation.GetHit = false;

            gameobjectManager.GameObjectActivation("Hit Effect", transform.position + Vector3.up * 3.0f);

            m_pSoundManager.PlaySound("Hit", 0.3f);

            if (Hp <= 0.0f)
            {
                m_pBodyRenderer.material = _NewBodyMaterial;

                m_pBowRenderer.material = _NewWeaponMaterial;

                m_pBodyRenderer.shadowCastingMode = ShadowCastingMode.Off;

                m_pBowRenderer.shadowCastingMode = ShadowCastingMode.Off;

                m_eBW_State = BW_State.BW_DEAD;

                m_pEnemyBar.OffEnemyBar();

                return;
            }

            Hp -= 10.0f;

            m_eBW_State = BW_State.BW_HIT;

            m_pEnemyBar.OnEnemyBar();

            Image _HpImage = m_pEnemyBar.GetHpImage;

            if (null == _HpImage)
                return;

            Image NameImage = m_pEnemyBar.GetNameImage;

            if (null == _HpImage)
                return;

            Image FaceImage = m_pEnemyBar.GetFaceImage;

            if (null == FaceImage)
                return;

            _HpImage.fillAmount = Hp / m_fMaxHp;

            NameImage.rectTransform.localPosition = m_vNamePosition;

            NameImage.rectTransform.localScale = m_vNameScale;

            NameImage.sprite = _NameImage;

            FaceImage.sprite = _FaceSprite;
        }
    }

    private void Idle()
    {
        Vector3 _Direction = _Zeroobejct.transform.position - transform.position;

        float _Angle = Vector3.Angle(Vector3.forward, _Direction);

        Vector3 _CrossVector = Vector3.Cross(Vector3.forward, _Direction);

        if (_CrossVector.y < 0)
            _Angle *= -1.0f;

        transform.rotation = Quaternion.Euler(transform.rotation.x, _Angle, transform.rotation.z);

        Collider[] _Collision = Physics.OverlapSphere(transform.position + Vector3.up, 15.0f, 1 << 8);

        if(_Collision.Length != 0)
        {
            m_eBW_State = BW_State.BW_ATTACKREADY;

            return;
        }

        m_pBWAnimation.BWAnimations = BW_Animations.BW_IDLE;
    }

    private void AttackReady()
    {
        //Collider[] _Collision = Physics.OverlapSphere(transform.position + Vector3.up, 15.0f, 1 << 8);

        //if (_Collision.Length == 0)
        //{
        //    m_eBW_State = BW_State.BW_IDLE;

        //    return;
        //}

        Vector3 _Direction = _Zeroobejct.transform.position - transform.position;

        float _Angle = Vector3.Angle(Vector3.forward, _Direction);

        Vector3 _CrossVector = Vector3.Cross(Vector3.forward, _Direction);

        if (_CrossVector.y < 0)
            _Angle *= -1.0f;

        transform.rotation = Quaternion.Euler(transform.rotation.x, _Angle, transform.rotation.z);

        if (m_fAttackTimeAcc >= AttackModeTime)
        {
            m_fAttackTimeAcc = 0.0f;

            m_eBW_State = BW_State.BW_ATTACK;

            return;
        }

        m_fAttackTimeAcc += Time.deltaTime;

        m_pBWAnimation.BWAnimations = BW_Animations.BW_ATTACKREADY;
    }

    private void Attack()
    {
        Vector3 _Direction = _Zeroobejct.transform.position - transform.position;

        float _Angle = Vector3.Angle(Vector3.forward, _Direction);

        Vector3 _CrossVector = Vector3.Cross(Vector3.forward, _Direction);

        if (_CrossVector.y < 0)
            _Angle *= -1.0f;

        transform.rotation = Quaternion.Euler(transform.rotation.x, _Angle, transform.rotation.z);

        m_pBWAnimation.BWAnimations = BW_Animations.BW_NORMALATTACK;
    }

    public void ResetAttackReady()
    {
        m_eBW_State = BW_State.BW_ATTACKREADY;
    }

    public void CreateArrow()
    {
        GameObject _InstanceGameObejct = gameobjectManager.DontInstacneObject("Arrow");

        if (null == _InstanceGameObejct)
        {
            gameobjectManager.CreateobjectToInsert("Arrow", _Arrow, Vector3.zero, Quaternion.identity, 1);

            _InstanceGameObejct = gameobjectManager.DontInstacneObject("Arrow");
        }

        _InstanceGameObejct.transform.SetParent(_ParentGameobject);

        _InstanceGameObejct.transform.localPosition = new Vector3(1.34f, -0.317f, 0.15f);

        _InstanceGameObejct.transform.localRotation = Quaternion.Euler(10.0f, 0.0f, 90.0f);

        _InstanceGameObejct.SetActive(true);

        _ArrowObject = _InstanceGameObejct;

        _InstanceArrow = _InstanceGameObejct.GetComponent<Arrow>();
    }

    public void FixPositionArrow()
    {
        if (null == _InstanceArrow)
            return;

        _InstanceArrow.transform.SetParent(null);
    }

    public void FireArrow()
    {
        _InstanceArrow.SetFireArrow = true;

        _InstanceArrow = null;
    }

    public void ResetIdle()
    {
        m_eBW_State = BW_State.BW_ATTACKREADY;
    }

    public void SetDead()
    {
        Destroy(gameObject);

        //gameobjectManager.GameObjectDisabled("Bow Kinght", gameobject);
    }

    private void DeadShaderMode()
    {
        if (m_pBodyRenderer.material != _NewBodyMaterial)
        {
            m_pBodyRenderer.material = _NewBodyMaterial;

            m_pBowRenderer.material = _NewWeaponMaterial;

            m_pBodyRenderer.shadowCastingMode = ShadowCastingMode.Off;
        }

        m_fDeadTimeAcc += Time.deltaTime;

        m_pBodyRenderer.material.SetFloat("_Cut", m_fDeadTimeAcc * 0.05f);

        m_pBowRenderer.material.SetFloat("_Cut", m_fDeadTimeAcc * 0.05f);
    }

    public void OffWeapon()
    {
        gameobjectManager.GameObjectDisabled("Arrow", _ArrowObject);

        _ArrowObject = null;       
    }

    public void DeadSound()
    {
        m_pSoundManager.PlaySound("Monter Dead");
    }
}

// 맞는 애니메이션 새롭게 변경 후 다시 애니메이션 클립 달아줄것 !! 






//private void AI_CheckingState()
//{
//    Collider[] _Collision = Physics.OverlapSphere(transform.position + Vector3.up, 15.0f, (1 << 8 | 1 << 9));

//    GameObject _CollisionObject = null;

//    for (int i = 0; i < _Collision.Length; ++i)
//    {
//        if (_Collision[i].gameObject.tag == "Player")
//        {
//            _CollisionObject = _Collision[i].gameObject;

//            break;
//        }
//    }

//    if (_CollisionObject == null)
//    {
//        m_eBW_State = BW_State.BW_IDLE;

//        return;
//    }

//    Vector3 _Direction = _CollisionObject.transform.position - transform.position;

//    float _Angle = Vector3.Angle(Vector3.forward, _Direction);

//    Vector3 _CrossVector = Vector3.Cross(Vector3.forward, _Direction);

//    if (_CrossVector.y < 0)
//        _Angle *= -1.0f;

//    transform.rotation = Quaternion.Euler(transform.rotation.x, _Angle, transform.rotation.z);

//    if (m_eBW_State != BW_State.BW_ATTACK)
//        m_eBW_State = BW_State.BW_ATTACKREADY;

//    if (m_fAttackTimeAcc >= AttackModeTime)
//    {
//        m_fAttackTimeAcc = 0.0f;

//        m_eBW_State = BW_State.BW_ATTACK;

//        return;
//    }

//    m_fAttackTimeAcc += Time.deltaTime;
//}