using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour {
    public CanvasGroup fadeCanvasGroup;
    public GameObject loading;
    private bool enterPressed = false;

    public GameObject pressEnter;

    public AudioSource source;
    public AudioClip Kamiverture;

    public Sprite titleScreenAlt;
    public GameObject KamibotsLogo;
    public GameObject background;

    void Start()
    {
        source = GetComponent<AudioSource>();

        source.clip = Kamiverture;
        source.Play();

        if (PlayerPrefs.HasKey("GameBeatenBefore")) {
            KamibotsLogo.SetActive(false);
            background.GetComponent<Image>().sprite = titleScreenAlt;

        }
    }

	// Update is called once per frame
	void FixedUpdate () {


        if (Input.GetButton("Enter") && !enterPressed)
        {
            Destroy(pressEnter);
            enterPressed = true;
            StartCoroutine(FadeToBlack(1));
            loading.SetActive(true);
            //StartCoroutine(Loading());
        }
        //if (fadeCanvasGroup.alpha == 0) SceneManager.LoadScene("Overworld");
        /*
        if (fadeCanvasGroup.alpha == 0)
        {
            loading.SetActive(true);
            SceneManager.LoadSceneAsync("CrayunTown");

        }
        */

    }

    public IEnumerator FadeToBlack(float speed)
    {
        while (fadeCanvasGroup.alpha > 0f)
        {
            fadeCanvasGroup.alpha -= speed * Time.deltaTime;
            yield return null;
        }

        SceneManager.LoadScene("CrayunTown");
    }

    /*
    IEnumerator Loading()
    {
        AsyncOperation async = SceneManager.LoadScene("CrayunTown");
        yield return null;
        //Debug.Log("Loading complete");
    }
    */
}
