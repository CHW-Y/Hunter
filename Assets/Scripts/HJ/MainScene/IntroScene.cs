using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

//  ��Ʈ�� �ó׸ӽ� ���� ��ũ��Ʈ
public class IntroScene : MonoBehaviour
{
    [Tooltip("��Ʈ�� PlayableDirector ������Ʈ")]
    public PlayableDirector intro_pd;
    [Tooltip("����Ŭ���� PlayableDirector ������Ʈ")]
    public PlayableDirector gameClear_pd;
    [Tooltip("��Ʈ�� �ó׸��� ī�޶� ������Ʈ")]
    public GameObject introCam;
    [Tooltip("����Ŭ���� �ó׸��� ī�޶� ������Ʈ")]
    public GameObject gameClearCam;
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
        if (introCam == null) introCam = GameObject.Find("IntroCamera");
        if (gameClearCam == null) gameClearCam = GameObject.Find("ClearCamera");
        if (intro_pd == null) intro_pd = introCam.GetComponent<PlayableDirector>();
        if (gameClear_pd == null) gameClear_pd = gameClearCam.GetComponent<PlayableDirector>();
        if (mainCam == null) mainCam = Camera.main.gameObject;
        if (fakePlayer == null) fakePlayer = GameObject.Find("FakePlayer");
        if (realPlayer == null) realPlayer = GameObject.Find("Player");

        StartCoroutine(IntroCinemachineOff());
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.U))
        {
          StartCoroutine(GameClearCinemachineOn());
        }
    }

    IEnumerator IntroCinemachineOff()
    {
        while (time < intro_pd.duration)
        {
            time += Time.deltaTime;
            yield return null;
        }
        //  �ó׸����� ������ ���������..

        mainCam.SetActive(true);
        fakePlayer.SetActive(false);
        realPlayer.SetActive(true);
        introCam.SetActive(false);
        intro_pd.enabled = false;

        GameManager.gm.um.SetActivePlayerUI(); //   UI Ȱ��ȭ
        yield return null;    
    }

    IEnumerator GameClearCinemachineOn()
    {
        gameClearCam.SetActive(true);
        yield return new WaitForSeconds((float)gameClear_pd.duration);
        gameClearCam.SetActive(false);
    }
}
