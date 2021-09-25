using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  무기에 붙는 충돌 체크 스크립트
//  보스에 충돌하면 데미지를 입힘
public class WeaponColCheck : MonoBehaviour
{    
    //  플레이어의 한 공격 패턴중에 공격에 맞은 적들을 담는 리스트 변수;
    List<string> currentAttackEnemyList;

    //  플레이어의 해당 공격 패턴의 공격 가중치
    float attackTypePower;
    //  플레이어의 공격력
    float playerAttackPower;

    [Tooltip("무기의 콜라이더 컴포넌트")]
    public BoxCollider colBox;

    [Tooltip("테스트용 오브젝트")]
    public GameObject textGo;

    private void Start()
    {
        if (colBox == null) colBox = GetComponent<BoxCollider>();
        currentAttackEnemyList = new List<string>();        
    }

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            if (!currentAttackEnemyList.Contains(other.transform.root.name))
            {
                currentAttackEnemyList.Add(other.transform.root.name);
                print("어택!");
                GameObject go = Instantiate(textGo, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                go.name = "HitPoint";

                GameManager.gm.am.StopPlayerAni(0.1f);
                GameManager.gm.cm.CameraShake(0.15f + attackTypePower * 0.01f, attackTypePower * 0.1f);
                //공격함수
                //other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);                
            }
        }
    }

    /// <summary>
    /// 공격했던 몬스터의 리스트를 초기화 하는 함수
    /// </summary>
    public void ResetList()
    {
        currentAttackEnemyList = new List<string>();
    }

    /// <summary>
    /// 무기의 콜라이더 컴포넌트를 비활성화/활성화 하는 함수
    /// </summary>
    public void ColBoxChange()
    {
        if (colBox.enabled) colBox.enabled = false;
        else colBox.enabled = true;
    }

    /// <summary>
    /// 플레이어의 공격력과 공격 패턴의 공격력 가중치를 전달하는 함수
    /// </summary>
    /// <param name="patternPower">공격 패턴 가중치</param>
    /// <param name="playerPower">플레이어 공격력</param>
    public void SetAttackValue(float patternPower, float playerPower)
    {
        attackTypePower = patternPower;
        playerAttackPower = playerPower;        
    }

}
