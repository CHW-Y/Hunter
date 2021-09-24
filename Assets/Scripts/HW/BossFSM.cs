using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFSM : MonoBehaviour
{
    public enum BossState
    { 
        Sleep,
        Walk,
        BasicAttack,
        FlameAttack,
        Stun,
        Die
    }

    public Transform target;
    public float moveSpeed = 5.0f;
    public float attackRange = 1.0f;
    public BoxCollider basicAttackTrigger;

    public BossState bossState = BossState.Sleep;
    NavMeshAgent agent;
    Animator anim;
    Vector3 targetPos;
    bool isAttack = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        basicAttackTrigger.enabled = false;
    }

    void Update()
    {
        switch (bossState)
        {
            case BossState.Sleep:
                break;
            case BossState.Walk:
                Walk();
                break;
            case BossState.BasicAttack:
                BasicAttack();
                break;
            case BossState.FlameAttack:
                FlameAttack();
                break;
            case BossState.Stun:
                break;
            case BossState.Die:
                break;
            default:
                break;
        }
    }

    void Walk()
    {
        isAttack = false;

        // NavMeshAgent 설정
        agent.enabled = true;
        agent.SetDestination(target.position);
        agent.isStopped = false;

        agent.speed = moveSpeed;
        targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        // 항상 타켓을 바라보게 설정한다.
        transform.LookAt(targetPos);

        // 걷기 애니메이션 실행
        anim.SetTrigger("Walk");

        // 공격 사거리 안에 들어오면 랜덤으로 공격한다.
        if (Vector3.Distance(target.position, transform.position) > attackRange)
        {
            int random = Random.Range(0, 2);

            // 이동 정지
            agent.isStopped = true;

            // 0이면, BasicAttack
            if (random == 0)
            {
                bossState = BossState.BasicAttack;
            }
            // 1이면, FlameAttack
            else
            {
                bossState = BossState.FlameAttack;
            }
        }
    }

    void BasicAttack() 
    {
        // 공격할 때 basicAttackTrigger 키기
        basicAttackTrigger.enabled = true;

        // 기본 공격 애니메이션 실행
        anim.SetTrigger("BasicAttack");

        isAttack = true;

        // 공격 실행 후 이동 상태로 변경
        if (isAttack)
        {
            bossState = BossState.Walk;
        }
    }

    void FlameAttack()
    {
        // 파티클 생성 후 플레이어가 파티클에 맞으면 데미지입음 (OnParticleCollision)
        // https://funfunhanblog.tistory.com/37

        anim.SetTrigger("FlameAttack");

        isAttack = true;

        // 공격 실행 후 이동 상태로 변경
        if (isAttack)
        {
            bossState = BossState.Walk;
        }
    }
}
