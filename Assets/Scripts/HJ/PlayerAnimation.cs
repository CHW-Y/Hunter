using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Tooltip("플레이어의 애니메이터 컴포넌트를 담을 변수")]
    public Animator am;
    [Tooltip("플레이어의 PlayerMove 컴포넌트를 담을 변수")]
    public PlayerMove ps;

    float aniValue;

    void Start()
    {
        if (am == null) am = GetComponent<Animator>();
        if (ps == null) ps = GetComponent<PlayerMove>();
    }

    // Update is called once per frame
    void Update()
    {        
        if (ps.moveStateCheck)
        {
            PlayerMoveAni();
        }
        else
            aniValue = Mathf.Lerp(aniValue, 0f, Time.deltaTime * 3.0f);

        am.SetFloat("MoveAni", aniValue);

        //  플레이어 공격 애니메이션 경직 테스트용
<<<<<<< HEAD
        if (Input.GetKeyDown(KeyCode.L))
        {
            StopPlayerAni(0.1f);
        }
=======
        //if (Input.GetKeyDown(KeyCode.L))
        //{
        //    StopPlayerAni(0.1f);
        //}
>>>>>>> main
        
        if (am.GetCurrentAnimatorStateInfo(2).IsName("SwingReady"))
        {
            if(am.GetFloat("AniSpeed") != 1f)
            {
                am.SetFloat("AniSpeed", 1f);
            }
        }
    }

    void PlayerMoveAni()
    {
        if (ps.dashStateCheck)
            aniValue = Mathf.Lerp(aniValue, 1f, Time.deltaTime * 3.0f);
        else
            aniValue = Mathf.Lerp(aniValue, 0.5f, Time.deltaTime * 5.0f);
    }

    IEnumerator MyStopPlayerAni(float stopTime)
    {
        float t = 0;
        while (t < stopTime)
        {
            am.SetFloat("AniSpeed", 0f);
            t += Time.deltaTime;
            yield return null;
        }

        am.SetFloat("AniSpeed", 1.05f);
        yield return null;
    }

    /// <summary>
    /// 플레이어 애니메이션을 일정 시간동안 경직시키는 함수
    /// </summary>
    /// <param name="stopTime">경직 시간</param>
    /// <returns></returns>
    public void StopPlayerAni(float stopTime)
    {
        StartCoroutine(MyStopPlayerAni(stopTime));
    }
}
