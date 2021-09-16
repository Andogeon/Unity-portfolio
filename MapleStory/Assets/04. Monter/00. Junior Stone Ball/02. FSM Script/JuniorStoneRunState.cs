using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JuniorStoneRunState : StateMachineBehaviour
{
    private JuniorStoneBall m_pStoneBall = null;

    private int iSelectMode = -1; 

    private bool m_bIsCollision = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pStoneBall = animator.gameObject.GetComponent<JuniorStoneBall>();

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
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) // 이건 잠시 대기하고 
    {
        Transform _Transform = animator.gameObject.transform;

        Collider2D _Collision = Physics2D.OverlapBox(_Transform.position, m_pStoneBall.AccessBoxSize, 0.0f, m_pStoneBall.AccessLayer);

        // 맵에 임의로 설정된 박스에 충돌 했다면 
        if (_Collision != null && m_bIsCollision == false)
        {
            m_bIsCollision = true;

            if (_Transform.eulerAngles.y == 180.0f)
                _Transform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f); // 0도로 회전
            else if (_Transform.eulerAngles.y == 0.0f)
                _Transform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f); // 180도 회전 
        } 
        else if (_Collision == null)
            m_bIsCollision = false;

        _Transform.position += (_Transform.right * -1.0f) * 3.0f * Time.deltaTime; // 현 오브젝트 우측방향으로 이동한다 
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_bIsCollision = false;

        m_pStoneBall = null;
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
