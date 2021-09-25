using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//  게임 매니저 스크립트
public class GameManager : MonoBehaviour
{
    public static GameManager gm;
    
    [Tooltip("PlayerAnimation 스크립트")]
    public PlayerAnimation am;

    [Tooltip("CameraMove 스크립트")]
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
