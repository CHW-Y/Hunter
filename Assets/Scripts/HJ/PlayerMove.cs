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
    Quaternion dodgeRot;
    Vector3 dodgeVec;
    //  WASD 키를 눌러서 이동중인지 체크하는 변수
    bool moveCheck;
    //  속도를 올리는 치트키
    bool moveCheat = false;
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
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));        

        //  카메라를 기준으로 회전
        move = Camera.main.transform.TransformDirection(move);
        move.Normalize();

        //  플레이어가 움직이는 중이라면..
        if(move!= Vector3.zero)
        {                          
            //  이동 조작을 입력받았는지를 체크하는 moveCheck 활성화
            moveCheck = true;
            moveStateCheck = true;
            if (moveCheck && dodgeReadyCheck && !attackStateCheck) rotate = Quaternion.LookRotation(new Vector3(move.x, 0, move.z));            

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
        }
        //  이동 입력을 받지 않으면 움직임 상태 채크 관련 변수들 false
        else
        {
            moveCheck = false;
            moveStateCheck = false;
            dashStateCheck = false;
            rotate = transform.rotation;
        }

        //  공격중일 때는 이동하지 못하게 최종 속도를 0으로 한다.
        if (attackStateCheck)
        {
            totalMoveSpeed = 0;
        }

        //  치트 활성화
        if (Input.GetKeyDown(KeyCode.C))
        {
            moveCheat = !moveCheat;
        }
    }

    private void FixedUpdate()
    {
        //  이동 입력을 받을 경우에만 움직임  
        if(moveCheck)
        {
            playerController.Move(transform.forward * totalMoveSpeed * Time.deltaTime);            
        }
        else if (moveCheat)
        {
            //  치트
            playerController.Move(transform.forward * Time.deltaTime * 18.0f);
        }

        //  플레이어 회피 이동
        playerController.Move(dodgeVec * dodgeMoveSpeed * Time.deltaTime);

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
        if (dodgeReadyCheck)
        {
            StartCoroutine(DodgeStateCheck());
            dodgeVec = move;
            dodgeRot = Quaternion.LookRotation(new Vector3(move.x, 0, move.z));
            rotate = dodgeRot;
            GameManager.gm.am.am.SetTrigger("DodgeTrig");
            GameManager.gm.am.am.SetBool("AttackBool", false);
            GameManager.gm.pa.weaponCol.ColBoxChange(false);
            StartCoroutine(SetAbsoluteStateTime(0.266f));
            StartCoroutine(DodgeMove());
            dodgeReadyCheck = false;
        }
    }

    //  dodgeMoveSpeed 를 조정해서 구르기 효과를 내는 함수
    IEnumerator DodgeMove()
    {
        float time = 0;
        dodgeMoveSpeed = 6.5f;
        transform.rotation = dodgeRot;
        yield return null;
        while(time < 1f)
        {
            dodgeMoveSpeed -= Time.deltaTime * 5.0f;
            time += Time.deltaTime;
            yield return null;
        }
        dodgeMoveSpeed = 0f;
    }

    //  dodgeState 상태 채크
    IEnumerator DodgeStateCheck()
    {
        dodgeStateCheck = true;
        yield return new WaitUntil(() => GameManager.gm.am.am.GetCurrentAnimatorStateInfo(2).IsName("Dodge")
                                    && GameManager.gm.am.am.GetCurrentAnimatorStateInfo(2).normalizedTime >= 1.0f);
        GameManager.gm.pa.weaponCol.ResetList();
        dodgeStateCheck = false;
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

