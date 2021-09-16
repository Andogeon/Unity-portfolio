using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ATTACKDIRECTION
{
    ATTACK_READY,
    ATTACK_LEFT,
    ATTACK_RIGHT
}

//애니메이터 주니어 스톤볼 공격 모션 클래스입니다.

public class JuniorStoneAttackState : StateMachineBehaviour
{
    private Transform m_pStoneTransform = null;

    private Transform m_pFollowTransform = null;

    private ATTACKDIRECTION m_eAttackDirection = ATTACKDIRECTION.ATTACK_READY;

    private JuniorStoneBall m_pStoneBall = null;

    private bool m_bIsloop = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pFollowTransform = GameObject.Find("Player").transform;

        m_pStoneTransform = animator.gameObject.transform;

        m_pStoneBall = animator.gameObject.GetComponent<JuniorStoneBall>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if(m_pStoneBall.AccessHp <= 0.0f) // 체력이 없다면 
        {
            animator.SetBool("RUN", false);

            animator.SetBool("ATTACK", false);

            animator.SetBool("HIT", false);

            animator.SetBool("DEAD", true);

            // 사망 애니메이션으로 변경 

            return;
        }

        Vector2 _Direction = m_pFollowTransform.position - m_pStoneTransform.position;

        // 플레이어와 몬스터의 길이를 구하기 위한 백터를 구한다.

        if (_Direction.magnitude <= 0.5f) // 플레이어와 접근 했다면 
            m_bIsloop = true; // 공격 루프를 킨다 
        else if (m_bIsloop == true && _Direction.magnitude >= 5.0f) // 플레이어와 거리가 너무 떨어지게 된다면 
            m_bIsloop = false; // 공격 루프를 끈다 

        Collider2D _Collision = Physics2D.OverlapBox(animator.gameObject.transform.position, m_pStoneBall.AccessBoxSize, 0.0f, m_pStoneBall.AccessLayer);

        if(null != _Collision)
        {
            animator.SetBool("ATTACK", false);

            animator.SetBool("RUN", false);

            return;
        }

        // 플레이어 주위의 좌우로 움직이게 구현햇습니다.

        if (m_bIsloop == true)
        {
            Vector2 _Position = m_pStoneTransform.position;

            _Position.y = 0.0f;

            // 플레이어 좌표 기준으로 좌, 우측의 좌표를 구함 

            Vector2 LeftPosition = new Vector2(m_pFollowTransform.position.x - 3.0f, 0.0f);

            Vector2 RightPosition = new Vector2(m_pFollowTransform.position.x + 3.0f, 0.0f);

            // 현재 플레이어와 해당 몬스터의 좌, 우 길이를 구한다.

            float LeftLength = (LeftPosition - _Position).magnitude;

            float RightLength = (RightPosition - _Position).magnitude;

            // 위에서 구한 좌표까지 가까워지고 있거나 이미 왼쪽 공격이라면 

            if (LeftLength >= 0.004f && m_eAttackDirection == ATTACKDIRECTION.ATTACK_READY || m_eAttackDirection == ATTACKDIRECTION.ATTACK_LEFT)
            {
                // 공격 준비중 일시 왼쪽 공격으로 변경
                m_eAttackDirection = ATTACKDIRECTION.ATTACK_LEFT;

                // 왼쪽 공격 도중 플레이어의 우쪽 좌표와 너무 멀리 떨어지게 된다면 우측으로 공격 변경
                if (RightLength >= 3.5f)
                    m_eAttackDirection = ATTACKDIRECTION.ATTACK_RIGHT;
            }

            // 위에서 구한 좌표까지 가까워지고 있거나 이미 우측 공격이라면 
            else if (RightLength >= 0.004f && m_eAttackDirection == ATTACKDIRECTION.ATTACK_READY || m_eAttackDirection == ATTACKDIRECTION.ATTACK_RIGHT)
            {
                // 공격 준비중 일시 왼쪽 공격으로 변경
                m_eAttackDirection = ATTACKDIRECTION.ATTACK_RIGHT;

                // 왼쪽 공격 도중 플레이어의 우쪽 좌표와 너무 멀리 떨어지게 된다면 좌측으로 공격 변경
                if (LeftLength >= 3.5f)
                    m_eAttackDirection = ATTACKDIRECTION.ATTACK_LEFT;
            }

            AttackDirection(); 
        }

        // 공격 루프가 꺼져 있다면 

        else
        {
            if(m_eAttackDirection != ATTACKDIRECTION.ATTACK_READY)
                m_eAttackDirection = ATTACKDIRECTION.ATTACK_READY; // 다시 준비 상태로 돌입

            _Direction.y = 0.0f; // 백터를 통하여 각도를 얻기 위해 Y축 좌표를 0으로 

            // 현재 좌측 방향 백터와 몬스터가 플레이어를 추척하기 위한 방향백터를 정규화하여 각도를 얻어낸다.

            float Angle = Vector2.Angle(_Direction.normalized, Vector2.left);

            // 얻은 방향 백터를 통하여 회전

            m_pStoneTransform.rotation = Quaternion.Euler(0.0f, Angle, 0.0f);

            // 몬스터가 플레이어를 따라가게 추적한다

            m_pStoneTransform.position += (Vector3)_Direction.normalized * 1.0f * Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        m_pStoneTransform = null;

        m_pFollowTransform = null;
    }


    // m_eAttackDirection에서 지정한 값에 따라 해당 오브젝트를 추척하는 함수입니다.
    private void AttackDirection()
    {
        Vector2 LeftPosition = new Vector2(m_pFollowTransform.position.x - 1.0f, 0.0f);

        Vector2 RightPosition = new Vector2(m_pFollowTransform.position.x + 1.0f, 0.0f);

        Vector2 _Direction = Vector2.zero;

        switch (m_eAttackDirection)
        {
            case ATTACKDIRECTION.ATTACK_LEFT: // 좌측일시 
                m_pStoneTransform.rotation = Quaternion.Euler(0.0f, 0.0f, 0.0f); // 0도로 회전하고 
                _Direction = LeftPosition - (Vector2)m_pStoneTransform.position;
                _Direction.y = 0.0f;
                m_pStoneTransform.position += (Vector3)_Direction.normalized * 1.0f * Time.deltaTime; // 추적한다.
                break;
            case ATTACKDIRECTION.ATTACK_RIGHT:
                m_pStoneTransform.rotation = Quaternion.Euler(0.0f, 180.0f, 0.0f); // 180도 회전하고 
                _Direction = RightPosition - (Vector2)m_pStoneTransform.position;
                _Direction.y = 0.0f;
                m_pStoneTransform.position += (Vector3)_Direction.normalized * 1.0f * Time.deltaTime; // 추적한다
                break;
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