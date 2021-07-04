using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float Speed = 0.0f;

    [SerializeField] private float LifeTime = 0.0f;

    private float LifeTimeAcc = 0.0f;

    private float OrlSpeed = 0.0f;

    private Vector3 m_TargetPosition = Vector3.zero;

    private GameobjectManager _GameObjectManager = null;

    private bool m_bArrowFire = false;

    public bool SetFireArrow
    {
        set { m_bArrowFire = value; }
    }

    private void OnEnable()
    {
        _GameObjectManager = GameobjectManager.GetInstance();

        Transform _TargetTransform = GameObject.Find("Zero").GetComponent<Transform>();

        if (_TargetTransform == null)
            return;

        m_TargetPosition = _TargetTransform.position;

        StartCoroutine("MoveArrow");
    }

    private void OnDisable()
    {
        m_bArrowFire = false;

        LifeTimeAcc = 0.0f;

        StopCoroutine("MoveArrow");
    }

    IEnumerator MoveArrow()
    {
        while (true)
        {
            if (m_bArrowFire == false)
                OrlSpeed = 0.0f;
            else
            {
                OrlSpeed = Speed;

                LifeTimeAcc += Time.deltaTime;
            }

            if (LifeTimeAcc > LifeTime)
            {
                _GameObjectManager.GameObjectDisabled("Arrow", gameObject);

                yield return null;
            }

            transform.position += transform.up * -1.0f * OrlSpeed * Time.deltaTime;

            yield return null;
        }
    }
}