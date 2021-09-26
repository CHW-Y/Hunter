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

    private void Start()
    {
        canvas = canvas == null ? GameObject.Find("Canvas") : canvas;   
    }

    /// <summary>
    /// 플레이어가 보스를 공격 성공시킨 위치에 데미지 텍스트를 띄우는 함수
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
}
