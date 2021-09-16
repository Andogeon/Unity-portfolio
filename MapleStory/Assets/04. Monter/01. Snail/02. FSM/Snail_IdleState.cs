using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail_IdleState : StateMachineBehaviour
{
    private float m_fTimeAcc = 0.0f;

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks

    // JuniorStoneIdleState 클래스 OnStateUpdate 함수와 동일합니다.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_fTimeAcc += Time.deltaTime;

        if (m_fTimeAcc >= 2.0f)
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
