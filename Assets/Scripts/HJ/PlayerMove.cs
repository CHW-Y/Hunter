using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//  �÷��̾� �̵� ��ũ��Ʈ
[RequireComponent(typeof(CharacterController))]
public class PlayerMove : PlayerStats
{        
    CharacterController playerController;
    
    Vector3 move;
    float dodgeMoveSpeed;
    Quaternion dodgeRot;
    Vector3 dodgeVec;
    //  WASD Ű�� ������ �̵������� üũ�ϴ� ����
    bool moveCheck;
    //  �ӵ��� �ø��� ġƮŰ
    bool moveCheat = false;
    Quaternion rotate;
    float rotateSpeed = 5.0f;
    //  ȸ�� �ڷ�ƾ �Լ��� ���������� üũ�ϴ� ����
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
        //  WASD ����
        move = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));        

        //  ī�޶� �������� ȸ��
        move = Camera.main.transform.TransformDirection(move);
        move.Normalize();

        //  �÷��̾ �����̴� ���̶��..
        if(move!= Vector3.zero)
        {                          
            //  �̵� ������ �Է¹޾Ҵ����� üũ�ϴ� moveCheck Ȱ��ȭ
            moveCheck = true;
            moveStateCheck = true;
            if (moveCheck && dodgeReadyCheck && !attackStateCheck) rotate = Quaternion.LookRotation(new Vector3(move.x, 0, move.z));            

            //  ���� ShiftŰ �Է½� �޸��� �Լ� ����
            if (Input.GetKey(KeyCode.LeftShift) &&
                    !battleReadyCheck)
            {                
                MoveDash();
                //  �޸��� ���� �̵� ����� �ٶ󺸴� ������ ������ ���� �̻� ���̰� ���� ��� ȸ�� �� �̵��ϰ� �Ѵ�.
                if (Vector3.Angle(transform.forward, move) > 150)
                {
                    //  �ڷ�ƾ �����߿��� ���� �Ұ� üũ
                    if (!rotateCorCheck) StartCoroutine(RotateDash());
                }
            }
            else
            {
                dashStateCheck = false;
                totalMoveSpeed = moveSpeed; ;
                rotateSpeed = 5.0f;                
            }
            //  Space ��ư�� ������ ȸ��(������) ����
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Dodge();
            }

            //  ȸ�� ���� ���� äũ
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
        //  �̵� �Է��� ���� ������ ������ ���� äũ ���� ������ false
        else
        {
            moveCheck = false;
            moveStateCheck = false;
            dashStateCheck = false;
            rotate = transform.rotation;
        }

        //  �������� ���� �̵����� ���ϰ� ���� �ӵ��� 0���� �Ѵ�.
        if (attackStateCheck)
        {
            totalMoveSpeed = 0;
        }

        //  ġƮ Ȱ��ȭ
        if (Input.GetKeyDown(KeyCode.C))
        {
            moveCheat = !moveCheat;
        }
    }

    private void FixedUpdate()
    {
        //  �̵� �Է��� ���� ��쿡�� ������  
        if(moveCheck)
        {
            playerController.Move(transform.forward * totalMoveSpeed * Time.deltaTime);            
        }
        else if (moveCheat)
        {
            //  ġƮ
            playerController.Move(transform.forward * Time.deltaTime * 18.0f);
        }

        //  �÷��̾� ȸ�� �̵�
        playerController.Move(dodgeVec * dodgeMoveSpeed * Time.deltaTime);

        //  �÷��̾ �߷� ����
        playerController.Move(new Vector3(0, gravity, 0) * Time.deltaTime);

        //  �÷��̾ �����̷��� �������� �ε巴�� ȸ��
        transform.rotation = Quaternion.Slerp(transform.rotation, rotate, rotateSpeed * Time.deltaTime);
    }

    //  �뽬 �Լ�
    void MoveDash()
    {
        dashStateCheck = true;
        rotateSpeed = 10.0f;
        totalMoveSpeed = sprintMoveSpeed * moveSpeed;
    }

    //  �޸��� ���� �̵� ����� �ٶ󺸴� ������ ������ ���� �̻� ���̰� ���� ��� ȸ�� �� �̵��ϴ� �Լ�
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

    //  Space�� ������ �÷��̾ ȸ��(������)�� �����ϴ� �Լ�
    //  �÷��̾ ���� ���� ��, ���⸦ ����ְ� ���� ��, �ǰݹ��� ������ ��, �̹� ȸ�Ǹ� �������� �� ����Ұ�
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

    //  dodgeMoveSpeed �� �����ؼ� ������ ȿ���� ���� �Լ�
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

    //  dodgeState ���� äũ
    IEnumerator DodgeStateCheck()
    {
        dodgeStateCheck = true;
        yield return new WaitUntil(() => GameManager.gm.am.am.GetCurrentAnimatorStateInfo(2).IsName("Dodge")
                                    && GameManager.gm.am.am.GetCurrentAnimatorStateInfo(2).normalizedTime >= 1.0f);
        GameManager.gm.pa.weaponCol.ResetList();
        dodgeStateCheck = false;
    }

    /// <summary>
    /// �÷��̾ �ش� �ð��� ��ŭ ���� ���·� �����ϴ� �Լ�
    /// </summary>
    /// <param name="time">���� �ð�</param>
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

