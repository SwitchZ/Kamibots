using UnityEngine;
using UnityEngine.UI;
using System.Collections;


public class BattleHUD : MonoBehaviour {
    public static BattleHUD instance;

    public Text NameField;
    public Text ArmorPointsField;
    public Text BatteryPointsField;
    public Text ShotPointsField;
    public Text ExperienceField;
    public Text LevelField;
    public Text kamiStatus;

    public Text TurnNumberField;
    public Text WhoseTurnField;
    public Text KamibotsLeftField;

    public GameObject endTurnButton;
    public GameObject kristyAvatar;
    public GameObject toddAvatar;
    public GameObject kamibotAvatar;

    public GameObject transitionScene;
    public GameObject battleOptions;
    public GameObject playerStatus;

    public GameObject mapCursor;
    public GameObject terrainObject;

    public bool onDisablingProcess = false;

    [Header("Battle buttons")]
    public GameObject moveButton;
    public GameObject attackButton;
    public GameObject waitButton;

    [Header("Kamibot Avatars")]
    public Sprite sliceknight;
    public Sprite aquasniper;
    public Sprite metaman;
    public Sprite mantial;
    public Sprite kinderbot;

    // Use this for initialization
    void Start () {
        UpdateProgressHUD();
    }
	
	// Update is called once per frame
	void Update () {
        Player playerUnderMouse = BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex];

        //cursor active
        if(BattleManager.instance.currentTurn == BattleManager.Turn.You)
        {
            if (!mapCursor.activeSelf) mapCursor.SetActive(true);
        }
        else
        {
            if (mapCursor.activeSelf) mapCursor.SetActive(false);
        }

        //PlayerStatus HUD events
        if (!BattleManager.instance.anyPlayerSelected || playerUnderMouse.HP <= 0) {
            if (playerStatus.activeSelf)
            {
                Invoke("DisableHUD", 1.0f); //turn off player HUD (avoids repetition) if no player (playerStatus is the object)
            }
        }
        else { //if there is a player under mouse
            CancelInvoke("DisableHUD");
            if (playerUnderMouse.HP > 0)
            {
                if(!playerStatus.activeSelf) playerStatus.SetActive(true); //turn on player HUD (avoids repetition) if player alive
                UpdatePlayerHUD();
            }
        }


        //Battle options Event
        if (BattleManager.instance.currentPlayerIndex < BattleManager.instance.firstEnemyIndex) {
            if(!battleOptions.activeSelf) battleOptions.SetActive(true); //Kristy's turn? Enable BATTLE OPTIONS (only once for optimization reasons)

        }
        else {
            if (battleOptions.activeSelf) battleOptions.SetActive(false); //Not Kristy's? Disable BATTLE OPTIONS (only once for optimization reasons)
        }


        //EndTurn Button
        if (BattleManager.instance.currentPlayerIndex < BattleManager.instance.firstEnemyIndex) {
            if (!endTurnButton.activeSelf) endTurnButton.SetActive(true); //Kristy's turn? Enable END TURN (only once for optimization reasons)
        }
        else {
            if (endTurnButton.activeSelf) endTurnButton.SetActive(false); //Not Kristy's turn? Disable END TURN (only once for optimization reasons)
        };


        //Battle progress events - update it during turn transitions
        if (BattleManager.instance.currentTurn == BattleManager.Turn.You ||
            BattleManager.instance.currentTurn == BattleManager.Turn.Opponent)
            UpdateProgressHUD();


        //Update terrain info
        if(BattleManager.instance.currentTerrain == "") terrainObject.SetActive(false);
        else terrainObject.SetActive(true);


        if(BattleManager.instance.currentTurn == BattleManager.Turn.You ) ButtonToggler();


        //kami status
        if (BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex].actionPoints == 0)  kamiStatus.GetComponent<Text>().text = "Standby";
        else kamiStatus.GetComponent<Text>().text = "";


        if (BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex].HP <= BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex].MaxHP/4) ArmorPointsField.GetComponent<Text>().color = Color.red;
        else ArmorPointsField.GetComponent<Text>().color = new Color(131/255f, 255/255f, 30/255f);

        if (BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex].shots <= BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex].MaxShots / 4) ShotPointsField.GetComponent<Text>().color = Color.blue;
        else ShotPointsField.GetComponent<Text>().color = new Color(131/255f, 255/255f, 30/255f);

        if (BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex].battery <= 4) BatteryPointsField.GetComponent<Text>().color = Color.yellow;
        else BatteryPointsField.GetComponent<Text>().color = new Color(131/255f, 255/255f, 30/255f);
    }


    void UpdatePlayerHUD()
    {
        Player playerUnderMouse = BattleManager.instance.players[BattleManager.instance.playerUnderMouseIndex];

        NameField.text = playerUnderMouse.playerName;
        ArmorPointsField.text = playerUnderMouse.HP.ToString();
        BatteryPointsField.text = playerUnderMouse.battery.ToString();
        ExperienceField.text = "" + ((playerUnderMouse.exp/Mathf.Pow(2, playerUnderMouse.level))*100).ToString() + "%"; //percentage, exp needed to pass level
        LevelField.text = "Lv=" + playerUnderMouse.level.ToString();

        if(playerUnderMouse.playerName == "Aquasniper") kamibotAvatar.GetComponent<Image>().sprite = aquasniper;
        else if (playerUnderMouse.playerName == "Sliceknight") kamibotAvatar.GetComponent<Image>().sprite = sliceknight;
        else if (playerUnderMouse.playerName == "Mantial") kamibotAvatar.GetComponent<Image>().sprite = mantial;
        else if (playerUnderMouse.playerName == "Metaman") kamibotAvatar.GetComponent<Image>().sprite = metaman;
        else if (playerUnderMouse.playerName == "Kinderbot") kamibotAvatar.GetComponent<Image>().sprite = kinderbot;

        if (playerUnderMouse.playerName != "Aquasniper")  ShotPointsField.text = "∞";
        else ShotPointsField.text = playerUnderMouse.shots.ToString();
    }


    void UpdateProgressHUD()
    {
        TurnNumberField.text = "Turn = " + BattleManager.instance.turnNumber.ToString();
        string currentCharacter = (BattleManager.instance.currentPlayerIndex < BattleManager.instance.firstEnemyIndex) ? "Kristy" : "Todd";
        WhoseTurnField.text = currentCharacter + "'s Turn";

        int kamibotsLeft = (BattleManager.instance.currentPlayerIndex < BattleManager.instance.firstEnemyIndex) ? BattleManager.instance.alliesLeft : BattleManager.instance.enemiesLeft;
        KamibotsLeftField.text = "Kamibots = " + kamibotsLeft.ToString();

        //Update whose turn
        if (BattleManager.instance.currentPlayerIndex < BattleManager.instance.firstEnemyIndex)
        {
            WhoseTurnField.color = Color.yellow;
            kristyAvatar.SetActive(true);
            toddAvatar.SetActive(false);
            kristyAvatar.GetComponent<RectTransform>().anchoredPosition = Vector2.Lerp(new Vector2(-41,61.5f), new Vector2(-81, 61.5f), -0.1f);
        }
        else
        {
            WhoseTurnField.color = new Color(0.5f, 0.75f, 1.0f);
            kristyAvatar.SetActive(false);
            toddAvatar.SetActive(true);
        }
    }

    /*
    IEnumerator disableHUD(GameObject desiredHUD, float delay)
    {
        Debug.Log("Function called");
        onDisablingProcess = true;
        yield return new WaitForSeconds(delay);
        if(!BattleManager.instance.anyPlayerSelected)
            desiredHUD.SetActive(false);
        onDisablingProcess = false;
        Debug.Log("Done");
    }
    */
    void DisableHUD()
    {
        playerStatus.SetActive(false);
    }


    public void ButtonToggler()
    {
        if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].moving)
        {
            moveButton.SetActive(true); //
            attackButton.SetActive(false); //
            waitButton.SetActive(false); //reappear other options of battle
        }
        else if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].attacking)
        {
            moveButton.SetActive(false);
            attackButton.SetActive(true);
            waitButton.SetActive(false);
        }

        else 
        {
            moveButton.SetActive(true);
            attackButton.SetActive(true);
            waitButton.SetActive(true);
        }

        if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].actionPoints == 0)
        {
            battleOptions.SetActive(false);
        }
        else
        {
            battleOptions.SetActive(true);
        }

    }


}
