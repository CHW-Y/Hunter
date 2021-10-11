using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

//  인트로 시네머신 제어 스크립트
public class IntroScene : MonoBehaviour
{
    [Tooltip("인트로 PlayableDirector 컴포넌트")]
    public PlayableDirector intro_pd;
    [Tooltip("게임클리어 PlayableDirector 컴포넌트")]
    public PlayableDirector gameClear_pd;
    [Tooltip("인트로 시네마신 카메라 오브젝트")]
    public GameObject introCam;
    [Tooltip("게임클리어 시네마신 카메라 오브젝트")]
    public GameObject gameClearCam;
    [Tooltip("메인 카메라 오브젝트")]
    public GameObject mainCam;
    [Tooltip("연출용 플레이어")]
    public GameObject fakePlayer;
    [Tooltip("실제 플레이어")]  
    public GameObject realPlayer;

    float time;
    
    void Start()
    {        
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        //  캐싱        
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
        //  시네마신이 끝까지 실행됐으면..

        mainCam.SetActive(true);
        fakePlayer.SetActive(false);
        realPlayer.SetActive(true);
        introCam.SetActive(false);
        intro_pd.enabled = false;

        GameManager.gm.um.SetActivePlayerUI(); //   UI 활성화
        yield return null;    
    }

    IEnumerator GameClearCinemachineOn()
    {
        gameClearCam.SetActive(true);
        yield return new WaitForSeconds((float)gameClear_pd.duration);
        gameClearCam.SetActive(false);
    }
}
