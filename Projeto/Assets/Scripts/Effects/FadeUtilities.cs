using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class FadeUtilities : MonoBehaviour {

    public IEnumerator LeaveCity(GameManager gameManager, float speed)
    {
        while (gameManager.fadeCanvasGroup.alpha < 1f)
        {
            gameManager.fadeCanvasGroup.alpha += speed * Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene("Overworld");
    }



    public IEnumerator FadeFromBlack(GameManager gameManager, float speed)
    {
        while (gameManager.fadeCanvasGroup.alpha > 0f)
        {
            gameManager.fadeCanvasGroup.alpha -= speed * Time.deltaTime;
            yield return null;
        }
    }


    public IEnumerator FadeToBlack(GameManager gameManager, float speed)
    {
        while (gameManager.fadeCanvasGroup.alpha < 1f)
        {
            gameManager.fadeCanvasGroup.alpha += speed * Time.deltaTime;
            yield return null;
        }
    }
}
