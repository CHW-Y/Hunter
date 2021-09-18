using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  �÷��̾� �̵� ��ũ��Ʈ
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(PlayerStats))]
public class PlayerMove : MonoBehaviour
{    
    PlayerStats playerStats;
    CharacterController playerController;
    
    Vector3 move;
    //  WASD Ű�� �������� üũ�ϴ� ����
    bool moveCheck;
    Quaternion rotate;
    float rotateSpeed = 5.0f;
    //  ȸ�� �ڷ�ƾ �Լ��� ���������� üũ�ϴ� ����
    bool rotateCorCheck;
    
    float gravity = -9.8f;

    float totalMoveSpeed;    

    void Start()
    {
        playerStats = GetComponent<PlayerStats>();
        playerController = GetComponent<CharacterController>();                 

        totalMoveSpeed = playerStats.moveSpeed;
    }

    void Update()
    {
        //  WASD ����
        if (!playerStats.attackStateCheck) move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        else move = Vector3.zero;
        //  ī�޶� �������� ȸ��
        move = Camera.main.transform.TransformDirection(move);
        move.Normalize();

        if(move!= Vector3.zero)
        {                          
            //  �̵� ������ �Է¹޾Ҵ����� üũ�ϴ� moveCheck Ȱ��ȭ
            moveCheck = true;
            playerStats.moveStateCheck = true;
            if (moveCheck) rotate = Quaternion.LookRotation(new Vector3(move.x, 0, move.z));

            //  ���� ShiftŰ �Է½� �޸��� �Լ� ����
            if (Input.GetKey(KeyCode.LeftShift))
            {                
                MoveDash();
            }
            else
            {
                playerStats.dashStateCheck = false;
                totalMoveSpeed = playerStats.moveSpeed;
                rotateSpeed = 5.0f;                
            }
        }
        else
        {
            moveCheck = false;
            playerStats.moveStateCheck = false;
            playerStats.dashStateCheck = false;
        }        

        //  �޸��� ���� �̵� ����� �ٶ󺸴� ������ ������ ���� �̻� ���̰� ���� ��� ȸ�� �� �̵��ϰ� �Ѵ�.
        if (Vector3.Angle(transform.forward, move) > 150 &&
                    Input.GetKey(KeyCode.LeftShift))
        {
            //  �ڷ�ƾ �����߿��� ���� �Ұ� üũ
            if(!rotateCorCheck) StartCoroutine(RotateDash());
        }                
    }

    private void FixedUpdate()
    {
        //  �̵� �Է��� ���� ��쿡�� ������  
        if(moveCheck)
        {
            playerController.Move(transform.forward * totalMoveSpeed * Time.deltaTime);            
        }

        //  �÷��̾ �߷� ����
        playerController.Move(new Vector3(0, gravity, 0) * Time.deltaTime);

        //  �÷��̾ �����̷��� �������� �ε巴�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);
    }

    void MoveDash()
    {
        playerStats.dashStateCheck = true;
        rotateSpeed = 10.0f;
        totalMoveSpeed = playerStats.sprintMoveSpeed * playerStats.moveSpeed;
    }
   
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
            playerStats.moveStateCheck = false;
            playerStats.dashStateCheck = false;
            t += Time.deltaTime;
            yield return null;
        }

        moveCheck = true;
        rotateCorCheck = false;        
    }
}

