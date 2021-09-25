using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  ���� �Ŵ��� ��ũ��Ʈ
public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    
    [Tooltip("PlayerAnimation ��ũ��Ʈ")]
    public PlayerAnimation am;

    [Tooltip("CameraMove ��ũ��Ʈ")]
    public CameraMove cm;

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
    }

}
