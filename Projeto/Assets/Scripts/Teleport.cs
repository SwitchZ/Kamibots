using UnityEngine;
using System.Collections;

public class Teleport : MonoBehaviour {
    public string teleportCode;

    private GameObject canvasBlock;
    private GameObject crayunBlock;

    public Vector3 teleportPosition;

    void Start()//define coordinates for each teleport
    {
        if (teleportCode == "House1") teleportPosition = new Vector3(-211.0374f, 0.8716321f, 96.50735f);
        else if (teleportCode == "House2") teleportPosition = new Vector3(0, 0, 0); //return teleport outside house

        else if (teleportCode == "CanvasEntrance") teleportPosition = canvasBlock.transform.position;
        //else if (teleportCode == "CrayunEntrance") teleportPosition = crayunBlock.transform.position;

    }

    /*
    void OnCollisionEnter(Collider other)
    {
        if(this.teleportCode)
    }
    */
}
