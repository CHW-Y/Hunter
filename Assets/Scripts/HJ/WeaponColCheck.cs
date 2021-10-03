using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  ���⿡ �ٴ� �浹 üũ ��ũ��Ʈ
//  ������ �浹�ϸ� �������� ����
public class WeaponColCheck : MonoBehaviour
{    
    //  �÷��̾��� �� ���� �����߿� ���ݿ� ���� ������ ��� ����Ʈ ����;
    List<string> currentAttackEnemyList;

    //  �÷��̾��� �ش� ���� ������ ���� ����ġ
    float attackTypePower;
    //  �÷��̾��� ���ݷ�
    float playerAttackPower;

    [Tooltip("������ �ݶ��̴� ������Ʈ")]
    public BoxCollider colBox;

    [Tooltip("�׽�Ʈ�� ������Ʈ")]
    public GameObject textGo;

    private void Start()
    {
        if (colBox == null) colBox = GetComponent<BoxCollider>();
        currentAttackEnemyList = new List<string>();        
    }

    private void Update()
    {
        // ���� ���߿� ȸ���ϸ� ���� �ݶ��̴� ����
        if (GameManager.gm.pa.pm.dodgeStateCheck)
        {
            colBox.enabled = false;
        }        
    }

    private void OnTriggerEnter(Collider other)
    {        
        //  �浹 ����� Boos��...
        if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {            
            float resultAttackValue = attackTypePower * playerAttackPower;  //  ���� ������ ���1
            resultAttackValue = Mathf.Round(Random.Range(resultAttackValue - 5.0f, resultAttackValue + 5.0f) * 10f) * 0.1f; // ���� ������ ���2

            //  �� ���� ������ ���ݿ� ���� ���� ����Ʈ�� �ִ��� üũ
            if (!currentAttackEnemyList.Contains(other.transform.root.name) && resultAttackValue != 0)
            {                
                currentAttackEnemyList.Add(other.transform.root.name); //  �������� ���� ���Ͽ� ������ ����Ʈ �߰�                
                Vector3 contactPos = other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position); //  ������ ���� ��ġ
                print("����!");

                //GameObject go = Instantiate(textGo, contactPos, Quaternion.identity); //�׽�Ʈ��
                //go.name = "HitPoint";

                GameManager.gm.am.StopPlayerAni(0.125f); //  ���ݿ� ������ �ִϸ��̼� ���� ȿ��

                //  ������ ���� ������ �Ӹ��κ��̸�..
                if (other.tag.Equals("Head"))
                {
                    GameManager.gm.um.SpawnDamageText(contactPos , resultAttackValue *1.5f); // ������ UI ���
                    GameManager.gm.cm.CameraShake(0.2f + attackTypePower * 0.01f, attackTypePower * 0.22f); //  ī�޶� ����ũ �Լ� ����
                    GameManager.gm.particleM.ActiveHitParticle(contactPos, ParticleManager.HitParticle.Hit_Type1);  //  ��ƼŬ �Լ� ����
                    GameManager.gm.soundM.PlayEffectSound("hammer_hit4", 1f);   //  ���� ���� �Լ� ����1
                    GameManager.gm.soundM.PlayEffectSound("headstunattack1", 1f);   //  ���� ���� �Լ� ����2
                }
                else
                {
                    GameManager.gm.um.SpawnDamageText(contactPos, resultAttackValue);
                    GameManager.gm.cm.CameraShake(0.15f + attackTypePower * 0.01f, attackTypePower * 0.15f);
                    GameManager.gm.particleM.ActiveHitParticle(contactPos, ParticleManager.HitParticle.Hit_Type2);
                    GameManager.gm.soundM.PlayEffectSound("hammer_hit4", 1f);
                }                
                //�����Լ� �־�ߵ�

                //other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position);                
            }
        }
    }

    /// <summary>
    /// �����ߴ� ������ ����Ʈ�� �ʱ�ȭ �ϴ� �Լ�
    /// </summary>
    public void ResetList()
    {
        currentAttackEnemyList = new List<string>();
    }

    /// <summary>
    /// ������ �ݶ��̴� ������Ʈ�� ��Ȱ��ȭ/Ȱ��ȭ �ϴ� �Լ�
    /// </summary>
    public void ColBoxChange()
    {
        if (colBox.enabled) colBox.enabled = false;
        else colBox.enabled = true;
    }

    /// <summary>
    /// ������ �ݶ��̴� ������Ʈ�� ��Ȱ��ȭ/Ȱ��ȭ �ϴ� �Լ� (�Ű������� �ִ� ����)
    /// </summary>
    /// <param name="check">Ȱ��ȭ ���� ��</param>
    public void ColBoxChange(bool check)
    {
        colBox.enabled = check;
    }

    /// <summary>
    /// �÷��̾��� ���ݷ°� ���� ������ ���ݷ� ����ġ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="patternPower">���� ���� ����ġ</param>
    /// <param name="playerPower">�÷��̾� ���ݷ�</param>
    public void SetAttackValue(float patternPower, float playerPower)
    {
        attackTypePower = patternPower;
        playerAttackPower = playerPower;
        print("test �� SetAttackValue");
    }

}
