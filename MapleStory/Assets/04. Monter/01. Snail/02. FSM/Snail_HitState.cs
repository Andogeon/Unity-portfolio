using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이터 달팽이 피격 모션 클래스입니다.
public class Snail_HitState : StateMachineBehaviour
{
    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("HIT", false);

        animator.SetBool("ATTACK", true);
    }
}
