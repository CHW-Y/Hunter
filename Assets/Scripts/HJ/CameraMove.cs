using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{    
    [Tooltip("������ ����� Transform")]
    public Transform targetTransform;
    [Tooltip("ī�޶�� Ÿ�� ������ �Ÿ� ��")]
    public float distance = 3.0f;
    [Tooltip("ī�޶� ȸ�� �ӵ� ��")]
    public float cameraRotateSpeed = 1.0f;
    [Tooltip("�󸶳� ���� ī�޶� ����(����ũ)�� ��Ÿ���� �����ϴ� �ð� ��")]
    public float cameraShakeTime = 0.8f;
    
    float rotateX;
    float rotateY;

    Vector3 pos;
    void Start()
    {        
        targetTransform = targetTransform == null ? GameObject.Find("Player").transform : targetTransform;
        pos = targetTransform.position;
    }
    
    void Update()
    {
        if (Input.GetMouseButton(1))
        {
            rotateX += Input.GetAxisRaw("Mouse X") * cameraRotateSpeed;
            rotateY += Input.GetAxisRaw("Mouse Y") * cameraRotateSpeed;
        }

        distance += -Input.GetAxis("Mouse ScrollWheel");

        rotateY = Mathf.Clamp(rotateY, -60f, 60f);
        distance = Mathf.Clamp(distance, 1.5f, 4.5f);

    }
    
    void FixedUpdate()
    {
        //  ī�޶� �ε巴�� ���󰡴� �ڵ�        
        pos = Vector3.Lerp(pos, targetTransform.position, 10f * Time.fixedDeltaTime);
        //  ��Ȯ�ϰ� ���󰡴� �ڵ�
        //pos = targetTransform.position;
    }
    
    private void LateUpdate()
    {                
        transform.rotation = Quaternion.Euler(-rotateY, rotateX, 0);                
        transform.position = pos + transform.rotation * new Vector3(0, 1, -distance);
    }

    /// <summary>
    /// ���� ����� ���� ��󿡼� ���� ������ �޾ƿ� Ÿ�� Ʈ���������� �ٲٴ� �Լ�
    /// </summary>
    /// <param name="target"></param>
    public void ChangeTarget(Transform target)
    {
        targetTransform = target;
    }
}
