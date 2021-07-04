using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour
{
    [SerializeField] private GameObject _Checkpoint = null;

    private GameobjectManager m_pInstanceManager = GameobjectManager.GetInstance();

    private AutoGateSwitch m_pAutoDoor = null;

    private List<Swordknight> m_pMonterList = new List<Swordknight>();

    private List<BowKnight> m_pBowKnight = new List<BowKnight>();

    private Boss m_pBoss = null;

    private GameObject _Zeroobject = null;

    private void Start()
    {
        m_pAutoDoor = _Checkpoint.GetComponent<AutoGateSwitch>();

        _Zeroobject = GameObject.Find("Zero");

        Point[] _Points = GetComponentsInChildren<Point>();

        if (null == _Points)
            return;

        for (int i = 0; i < _Points.Length; ++i)
        {
            Monter _Monter = _Points[i].GetMonter;

            if (_Monter as Swordknight != null)
            {
                Swordknight _Swordknight = _Monter as Swordknight;

                _Swordknight.SetSwordknightState = SwordknightState.IDLE;

                m_pMonterList.Add(_Swordknight);
            }
            else if (_Monter as BowKnight != null)
            {
                BowKnight _BowKnight = _Monter as BowKnight;

                _BowKnight.SetState = BW_State.BW_IDLE;

                m_pBowKnight.Add(_BowKnight);
            }
            else if(_Monter as Boss != null)
                m_pBoss = _Monter as Boss;
        }

        StartCoroutine("CheckingDoor");
    }

    IEnumerator CheckingDoor()
    {
        while (true)
        {
            if (m_pAutoDoor.GetOffDoor == true)
            {
                for (int i = 0; i < m_pMonterList.Count; ++i)
                {
                    m_pMonterList[i].SetMoving = _Zeroobject;

                    m_pMonterList[i].SetSwordknightState = SwordknightState.ATTACK_MOVING;
                }

                for (int i = 0; i < m_pBowKnight.Count; ++i)
                    m_pBowKnight[i].SetState = BW_State.BW_ATTACKREADY;

                if(m_pBoss != null)
                    m_pBoss.SetState = BOSS_STATE.BOSS_RUN;

                yield break;
            }

            yield return null;
        }
    }
}


//[SerializeField] private GameObject _CreatePosition = null;

//[SerializeField] private GameObject _CreateGameObject = null;

//private GameobjectManager _InstanceManager = GameobjectManager.GetInstance();

//private AutoGateSwitch _AutoDoor = null;

//private List<Swordknight> m_SwordKnight = new List<Swordknight>();

//// Start is called before the first frame update
//void Start()
//{
//    _AutoDoor = GameObject.Find("AutoDoorSwitch").GetComponent<AutoGateSwitch>();

//    Transform[] _CreatePositions = GetComponentsInChildren<Transform>();

//    for (int i = 1; i < _CreatePositions.Length; ++i)
//    {
//        Vector3 _Createposition = _CreatePositions[i].position;

//        _InstanceManager.CreateobjectToInsert("Sword Knight", _CreateGameObject, _Createposition, Quaternion.Euler(0.0f, 270.0f, 0.0f), 1);
//    }

//    List<GameObject> _DontInstanceGameObject = _InstanceManager.DontInstacneGameObject("Sword Knight");

//    if (null == _DontInstanceGameObject)
//        return;

//    for (int i = 0; i < _DontInstanceGameObject.Count; ++i) // 24??
//    {
//        Swordknight _Swordknight = _DontInstanceGameObject[i].GetComponent<Swordknight>();

//        if (null == _Swordknight)
//            return;

//        if (_DontInstanceGameObject[i].transform.position != Vector3.zero)
//        {
//            _Swordknight.SetSwordknightState = SwordknightState.IDLE;

//            _Swordknight.SetPositions = _CreatePositions;

//            if (i > 2)
//                m_SwordKnight.Add(_Swordknight);

//            _InstanceManager.GameObjectActivation("Sword Knight", Vector3.zero);
//        }
//    }

//    StartCoroutine("CheckingDoor");
//}

//IEnumerator CheckingDoor()
//{
//    while (true)
//    {
//        if (_AutoDoor.GetOffDoor == true)
//        {
//            for (int i = 0; i < m_SwordKnight.Count; ++i)
//                m_SwordKnight[i].SetSwordknightState = SwordknightState.RUN;

//            yield break;
//        }

//        yield return null;
//    }
//}
