using UnityEngine;
using System.Collections;

public class KamiInfoController : MonoBehaviour {

    public GameObject battleOptions;

    Vector3[] positionArray = new Vector3[2];

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (BattleManager.instance.anyPlayerSelected && BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex].HP > 0) { 

            positionArray[0] = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 214, 114, Camera.main.nearClipPlane)); //attempt to get a position at the screen, not canvas in the world
            positionArray[1] = BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex].HPBall.transform.position;
            this.GetComponent<LineRenderer>().SetPositions(positionArray);
        }
        else
        {
            positionArray[0] = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 214, 114, Camera.main.nearClipPlane));
            positionArray[1] = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width - 214, 114, Camera.main.nearClipPlane));
            this.GetComponent<LineRenderer>().SetPositions(positionArray);
        }

    }
}
