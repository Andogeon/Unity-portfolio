using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이터 달팽이 달리기 모션 클래스입니다.
public class Snail_RunState : StateMachineBehaviour
{
    private Snail m_pStoneBall = null;

    private int iSelectMode = -1;

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

        // 충돌 시 박스가 가지고 잇는 회전방향으로 몬스터를 회전
        if (_Collision != null)
            _Transform.eulerAngles = _Collision.transform.eulerAngles;

        // 이동

        _Transform.position += (_Transform.right * -1.0f) * 3.0f * Time.deltaTime;
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pStoneBall = null;
    }
}
