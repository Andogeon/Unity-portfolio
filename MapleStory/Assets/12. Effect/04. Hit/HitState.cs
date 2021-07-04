using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitState : StateMachineBehaviour
{
    private GameobjectManager m_pGameobjectManager = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pGameobjectManager = GameobjectManager.GetInstance();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(stateInfo.normalizedTime >= 0.9f)
        {
            string _HitEffectName = animator.gameObject.name;

            switch (_HitEffectName)
            {
                case "Sworld Effect(Clone)":
                    m_pGameobjectManager.Remove("Sword Hit Effect", animator.gameObject);
                    break;
                case "Stick Effect(Clone)":
                    m_pGameobjectManager.Remove("Stick Hit Effect", animator.gameObject);
                    break;
            }

            m_pGameobjectManager = null;
        }
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    string _HitEffectName = animator.gameObject.name;

    //    switch (_HitEffectName)
    //    {
    //        case "Sworld Effect (Clone)":
    //            m_pGameobjectManager.Remove("Sword Hit Effect", animator.gameObject);
    //            break;
    //        case "Stick Effect (Clone)":
    //            m_pGameobjectManager.Remove("Stick Hit Effect", animator.gameObject);
    //            break;
    //    }

    //    m_pGameobjectManager = null;
    //}
}




// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
//override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//{
//    
//}

// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
//override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
//{
//    
//}

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
