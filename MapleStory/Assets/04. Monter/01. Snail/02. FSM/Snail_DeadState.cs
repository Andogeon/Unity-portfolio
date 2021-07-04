using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail_DeadState : StateMachineBehaviour
{
    private MONTER m_pDeleteMonter = null;

    private GameobjectManager m_pGameobjectManager = GameobjectManager.GetInstance();

    private Snail m_pSnail = null;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pDeleteMonter = animator.gameObject.GetComponent<MONTER>();

        m_pSnail = m_pDeleteMonter as Snail;

        //GameObject _Object = m_pGameobjectManager.GameObejctPooling("Stone Etc Item", Vector3.zero, m_pDeleteMonter.transform.position, Quaternion.identity);

        //_Object.transform.localScale = new Vector2(3.0f, 3.0f);

        // 메소 획득 시 메소를 X값을 이동하여 만들어논다 !! 
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 0.9f)
        {
            m_pSnail.AccessPlayerObject.AccessExp += 15.0f;

            m_pGameobjectManager.Remove(m_pDeleteMonter.AccessMonterName, animator.gameObject);

            animator.SetBool("DEAD", false);
        }
    }

    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    m_pGameobjectManager.Remove(m_pDeleteMonter.AccessMonterName, animator.gameObject);
    //}
}
