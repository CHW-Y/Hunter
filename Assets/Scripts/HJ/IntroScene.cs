using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

//  ��Ʈ�� �ó׸ӽ� ���� ��ũ��Ʈ
public class IntroScene : MonoBehaviour
{
    [Tooltip("PlayableDirector ������Ʈ")]
    public PlayableDirector pd;
    [Tooltip("��Ʈ�� �ó׸��� ī�޶� ������Ʈ")]
    public GameObject introCam;
    [Tooltip("���� ī�޶� ������Ʈ")]
    public GameObject mainCam;
    [Tooltip("����� �÷��̾�")]
    public GameObject fakePlayer;
    [Tooltip("���� �÷��̾�")]  
    public GameObject realPlayer;

    float time;
    
    void Start()
    {

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //  ĳ��
        if (pd == null) pd = GetComponent<PlayableDirector>();
        if (introCam == null) introCam = GameObject.Find("IntroCamera");
        if (mainCam == null) mainCam = Camera.main.gameObject;
        if (fakePlayer == null) fakePlayer = GameObject.Find("FakePlayer");
        if (realPlayer == null) realPlayer = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;
        //  �ó׸����� ������ ���������..
        if(time >= pd.duration)
        {
            mainCam.SetActive(true);
            fakePlayer.SetActive(false);
            realPlayer.SetActive(true);
            introCam.SetActive(false);
            pd.enabled = false;            
        }
    }
}
