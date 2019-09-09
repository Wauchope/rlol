using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RoomShape
{
    Rectangle
}

public class Room
{
    public bool toDestroy { get; private set; } = false;

    public Vector2 centre { get; private set; }

    private Room[] connectedRooms;

    private RoomShape shape;

    private List<Vector2> tilePositions;

    private int tileBuffer = 1;

    public Room()
    {
        SetupRoom();
    }

    private void SetupRoom()
    {
        shape = SelectShape();
        

        //If each tile can be added to the grid
        if (TryAddToGrid())
        {
            Debug.Log("Success: Created room with centre: " + centre);
            //Create all tiles
            PlaceTiles();
            int distanceFromOrigin = GetDistanceFromOrigin();
            SetColour(LevelGenerator.GetColourFromInt(distanceFromOrigin));
            //Select biome
            //Spawn events
            //Spawn loot
        }
        else
        {
            //Signal that the room should no longer be created
            toDestroy = true;
        }
    }

    //Attempt to add each tile to the grid
    private bool TryAddToGrid()
    {
        bool available = true;

        //Iterates over each tile which occupies this room
        foreach (Vector2 position in tilePositions)
        {
            //If any of the tiles are in a position that exists already
            if (LevelGrid.Instance.DoesTileExistAtPoint(position))
            {
                //Cancel the creation of the room
                available = false;
            }
        }

        return available;
    }

    private RoomShape SelectShape()
    {
        RoomShape shape = GetRandomEnum<RoomShape>();
        switch (shape)
        {
            case RoomShape.Rectangle:
                CreateRectangle();
                break;
            default:
                Debug.Log("Trying to create shape that does not exist. Something's seriously going wrong here.");
                break;
        }

        return shape;
    }

    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(Random.Range(0, A.Length));
        return V;
    }

    //Get position of each tile for shape
    private void CreateRectangle()
    {
        Vector2 bottomLeft = new Vector2(Mathf.FloorToInt(Random.Range(0, LevelGrid.Instance.size.x)), Mathf.FloorToInt(Random.Range(0, LevelGrid.Instance.size.y)));
        Vector2 topRight = bottomLeft + new Vector2(Mathf.FloorToInt(Random.Range(5, 10)), Mathf.FloorToInt(Random.Range(5, 10)));

        centre = (bottomLeft + topRight) / 2;

        tilePositions = new List<Vector2>();

        for (int y = (int) bottomLeft.y + tileBuffer; y < topRight.y - tileBuffer; y++)
        {
            for (int x = (int) bottomLeft.x + tileBuffer; x < topRight.x - tileBuffer; x++)
            {
                tilePositions.Add(new Vector2(x, y));
            }
        }
    }

    private void PlaceTiles()
    {
        foreach (Vector2 position in tilePositions)
        {
            LevelGenerator.Instance.CreateTile(Mathf.FloorToInt(position.x), Mathf.FloorToInt(position.y));
        }
    }

    public int GetDistanceFromOrigin()
    {
        return Mathf.FloorToInt(Vector2.Distance(centre, Vector2.zero));
    }

    public List<Vector2> GetTilePositions()
    {
        return tilePositions;
    }

    public void SetColour(Color colour)
    {
        foreach (Vector2 tilePosition in tilePositions)
        {
            LevelGrid.Instance.GetTileAtPoint(tilePosition).SetColour(colour);
        }
    }

    public Tile GetTileOnEdge()
    {
        List<Tile> edgeTiles;

        switch (shape)
        {
            case RoomShape.Rectangle:
                //Some way of selecting a specific edge
                edgeTiles = GetEdgeRectangle(GetRandomEnum<Direction>());
                break;
            default:
                edgeTiles = new List<Tile>();
                break;
        }
        return null;
    }

    private List<Tile> GetEdgeRectangle(Direction direction)
    {
        List<Tile> edgeTiles = new List<Tile>();

        return edgeTiles;
    }
}

