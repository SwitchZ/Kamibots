using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class TilePathfinder : MonoBehaviour {
    public static List<Tile> FindPath(Tile originTile, Tile destinationTile)
    {
        return FindPath(originTile, destinationTile, new Vector2[0]);
    }

    public static List<Tile> FindPath(Tile originTile, Tile destinationTile, Vector2[] occupied) //A* pathfinding thingy
    {
        List<Tile> closed = new List<Tile>(); //tiles evaluated (visited/checked)
        List<TilePath> open = new List<TilePath>(); //tiles to be evaluated

        TilePath originPath = new TilePath();
        originPath.addTile(originTile); //contains now the start tile

        open.Add(originPath); //add the list with start tile to OPEN

        while (open.Count > 0)
        {
            //open = open.OrderBy(x => x.costOfPath).ToList();
            TilePath current = open[0]; //current = node in OPEN with the lowest f_cost
            open.Remove(open[0]); //remove current from OPEN

            if (closed.Contains(current.lastTile))
            {
                continue;
            }
            if (current.lastTile == destinationTile) //current is the destination tile: path has been found
            {
                current.listOfTiles.Distinct(); //returns distinct elements from the path-list
                current.listOfTiles.Remove(originTile); //exclude start tile from the path
                return current.listOfTiles; //return the path to be followed
            }

            closed.Add(current.lastTile); //add current to CLOSED

            foreach (Tile neighbor in current.lastTile.neighbors) //for each neighbor of CURRENT node
            {
                if (neighbor.impassible || occupied.Contains(neighbor.gridPosition)) continue; //if tile is impassible OR neighbor in occupied(skip tiles occupied by players?)
                TilePath newTilePath = new TilePath(current);
                newTilePath.addTile(neighbor); 
                open.Add(newTilePath); //add the neighbor to open
            }
        }
        return null;
    }

    static public List<Tile> AStarPathfinding(Tile originTile, Tile destinationTile)
    {
        List<Tile> openSet = new List<Tile>();
        List<Tile> closedSet = new List<Tile>();
        openSet.Add(originTile);

        while (openSet.Count > 0)
        {
            ///getting the lowest F cost node on the open list
            Tile currentTile = openSet[0];  // currentTile = lowest F cost node on the open list

            for (int i = 1; i < openSet.Count; i++) 
            {
                if (openSet[i].fCost < currentTile.fCost || openSet[i].fCost == currentTile.fCost && openSet[i].hCost < currentTile.hCost)
                {
                    currentTile = openSet[i];
                }
            }
            ///

            openSet.Remove(currentTile);
            closedSet.Add(currentTile);

            if (currentTile == destinationTile) //current is the target node?
            {
                // TODO: Do the retrace shit before the return?
                // or even a return RetracePath();
                print("Returned the path:");
                Debug.Log(RetracePath(originTile, destinationTile));
                RetracePath(originTile,destinationTile);

            } 


            foreach (Tile neighbour in currentTile.neighbors){
                if (!neighbour.impassible || closedSet.Contains(neighbour)) continue; //skip to the next neighbor

                int newMovementCostToNeighbour = currentTile.gCost + GetDistance(currentTile, neighbour);

                //if new path to neighbour is shorter OR neighbour is not in OPEN
                if (newMovementCostToNeighbour < neighbour.gCost || !openSet.Contains(neighbour))
                {
                    neighbour.gCost = newMovementCostToNeighbour;
                    neighbour.hCost = GetDistance(neighbour, destinationTile);
                    //The fCost property of the tile will do the job of storing the F cost of that neighbour
                    neighbour.parent = currentTile;


                    Debug.Log(neighbour.gCost);
                    Debug.Log(neighbour.hCost);
                    Debug.Log(neighbour.parent);

                    if (!openSet.Contains(neighbour))
                        openSet.Add(neighbour);
                }
            }
        }

        print("Null path");
        return RetracePath(originTile, destinationTile);
    }

    static List<Tile> RetracePath(Tile originTile, Tile destinationTile)
    {
        List<Tile> path = new List<Tile>();
        Tile currentTile = destinationTile;

        while (currentTile != originTile)
        {
            path.Add(currentTile);
            currentTile = currentTile.parent;
        }
        path.Reverse();

        return path;

    }

    static int GetDistance(Tile tileA, Tile tileB)
    {
        int dstX = (int) Mathf.Abs(tileA.gridPosition.x - tileB.gridPosition.x);
        int dstY = (int) Mathf.Abs(tileA.gridPosition.y - tileB.gridPosition.y); 

        return 10 * dstX + 10 * dstY; //it's always going to be 10. Why did i even bother to keep this logic?

        /* unused stuff
        if (dstX > dstY)
            return 14 * dstY + 10 * (dstX - dstY);
        return 14 * dstX + 10 * (dstY - dstX);
        */
    }
}
