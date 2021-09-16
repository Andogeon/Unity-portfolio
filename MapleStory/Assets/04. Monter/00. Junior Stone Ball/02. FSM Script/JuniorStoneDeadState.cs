using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이터 주니어 스톤볼 사냥 모션 클래스입니다.
public class JuniorStoneDeadState : StateMachineBehaviour
{
    private MONTER m_pDeleteMonter = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private SoundManager m_pSoundManager = SoundManager.GetInstance();

    private JuniorStoneBall m_pJuniorStoneBall = null;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pDeleteMonter = animator.gameObject.GetComponent<MONTER>();

        m_pJuniorStoneBall = m_pDeleteMonter as JuniorStoneBall;

        GameObject _Object = m_pGameobjectManager.GameObejctPooling("Stone Etc Item", Vector3.zero, m_pDeleteMonter.transform.position, Quaternion.identity);

        _Object.transform.localScale = new Vector2(3.0f, 3.0f);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // 해당 애니메이션이 종료 된다면 
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f) 
        {
            m_pJuniorStoneBall.AccessPlayerObject.AccessExp += 10.0f; // 플레이어에게 경험치를 주고 

            m_pGameobjectManager.Remove(m_pDeleteMonter.AccessMonterName, animator.gameObject); // 오브젝트 풀링에 다시 넣는다.

            animator.SetBool("DEAD", false); // 애니메이션을 사망끈다
        }
    }
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