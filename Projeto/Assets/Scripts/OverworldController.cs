using UnityEngine;
using System.Collections;

public class OverworldController : MonoBehaviour
{
    public Character kristy;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        kristy.Move();
    }
}