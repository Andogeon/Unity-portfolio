﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuniorStoneHitState : StateMachineBehaviour
{
    private JuniorStoneBall m_pStoneBall = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pStoneBall = animator.gameObject.GetComponent<JuniorStoneBall>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_pStoneBall.AccessHp <= 0.0f)
        {
            animator.SetBool("RUN", false);

            animator.SetBool("ATTACK", false);

            animator.SetBool("HIT", false);

            animator.SetBool("DEAD", true);

            return;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("HIT", false);
        
        animator.SetBool("ATTACK", true);
    }
}


// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//{

//}

// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
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