using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartScene : MonoBehaviour
{
    public Image fadeOutImage;

    public void StartButton()
    {
        StartCoroutine(FadeOutScene());
    }

    public void QuitButton()
    {
        Application.Quit();
    }

    IEnumerator FadeOutScene()
    {
        float t = 1;
        while (t >= 0)
        {
            fadeOutImage.color = new Color(fadeOutImage.color.r, fadeOutImage.color.g, fadeOutImage.color.b, 1f-t);
            t -= Time.deltaTime * 0.5f;
            yield return null;
        }
        SceneManager.LoadScene(1);
    }
}
