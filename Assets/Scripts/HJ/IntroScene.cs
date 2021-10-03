using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

//  인트로 시네머신 제어 스크립트
public class IntroScene : MonoBehaviour
{
    [Tooltip("PlayableDirector 컴포넌트")]
    public PlayableDirector pd;
    [Tooltip("인트로 시네마신 카메라 오브젝트")]
    public GameObject introCam;
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
        //  시네마신이 끝까지 실행됐으면..
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
