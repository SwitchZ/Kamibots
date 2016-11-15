using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Item : MonoBehaviour {

    public Text itemDescription;

    public int glueAmount = 7;
    public int batteryAmount = 5;
    public int waterAmount = 3;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        if (glueAmount == 0 && batteryAmount == 0 && waterAmount == 0) Destroy(this.gameObject);
	}

    public void UpdateItemDescription(GameObject button)
    {
        if (button.name == "WhiteGlue") itemDescription.text = "White Glue: Restore your kamibot's armor points. Owns: "+ glueAmount;
        else if (button.name == "AlkalineBattery") itemDescription.text = "Alkaline Battery: Restore your kamibot's battery points. Owns: " + batteryAmount;
        else if (button.name == "WaterRefill") itemDescription.text = "Water Refill: Restore your kamibot's shot points. Owns: " + waterAmount;
    }

    public void RemoveItemDescription()
    {
        itemDescription.text = "";
       
    }

    public void UseItem(GameObject button)
    {
        if (button.name == "WhiteGlue")
        {
            glueAmount--;
            BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].HP = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].MaxHP;
            if (glueAmount == 0) Destroy(button);
            AudioSource.PlayClipAtPoint(BattleManager.instance.aura, Camera.main.transform.position);
            BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].HPBall.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (button.name == "AlkalineBattery")
        {
            batteryAmount--;
            BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].battery = 10;
            if (batteryAmount == 0) Destroy(button);
            AudioSource.PlayClipAtPoint(BattleManager.instance.aura, Camera.main.transform.position);
            BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].HPBall.GetComponent<Renderer>().material.color = Color.green;
        }
        else if (button.name == "WaterRefill")
        {
            waterAmount--;
            BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].shots = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].MaxShots;
            if (waterAmount == 0) Destroy(button);
            AudioSource.PlayClipAtPoint(BattleManager.instance.aura, Camera.main.transform.position);
            BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].HPBall.GetComponent<Renderer>().material.color = Color.green;
        }

        RemoveItemDescription();
    }
}
