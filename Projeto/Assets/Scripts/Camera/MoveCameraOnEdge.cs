using UnityEngine;


public class MoveCameraOnEdge : MonoBehaviour {

    private int Boundary = 80; // distance from edge scrolling starts
    private int speed = 15; //speed which camera moves
    private float clippingRight, clippingLeft, clippingTop, clippingBottom;
    public GameObject battleUI;


    void Start()
    {
        clippingRight = 5.0f;
        clippingLeft = -5.0f;
        clippingTop = -8.0f;
        clippingBottom = -18.0f;
}

    void Update()
    {
        if (BattleManager.instance.currentTurn == BattleManager.Turn.You && !battleUI.GetComponent<UIController>().isHoveringUI) {
            if (Input.mousePosition.x > Screen.width - Boundary && transform.position.x < clippingRight)
            {
                // move on +X axis
                transform.position += new Vector3(speed * Time.deltaTime, 0, 0);
            }
            if (Input.mousePosition.x < 0 + Boundary && transform.position.x > clippingLeft)
            {
                // move on -X axis
                transform.position -= new Vector3(speed * Time.deltaTime, 0, 0);
            }
            if (Input.mousePosition.y > Screen.height - Boundary && transform.position.z < clippingTop)
            {
                // move on +Z axis
                transform.position += new Vector3(0, 0, speed * Time.deltaTime);
            }
            if (Input.mousePosition.y < 0 + Boundary && transform.position.z > clippingBottom)
            {
                // move on -Z axis
                transform.position -= new Vector3(0, 0, speed * Time.deltaTime);
            }

        }if(BattleManager.instance.currentTurn == BattleManager.Turn.Opponent /*|| BattleManager.instance.currentTurn == BattleManager.Turn.You*/)
        {
            transform.position = Vector3.Lerp(transform.position,new Vector3 (
                BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].transform.position.x,15.0f, 
                BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].transform.position.z-12
                ),0.1f);
        }
    }


}