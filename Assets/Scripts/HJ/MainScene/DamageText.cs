using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  데미지 텍스트 프리팹 스크립트
public class DamageText : MonoBehaviour
{
    [HideInInspector]
    public Vector3 temp;
    [HideInInspector]
    public float attackValue;

    Text myText;    

    private void Start()
    {
        myText = GetComponent<Text>();
        myText.text = attackValue.ToString();

        StartCoroutine(TextAlphaLerp());
    }

    void Update()
    {
        //  해당 포지션에 계속 고정시킴
        transform.position = Camera.main.WorldToScreenPoint(temp);        

        //  알파 값이 0 이하로 떨어지면 오브젝트 삭제
        if (myText.color.a <= 0)
        {
            Destroy(gameObject);
        }

        //  화면 밖으로 넘어가면 비활성화
        if (myText.transform.position.z < 0)
        {
            myText.enabled = false;
        }
        else
        {
            myText.enabled = true;
        }
    }

    //  데미지 텍스트의 알파 값을 Lerp하게 내리는 함수
    IEnumerator TextAlphaLerp()
    {        
        float time = 0;
        yield return new WaitForSeconds(1.0f);
        while (time < 1.0f)
        {
            myText.color = new Color(myText.color.r, myText.color.g, myText.color.b, Mathf.Lerp(1f, -0.1f, time));
            time += Time.deltaTime;
            yield return null;
        }
    }
}
