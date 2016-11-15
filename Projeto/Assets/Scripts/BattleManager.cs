using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour {

    public enum Turn{
        You, Opponent, WaitingYou, WaitingOpponent, Victory, Defeat
    }

    public static BattleManager instance;

    private RaycastHit hit;

    public Camera battleCamera;
    public XmlDocument container2;


    public AudioClip Battle;
    public AudioClip Battle2;
    public AudioClip Victory;
    public AudioClip Victory2;
    public AudioSource source;

    public AudioClip sword;
    public AudioClip punch;
    public AudioClip dead;
    public AudioClip beep;
    public AudioClip run;
    public AudioClip shot;
    public AudioClip aura;


    [Header("Battle prefabs")]
    public GameObject TilePrefab;
    public GameObject UserPlayerPrefab;
    public GameObject AIPlayerPrefab;
    public GameObject SliceknightPrefab; //well sh*t happens dude

    public GameObject Mountain;
    public GameObject Boulder;
    public GameObject Fire;
    public GameObject Tree;
    public GameObject waterShot;
    //public GameObject sword;

    public int mapWidth = 20;  //width of arena
    public int mapHeight = 15; //height of arena
    Transform mapTransform;

    public Player target = null;
    public int amountOfDamage;


    public List <List<Tile>> map = new List<List<Tile>>(); //map is matrix of arena tiles (list of list of tiles)
    public List <Player> players = new List<Player>(); //list of players
    public int currentPlayerIndex = 0; //index of current player in turn
    public int playerUnderMouseIndex = 0; //index of player under mouse position
    public bool anyPlayerSelected = false;

    public int firstEnemyIndex;
    public bool playingTransition = false;
    public Turn currentTurn = Turn.WaitingYou;
    public int turnNumber = 1;
    public int alliesLeft, enemiesLeft = 0;

    public Animator kristyAnimator;
    //public Animator allyAnimator;
    //public GameObject kristy;

    public string currentTerrain = "";
    public UIController uiController;

    [Header("Canvases")]
    public GameObject transitionCanvas; //transition canvas
    public GameObject battleCanvas; //battle canvas
    public GameObject battleVictoryCanvas;
    public GameObject battleDefeatCanvas;

    [Header("Results")]
    public Text resultsInfo;

    [Header("Progress pointers")]
    public ProgressManager progressManager; //transition canvas

    void Awake()
    {
        instance = this;

        mapTransform = transform.FindChild("Map");

    }

    // Use this for initialization
    void Start()
    {
        //source = GetComponent<AudioSource>();

        source.clip = Battle;
        source.loop = false;
        source.Play();
        
        progressManager = GameObject.Find("ProgressManager").GetComponent<ProgressManager>();
        waterShot = GameObject.Find("Hose");

        //kristyAnimator = kristy.GetComponent<Animator>();
        kristyAnimator.SetBool("battling", true);
        generateMap();
        generatePlayers();

        alliesLeft = firstEnemyIndex; //index of first enemy in array is exactly the number of allies in start of battle
        enemiesLeft = players.Count - alliesLeft; //enemies = total of players MINUS allies left
        //battleCamera = Camera.main;
    }


    IEnumerator YourTurnTransition()
    {
        playingTransition = true;
        //animate cutscene here
        battleCanvas.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        transitionCanvas.GetComponent<CanvasGroup>().alpha = 1; //replace by card/avatar animation

        yield return new WaitForSeconds(2.0f);
        transitionCanvas.GetComponent<CanvasGroup>().alpha = 0; //replace by card/avatar animation
        battleCanvas.SetActive(true);



        playingTransition = false;
        currentTurn = Turn.You;
    }


    IEnumerator OpponentTurnTransition()
    {
        playingTransition = true;
        //animate cutscene here
        battleCanvas.SetActive(false);
        yield return new WaitForSeconds(1.0f);
        transitionCanvas.GetComponent<CanvasGroup>().alpha = 1; //replace by card/avatar animation
        yield return new WaitForSeconds(2.0f);
        transitionCanvas.GetComponent<CanvasGroup>().alpha = 0; //replace by card/avatar animation
        battleCanvas.SetActive(true);

        playingTransition = false;
        currentTurn = Turn.Opponent;
    }


    // Update is called once per frame
    void FixedUpdate()
    {
        //music
        if (!source.isPlaying && !source.loop && currentTurn != Turn.Victory && currentTurn != Turn.Defeat)
        {
            source.clip = Battle2;
            source.loop = true;
            source.Play();
        }

        switch (currentTurn)
        {
            case Turn.WaitingYou:
                if(!playingTransition) StartCoroutine(YourTurnTransition());
                uiController.ExitedUI();
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(-0.47f, 19.9f, -17.59f), 0.1f);
                break;

            case Turn.WaitingOpponent:
                if (!playingTransition) StartCoroutine(OpponentTurnTransition());
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(-0.47f, 19.9f, -17.59f), 0.1f);
                break;

            case Turn.You:
                if(Camera.main.transform.position.y > 15.1f)
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(
                    Camera.main.transform.position.x, 15.0f,
                    Camera.main.transform.position.z
                    ), 0.1f);
                break;

            case Turn.Opponent:
                break;

            case Turn.Victory:
                StartCoroutine(ExecuteVictory());
                Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(-0.47f, 19.9f, -17.59f), 0.1f);
                resultsInfo.text = "Turns elapsed: " + turnNumber +"\n" +
                    "Kamibots wasted: " + (5- alliesLeft) + "\n\n" +
                    "You got <color=#FFF700FF>" + 1000 + "Ѧ</color>."
                    ;

                //music
                if (!source.isPlaying && !source.loop)
                {
                    source.clip = Victory2;
                    source.loop = true;
                    source.Play();
                }

                if (battleVictoryCanvas.activeSelf && Input.GetButtonDown("Enter"))
                {
                    progressManager.defeatedTodd = true;
                    progressManager.justLeftBattle = true;
                    SceneManager.LoadScene("CrayunTown");
                }
                //do more stuff for victory events
                break;

            case Turn.Defeat:
                StartCoroutine(ExecuteDefeat());
                if (battleDefeatCanvas.activeSelf && Input.GetButtonDown("Enter"))
                    SceneManager.LoadScene("MainMenu");
                //do more stuff for defeat events
                break;

            default:
                Debug.Log("WTF happened");
                break;
        }

        //if it's actually someone's turn - logic may be wrong
        if (currentTurn == Turn.You || currentTurn == Turn.Opponent)
        {
            if (alliesLeft > 0 && enemiesLeft > 0)
            {

                if (players[currentPlayerIndex].battery <= 0 && currentPlayerIndex >= firstEnemyIndex) nextTurn();
                //The current player is alive? Great, he can do his job
                else if (players[currentPlayerIndex].HP > 0 ) players[currentPlayerIndex].TurnUpdate();
                //If not, just move on to the next player
                else nextTurn();
            }

        }


        //Check end of battle
        if (alliesLeft == 0) currentTurn = Turn.Defeat;
        else if (enemiesLeft == 0) currentTurn = Turn.Victory;
    }


    //define next player through their index
    public void nextTurn()
    {
        //If it's not the last player of the round, keep passing to the next.
        if (currentPlayerIndex + 1 < players.Count)
        {
            currentPlayerIndex++;
        }
        //Else, back to beginning of the queue.
        else
        {
            currentPlayerIndex = 0;
            turnNumber++;
            for (int i = 0; i < players.Count; i++) //i was firstEnemyIndex before, did this help?
            {
                players[i].actionPoints = 2;
                players[i].transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.white; 
                players[i].characterisIdle();
                players[i].attacking = false;
            }

            currentTurn = Turn.WaitingYou;
        }
    }

    public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance)
    {
        highlightTilesAt(originLocation, highlightColor, distance, true);
    }

    //highlight tile (location, color wanted, extension of the highlights, ignoring players?)
    public void highlightTilesAt(Vector2 originLocation, Color highlightColor, int distance, bool ignorePlayers)
    {
        //tiles to be highlighted
        List<Tile> highlightedTiles = new List<Tile>();

        if (ignorePlayers) highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance, highlightColor == Color.red);
        else highlightedTiles = TileHighlight.FindHighlight(map[(int)originLocation.x][(int)originLocation.y], distance, players.Where(x => x.gridPosition != originLocation).Select(x => x.gridPosition).ToArray(), highlightColor == Color.red);

        //start highlighting of them
        foreach (Tile t in highlightedTiles)
        {
            t.visual.transform.GetComponent<Renderer>().materials[0].color = highlightColor; //highlight it in color specified
        }

    }

    public void removeTileHighlights() //reset the tile highlights...
    {
        for (int i = 0; i < mapWidth; i++)
        {
            for (int j = 0; j < mapHeight; j++)
            {
                if (!map[i][j].impassible)  //for the tiles not impassible
                    map[i][j].visual.transform.GetComponent<Renderer>().materials[0].color = Color.white; //back to normal color (here, back to white)
            }
        }
    }

    //self-explainatory, i assume
    //destTile being target tile
    public void moveCurrentPlayer(Tile destTile)
    {
        
        //if (players[currentPlayerIndex].actionPoints > 1)
        //{

            if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassible && players[currentPlayerIndex].positionQueue.Count == 0) //only accept tiles in player's range (not white) and are crossable...
            {
                removeTileHighlights(); //turn off highlights
                players[currentPlayerIndex].moving = false;
                foreach (Tile t in TilePathfinder.FindPath(map[(int)players[currentPlayerIndex].gridPosition.x][(int)players[currentPlayerIndex].gridPosition.y], destTile, players.Where(x => x.gridPosition != destTile.gridPosition && x.gridPosition != players[currentPlayerIndex].gridPosition).Select(x => x.gridPosition).ToArray()))
                {
                    players[currentPlayerIndex].positionQueue.Add(map[(int)t.gridPosition.x][(int)t.gridPosition.y].transform.position + 0.47f * Vector3.up); //absolute world position to be target on the battle map
                    //Debug.Log("(" + players[currentPlayerIndex].positionQueue[players[currentPlayerIndex].positionQueue.Count - 1].x + "," + players[currentPlayerIndex].positionQueue[players[currentPlayerIndex].positionQueue.Count - 1].y + ")");
                }
                players[currentPlayerIndex].gridPosition = destTile.gridPosition;
            }
            else { //...or he'll get nowhere.
                   Debug.Log("destination invalid");
                   removeTileHighlights(); //turn off highlights
                   players[currentPlayerIndex].moving = false;
        }
        //}
    }

    //also self-explainatory, i assume
    //destTile being target tile
    public void attackWithCurrentPlayer(Tile destTile)
    {
        if (destTile.visual.transform.GetComponent<Renderer>().materials[0].color != Color.white && !destTile.impassible) //like above, only accept tiles in player's range and passible
        {
            //Player target = null;

            //defining which is the target player of the attack...
            foreach (Player p in players) //...in the list of players
            {
                if (p.gridPosition == destTile.gridPosition) //if the target player is right on the target tile
                {
                    target = p; //he is the one you're looking for!! PREPARE ATTACK!!!! SwiftRage

                }
            }


            if (target != null) //after target found
            {
                //Player is adjacent to target?
                
                //players[currentPlayerIndex].actionPoints = 0; //attack consumes all the action points
                players[currentPlayerIndex].characterisInactive();
                if (players[currentPlayerIndex].playerName == "Aquasniper")
                    players[currentPlayerIndex].shots--;
                players[currentPlayerIndex].battery--;

                //make the enemy face the direction he's attacking
                players[currentPlayerIndex].transform.LookAt(new Vector3(target.transform.position.x,
                    players[currentPlayerIndex].transform.position.y,
                    target.transform.position.z));

                removeTileHighlights();
                players[currentPlayerIndex].attacking = false;

                //attack logic
                //damage logic: damage = base + d6
                players[currentPlayerIndex].characterisAttacking();
                amountOfDamage = Mathf.Max(0, (int)Mathf.Floor(players[currentPlayerIndex].damageBase) - target.damageReduction - destTile.defenseBonus);
               
                //subtract HP from attacked target
                //target.HP -= amountOfDamage;

                Debug.Log(players[currentPlayerIndex].playerName + " successfuly hit " + target.playerName + " for " + amountOfDamage + " damage!");
               
            }
        }
        else
        {
            Debug.Log("destination invalid");
            removeTileHighlights(); //turn off highlights
            players[currentPlayerIndex].attacking = false;
        }
    }


    // Generate arena map.
    void generateMap()
    {
        loadMapFromXml();

    }

    void loadMapFromXml()
    {
        MapXmlContainer container = MapSaveLoad.Load("map.xml");

        mapWidth = container.width;
        mapHeight = container.height;

        //initially remove all children
        for (int i = 0; i < mapTransform.childCount; i++)
        {
            Destroy(mapTransform.GetChild(i).gameObject);
        }

        map = new List<List<Tile>>();
        for (int i = 0; i < mapWidth; i++)
        {
            List<Tile> row = new List<Tile>();
            for (int j = 0; j < mapHeight; j++)
            {
                Tile tile = ((GameObject)Instantiate(PrefabHolder.instance.BASE_TILE_PREFAB, new Vector3(i - Mathf.Floor(mapWidth / 2), 0, -j + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3()))).GetComponent<Tile>();
                tile.battleUI = GameObject.Find("BattleCanvas"); //used for logic to avoid "raycast" passing through the button
                tile.transform.parent = mapTransform;
                tile.gridPosition = new Vector2(i, j);
                tile.setType((TileType)container.tiles.Where(x => x.locX == i && x.locY == j).First().id);
                if (tile.type == TileType.Impassible) {
                    if ((i == 8 || i == 12) && j == 8)
                    {
                        Instantiate(Boulder, new Vector3(i - Mathf.Floor(mapWidth / 2), 0.5f, -j + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(270, 0, 0));
                        tile.terrain = "Boulder";
                    }
                    else if ((i >= 9 && i <= 11) && (j >= 7 && j <= 9))
                    {
                        //Instantiate(Fire, new Vector3(i - Mathf.Floor(mapWidth / 2), 1.1f, -j + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(270, 0, 0));
                        //tile.visual.transform.GetComponent<Renderer>().material.color = new Color(1.0f, 0.5f, 0.0f);
                        tile.visual.transform.GetComponent<Renderer>().material.color = Color.black;
                        tile.terrain = "Fire";
                    }
                    else
                    {
                        Instantiate(Mountain, new Vector3(i - Mathf.Floor(mapWidth / 2), 0.5f, -j + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(270, 0, 0));
                        tile.terrain = "Mountain";
                    }

                }else if (tile.type == TileType.VeryDifficult) {
                    Instantiate(Tree, new Vector3(i - Mathf.Floor(mapWidth / 2), 1.5f, -j + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(270, 0, 0));
                    tile.terrain = "Tree";
                }

                row.Add(tile);
            }
            map.Add(row);
        }
    }

    
    // Generate players = kamibots
    void generatePlayers()
    {
        UserPlayer player;

        //instantiating allies
        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(10 - Mathf.Floor(mapWidth / 2), 0.5f, -11 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(10, 11);
        player.playerName = "Aquasniper";
        player.specs = Specs.FromType(KamibotType.Aquasniper);
        player.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = player.Aquasniper;
        player.order = 0;
        player.attackerCode = 0;
        
        players.Add(player);

        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(4 - Mathf.Floor(mapWidth / 2), 0.5f, -10 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(4, 10);
        player.playerName = "Mantial";
        player.specs = Specs.FromType(KamibotType.Mantial);
        player.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = player.Mantial;
        player.order = 1;
        player.attackerCode = 2;

        players.Add(player);

        player = ((GameObject)Instantiate(SliceknightPrefab, new Vector3(1 - Mathf.Floor(mapWidth / 2), 0.5f, -13 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(1, 13);
        player.playerName = "Sliceknight";
        player.specs = Specs.FromType(KamibotType.Sliceknight);
        player.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = player.Sliceknight;
        player.kamibotAnimator = player.GetComponent<Animator>();
        player.order = 2;
        player.attackerCode = 1;

        //player.Sword.SetActive(true);


        players.Add(player);

        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(14 - Mathf.Floor(mapWidth / 2), 0.5f, -11 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(14, 11);
        player.playerName = "Metaman";
        player.specs = Specs.FromType(KamibotType.Metaman);
        player.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = player.Metaman;
        player.order = 3;
        player.attackerCode = 3;

        players.Add(player);

        player = ((GameObject)Instantiate(UserPlayerPrefab, new Vector3(18 - Mathf.Floor(mapWidth / 2), 0.5f, -9 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3()))).GetComponent<UserPlayer>();
        player.gridPosition = new Vector2(18, 9);
        player.playerName = "Aquasniper";
        player.specs = Specs.FromType(KamibotType.Aquasniper);
        player.transform.GetChild(1).gameObject.GetComponent<Renderer>().material = player.Aquasniper;
        player.order = 4;
        player.attackerCode = 0;

        players.Add(player);

        //enemies
        firstEnemyIndex = 5;

        AIPlayer aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(6 - Mathf.Floor(mapWidth / 2), 0.5f, -2 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3(0, -180, 0)))).GetComponent<AIPlayer>();
        aiplayer.gridPosition = new Vector2(6, 2);
        aiplayer.playerName = "Kinderbot";
        aiplayer.specs = Specs.FromType(KamibotType.Kinderbot);
        aiplayer.attackerCode = 1;

        players.Add(aiplayer);

        aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(8 - Mathf.Floor(mapWidth / 2), 0.5f, -2 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3(0,-180,0)))).GetComponent<AIPlayer>();
        aiplayer.gridPosition = new Vector2(8, 2);
        aiplayer.playerName = "Kinderbot";
        aiplayer.specs = Specs.FromType(KamibotType.Kinderbot);
        aiplayer.attackerCode = 1;

        players.Add(aiplayer);

        aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(10 - Mathf.Floor(mapWidth / 2), 0.5f, -2 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3(0, -180, 0)))).GetComponent<AIPlayer>();
        aiplayer.gridPosition = new Vector2(10, 2);
        aiplayer.playerName = "Kinderbot";
        aiplayer.specs = Specs.FromType(KamibotType.Kinderbot);
        aiplayer.attackerCode = 1;

        players.Add(aiplayer);

        aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(12 - Mathf.Floor(mapWidth / 2), 0.5f, -2 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3(0, -180, 0)))).GetComponent<AIPlayer>();
        aiplayer.gridPosition = new Vector2(12, 2);
        aiplayer.playerName = "Kinderbot";
        aiplayer.specs = Specs.FromType(KamibotType.Kinderbot);
        aiplayer.attackerCode = 1;

        players.Add(aiplayer);

        aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(7 - Mathf.Floor(mapWidth / 2), 0.5f, -1 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3(0, -180, 0)))).GetComponent<AIPlayer>();
        aiplayer.gridPosition = new Vector2(7, 1);
        aiplayer.playerName = "Kinderbot";
        aiplayer.specs = Specs.FromType(KamibotType.Kinderbot);
        aiplayer.attackerCode = 1;

        players.Add(aiplayer);

        aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(9 - Mathf.Floor(mapWidth / 2), 0.5f, -1 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3(0, -180, 0)))).GetComponent<AIPlayer>();
        aiplayer.gridPosition = new Vector2(9, 1);
        aiplayer.playerName = "Kinderbot";
        aiplayer.specs = Specs.FromType(KamibotType.Kinderbot);
        aiplayer.attackerCode = 1;

        players.Add(aiplayer);

        aiplayer = ((GameObject)Instantiate(AIPlayerPrefab, new Vector3(11 - Mathf.Floor(mapWidth / 2), 0.5f, -1 + Mathf.Floor(mapHeight / 2)), Quaternion.Euler(new Vector3(0, -180, 0)))).GetComponent<AIPlayer>();
        aiplayer.gridPosition = new Vector2(11, 1);
        aiplayer.playerName = "Kinderbot";
        aiplayer.specs = Specs.FromType(KamibotType.Kinderbot);
        aiplayer.attackerCode = 1;

        players.Add(aiplayer);

    }

    
    IEnumerator ExecuteVictory() 
    {
        transitionCanvas.SetActive(false); //transition canvas
        battleCanvas.SetActive(false); //battle canvas
        yield return new WaitForSeconds(3.0f);
        if (!battleVictoryCanvas.activeSelf) source.clip = Victory;
        if (!battleVictoryCanvas.activeSelf) source.loop = false;
        if (!source.isPlaying && !battleVictoryCanvas.activeSelf) source.Play();
        battleVictoryCanvas.SetActive(true); //victory screen Canvas
    }


    IEnumerator ExecuteDefeat()
    {
        source.Stop();
        transitionCanvas.SetActive(false); //transition canvas
        battleCanvas.SetActive(false); //battle canvas
        yield return new WaitForSeconds(3.0f);
        battleDefeatCanvas.SetActive(true); //defeat screen Canvas
    }

    void UpdateReward()
    {
        
    }


}
