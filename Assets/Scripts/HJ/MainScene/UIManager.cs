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

    [Tooltip("�÷��̾� hp/mp UI ������Ʈ")]
    public GameObject playerUI;    
    [Tooltip("�÷��̾� HP Image ������Ʈ")]public Image hpBar;
    [Tooltip("�÷��̾� MP Image ������Ʈ")]public Image mpBar;

    private void Start()
    {
        canvas = canvas == null ? GameObject.Find("Canvas") : canvas;   
    }

    /// <summary>
    /// �÷��̾ ������ ������ ��ġ�� ������ �ؽ�Ʈ�� ���� �Լ�
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

    /// <summary>
    /// �÷��̾� UI�� Ȱ��ȭ �ϴ� �Լ�
    /// </summary>
    public void SetActivePlayerUI()
    {
        playerUI.SetActive(true);
    }

    /// <summary>
    /// �÷��̾��� ü�°� ����� ���� ��ġ�� �����ϴ� �Լ�.
    /// </summary>
    /// <param name="hpValue">���� hp ���� (ex. ����hp/�ִ�hp)</param>
    /// <param name="mpValue">���� mp ����</param>
    public void SetPlayerHPMP(float hpValue, float mpValue)
    {
        hpBar.fillAmount = hpValue;
        mpBar.fillAmount = mpValue;
    }
}
