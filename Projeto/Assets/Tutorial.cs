using UnityEngine;
using System.Collections;

public class Tutorial : MonoBehaviour {

    public GameObject poemSheet;
    public ProgressManager progressManager;

	// Use this for initialization
	void Start () {
        progressManager = GameObject.Find("ProgressManager").GetComponent<ProgressManager>();
    }
	
	// Update is called once per frame
	void Update () {
	    if((Input.GetButtonDown("Enter") && poemSheet.transform.position.y > -5) || (progressManager.justLeftBattle))
        {
            Destroy(this.gameObject);
        }
	}
}
