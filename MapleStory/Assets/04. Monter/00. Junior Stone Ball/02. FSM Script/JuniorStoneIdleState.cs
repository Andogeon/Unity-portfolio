using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이터 주니어스톤볼 대기 모션 클래스입니다.

public class JuniorStoneIdleState : StateMachineBehaviour
{
    private JuniorStoneBall m_pStoneBall = null;

    private float m_fTimeAcc = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pStoneBall = animator.gameObject.GetComponent<JuniorStoneBall>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_fTimeAcc += Time.deltaTime;

        if(m_fTimeAcc >= 2.0f) // 2초 간격으로 달리기로 애니메이션 변경 
        {
            m_fTimeAcc = 0.0f;

            animator.SetBool("RUN", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
