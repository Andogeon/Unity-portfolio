using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AIState
{
    IDLE,
    RUN,
    REMOVING
}


public class Point : MonoBehaviour
{
    [SerializeField] private Monter _MonterObject = null;

    [SerializeField] private GameObject _MovePositions = null;

    [SerializeField] private AIState _AIState = AIState.IDLE;

    [SerializeField] private float Angle = 0.0f;

    private GameobjectManager _GameobjectManager = GameobjectManager.GetInstance();

    private Monter m_pMonter = null;

    public Monter GetMonter
    {
        get { return m_pMonter; }
    }

    private void Awake()
    {
        if (null == _MonterObject)
            return;

        Transform[] _Transforms = _MovePositions.GetComponentsInChildren<Transform>();

        _GameobjectManager.CreateobjectToInsert(_MonterObject.name, _MonterObject.gameObject, Vector3.zero, Quaternion.identity, 1);

        m_pMonter = _GameobjectManager.DontInstacneObject(_MonterObject.name).GetComponent<Monter>();

        if (m_pMonter as Swordknight != null)
        {
            Swordknight _Swordknight = m_pMonter as Swordknight;

            switch (_AIState)
            {
                case AIState.IDLE:
                    _Swordknight.SetSwordknightState = SwordknightState.IDLE;
                    break;
                case AIState.REMOVING:
                    _Swordknight.SetSwordknightState = SwordknightState.REMOVE_POSITION;
                    break;
                case AIState.RUN:
                    _Swordknight.SetSwordknightState = SwordknightState.RUN;
                    break;
            }
        }

        m_pMonter.SetPositions = _Transforms;

        _GameobjectManager.GameObjectActivation(_MonterObject.name, transform.position, Angle);
    }
}
