using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TransitionHUD : MonoBehaviour {

    public Text turnText;

    public GameObject kristyCard;
    public GameObject kristyFaceset;
    public GameObject toddCard;
    public GameObject toddFaceset;


    // Use this for initialization
    void Start () {

	}

	// Update is called once per frame
	void FixedUpdate () {
	    if(BattleManager.instance.currentTurn == BattleManager.Turn.WaitingYou)
        {
            kristyCard.SetActive(true);
            kristyFaceset.SetActive(true);
            turnText.text = "PLAYER TURN #" + BattleManager.instance.turnNumber.ToString();
            turnText.GetComponent<Outline>().effectColor = Color.red;
            turnText.GetComponent<Shadow>().effectColor = Color.red;

            toddCard.SetActive(false);
            toddFaceset.SetActive(false);
        }
        else if(BattleManager.instance.currentTurn == BattleManager.Turn.WaitingOpponent)
        {
            toddCard.SetActive(true);
            toddFaceset.SetActive(true);
            turnText.text = "ENEMY TURN #" + BattleManager.instance.turnNumber.ToString();
            turnText.GetComponent<Outline>().effectColor = Color.blue;
            turnText.GetComponent<Shadow>().effectColor = Color.blue;

            kristyCard.SetActive(false);
            kristyFaceset.SetActive(false);
        }
	}


}
