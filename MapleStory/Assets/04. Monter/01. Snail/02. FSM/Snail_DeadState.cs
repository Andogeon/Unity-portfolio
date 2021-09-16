using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 애니메이터 달팽이 사망 모션 클래스입니다.
public class Snail_DeadState : StateMachineBehaviour
{
    private MONTER m_pDeleteMonter = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private Snail m_pSnail = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pDeleteMonter = animator.gameObject.GetComponent<MONTER>();

        m_pSnail = m_pDeleteMonter as Snail;
    }

    // JuniorStoneDeadState 클래스 OnStateUpdate 함수와 동입니다.
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            m_pSnail.AccessPlayerObject.AccessExp += 15.0f;

            m_pGameobjectManager.Remove(m_pDeleteMonter.AccessMonterName, animator.gameObject);

            animator.SetBool("DEAD", false);
        }
    }
}
