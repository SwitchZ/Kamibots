using UnityEngine;
using System.Collections;

public class UIController : MonoBehaviour {

    public bool isHoveringUI = false;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void EnteredUI()
    {
        isHoveringUI = true;
    }

    public void ExitedUI()
    {
        isHoveringUI = false;
    }
}
