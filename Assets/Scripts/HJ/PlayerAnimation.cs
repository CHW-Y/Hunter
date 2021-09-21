using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [Tooltip("�÷��̾��� �ִϸ����� ������Ʈ�� ���� ����")]
    public Animator am;
    [Tooltip("�÷��̾��� PlayerMove ������Ʈ�� ���� ����")]
    public PlayerMove ps;

    float aniValue;

    void Start()
    {
        if (am == null) am = GetComponentInChildren<Animator>();
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
            aniValue = Mathf.Lerp(aniValue, 0f, Time.deltaTime * 7.0f);

        am.SetFloat("MoveAni", aniValue);
    }

    void PlayerMoveAni()
    {
        if (ps.dashStateCheck)
            aniValue = Mathf.Lerp(aniValue, 1f, Time.deltaTime * 2.0f);
        else
            aniValue = Mathf.Lerp(aniValue, 0.5f, Time.deltaTime * 7.0f);
    }
}
