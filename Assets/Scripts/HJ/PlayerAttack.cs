using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public delegate void mydele();

[RequireComponent(typeof(PlayerMove))]
public class PlayerAttack : MonoBehaviour
{
    [Tooltip("플레이어가 사용하는 무기 오브젝트")]
    public GameObject playerWeapon;
    [Tooltip("플레이어 애니메이터")]
    public Animator am;
    [Tooltip("플레이어 무브 스크립트")]
    public PlayerMove pm;
    [Tooltip("플레이어 무기의 충돌 감지 스크립트")]
    public WeaponColCheck weaponCol;
    
    //  true일 경우에만 공격 입력을 받게하는 bool 변수    
    [HideInInspector]
    public bool attackInputChance;

    void Start()
    {
        if (am == null) am = GetComponent<Animator>();
        if (pm == null) pm = GetComponent<PlayerMove>();
        if (weaponCol == null) weaponCol = GetComponentInChildren<WeaponColCheck>();

        attackInputChance = true;        
        StartCoroutine(CheckAttackInputChance());
        StartCoroutine(CheckAttackReadyState());
    }

    void Update()
    {

        //  마우스 좌클릭시 해머 공격 패턴 1 시작
        if (Input.GetMouseButtonDown(0))
        {
            HammerSwingPattern1();            
        }

        //  무기 꺼내기/넣기 함수 실행
        if (Input.GetKeyDown(KeyCode.F))
        {
            StartCoroutine(SheatheWeapon());
        }

    }

    //  애니메이션 이벤트용 함수
    void TurnOnAttack()
    {
        am.SetBool("AttackBool", false);
        attackInputChance = true;
    }

    //  애니메이션 이벤트용 함수
    void TurnOffAttack()
    {
        attackInputChance = false;
    }

    //  좌클릭을 계속 할 경우 해머 공격 패턴 1 연계
    void HammerSwingPattern1()
    {
        if (am.GetCurrentAnimatorStateInfo(1).IsName("BattleReady") && attackInputChance)
        {           
            am.SetBool("AttackBool", true);
            attackInputChance = false;            
        }
    }

    //  공격 상태가 아니면 공격입력 구간을 항상 On으로 하는 함수
    //  공격중일 때는 PlayerMove의 attackStateCheck변수를 true로 하여 움직이지 못하게 함.
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

    //  무기를 꺼내든 상태인지 체크하는 함수
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

    //  무기 납도/발도 함수
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

    //  애니메이션 이벤트용 함수
    void ResetColList()
    {
        weaponCol.ResetList();
    }

    //  애니메이션 이벤트용 함수
    void WeaponColBool(float value)
    {
        weaponCol.SetAttackValue(value, pm.attackPower);
        weaponCol.ColBoxChange();
        
    }
}
