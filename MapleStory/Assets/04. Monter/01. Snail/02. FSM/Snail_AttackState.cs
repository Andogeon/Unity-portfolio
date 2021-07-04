using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snail_AttackState : StateMachineBehaviour
{
    private Transform m_pStoneTransform = null;

    private Transform m_pFollowTransform = null;

    private ATTACKDIRECTION m_eAttackDirection = ATTACKDIRECTION.ATTACK_READY;

    private Snail m_pStoneBall = null;

    private bool m_bIsloop = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pFollowTransform = GameObject.Find("Player").transform;

        m_pStoneTransform = animator.gameObject.transform;

        m_pStoneBall = animator.gameObject.GetComponent<Snail>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (m_pStoneBall.AccessHp <= 0.0f)
        {
            animator.SetBool("DEAD", true);

            animator.SetBool("RUN", false);

            animator.SetBool("ATTACK", false);

            return;
        }

        Vector2 _Direction = m_pFollowTransform.position - m_pStoneTransform.position;

        if (_Direction.magnitude <= 0.5f)
            m_bIsloop = true;
        else if (m_bIsloop == true && _Direction.magnitude >= 5.0f)
            m_bIsloop = false;

        Collider2D _Collision = Physics2D.OverlapBox(animator.gameObject.transform.position, m_pStoneBall.AccessBoxSize, 0.0f, m_pStoneBall.AccessLayer);

        if (null != _Collision)
        {
            animator.SetBool("ATTACK", false);

            animator.SetBool("RUN", false);

            return;
        }

        if (m_bIsloop == true)
        {
            Vector2 _Position = m_pStoneTransform.position;

            _Position.y = 0.0f;

            Vector2 LeftPosition = new Vector2(m_pFollowTransform.position.x - 3.0f, 0.0f);

            Vector2 RightPosition = new Vector2(m_pFollowTransform.position.x + 3.0f, 0.0f);

            float LeftLength = (LeftPosition - _Position).magnitude;

            float RightLength = (RightPosition - _Position).magnitude;

            if (LeftLength >= 0.004f && m_eAttackDirection == ATTACKDIRECTION.ATTACK_READY || m_eAttackDirection == ATTACKDIRECTION.ATTACK_LEFT)
            {
                m_eAttackDirection = ATTACKDIRECTION.ATTACK_LEFT;

                if (RightLength >= 3.5f)
                    m_eAttackDirection = ATTACKDIRECTION.ATTACK_RIGHT;
            }
            else if (RightLength >= 0.004f && m_eAttackDirection == ATTACKDIRECTION.ATTACK_READY || m_eAttackDirection == ATTACKDIRECTION.ATTACK_RIGHT)
            {
                m_eAttackDirection = ATTACKDIRECTION.ATTACK_RIGHT;

                if (LeftLength >= 3.5f)
                    m_eAttackDirection = ATTACKDIRECTION.ATTACK_LEFT;
            }

            AttackDirection();
        }
        else // 추적
        {
            if (m_eAttackDirection != ATTACKDIRECTION.ATTACK_READY)
                m_eAttackDirection = ATTACKDIRECTION.ATTACK_READY;

            if(_Direction.magnitude >= 7.0f)
            {
                animator.SetBool("ATTACK", false);

                animator.SetBool("RUN", true);

                return;
            }

            // 버그 생길시 여기서 확인해볼것 !

            _Direction.y = 0.0f;

            float Angle = Vector2.Angle(_Direction.normalized, Vector2.left);

            m_pStoneTransform.rotation = Quaternion.Euler(0.0f, Angle, 0.0f);

            m_pStoneTransform.position += (Vector3)_Direction.normalized * 1.0f * Time.deltaTime;
        }


        //else // 추적
        //{
        //    if (m_eAttackDirection != ATTACKDIRECTION.ATTACK_READY)
        //        m_eAttackDirection = ATTACKDIRECTION.ATTACK_READY;

        //    _Direction.y = 0.0f;

        //    float Angle = Vector2.Angle(_Direction.normalized, Vector2.left);

        //    m_pStoneTransform.rotation = Quaternion.Euler(0.0f, Angle, 0.0f);

        //    m_pStoneTransform.position += (Vector3)_Direction.normalized * 1.0f * Time.deltaTime;
        //}
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pStoneTransform = null;

        m_pFollowTransform = null;
    }

    private void AttackDirection()
    {
        Vector2 LeftPosition = new Vector2(m_pFollowTransform.position.x - 1.0f, 0.0f);

        Vector2 RightPosition = new Vector2(m_pFollowTransform.position.x + 1.0f, 0.0f);

        Vector2 _Direction = Vector2.zero;

        switch (m_eAttackDirection)
        {
            case ATTACKDIRECTION.ATTACK_LEFT:
                m_pStoneTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                _Direction = LeftPosition - (Vector2)m_pStoneTransform.position;
                _Direction.y = 0.0f;
                m_pStoneTransform.position += (Vector3)_Direction.normalized * 1.0f * Time.deltaTime;
                break;
            case ATTACKDIRECTION.ATTACK_RIGHT:
                m_pStoneTransform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f);
                _Direction = RightPosition - (Vector2)m_pStoneTransform.position;
                _Direction.y = 0.0f;
                m_pStoneTransform.position += (Vector3)_Direction.normalized * 1.0f * Time.deltaTime;
                break;
        }
    }
}
