using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelGrid
{
    public static LevelGrid Instance;

    public Vector2 size { get; private set; }
    //Allows referencing a tile's id (int) by providing its position (int[,])
    private Dictionary<Vector2, Tile> pointDict;
    
    

    public LevelGrid(Vector2 size)
    {
        this.size = size;
        pointDict = new Dictionary<Vector2, Tile>();
        Instance = this;
    }

    private void CreateGrid()
    {
        for (int xPos = 0; xPos < size.x; xPos++)
        {
            for (int yPos = 0; yPos < size.y; yPos++)
            {
                Vector2 position = new Vector2(xPos, yPos);
                if (isPosOnEdgeOfMap(position))
                {

                    //pointDict.Add(position, null);
                }
            }
        }
    }

    private bool isPosOnEdgeOfMap(Vector2 position)
    {
        //Only allows for rectangular maps
        if (position.x == 0 || position.y == 0 || position.x == size.x || position.y == size.y)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool IsPosAValidEdge(Vector2 position, int maxFreeSides, out List<Vector2> freePositions)
    {
        //Check adjacent positions, see if they contain a tile
        //If n (num edges) > maxFreeSides return false
        //else return true

        int numFreeSides = 0;
        freePositions = new List<Vector2>();

        //For each adjacent tile (includes corners)
        for (int x = (int) position.x - 1; x <= (int) position.x + 1; x++)
        {
            for (int y = (int) position.y - 1; y <= (int) position.y + 1; y++)
            {
                //If the position to check isnt at a diagonal to the given position
                //Excludes the corners
                if (Mathf.Approximately(Mathf.Abs(x), Mathf.Abs(y)))
                {
                    Vector2 posToCheck = new Vector2(x, y);
                    //Checks the tile dictionary for a tile at point x, y
                    pointDict.TryGetValue(posToCheck, out Tile tile);

                    //If no tile is found
                    if (tile == null)
                    {
                        numFreeSides++;
                        //Adds the grid position of the empty space to a list
                        freePositions.Add(posToCheck);
                    }
                    //else Debug.Log("tile found");
                }
            }
        }

        if (numFreeSides > 0 && numFreeSides <= maxFreeSides)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void AddPosToGrid(Vector2 position, Tile tile)
    {
        pointDict.Add(position, tile);
    }

    //Occasionally returns a tile 1 square away from existing edge
    public Tile GetRandomEdgeTile(int maxFreeSides, out List<Vector2> freePositions)
    {

        freePositions = new List<Vector2>();

        if (pointDict.Count > 0)
        {
            //Get random tile and check if its a valid edge

            foreach (Vector2 key in RandomKeys(pointDict).Take(1))
            {
                if (!IsPosAValidEdge(key, maxFreeSides, out freePositions))
                {
                    pointDict.TryGetValue(key, out Tile tile);
                    return tile;
                }
            }
        }


        return null;
    }

    public Tile GetRandomTile(out Vector2 tilePos)
    {
        tilePos = new Vector2(Random.Range(0, (int) size.x), Random.Range(0, (int) size.y));
        pointDict.TryGetValue(tilePos, out Tile tile);
        return tile;
    }

    public Tile GetRandomTileOnEdgeOfGrid(out Vector2 tilePos)
    {
        // 0=North 1=East 2=South 3=West
        int side = Random.Range(0, 4);

        switch (side)
        {
            //y is max
            case 0:
                tilePos = new Vector2(Random.Range(0, (int)size.x), size.y);
                break;
            //x is max
            case 1:
                tilePos = new Vector2(size.x, Random.Range(0, (int)size.y));
                break;
            //y is min
            case 2:
                tilePos = new Vector2(Random.Range(0, (int)size.x), 0);
                break;
            //x is min
            case 3:
                tilePos = new Vector2(0, Random.Range(0, (int)size.y));
                break;
            default:
                tilePos = Vector2.zero;
                return null;
        }

        pointDict.TryGetValue(tilePos, out Tile tile);
        return tile;
    }

    public bool DoesTileExistAtPoint(Vector2 point)
    {
        if (pointDict.ContainsKey(point))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public int GetNeighbourCount(Vector2 position)
    {
        int neighbours = 0;

        for (int x = (int)position.x - 1; x <= (int)position.x + 1; x++)
        {
            for (int y = (int)position.y - 1; y <= (int)position.y + 1; y++)
            {
                pointDict.TryGetValue(new Vector2(x, y), out Tile tile);
                if (tile != null)
                {
                    neighbours++;
                }
            }
        }

        return neighbours;
    }

    //IMPORTED FROM https://stackoverflow.com/questions/1028136/random-entry-from-dictionary
    public IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        Random rand = new Random();
        List<TValue> values = Enumerable.ToList(dict.Values);
        int size = dict.Count;
        while (true)
        {
            yield return values[Random.Range(0, size)];
        }
    }

    public IEnumerable<TKey> RandomKeys<TKey, TValue>(IDictionary<TKey, TValue> dict)
    {
        Random rand = new Random();
        List<TKey> values = Enumerable.ToList(dict.Keys);
        int size = dict.Count;
        while (true)
        {
            yield return values[Random.Range(0, size)];
        }
    }
    //ENDIMPORT
}
