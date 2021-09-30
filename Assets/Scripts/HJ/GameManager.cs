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

    [Tooltip("ParticleManager ��ũ��Ʈ")]
    public ParticleManager particleM;

    [Tooltip("SoundManager ��ũ��Ʈ")]
    public SoundManager soundM;

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
        if (particleM == null) particleM = GetComponent<ParticleManager>();
        if (soundM == null) soundM = GetComponent<SoundManager>();
    }

}
