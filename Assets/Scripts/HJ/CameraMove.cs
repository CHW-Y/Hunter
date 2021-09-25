using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{    
    [Tooltip("추적할 대상의 Transform")]
    public Transform targetTransform;
    [Tooltip("카메라와 타겟 사이의 거리 값")]
    public float distance = 3.0f;
    [Tooltip("카메라 회전 속도 값")]
    public float cameraRotateSpeed = 1.0f;
    
    float rotateX;
    float rotateY;

    Vector3 pos;

    GameObject originPos;
    Vector3 targetToOrigin;
    Ray originRay;
    RaycastHit hitInfo;

    void Start()
    {        
        targetTransform = targetTransform == null ? GameObject.Find("Player").transform : targetTransform;
        pos = targetTransform.position;
        originPos = new GameObject();
        originPos.name = "OriginCameraPosObject";
        
    }
    
    void Update()
    {
        //  오른쪽 마우스 클릭 유지
        if (Input.GetMouseButton(1))
        {
            //  카메라 회전
            rotateX += Input.GetAxisRaw("Mouse X") * cameraRotateSpeed;
            rotateY += Input.GetAxisRaw("Mouse Y") * cameraRotateSpeed;
        }

        //  마우스 휠로 카메라와 타겟 거리 조절
        //distance += -Input.GetAxis("Mouse ScrollWheel");

        rotateY = Mathf.Clamp(rotateY, -60f, 60f);
        distance = Mathf.Clamp(distance, 1.5f, 4.5f);

        //  카메라 쉐이크 테스트용
        if (Input.GetKeyDown(KeyCode.P))
        {
            CameraShake(0.5f, 1.5f);
        }

        //  카메라와 타겟 사이 Ray
        originPos.transform.position = pos + transform.rotation * new Vector3(0, 0.8f, -distance);
        targetToOrigin = originPos.transform.position - targetTransform.position;
        originRay = new Ray(targetTransform.position, targetToOrigin);
        Debug.DrawRay(targetTransform.position, targetToOrigin, Color.red);
    }
    
    void FixedUpdate()
    {
        //  카메라가 부드럽게 따라가는 코드        
        pos = Vector3.Lerp(pos, targetTransform.position, 15f * Time.fixedDeltaTime);
        //  정확하게 따라가는 코드
        //pos = targetTransform.position;
    }
    
    private void LateUpdate()
    {                
        transform.rotation = Quaternion.Euler(-rotateY, rotateX, 0);                
        transform.position = pos + transform.rotation * new Vector3(0, 0.8f, -distance);

        //  카메라와 타겟 사이에 장애물이 있을 경우 카메라 위치를 변경
        if (Physics.Raycast(originRay, out hitInfo, targetToOrigin.magnitude, 1 << LayerMask.NameToLayer("Obstacle")))
        {
            transform.position = hitInfo.point;
        }
    }

    /// <summary>
    /// 추적 대상을 현재 대상에서 인자 값으로 받아온 타켓 트랜스폼으로 바꾸는 함수
    /// </summary>
    /// <param name="target">타겟 트랜스폼</param>
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
            p *= -1f;
            t += Time.deltaTime;
            yield return null;
        }        
    }

    /// <summary>
    /// 카메라 떨림(쉐이크) 함수
    /// </summary>
    /// <param name="shakeTime">얼마나 오래 카메라 떨림(쉐이크)이 나타날지 설정하는 시간 값</param>
    /// <param name="shakePower">카메라 떨림(쉐이크)의 강도 값 (최소값 0 최대값 1.5)</param>
    /// <returns></returns>
    public void CameraShake(float shakeTime, float shakePower)
    {
        StartCoroutine(MyCameraShake(shakeTime, shakePower));
    }
}
