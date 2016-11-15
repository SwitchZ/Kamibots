using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tile : MonoBehaviour {

    GameObject PREFAB;

    public GameObject visual;
    public GameObject mapCursor;
    public GameObject battleUI;

    public TileType type = TileType.Normal;

    public Vector2 gridPosition = Vector2.zero;
    public int movementCost = 1;
    public int defenseBonus = 0;
    public int gCost;
    public int hCost;
    public Tile parent;
    public bool impassible = false; //solid tile

    public string terrain = "Plain";
    public string spritecode = "grass_terrain";

    public List<Tile> neighbors = new List<Tile>(); //list of neighbors of the tile

    // Use this for initialization
    void Start () {
        
        mapCursor = GameObject.Find("MapCursor");
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Battlefield")  generateNeighbors(); //self-explainatory. resembles the concept of parenting nodes
    }

    public static explicit operator Tile(GameObject v)
    {
        throw new NotImplementedException();
    }

    //generate neighbors
    public void generateNeighbors()
    {
        neighbors = new List<Tile>(); //create a list for storing neighbors of the tile

        //up
        if (gridPosition.y > 0) //tile not in upper bounds
        {
            Vector2 n = new Vector2(gridPosition.x, gridPosition.y - 1); //make an upper neighbor
            neighbors.Add(BattleManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]); //adding the tile in list of neighbors?
        }
        //down
        if (gridPosition.y < BattleManager.instance.mapHeight - 1) //tile not in lower bounds
        {
            Vector2 n = new Vector2(gridPosition.x, gridPosition.y + 1); //lower neighbor
            neighbors.Add(BattleManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }

        //left
        if (gridPosition.x > 0) //tile not in left bounds
        {
            Vector2 n = new Vector2(gridPosition.x - 1, gridPosition.y); //neighbor from left
            neighbors.Add(BattleManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }
        //right
        if (gridPosition.x < BattleManager.instance.mapWidth - 1) //tile not in right bounds
        {
            Vector2 n = new Vector2(gridPosition.x + 1, gridPosition.y); //neighbor from right
            neighbors.Add(BattleManager.instance.map[(int)Mathf.Round(n.x)][(int)Mathf.Round(n.y)]);
        }
    }


    // Update is called once per frame
    void Update () {

    }

    void OnMouseEnter()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapCreatorScene" && Input.GetMouseButton(0))
        {
            setType(MapCreatorManager.instance.palletSelection);
        }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Battlefield")
        {
            //highlight tile with a[] thing, like a cursor
            GameObject cursor = GameObject.Find("MapCursor");
            //set cursor positon
            float x = transform.position.x;
            float z = transform.position.z;
            mapCursor.transform.position = new Vector3(x, 0.51f, z);

            BattleManager.instance.currentTerrain = terrain; //store terrain name           
        }
    }

    void OnMouseExit()
    {
        //un-highlight tile <[]> 
        mapCursor.transform.position = new Vector3(0,0,0);

        BattleManager.instance.currentTerrain = ""; //store terrain name


    }

    //tile clicked
    void OnMouseDown()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "Battlefield")
        {
            if (!battleUI.GetComponent<UIController>().isHoveringUI) {
                //if the guy is on move mode, a click on cell results in his movement to there
                if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].moving)
                {
                    BattleManager.instance.moveCurrentPlayer(this);
                }
                //if the guy is on attack mode, a click on cell with a enemy should trigger an attack in there
                //this condition needs to be customized for my game though
                else if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].attacking)
                {
                    BattleManager.instance.attackWithCurrentPlayer(this);

                    if (this.impassible) DestroyObstacle(this);
                }
            }

        }else if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "MapCreatorScene")
        {
            setType(MapCreatorManager.instance.palletSelection);
        }

    }


    public void DestroyObstacle(Tile destTile)
    {
        RaycastHit hit;
        Vector3 dwn = transform.TransformDirection(Vector3.up);
        Player activePlayer = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex];

        if (destTile.impassible)
        {
            print("It's possible there is some obstacle in this impassible tile.");

            if (Physics.Raycast(transform.position, dwn, out hit, 10))
            {
                print("There is something down of the object!");
                Debug.Log(hit.transform.gameObject);

                if (activePlayer.playerName == "Aquasniper" && destTile.terrain == "Fire")
                {
                    Destroy(hit.transform.gameObject);
                    destTile.setType(TileType.Normal);
                    print("Fire extinguished.");
                    activePlayer.actionPoints = 0;
                    BattleManager.instance.removeTileHighlights();
                } 
                else if(activePlayer.playerName != "Aquasniper" && destTile.terrain == "Boulder")
                {
                    Destroy(hit.transform.gameObject);
                    destTile.setType(TileType.Normal);
                    print("Boulder cracked apart.");
                    activePlayer.actionPoints = 0;
                    BattleManager.instance.removeTileHighlights();
                }
            }
        }
    }


    public void setType(TileType t)
    {
        type = t;
        //definitions of TileType properties
        switch (t)
        {
            case TileType.Normal:
                movementCost = 1;
                defenseBonus = 0;
                impassible = false;
                PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                terrain = "Plain";
                spritecode = "grass_terrain";
                break;

            case TileType.Difficult:
                movementCost = 1;
                defenseBonus = 0;
                impassible = false;
                PREFAB = PrefabHolder.instance.TILE_DIFFICULT_PREFAB;
                terrain = "Sand";
                spritecode = "sand_terrain";
                break;

            case TileType.VeryDifficult:
                movementCost = 2;
                defenseBonus = 2;
                impassible = false;
                PREFAB = PrefabHolder.instance.TILE_VERY_DIFFICULT_PREFAB;
                terrain = "Soil";
                break;

            case TileType.Impassible:
                movementCost = 1;
                defenseBonus = 0;
                impassible = true;
                PREFAB = PrefabHolder.instance.TILE_IMPASSIBLE_PREFAB;
                terrain = "Hill";
                break;

            case TileType.Obstacle:
                movementCost = 1;
                impassible = false;
                PREFAB = PrefabHolder.instance.TILE_OBSTACLE_PREFAB;
                terrain = "Obstacle";
                break;

            default:
                movementCost = 1;
                defenseBonus = 0;
                impassible = false;
                PREFAB = PrefabHolder.instance.TILE_NORMAL_PREFAB;
                break;
        }

        generateVisuals();
    }

    public void generateVisuals()
    {
        GameObject container = transform.FindChild("Visuals").gameObject;
        //initially remove all children
        for (int i = 0; i < container.transform.childCount; i++)
        {
            Destroy(container.transform.GetChild(i).gameObject);
        }

        GameObject newVisual = (GameObject)Instantiate(PREFAB, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
        newVisual.transform.parent = container.transform;

        visual = newVisual;
    }


    public int fCost //tile F cost
    {
        get
        {
            return gCost + hCost;
        }
    }
}


