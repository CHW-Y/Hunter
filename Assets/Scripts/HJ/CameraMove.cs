using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{    
    [Tooltip("������ �÷��̾��� Transform")]
    public Transform targetTransform;
    [Tooltip("������ ������ Transform")]
    public Transform bossTargetTransform;
    [Tooltip("ī�޶�� Ÿ�� ������ �Ÿ� ��")]
    public float distance = 3.0f;
    [Tooltip("ī�޶� ȸ�� �ӵ� ��")]
    public float cameraRotateSpeed = 1.0f;
    
    float rotateX;
    float rotateY;

    Vector3 pos;

    GameObject originPos;
    Vector3 targetToOrigin;
    Ray originRay;
    RaycastHit hitInfo;

    Coroutine lookOnCor;

    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        targetTransform = targetTransform == null ? GameObject.Find("Player").transform : targetTransform;
        pos = targetTransform.position;
        originPos = new GameObject();
        originPos.name = "OriginCameraPosObject";        
    }
    
    void Update()
    {
        //  ������ ���콺 Ŭ�� ����
        if (Input.GetMouseButton(1))
        {
            //  ī�޶� ȸ��
            rotateX += Input.GetAxisRaw("Mouse X") * cameraRotateSpeed;
            rotateY += Input.GetAxisRaw("Mouse Y") * cameraRotateSpeed;
        }

        //  ���콺 �ٷ� ī�޶�� Ÿ�� �Ÿ� ����
        distance += -Input.GetAxis("Mouse ScrollWheel");

        rotateY = Mathf.Clamp(rotateY, -45f, 45f);
        distance = Mathf.Clamp(distance, 1.5f, 4.5f);

        //  ī�޶� ����ũ �׽�Ʈ��
        if (Input.GetKeyDown(KeyCode.P))
        {
            CameraShake(0.5f, 1.5f);
        }

        //  �÷��̾��� ī�޶� �� ������ �� �� �����ϴ� �Լ�
        if (Input.GetKeyDown(KeyCode.E))
        {
            if(lookOnCor != null) StopCoroutine(lookOnCor);
            lookOnCor = StartCoroutine(LookOnCamera());            
        }

        //  ī�޶�� Ÿ�� ���� Ray
        originPos.transform.position = pos + transform.rotation * new Vector3(0, 0.8f, -distance);
        targetToOrigin = originPos.transform.position - targetTransform.position;
        originRay = new Ray(targetTransform.position, targetToOrigin);
        Debug.DrawRay(targetTransform.position, targetToOrigin, Color.red);
    }
    
    void FixedUpdate()
    {
        //  ī�޶� �ε巴�� ���󰡴� �ڵ�        
        pos = Vector3.Lerp(pos, targetTransform.position, 15f * Time.fixedDeltaTime);
        //  ��Ȯ�ϰ� ���󰡴� �ڵ�
        //pos = targetTransform.position;
    }
    
    private void LateUpdate()
    {                
        transform.rotation = Quaternion.Euler(-rotateY, rotateX, 0);                
        transform.position = pos + transform.rotation * new Vector3(0, 0.8f, -distance);

        //  ī�޶�� Ÿ�� ���̿� ��ֹ��� ���� ��� ī�޶� ��ġ�� ����
        if (Physics.Raycast(originRay, out hitInfo, targetToOrigin.magnitude, 1 << LayerMask.NameToLayer("Obstacle")))
        {
            transform.position = hitInfo.point;
        }
    }

    /// <summary>
    /// ���� ����� ���� ��󿡼� ���� ������ �޾ƿ� Ÿ�� Ʈ���������� �ٲٴ� �Լ�
    /// </summary>
    /// <param name="target">Ÿ�� Ʈ������</param>
    public void ChangeTarget(Transform target)
    {
        targetTransform = target;
    }

    IEnumerator MyCameraShake(float shakeTime, float shakePower)
    {        
        float t = 0;
        float p = -Mathf.Clamp(shakePower, 0f, 1.5f) / 10f;        
        while(t < shakeTime)
        {
            pos += new Vector3(0, p, 0);
            p *= -1f * 0.98f;
            t += Time.deltaTime;
            yield return null;
        }        
    }

    /// <summary>
    /// ī�޶� ����(����ũ) �Լ�
    /// </summary>
    /// <param name="shakeTime">�󸶳� ���� ī�޶� ����(����ũ)�� ��Ÿ���� �����ϴ� �ð� ��</param>
    /// <param name="shakePower">ī�޶� ����(����ũ)�� ���� �� (�ּҰ� 0 �ִ밪 1.5)</param>
    /// <returns></returns>
    public void CameraShake(float shakeTime, float shakePower)
    {
        StartCoroutine(MyCameraShake(shakeTime, shakePower));
    }

    /// <summary>
    /// �÷��̾��� ī�޶� �� ������ �� �� �����ϴ� �Լ�
    /// </summary>
    /// <returns></returns>
    IEnumerator LookOnCamera()
    {
        if (bossTargetTransform == null)
        {
            bossTargetTransform = GameObject.Find("Boss").transform;
        }
        
        float time = 0;
        Quaternion a = transform.rotation;                 

        while (time <= 1.5f)
        {
            originPos.transform.LookAt(bossTargetTransform);
            transform.rotation = Quaternion.Slerp(a, originPos.transform.rotation, time);            
            rotateX = transform.rotation.eulerAngles.y;

            time += Time.fixedDeltaTime * 5.0f;
            yield return null;
        }
    }
}
