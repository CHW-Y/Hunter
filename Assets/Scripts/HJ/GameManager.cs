using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(UIManager))]
//  ���� �Ŵ��� ��ũ��Ʈ
public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    
    [Tooltip("PlayerAnimation ��ũ��Ʈ")]
    public PlayerAnimation am;

    [Tooltip("CameraMove ��ũ��Ʈ")]
    public CameraMove cm;

    [Tooltip("UIManager ��ũ��Ʈ")]
    public UIManager um;

    [Tooltip("PlayerAttack ��ũ��Ʈ")]
    public PlayerAttack pa;

    private void Awake()
    {
        if(gm == null)
        {
            gm = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        if (am == null) am = GameObject.Find("Player").GetComponent<PlayerAnimation>();
        if (cm == null) cm = GameObject.Find("Main Camera").GetComponent<CameraMove>();
        if (um == null) um = GetComponent<UIManager>();
        if (pa == null) pa = GameObject.Find("Player").GetComponent<PlayerAttack>();
    }

}
