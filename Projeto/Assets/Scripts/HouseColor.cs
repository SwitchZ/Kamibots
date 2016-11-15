using UnityEngine;
using System.Collections;

public class HouseColor : MonoBehaviour {

    public Color color;
    public bool isShop = false;

	void Start () {
        //redefine shop base colors
        if (isShop)
        {
            foreach (Transform child in transform)
            {
                child.GetComponent<Renderer>().material.color = Color.white;
            }

        }

        //paint the walls
        gameObject.transform.GetChild(18).GetComponent<Renderer>().material.color = color;
        gameObject.transform.GetChild(19).GetComponent<Renderer>().material.color = color;

    }
	

}
