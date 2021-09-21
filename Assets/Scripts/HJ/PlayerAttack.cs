using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void mydele();

[RequireComponent(typeof(PlayerMove))]
public class PlayerAttack : MonoBehaviour
{
    [Tooltip("�÷��̾ ����ϴ� ���� ������Ʈ")]
    public GameObject playerWeapon;
    [Tooltip("�÷��̾� �ִϸ�����")]
    public Animator am;
    [Tooltip("�÷��̾� ���� ��ũ��Ʈ")]
    public PlayerMove pm;

    bool weaponColCheck;
    //  true�� ��쿡�� ���� �Է��� �ް��ϴ� bool ����
    public bool attackInputChance;


    void Start()
    {
        if (am == null) am = GetComponent<Animator>();
        if (pm == null) pm = GetComponent<PlayerMove>();

        attackInputChance = true;

        StartCoroutine(CheckAttackInputChance());
    }

    void Update()
    {
        //  ���콺 ��Ŭ���� �ظ� ���� ���� 1 ����
        if (Input.GetMouseButtonDown(0))
        {
            HammerSwingPattern1();   
        }
    }

    void TurnOnAttack()
    {
        am.SetBool("AttackBool", false);
        attackInputChance = true;
    }

    void TurnOffAttack()
    {
        attackInputChance = false;
    }

    //  ��Ŭ���� ��� �� ��� �ظ� ���� ���� 1 ����
    void HammerSwingPattern1()
    {
        if (am.GetCurrentAnimatorStateInfo(1).IsName("AttackReady") && attackInputChance)
        {
                am.SetBool("AttackBool", true);
                attackInputChance = false;
        }
    }

    //  �ߵ� ������ ���� �����Է� ������ �׻� On���� �ϴ� �Լ�
    //  �������� ���� PlayerMove�� attackStateCheck������ true�� �Ͽ� �������� ���ϰ� ��.
    IEnumerator CheckAttackInputChance()
    {
        while (true)
        {
            if (am.GetCurrentAnimatorStateInfo(2).IsName("SwingReady"))
            {
                attackInputChance = true;
                pm.attackStateCheck = false;
            }
            else if (!am.GetCurrentAnimatorStateInfo(2).IsName("SwingReady"))
            {
                pm.attackStateCheck = true;
            }
            yield return null;
        }
    }
}
