using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LogoManager : MonoBehaviour {
    public GameObject chromaLogo;
    public CanvasGroup fadeCanvasGroup;

    // Use this for initialization
    void Start() {
   

        StartCoroutine("NextScene", 4.0f);

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator NextScene()
    {
        while (fadeCanvasGroup.alpha < 1f) //trocar transição para o troço redondo
        {
            fadeCanvasGroup.alpha += Time.deltaTime;
            yield return null;
        }
        yield return new WaitForSeconds(4f);
        SceneManager.LoadScene("MainMenu");
    }
}
