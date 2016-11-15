using UnityEngine;
using System.Collections;

public class TextureFixer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Renderer>().material.mainTextureScale = new Vector2(transform.localScale.x/4, transform.localScale.y/4);
    }
}
