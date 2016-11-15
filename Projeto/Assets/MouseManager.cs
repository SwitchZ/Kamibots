using UnityEngine;
using System.Collections;

public class MouseManager : MonoBehaviour {
    public GameObject handCursor;

	// Use this for initialization
	void Start () {
        Cursor.visible = false;
    }
	
	// Update is called once per frame
	void LateUpdate () {
        handCursor.transform.position = Input.mousePosition;
	}
}
