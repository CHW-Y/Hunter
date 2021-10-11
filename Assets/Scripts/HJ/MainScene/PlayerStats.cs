using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  �÷��̾� ü��, ���¹̳�, �ӵ�, ����... ���� ��ũ��Ʈ
public class PlayerStats : MonoBehaviour
{
    //  �÷��̾� ü�� ���� ����
    [Tooltip("�÷��̾� ü�� Max��ġ")]
    public float maxHP = 100;
    //  ���� ü��
    [ReadOnly][SerializeField]protected float hp;    

    //  �÷��̾� ���¹̳� ���� ����
    [Tooltip("�÷��̾� ���¹̳� Max��ġ")]
    public float maxMP = 100;
    //  ���� ���¹̳�
    protected float mp;

    //  �÷��̾� �̵��ӵ� ���� ����
    [Tooltip("�÷��̾� �⺻ �̵��ӵ� ��ġ")]
    public float moveSpeed = 2.0f;
    [Tooltip("�÷��̾� �뽬 �̵��ӵ� (�⺻ �̵��ӵ��� ���ؼ� �����)")]
    public float sprintMoveSpeed = 2.0f;

    //  �÷��̾� ���ݷ� ���� ����
    [Tooltip("�÷��̾� �⺻ ���ݷ� ��ġ")]
    public float attackPower = 5.0f;

    //  �÷��̾��� ���� �ൿ���¸� üũ�ϴ� ����
    [HideInInspector]
    //  ���� ���� ����
    public bool attackStateCheck;
    [HideInInspector]
    //  �̵� ����
    public bool moveStateCheck;
    [HideInInspector]
    //  �뽬 ����
    public bool dashStateCheck;
    [HideInInspector]
    //  ���� ����
    public bool absoluteStateCheck;
    [HideInInspector]
    //  ���� ��� ���� ����(�ߵ� ����)
    public bool battleReadyCheck;
    [HideInInspector]
    //  ȸ��(������)�� ������ ����
    public bool dodgeReadyCheck;
    [HideInInspector]
    //  ȸ�� ���� ����
    public bool dodgeStateCheck;

    private void Awake()
    {
        hp = maxHP;
        mp = maxMP;
    }    

    /// <summary>
    /// �÷��̾��� ���� HP���� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHP()
    {
        return hp;
    }

    /// <summary>
    /// �÷��̾��� ���� MP���� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    public float GetCurrentMP()
    {
        return mp;
    }

    /// <summary>
    /// value ���� �޾� ���� �÷��̾��� HP�� ���ϴ� �Լ� (�������� ���ظ� ���� �� �÷��̾� ü�¿� �������� �����Ҷ� ���)
    /// </summary>
    /// <param name="value"></param>
    public void AddHP(int value)
    {
        hp += value;
    }

    /// <summary>
    /// value ���� �޾� ���� �÷��̾��� MP�� ���ϴ� �Լ�
    /// </summary>
    /// <param name="value"></param>
    public void AddMP(int value)
    {
        mp += value;
    }

    /// <summary>
    /// �÷��̾��� HP�� MP�� Max ��ġ ������ �ʱ�ȭ�ϴ� �Լ�
    /// </summary>
    public void ResetHMP()
    {
        hp = maxHP;
        mp = maxMP;
    }
}