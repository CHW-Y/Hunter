using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Tooltip("보스 공격시 생기는 파티클 이펙트 1")]
    public GameObject hitParticle_01;

    [Tooltip("보스 공격시 생기는 파티클 이펙트 2")]
    public GameObject hitParticle_02;

    //  이펙트 타입 정의 열거형 변수
    public enum HitParticle
    {
        Hit_Type1,
        Hit_Type2
    }

    
    /// <summary>
    /// 인자값으로 받은 포지션 위치에 해당 타입의 공격 이펙트를 실행시키는 함수
    /// </summary>
    /// <param name="pos">이펙트를 생성할 위치</param>
    /// <param name="type">이펙트의 타입</param>
    public void ActiveHitParticle(Vector3 pos, HitParticle type)
    {
        switch (type)
        {
            case HitParticle.Hit_Type1:
                hitParticle_01.transform.position = pos;
                hitParticle_01.GetComponent<ParticleSystem>().Play();
                break;
            case HitParticle.Hit_Type2:
                hitParticle_02.transform.position = pos;
                hitParticle_02.GetComponent<ParticleSystem>().Play();
                break;
        }

    }
}
