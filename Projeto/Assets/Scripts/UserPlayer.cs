using UnityEngine;
using System.Collections;

public class UserPlayer : Player
{
    public int order = 0;

    public GameObject battleOptions;


    // Use this for initialization
    void Start()
    {
        /*
        moveButton = GameObject.Find("BattleCanvas/BattleOptions/MoveButton");
        attackButton = GameObject.Find("BattleCanvas/BattleOptions/AttackButton");
        waitButton = GameObject.Find("BattleCanvas/BattleOptions/WaitButton");
        */

        kamibotAnimator = this.GetComponent<Animator>();
        battleOptions = GameObject.Find("BattleOptions");
    }

    // Update is called once per frame
    public override void Update()
    {
        
        if (HP <= 0) //player is dead
        {
            characterisDead();
            //transform.GetComponent<Renderer>().material.color = Color.red; //paint it red (will be unused)

            if (!eliminated)
            {
                BattleManager.instance.alliesLeft--;
                AudioSource.PlayClipAtPoint(BattleManager.instance.dead, transform.position);
                eliminated = true;
            }
        }

        Player selectedPlayer = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex];
        if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex] == this) //player's current turn?
        {
            //transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.green; //paint him green
        }
        else { //if not
            //transform.GetChild(1).gameObject.GetComponent<Renderer>().material.color = Color.white; //back to white
        }
        //^ this stuff above probably won't be used like this though. will be something like Advance Wars ^


        //level up system
        if (((selectedPlayer.exp / Mathf.Pow(2, selectedPlayer.level)) * 100) >= 100)//if exp needed
        {
            selectedPlayer.exp -= (int)(Mathf.Pow(2, selectedPlayer.level)); // avoids exp not being accumulated for next level;
            selectedPlayer.level++;
            selectedPlayer.SpecsUp();
        } 
    

        base.Update();


        //idle or not
        if (actionPoints <= 0) characterisInactive();
        //else if (actionPoints > 0) characterisIdle();


        //battleover
        if(BattleManager.instance.currentTurn == BattleManager.Turn.Victory) kamibotAnimator.SetBool("battleIsOver", true);
    }

    //Do the turn events for the player i guess
    public override void TurnUpdate()
    {
        //highlight
        //
        //

        if (positionQueue.Count > 0) //waypoints are being manipulated here, being deleted when reached. Until there isn't any left (arrived at destination)
        {
            //transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime; //this code is shit
            transform.position = Vector3.Lerp(transform.position, positionQueue[0], 0.4f);
            characterisWalking();

            //make the enemy face the direction he's moving
            transform.LookAt(new Vector3(positionQueue[0].x, this.transform.position.y, positionQueue[0].z));

            Vector3 directionToTarget = positionQueue[0] - transform.position;
            float distanceSqrToTarget = directionToTarget.sqrMagnitude;

            if (distanceSqrToTarget <= 0.01f)
            {
                transform.position = positionQueue[0];
                positionQueue.RemoveAt(0);
                AudioSource.PlayClipAtPoint(BattleManager.instance.run, transform.position);
                if (positionQueue.Count == 0)
                {
                    actionPoints--; //1 action less the player can do.
                    battery--;
                    characterisIdle();
                }

            }
        }

        base.TurnUpdate();
    }

    public void MoveButton()
    {
        Player user = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex];
        if (!user.moving && user.actionPoints > 0 && user.battery > 0)
        {
            BattleManager.instance.removeTileHighlights();
            user.moving = true; //player will be moving
            user.attacking = false; //and not attacking
            BattleManager.instance.highlightTilesAt(user.gridPosition, new Color(0.5f, 0.5f, 1.0f, 0.5f), user.movementPerActionPoint, false); //BLUE COLOR. NEEDS CUSTOMIZATION FOR MY GAME


        }
        else
        {
            user.moving = false;
            user.attacking = false;
            BattleManager.instance.removeTileHighlights();


        }
    }

    public void AttackButton()
    {
        Player user = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex];
        if (!user.attacking && user.actionPoints > 0 && user.battery > 0 && user.shots > 0)
        {
            BattleManager.instance.removeTileHighlights();
            user.moving = false; //player will not be moving
            user.attacking = true; //but attacking
            BattleManager.instance.highlightTilesAt(user.gridPosition, new Color(1.0f,0.5f,0.5f,0.5f), user.attackRange); //NEEDS CUSTOMIZATION FOR MY GAME
            //BattleManager.instance.highlightAttackTilesAt(user.gridPosition, new Color(1.0f,0.5f,0.5f,0.5f), user.attackRange)


        }
        else
        {
            user.moving = false;
            user.attacking = false;
            BattleManager.instance.removeTileHighlights();

        }
    }

    public void WaitButton()
    {
        Player user = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex];

        BattleManager.instance.removeTileHighlights();
        user.actionPoints = 0;
        characterisInactive();
        user.moving = false; //player will not be moving
        user.attacking = false; //neither attacking, justing staying still
    }

    public void EndTurn()
    {
        Player user = BattleManager.instance.players[BattleManager.instance.currentPlayerIndex];

        BattleManager.instance.removeTileHighlights();
        for (int i = 0; i < BattleManager.instance.firstEnemyIndex; i++)
        {
            BattleManager.instance.players[i].actionPoints = 2; //reset action points for all players
            BattleManager.instance.players[i].characterisIdle();
        }

        user.moving = false; //player will not be moving
        user.attacking = false; //neither attacking, justing staying still
        BattleManager.instance.currentTurn = BattleManager.Turn.WaitingOpponent;
        BattleManager.instance.currentPlayerIndex = BattleManager.instance.firstEnemyIndex;
    }


    public void OnMouseDown()
    {
        if (BattleManager.instance.currentPlayerIndex < BattleManager.instance.firstEnemyIndex) //selecting player is only allowed in your turn!
        {
            if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].actionPoints == 0 ||
                BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].actionPoints == 2)
            {
                BattleManager.instance.removeTileHighlights();

                BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].moving = false; //on a player switch, it goes back to not moving
                BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].attacking = false; //and not attacking as well

                BattleManager.instance.currentPlayerIndex = this.order; //set the new active player
                AudioSource.PlayClipAtPoint(BattleManager.instance.beep, transform.position);

            }
        }

        if(BattleManager.instance.currentTurn == BattleManager.Turn.You && this.actionPoints > 0)
            HPBall.GetComponent<Renderer>().material.color = Color.white;
    }

    public override void SpecsUp()
    {
        baseDamageBase += 2;
        MaxHP += 2;
        MaxShots += 2;


    }
}   
