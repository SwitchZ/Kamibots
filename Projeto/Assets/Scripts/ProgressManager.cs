using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour {


    [Header("Kristy attributes")]
    public GameObject kristy;
    public int money = 100;
    public Vector3 kristyPosition;
    public Quaternion kristyRotation;

    [Header("Game progress attributes")]
    //public firstTime
    public bool defeatedTodd = false;
    public bool justLeftBattle = false;
    public bool firstTimeVisitingCrayun = true;

    void Awake () {
        DontDestroyOnLoad(transform.gameObject);
    }
	
    void Start()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            defeatedTodd = false;
            justLeftBattle = false;
            firstTimeVisitingCrayun = true;
        }
    }

	// Update is called once per frame
	void Update () {
	
	}

    public void SaveKristyPosition()
    {
        kristyPosition = GameObject.Find("Kristy").transform.position;
        kristyRotation = GameObject.Find("Kristy").transform.rotation;
    }

    public void ResetKristyPosition()
    {
        kristyPosition = new Vector3(100f,-0.5f, -65.3f);
        kristyRotation = GameObject.Find("Kristy").transform.rotation;
    }
}
