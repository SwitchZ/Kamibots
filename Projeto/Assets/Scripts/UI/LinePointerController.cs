using UnityEngine;
using System.Collections;

public class LinePointerController : MonoBehaviour {

    public GameObject battleOptions;

    Vector3[] positionArray = new Vector3[2];

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if(BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].actionPoints == 0)
        {
            positionArray[0] = Camera.main.ScreenToWorldPoint(new Vector3(150, 200, Camera.main.nearClipPlane)); 
            positionArray[1] = Camera.main.ScreenToWorldPoint(new Vector3(150, 200, Camera.main.nearClipPlane));
            this.GetComponent<LineRenderer>().SetPositions(positionArray);
        }

        if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].actionPoints > 0 &&
            BattleManager.instance.currentTurn == BattleManager.Turn.You &&
            !BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].moving && //necessary?
            !BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].attacking //necessary?
            ) {
            positionArray[0] = Camera.main.ScreenToWorldPoint(new Vector3(150, 200, Camera.main.nearClipPlane)); //attempt to get a position at the screen, not canvas in the world
            positionArray[1] = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].HPBall.transform.position;
            this.GetComponent<LineRenderer>().SetPositions(positionArray);
        }
        else
        {
            positionArray[0] = Camera.main.ScreenToWorldPoint(new Vector3(150, 200, Camera.main.nearClipPlane));
            positionArray[1] = Camera.main.ScreenToWorldPoint(new Vector3(150, 200, Camera.main.nearClipPlane));
            this.GetComponent<LineRenderer>().SetPositions(positionArray);
        }

	}
}
