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

    private void OnTriggerEnter(Collider other)
    {        
        if (other.gameObject.layer == LayerMask.NameToLayer("Boss"))
        {
            if (!currentAttackEnemyList.Contains(other.transform.root.name))
            {
                currentAttackEnemyList.Add(other.transform.root.name);
                print("����!");
                GameObject go = Instantiate(textGo, other.gameObject.GetComponent<Collider>().ClosestPointOnBounds(transform.position), Quaternion.identity);
                go.name = "HitPoint";

                GameManager.gm.am.StopPlayerAni(0.1f);
                GameManager.gm.cm.CameraShake(0.15f + attackTypePower * 0.01f, attackTypePower * 0.1f);
                //�����Լ�
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
    /// �÷��̾��� ���ݷ°� ���� ������ ���ݷ� ����ġ�� �����ϴ� �Լ�
    /// </summary>
    /// <param name="patternPower">���� ���� ����ġ</param>
    /// <param name="playerPower">�÷��̾� ���ݷ�</param>
    public void SetAttackValue(float patternPower, float playerPower)
    {
        attackTypePower = patternPower;
        playerAttackPower = playerPower;        
    }

}
