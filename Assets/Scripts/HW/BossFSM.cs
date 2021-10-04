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
    public float moveSpeed = 5.0f;
    public float attackRange = 7.0f;
    public BoxCollider basicAttackTrigger;
    public BossState bossState = BossState.Sleep;

    public int maxHP = 1000;
    public int currentHP = 0;
    public int attackDamage = 5;

    NavMeshAgent agent;
    Animator anim;
    Vector3 targetPos;
    RuntimeAnimatorController ac;
    float currentTime = 0;
    float delayTime = 0;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        ac = anim.runtimeAnimatorController;

        basicAttackTrigger.enabled = false;

        // ���� ü���� �ִ� ü������ �÷����´�.
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

        // ���� HP�� 0���ϰ� �Ǹ�
        if (currentHP <= 0 && bossState != BossState.Die)
        {
            bossState = BossState.Die;

            // ���� �ִϸ��̼� ����
            anim.SetTrigger("Die");
        }
    }


    void Sleep()
    {
        // �ǰ� �� Walk ���·� ��ȯ
        if (currentHP < maxHP)
        {
            // �ȱ� �ִϸ��̼� ����
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

            // �ȱ� �ִϸ��̼� ����
            anim.SetTrigger("Walk");
            bossState = BossState.Walk;
        }
    }

    void Walk()
    {
        // NavMeshAgent ����
        agent.enabled = true;
        agent.isStopped = false;
        agent.SetDestination(target.position);

        agent.speed = moveSpeed;
        targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);

        // �׻� Ÿ���� �ٶ󺸰� �����Ѵ�.
        transform.LookAt(targetPos);

        // ���� ��Ÿ� �ȿ� ������ �������� �����Ѵ�.
        if (Vector3.Distance(target.position, transform.position) < attackRange)
        {
            // �̵� ����
            agent.isStopped = true;
            agent.velocity = Vector3.zero;
            agent.stoppingDistance = attackRange;

            AttackPattern();
        }
    }

    void BasicAttack()
    {
        // ������ �� basicAttackTrigger Ű��
        basicAttackTrigger.enabled = true; 

        delayTime = AnimationTime(BossState.BasicAttack) + 1.5f;
        currentTime += Time.deltaTime;

        if (currentTime >= delayTime)
        {
            currentTime = 0;

            basicAttackTrigger.enabled = false;


            // ��� �ִϸ��̼� ����
            anim.SetTrigger("Idle");

            bossState = BossState.Idle;
        }
    }


    void FlameAttack()
    {
        // ��ƼŬ ���� �� �÷��̾ ��ƼŬ�� ������ ���������� (OnParticleCollision)
        // https://funfunhanblog.tistory.com/37

        delayTime = AnimationTime(BossState.FlameAttack);
        currentTime += Time.deltaTime;

        if (currentTime >= delayTime)
        {
            currentTime = 0;

            // ��� �ִϸ��̼� ����
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

                // �⺻ ���� �ִϸ��̼� ����
                anim.SetTrigger("BasicAttack");
                break;
            case 1:
                bossState = BossState.FlameAttack;

                // ���� �ִϸ��̼� ����
                anim.SetTrigger("FlameAttack");
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
