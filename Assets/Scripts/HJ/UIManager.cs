using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameManager))]
//  UIManager ��ũ��Ʈ
public class UIManager : MonoBehaviour
{
    [Tooltip("������ �ؽ�Ʈ ������")]
    public Text damageText;

    [Tooltip("ĵ���� ������Ʈ")]
    public GameObject canvas;

    private void Start()
    {
        canvas = canvas == null ? GameObject.Find("Canvas") : canvas;   
    }

    /// <summary>
    /// �÷��̾ ������ ���� ������Ų ��ġ�� ������ �ؽ�Ʈ�� ���� �Լ�
    /// </summary>
    /// <param name="spawnPos">�÷��̾��� ������ ���� ������</param>
    /// <param name="damageValue">�� ������ ��ġ</param>
    public void SpawnDamageText(Vector3 spawnPos, float damageValue)
    {
        Text go = Instantiate(damageText, canvas.transform);
        DamageText dT = go.GetComponent<DamageText>();
        dT.temp = spawnPos;
        dT.attackValue = Mathf.Round(damageValue);
    }
}
