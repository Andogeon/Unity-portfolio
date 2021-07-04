using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapleDeadState : StateMachineBehaviour
{
    private GameobjectManager m_pGameObjectManager = GameobjectManager.GetInstance();

    private MapleBox m_pMapleBox = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(null == m_pMapleBox)
            m_pMapleBox = animator.gameObject.GetComponent<MapleBox>();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pGameObjectManager.GameObejctPooling(m_pMapleBox.AccessFieldName, Vector3.zero, animator.gameObject.transform.position, Quaternion.identity);

        m_pGameObjectManager = null;

        Destroy(animator.gameObject);

        m_pMapleBox = null;
    }
}
