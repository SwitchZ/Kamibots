using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Overworld : MonoBehaviour
{
    //public GameObject crayunCity;
    public Text cityName;
    public string gateName;
    public string sceneName;
    public string targetScene;

    public CanvasGroup fadeCanvasGroup;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButton("Enter") && sceneName != "")
        {
            StartCoroutine(EnterCity(1));
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Kristy")
        {
            cityName.text = gateName;
            targetScene = this.sceneName;
        }

        
    }

    void OnTriggerExit(Collider other)
    {
         cityName.text = "";
    }



    public IEnumerator EnterCity(float speed)
    {
        cityName.text = "";
        while (fadeCanvasGroup.alpha < 1f)
        {
            fadeCanvasGroup.alpha += speed * Time.deltaTime;
            yield return null;
        }
        SceneManager.LoadScene(targetScene);
    }
}