using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail_RunState : StateMachineBehaviour
{
    private Snail m_pStoneBall = null;

    private int iSelectMode = -1;

    //private bool m_bIsCollision = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pStoneBall = animator.gameObject.GetComponent<Snail>();

        iSelectMode = Random.Range(0, 1);

        switch (iSelectMode)
        {
            case 0:
                m_pStoneBall.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                break;
            case 1:
                m_pStoneBall.transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                break;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Transform _Transform = animator.gameObject.transform;

        Collider2D _Collision = Physics2D.OverlapBox(_Transform.position, m_pStoneBall.AccessBoxSize, 0.0f, m_pStoneBall.AccessLayer);

        if (_Collision != null)
            _Transform.eulerAngles = _Collision.transform.eulerAngles;

        _Transform.position += (_Transform.right * -1.0f) * 3.0f * Time.deltaTime;

        //if (_Collision != null && m_bIsCollision == false)
        //{
        //    if (_Transform.eulerAngles.y == 180.0f)
        //        _Transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        //    else if (_Transform.eulerAngles.y == 0.0f)
        //        _Transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);

        //    m_bIsCollision = true;
        //}

        //else if (_Collision == null) // null 아니구 .. 그냥 무조건 간다?!
        //    m_bIsCollision = false;

        ////else if (_Collision != null && m_bIsCollision ==true)
        ////{
        ////    if (_Transform.eulerAngles.y == 180.0f)
        ////        _Transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
        ////    else if (_Transform.eulerAngles.y == 0.0f)
        ////        _Transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
        ////}
        //else if (_Collision == null) // null 아니구 .. 그냥 무조건 간다?!
        //    m_bIsCollision = false;

        //_Transform.position += (_Transform.right * -1.0f) * 3.0f * Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //m_bIsCollision = false;

        m_pStoneBall = null;
    }
}
