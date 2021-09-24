using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
    [HideInInspector]
    public bool attackInputChance;


    void Start()
    {
        if (am == null) am = GetComponent<Animator>();
        if (pm == null) pm = GetComponent<PlayerMove>();

        attackInputChance = true;

        StartCoroutine(CheckAttackInputChance());
        StartCoroutine(CheckAttackReadyState());
    }

    void Update()
    {

        //  ���콺 ��Ŭ���� �ظ� ���� ���� 1 ����
        if (Input.GetMouseButtonDown(0))
        {
            HammerSwingPattern1();            
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(SheatheWeapon());
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
        if (am.GetCurrentAnimatorStateInfo(1).IsName("BattleReady") && attackInputChance)
        {           
            am.SetBool("AttackBool", true);
            attackInputChance = false;            
        }
    }

    //  ���� ���°� �ƴϸ� �����Է� ������ �׻� On���� �ϴ� �Լ�
    //  �������� ���� PlayerMove�� attackStateCheck������ true�� �Ͽ� �������� ���ϰ� ��.
    IEnumerator CheckAttackInputChance()
    {
        while (true)
        {
            if (am.GetCurrentAnimatorStateInfo(2).IsName("SwingReady"))
            {
                pm.attackStateCheck = false;
                attackInputChance = true;
            }
            else if (!am.GetCurrentAnimatorStateInfo(2).IsName("SwingReady"))
            {
                pm.attackStateCheck = true;
            }
            yield return null;
        }
    }

    //  ���⸦ ������ �������� üũ�ϴ� �Լ�
    IEnumerator CheckAttackReadyState()
    {
        while (true)
        {
            if (am.GetCurrentAnimatorStateInfo(1).IsName("BattleReady"))
            {
                pm.battleReadyCheck = true;
            }
            else if (!am.GetCurrentAnimatorStateInfo(1).IsName("BattleReady"))
            {
                pm.battleReadyCheck = false;
            }
            yield return null;
        }
    }

    //  ���� ����/�ߵ� �Լ�
    IEnumerator SheatheWeapon()
    {
        if (pm.battleReadyCheck && am.GetCurrentAnimatorStateInfo(2).IsName("SwingReady"))
        {
            am.SetBool("BattleReadyBool", false);
            yield return new WaitForSeconds(0.65f);
            playerWeapon.SetActive(false);
        }
        else if(!pm.battleReadyCheck && am.GetCurrentAnimatorStateInfo(1).IsName("IdleState"))
        {
            am.SetBool("BattleReadyBool", true);
            yield return new WaitForSeconds(0.2f);
            playerWeapon.SetActive(true);
        }
        
    }
}