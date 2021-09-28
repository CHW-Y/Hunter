using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  플레이어 이동 스크립트
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : PlayerStats
{        
    CharacterController playerController;
    
    Vector3 move;
    float dodgeMoveSpeed;
    //  WASD 키를 눌러서 이동중인지 체크하는 변수
    bool moveCheck;
    Quaternion rotate;
    float rotateSpeed = 5.0f;
    //  회전 코루틴 함수가 실행중인지 체크하는 변수
    bool rotateCorCheck;
    
    float gravity = -9.8f;

    float totalMoveSpeed;       

    void Start()
    {        
        playerController = GetComponent<CharacterController>();

        totalMoveSpeed = moveSpeed;                
    }

    void Update()
    {        
        //  WASD 조작
        if (!attackStateCheck) move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        else move = Vector3.zero;

        //  카메라를 기준으로 회전
        move = Camera.main.transform.TransformDirection(move);
        move.Normalize();

        //  플레이어가 움직이는 중이라면..
        if(move!= Vector3.zero)
        {                          
            //  이동 조작을 입력받았는지를 체크하는 moveCheck 활성화
            moveCheck = true;
            moveStateCheck = true;
            if (moveCheck) rotate = Quaternion.LookRotation(new Vector3(move.x, 0, move.z));            

            //  왼쪽 Shift키 입력시 달리기 함수 실행
            if (Input.GetKey(KeyCode.LeftShift) &&
                    !battleReadyCheck)
            {                
                MoveDash();
                //  달리기 도중 이동 방향과 바라보는 방향의 각도가 일정 이상 차이가 있을 경우 회전 후 이동하게 한다.
                if (Vector3.Angle(transform.forward, move) > 150)
                {
                    //  코루틴 실행중에는 실행 불가 체크
                    if (!rotateCorCheck) StartCoroutine(RotateDash());
                }
            }
            else
            {
                dashStateCheck = false;
                totalMoveSpeed = moveSpeed; ;
                rotateSpeed = 5.0f;                
            }
            //  Space 버튼을 누르면 회피(구르기) 실행
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dodge();
            }
        }
        else
        {
            moveCheck = false;
            moveStateCheck = false;
            dashStateCheck = false;
            rotate = transform.rotation;
        }
    }

    private void FixedUpdate()
    {
        //  이동 입력을 받을 경우에만 움직임  
        if(moveCheck)
        {
            playerController.Move(transform.forward * totalMoveSpeed * Time.deltaTime);            
        }

        //  플레이어 대쉬 이동
        playerController.Move(transform.forward * dodgeMoveSpeed * Time.deltaTime);

        //  플레이어에 중력 적용
        playerController.Move(new Vector3(0, gravity, 0) * Time.deltaTime);

        //  플레이어가 움직이려는 방향으로 부드럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);
    }

    //  대쉬 함수
    void MoveDash()
    {
        dashStateCheck = true;
        rotateSpeed = 10.0f;
        totalMoveSpeed = sprintMoveSpeed * moveSpeed;
    }

    //  달리기 도중 이동 방향과 바라보는 방향의 각도가 일정 이상 차이가 있을 경우 회전 후 이동하는 함수
    IEnumerator RotateDash()
    {
        float t = 0;
        moveCheck = false;
        rotateCorCheck = true;
        yield return null;

        while (t < 0.25f)
        {
            rotateSpeed = 12f;
            moveCheck = false;
            moveStateCheck = false;
            dashStateCheck = false;
            t += Time.deltaTime;
            yield return null;
        }

        moveCheck = true;
        rotateCorCheck = false;        
    }

    //  Space를 누르면 플레이어가 회피(구르기)를 실행하는 함수
    //  플레이어가 공격 중일 때, 무기를 집어넣고 있을 때, 피격받은 상태일 때, 이미 회피를 실행중일 때 실행불가
    void Dodge()
    {
        //  회피 가능 상태 채크
        if (GameManager.gm.am.am.GetCurrentAnimatorStateInfo(2).IsName("Dodge")
            || GameManager.gm.am.am.GetCurrentAnimatorStateInfo(2).IsName("Damaged"))
        {
            dodgeReadyCheck = false;
        }
        else
        {
            if (!absoluteStateCheck)
            {
                dodgeReadyCheck = true;
            }
        }
        if (dodgeReadyCheck)
        {
            transform.rotation = rotate;
            GameManager.gm.am.am.SetTrigger("DodgeTrig");
            StartCoroutine(SetAbsoluteStateTime(0.266f));
            StartCoroutine(DodgeMove());
            dodgeReadyCheck = false;
        }
    }

    /// <summary>
    /// 회피(구르기)가 가능한 상태를 체크하는 bool 변수를 true/false 하는 함수
    /// </summary>
    public void DodgeStateOnOff(bool check)
    {
        dodgeReadyCheck = check;
    }

    IEnumerator DodgeMove()
    {
        float time = 0;
        dodgeMoveSpeed = 7f;
        while(time < 1f)
        {
            dodgeMoveSpeed -= Time.deltaTime * 5.0f;
            time += Time.deltaTime;
            yield return null;
        }
        dodgeMoveSpeed = 0f;
    }

    /// <summary>
    /// 플레이어를 해당 시간값 만큼 무적 상태로 설정하는 함수
    /// </summary>
    /// <param name="time">무적 시간</param>
    public IEnumerator SetAbsoluteStateTime(float abTime)
    {
        float time = 0;
        absoluteStateCheck = true;

        while(time <= abTime)
        {            
            time += Time.deltaTime;
            yield return null;
        }

        absoluteStateCheck = false;
    }
}

