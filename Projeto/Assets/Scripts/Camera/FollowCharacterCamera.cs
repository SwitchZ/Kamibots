using UnityEngine;
using System.Collections;

public class FollowCharacterCamera : MonoBehaviour
{

    public static GameObject player;
    public Transform targetPos;


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Kristy");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public static void UpdateCameraInsideBuilding()
    {
        Camera.main.GetComponent<Animator>().enabled = false;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(player.transform.position.x, player.transform.position.y+17f, player.transform.position.z - 9.0f), 0.15f);
        //Camera.main.transform.rotation = Quaternion.Euler(51f, 180f, 0f);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(51f, 0f, 0f), 0.07f);
    }


    public static void UpdateCameraOutdoors()
    {
        Camera.main.GetComponent<Animator>().enabled = false;
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(player.transform.position.x, 100.0f, player.transform.position.z - 30.0f), 0.15f);
        //Camera.main.transform.rotation = Quaternion.Euler(72.9486f, 0f, 0f);
        Camera.main.transform.rotation = Quaternion.Lerp(Camera.main.transform.rotation, Quaternion.Euler(72.9486f, 0f, 0f), 0.07f);
    }
}
