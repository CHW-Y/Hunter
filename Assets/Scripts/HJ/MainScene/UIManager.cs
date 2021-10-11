using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(GameManager))]
//  UIManager 스크립트
public class UIManager : MonoBehaviour
{
    [Tooltip("데미지 텍스트 프리팹")]
    public Text damageText;

    [Tooltip("캔버스 오브젝트")]
    public GameObject canvas;

    [Tooltip("플레이어 hp/mp UI 오브젝트")]
    public GameObject playerUI;    
    [Tooltip("플레이어 HP Image 컴포넌트")]public Image hpBar;
    [Tooltip("플레이어 MP Image 컴포넌트")]public Image mpBar;

    private void Start()
    {
        canvas = canvas == null ? GameObject.Find("Canvas") : canvas;   
    }

    /// <summary>
    /// 플레이어가 보스를 공격한 위치에 데미지 텍스트를 띄우는 함수
    /// </summary>
    /// <param name="spawnPos">플레이어의 공격이 맞은 포지션</param>
    /// <param name="damageValue">총 데미지 수치</param>
    public void SpawnDamageText(Vector3 spawnPos, float damageValue)
    {
        Text go = Instantiate(damageText, canvas.transform);
        DamageText dT = go.GetComponent<DamageText>();
        dT.temp = spawnPos;
        dT.attackValue = Mathf.Round(damageValue);        
    }

    /// <summary>
    /// 플레이어 UI를 활성화 하는 함수
    /// </summary>
    public void SetActivePlayerUI()
    {
        playerUI.SetActive(true);
    }

    /// <summary>
    /// 플레이어의 체력과 기력을 현재 수치로 조정하는 함수.
    /// </summary>
    /// <param name="hpValue">현재 hp 비율 (ex. 현재hp/최대hp)</param>
    /// <param name="mpValue">현재 mp 비율</param>
    public void SetPlayerHPMP(float hpValue, float mpValue)
    {
        hpBar.fillAmount = hpValue;
        mpBar.fillAmount = mpValue;
    }
}
