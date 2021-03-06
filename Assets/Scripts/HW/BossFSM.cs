using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossFSM : MonoBehaviour
{
    public enum BossState
    { 
        Sleep,
        Idle,
        Walk,
        BasicAttack,
        FlameAttack,
        Stun,
        Die
    }

    public Transform target;
    public BoxCollider basicAttackTrigger;
    public GameObject fire;
    public Transform firePos;
    public float moveSpeed = 5.0f;
    public float attackRange = 7.0f;
    public BossState bossState = BossState.Sleep;
    

    public int maxHP = 1000;
    public int currentHP = 0;
    public int attackDamage = 5;
    public float rotSpeed = 1;

    NavMeshAgent agent;
    Animator anim;
    RuntimeAnimatorController ac;
    float currentTime = 0;
    float delayTime = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        ac = anim.runtimeAnimatorController;

        basicAttackTrigger.enabled = false;

        // 현재 체력을 최대 체력으로 올려놓는다.
        currentHP = maxHP;
    }

    void Update()
    {
        switch (bossState)
        {
            case BossState.Sleep:
                Sleep();
                break;
            case BossState.Idle:
                Idle();
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

        // 현재 HP가 0이하가 되면
        if (currentHP <= 0 && bossState != BossState.Die)
        {
            bossState = BossState.Die;

            // 죽음 애니메이션 실행
            anim.SetTrigger("Die");
        }
    }


    void Sleep()
    {
        // 피격 시 Walk 상태로 전환
        if (currentHP < maxHP)
        {
            // 걷기 애니메이션 실행
            anim.SetTrigger("Walk");
            bossState = BossState.Walk;
        }
    }

    void Idle()
    {
        delayTime = AnimationTime(BossState.Idle);
        currentTime += Time.deltaTime;

        if (currentTime >= delayTime)
        {
            currentTime = 0;

            // 걷기 애니메이션 실행
            anim.SetTrigger("Walk");
            bossState = BossState.Walk;
        }
    }

    void Walk()
    {
        // NavMeshAgent 설정
        agent.enabled = true;
        agent.isStopped = false;
        agent.SetDestination(target.position);

        agent.speed = moveSpeed;
        Vector3 dir = target.position - transform.position;

        // 항상 타켓을 바라보게 설정한다.
        //transform.LookAt(targetPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), rotSpeed * Time.deltaTime);


        // 공격 사거리 안에 들어오면 랜덤으로 공격한다.
        if (Vector3.Distance(target.position, transform.position) < attackRange)
        {
            // 이동 정지
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.stoppingDistance = attackRange;

            AttackPattern();
        }
    }

    void BasicAttack()
    {
        // 공격할 때 basicAttackTrigger 키기
        basicAttackTrigger.enabled = true; 

        delayTime = AnimationTime(BossState.BasicAttack) + 1.5f;
        currentTime += Time.deltaTime;

        if (currentTime >= delayTime)
        {
            currentTime = 0;

            basicAttackTrigger.enabled = false;


            // 대기 애니메이션 실행
            anim.SetTrigger("Idle");

            bossState = BossState.Idle;
        }
    }


    void FlameAttack()
    {
        // 파티클 생성 후 플레이어가 파티클에 맞으면 데미지입음 (OnParticleCollision)
        // https://funfunhanblog.tistory.com/37

        delayTime = AnimationTime(BossState.FlameAttack);
        currentTime += Time.deltaTime;


        if (currentTime >= delayTime)
        {
            currentTime = 0;

            // 대기 애니메이션 실행
            anim.SetTrigger("Idle");

            bossState = BossState.Idle;
        }
    }

    void AttackPattern()
    {
        int randomAction = Random.Range(0, 2);

        switch (randomAction)
        {
            case 0:
                bossState = BossState.BasicAttack;

                // 기본 공격 애니메이션 실행
                anim.SetTrigger("BasicAttack");
                break;
            case 1:
                bossState = BossState.FlameAttack;

                // 공격 애니메이션 실행
                anim.SetTrigger("FlameAttack");

                Instantiate(fire, firePos.position, Quaternion.Euler(new Vector3(90, 0, 0)));
                break;
            default:
                break;
        }
    }

    public float AnimationTime(BossState bossState)
    {
        string animName = string.Empty;

        switch (bossState)
        {
            case BossState.Sleep:
                animName = "Sleep";
                break;
            case BossState.Idle:
                animName = "Idle01";
                break;
            case BossState.Walk:
                animName = "Walk";
                break;
            case BossState.BasicAttack:
                animName = "Basic Attack";
                break;
            case BossState.FlameAttack:
                animName = "Flame Attack";
                break;
            case BossState.Stun:
                animName = "Stun";
                break;
            case BossState.Die:
                animName = "Die";
                break;
            default:
                break;
        }

        float delayTime = 0;

        for (int i = 0; i < ac.animationClips.Length; i++)
        {
            if (ac.animationClips[i].name == animName)
            {
                delayTime = ac.animationClips[i].length;
            }
        }

        return delayTime;
    }
}
