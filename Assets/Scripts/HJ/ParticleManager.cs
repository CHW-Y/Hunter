using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    [Tooltip("���� ���ݽ� ����� ��ƼŬ ����Ʈ 1")]
    public GameObject hitParticle_01;

    [Tooltip("���� ���ݽ� ����� ��ƼŬ ����Ʈ 2")]
    public GameObject hitParticle_02;

    //  ����Ʈ Ÿ�� ���� ������ ����
    public enum HitParticle
    {
        Hit_Type1,
        Hit_Type2
    }

    
    /// <summary>
    /// ���ڰ����� ���� ������ ��ġ�� �ش� Ÿ���� ���� ����Ʈ�� �����Ű�� �Լ�
    /// </summary>
    /// <param name="pos">����Ʈ�� ������ ��ġ</param>
    /// <param name="type">����Ʈ�� Ÿ��</param>
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
