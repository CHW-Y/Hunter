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

    private void Update()
    {
        // 공격 도중에 회피하면 무기 콜라이더 해제
        if (GameManager.gm.pa.pm.dodgeStateCheck)
        {
            colBox.enabled = false;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {        
        //  충돌 대상이 Boos면...
        if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {            
            float resultAttackValue = attackTypePower * playerAttackPower;  //  최종 데미지 계산1
            resultAttackValue = Mathf.Round(Random.Range(resultAttackValue - 5.0f, resultAttackValue + 5.0f) * 10f) * 0.1f; // 최종 데미지 계산2

            //  한 공격 패턴중 공격에 맞은 상태 리스트에 있는지 체크
            if (!currentAttackEnemyList.Contains(other.transform.root.name) && resultAttackValue != 0)
            {                
                currentAttackEnemyList.Add(other.transform.root.name); //  실행중인 공격 패턴에 맞으면 리스트 추가                
                Vector3 contactPos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position); //  공격이 맞은 위치
                print("어택!");

                //GameObject go = Instantiate(textGo, contactPos, Quaternion.identity); //테스트용
                //go.name = "HitPoint";

                GameManager.gm.am.StopPlayerAni(0.125f); //  공격에 성공시 애니메이션 경직 효과

                //  공격이 맞은 부위가 머리부분이면..
                if (other.tag.Equals("Head"))
                {
                    GameManager.gm.um.SpawnDamageText(contactPos , resultAttackValue *1.5f); // 데미지 UI 출력
                    GameManager.gm.cm.CameraShake(0.2f + attackTypePower * 0.01f, attackTypePower * 0.22f); //  카메라 쉐이크 함수 실행
                    GameManager.gm.particleM.ActiveHitParticle(contactPos, ParticleManager.HitParticle.Hit_Type1);  //  파티클 함수 실행
                    GameManager.gm.soundM.PlayEffectSound("hammer_hit4", 1f);   //  공격 사운드 함수 실행1
                    GameManager.gm.soundM.PlayEffectSound("headstunattack1", 1f);   //  공격 사운드 함수 실행2
                }
                else
                {
                    GameManager.gm.um.SpawnDamageText(contactPos, resultAttackValue);
                    GameManager.gm.cm.CameraShake(0.15f + attackTypePower * 0.01f, attackTypePower * 0.15f);
                    GameManager.gm.particleM.ActiveHitParticle(contactPos, ParticleManager.HitParticle.Hit_Type2);
                    GameManager.gm.soundM.PlayEffectSound("hammer_hit4", 1f);
                }                
                //공격함수 넣어야됨

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
    /// 무기의 콜라이더 컴포넌트를 비활성화/활성화 하는 함수 (매개변수가 있는 형태)
    /// </summary>
    /// <param name="check">활성화 유무 값</param>
    public void ColBoxChange(bool check)
    {
        colBox.enabled = check;
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
        print("test 용 SetAttackValue");
    }

}
