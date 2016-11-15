using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class AIPlayer : Player
{
    private bool looted = false;
    public bool isAttacking = false;

    // Use this for initialization
    void Start()
    {
        kamibotAnimator = this.GetComponent<Animator>();
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
                BattleManager.instance.enemiesLeft--;
                AudioSource.PlayClipAtPoint(BattleManager.instance.dead, transform.position);
                eliminated = true;
            }
        }

        if (BattleManager.instance.players[BattleManager.instance.currentPlayerIndex] == this) //player's current turn?
        {
            //transform.GetComponent<Renderer>().material.color = Color.green; //paint him green
        }
        else { //if not
            transform.GetComponent<Renderer>().material.color = Color.white; //back to white
        }
        //^ this stuff above probably won't be used like this though. will be something like Advance Wars ^

        if (HP <= 0 & !looted)
        {
            BattleManager.instance.players[BattleManager.instance.currentPlayerIndex].exp += 3;
            looted = true;
        }

        base.Update();
    }

    //Do the turn events for the player i guess
    public override void TurnUpdate()
    {
        if (positionQueue.Count > 0) //waypoints are being manipulated here, being deleted when reached. Until there isn't any left (arrived at destination)
        {
            //transform.position += (positionQueue[0] - transform.position).normalized * moveSpeed * Time.deltaTime; //this code is shit
            transform.position = Vector3.Lerp(transform.position, positionQueue[0], 0.4f);
            characterisWalking();

            //make the enemy face the direction he's moving
            transform.LookAt(new Vector3(positionQueue[0].x, positionQueue[0].y, positionQueue[0].z));

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
        else
        {
            //priority queue
            List<Tile> attacktilesInRange = TileHighlight.FindHighlight(BattleManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], attackRange, true);
            //List<Tile> movementToAttackTilesInRange = TileHighlight.FindHighlight(BattleManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint + attackRange);
            List<Tile> movementTilesInRange = TileHighlight.FindHighlight(BattleManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint + 1000);
            //attack if in range and with lowest HP
            if (attacktilesInRange.Where(x => BattleManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = attacktilesInRange.Select(x => BattleManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? BattleManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).First();

                BattleManager.instance.removeTileHighlights();
                moving = false;
                attacking = true;
                BattleManager.instance.highlightTilesAt(gridPosition, Color.red, attackRange);

                //FindNearestPlayer();

                    BattleManager.instance.attackWithCurrentPlayer(BattleManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y]);
                    isAttacking = true;
                actionPoints--;
                

            }
            //move toward nearest attack range of opponent
            /*
            else if (!moving && movementToAttackTilesInRange.Where(x => BattleManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = movementToAttackTilesInRange.Select(x => BattleManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? BattleManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).ThenBy(x => x != null ? TilePathfinder.FindPath(BattleManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], BattleManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000).First();

                BattleManager.instance.removeTileHighlights();
                moving = true;
                attacking = false;
                BattleManager.instance.highlightTilesAt(gridPosition, Color.blue, movementPerActionPoint, false);

                List<Tile> path = TilePathfinder.FindPath(BattleManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], BattleManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y], BattleManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());
                BattleManager.instance.moveCurrentPlayer(path[(int)Mathf.Max(0, path.Count - 1 - attackRange)]);
            }
            */
            //move toward nearest opponent
            else if (!moving && movementTilesInRange.Where(x => BattleManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0).Count() > 0)
            {
                var opponentsInRange = movementTilesInRange.Select(x => BattleManager.instance.players.Where(y => y.GetType() != typeof(AIPlayer) && y.HP > 0 && y != this && y.gridPosition == x.gridPosition).Count() > 0 ? BattleManager.instance.players.Where(y => y.gridPosition == x.gridPosition).First() : null).ToList();
                Player opponent = opponentsInRange.OrderBy(x => x != null ? -x.HP : 1000).ThenBy(x => x != null ? TilePathfinder.FindPath(BattleManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], BattleManager.instance.map[(int)x.gridPosition.x][(int)x.gridPosition.y]).Count() : 1000).First();

                BattleManager.instance.removeTileHighlights();
                moving = true;
                attacking = false;
                BattleManager.instance.highlightTilesAt(gridPosition, new Color (0.5f, 0.5f, 1.0f, 0.5f), movementPerActionPoint, false); //color BLUE

                List<Tile> path = TilePathfinder.FindPath(BattleManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], BattleManager.instance.map[(int)opponent.gridPosition.x][(int)opponent.gridPosition.y], BattleManager.instance.players.Where(x => x.gridPosition != gridPosition && x.gridPosition != opponent.gridPosition).Select(x => x.gridPosition).ToArray());
                if (path.Count() > 1)
                {
                    List<Tile> actualMovement = TileHighlight.FindHighlight(BattleManager.instance.map[(int)gridPosition.x][(int)gridPosition.y], movementPerActionPoint, BattleManager.instance.players.Where(x => x.gridPosition != gridPosition).Select(x => x.gridPosition).ToArray());
                    path.Reverse();
                    if (path.Where(x => actualMovement.Contains(x)).Count() > 0) BattleManager.instance.moveCurrentPlayer(path.Where(x => actualMovement.Contains(x)).First());
                }
            }
        }

        //Analyze this thing below
        /*
        if (actionPoints <= 1 && (attacking || moving))
        {
            moving = false;      //force player's unability of moving when actionpoints are 0 or less?
            attacking = false;
            BattleManager.instance.nextTurn();  //just for test
        }
        */


        if (actionPoints <= 1 && !attacking) //basically means if you did all the actions you could, YOU SHALL PASS the turn
        {
            //actionPoints = 2; //resetting the action counter must me somewhere else
            moving = false;
            attacking = false;
            BattleManager.instance.nextTurn(); //next turn for another player (NOT VALID FOR PLAYER)
            //characterisInactive();
        }

        base.TurnUpdate();
    }

    /*
    public Transform FindNearestPlayer()
    {
        Transform nearestPlayer = null;
        float closestDistanceSqr = Mathf.Infinity;

        // sqrMagnitude: Returns the squared length of this vector
        // square also helps to not get a negative number, being kind of absolute
        foreach (Player potentialTarget in players)
        {
            Vector3 directionToTarget = potentialTarget.transform.position - transform.position;
            float distanceSqrToTarget = directionToTarget.sqrMagnitude; 

            if (distanceSqrToTarget < closestDistanceSqr)
            {
                closestDistanceSqr = distanceSqrToTarget;
                nearestPlayer = potentialTarget.transform;
            }
        }

        Debug.Log(nearestPlayer);
        return nearestPlayer;
    }
    */
}
