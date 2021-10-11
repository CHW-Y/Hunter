using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//  ������ �ؽ�Ʈ ������ ��ũ��Ʈ
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
        //  �ش� �����ǿ� ��� ������Ŵ
        transform.position = Camera.main.WorldToScreenPoint(temp);        

        //  ���� ���� 0 ���Ϸ� �������� ������Ʈ ����
        if (myText.color.a <= 0)
        {
            Destroy(gameObject);
        }

        //  ȭ�� ������ �Ѿ�� ��Ȱ��ȭ
        if (myText.transform.position.z < 0)
        {
            myText.enabled = false;
        }
        else
        {
            myText.enabled = true;
        }
    }

    //  ������ �ؽ�Ʈ�� ���� ���� Lerp�ϰ� ������ �Լ�
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
