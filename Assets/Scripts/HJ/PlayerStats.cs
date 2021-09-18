using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  �÷��̾� ü��, ���¹̳�, �ӵ�... ���� ��ũ��Ʈ
public class PlayerStats : MonoBehaviour
{
    //  �÷��̾� ü�� ���� ����
    [Tooltip("�÷��̾� ü�� Max��ġ")]
    public int maxHP = 100;
    int hp;    

    //  �÷��̾� ���¹̳� ���� ����
    [Tooltip("�÷��̾� ���¹̳� Max��ġ")]
    public int maxMP = 100;
    int mp;

    //  �÷��̾� �̵��ӵ� ���� ����
    [Tooltip("�÷��̾� �⺻ �̵��ӵ� ��ġ")]
    public float moveSpeed = 2.0f;
    [Tooltip("�÷��̾� �뽬 �̵��ӵ� (�⺻ �̵��ӵ��� ���ؼ� �����)")]
    public float sprintMoveSpeed = 2.0f;

    //  �÷��̾��� ���� �ൿ���¸� üũ�ϴ� ����
    [HideInInspector]
    public bool attackStateCheck;
    [HideInInspector]
    public bool moveStateCheck;
    [HideInInspector]
    public bool dashStateCheck;

    private void Awake()
    {
        hp = maxHP;
        mp = maxMP;
    }

    /// <summary>
    /// �÷��̾��� ���� HP���� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    public int GetCurrentHP()
    {
        return hp;
    }

    /// <summary>
    /// �÷��̾��� ���� MP���� �������� �Լ�
    /// </summary>
    /// <returns></returns>
    public int GetCurrentMP()
    {
        return mp;
    }

    /// <summary>
    /// value ���� �޾� ���� �÷��̾��� HP�� ���ϴ� �Լ�
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
