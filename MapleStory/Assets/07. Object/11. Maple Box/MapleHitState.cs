using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleHitState : StateMachineBehaviour // 카운터 조정 다시해야 됨 !
{
    //[SerializeField] MapleBox _BoxObject = null;

    private MapleBox m_pMapleBox = null;

    //private bool m_bIsHit = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (null == m_pMapleBox)
            m_pMapleBox = animator.gameObject.GetComponent<MapleBox>();
    }

    //// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("HIT", false);

        m_pMapleBox.AccessHit = false;
    }

    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion

    //    m_pMapleBox.AccessHit = false;
    //}
}
