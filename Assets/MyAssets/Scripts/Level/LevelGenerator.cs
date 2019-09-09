using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum Direction
{
    NORTH=0, EAST=1, SOUTH=2, WEST=3
}

public class LevelGenerator : MonoBehaviour
{
    public static LevelGenerator Instance;

    [SerializeField] public GameObject tileObject;
    [SerializeField] private GameObject walkerObject;

    private NavMeshSurface navMeshSurface;

    private List<Room> rooms;

    private int tileWidth = 25;
    private LevelGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface = GetComponentInParent<NavMeshSurface>();
        rooms = new List<Room>();


        if (Instance == null)
        {
            Instance = this;
        }

        GenerateLevel();

    }

    // Update is called once per frame
    void Update()
    {

    }

    private void GenerateLevel()
    {
        //1.  Create grid of size (gridSizeX, gridSizeY)
        //2.  Create start area of size (startSizeX, startSizeY)
        //LOOP START
        //3a. Create a "walker" at edge of grid
        //3b. Select edge of existing area
        //3c. Pathfind to selected edge
        //3d. Place tile at each point on the grid that's been "walked"
        //3e. Destroy walker
        //LOOP END
        //4. Build NavMesh
        



        //1
        grid = CreateGrid(100, 100);
        //2
        CreateStartArea(3, 3);

        //3 & 4
        StartCoroutine(CreateLevel());

        //3 WITH ROOMS
        //CreateRooms(25);
    }

    private IEnumerator CreateLevel()
    {
        CreateRooms(25);
        //3
        for (int i = 0; i < 10; i++)
        {
            CreateRandomBranch(out Walker walker);
            yield return new WaitWhile(() => walker.walking);
        }

        //4
        BuildNavMesh();

    }

    //1
    private LevelGrid CreateGrid(int x, int y)
    {
        return new LevelGrid(new Vector2(x, y));
    }

    //2

    private void CreateStartArea(int startSizeX, int startSizeY)
    {
        //If the start area is the same size as or larger than the grid
        if (startSizeX - grid.size.x >= 0 || startSizeY - grid.size.y >= 0)
        {
            //Return an error
            Debug.Log("Start area too large. Aborting.");
            return;
        }
        else
        {
            //Create the spawn area
            for (int y = 0; y < startSizeY; y++)
            {
                for (int x = 0; x < startSizeX; x++)
                {
                    Vector3 tilePos = new Vector3(x, 0, y) * tileWidth;

                    Tile tile = Instantiate(tileObject, tilePos, Quaternion.identity, transform).GetComponent<Tile>();
                    if (tile != null)
                    {
                        grid.AddPosToGrid(new Vector2(x, y), tile);
                    }
                }
            }
        }
    }

    //3
    private void CreateRandomBranch(out Walker walker)
    {
        //3a
        walker = Instantiate(walkerObject).GetComponent<Walker>();
        walker.transform.parent = transform;

        //3b
        List<Vector2> freePositions = new List<Vector2>();

        //Selects a random empty position next to the tile.
        int selectedIndex = Random.Range(0, freePositions.Count);
        while (freePositions.Count == 0)
        {
            //Debug.Log("Error: No Free Sides.");
            grid.GetRandomEdgeTile(2, out freePositions);
        }

        Vector2 startPosition = freePositions[selectedIndex];

        //Sets goal position to random tile on edge of grid
        //grid.GetRandomTileOnEdgeOfGrid(out Vector2 goalPosition);

        //Sets goal position to random tile on grid
        grid.GetRandomTile(out Vector2 goalPosition);

        //3c
        walker.StartWalking(startPosition, goalPosition, tileWidth);
    }

    //LOOP START WITH ROOMS
    //a. CREATE ROOMS IN GRID RANDOMLY
    //b. ENSURE ROOMS DONT OVERLAP
    //   REMOVE ROOMS WHICH DO OVERLAP
    //   PLCAE ROOM TILES
    //
    //c. ITERATE OVER ROOMS
    //   calculate distance from start
    //d. create node link from start to closest room
    //REPEAT
    //e. pathfind from room to next closest rooms
    //Once all rooms linked up
    //
    //LOOP END

    //3 WITH ROOMS
    private void CreateRooms(int maxRooms)
    {
        for (int i = 0; i < maxRooms; i++)
        {
            Room newRoom = TryCreateRoom();

            if (newRoom != null)
            {
                rooms.Add(newRoom);
            }
        }
        Debug.Log(rooms.Count);
    }

    private Room TryCreateRoom()
    {
        Room newRoom = new Room();
        if (newRoom.toDestroy)
        {
            newRoom = null;
        }

        return newRoom;
    }

    //4
    private void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
    }

    public int GetTileWidth()
    {
        return tileWidth;
    }

    public Tile CreateTile(int x, int y)
    {
        Vector3 tilePos = new Vector3(x, 0, y) * tileWidth;

        Tile tile = Instantiate(tileObject, tilePos, Quaternion.identity, transform).GetComponent<Tile>();
        if (tile != null)
        {
            grid.AddPosToGrid(new Vector2(x, y), tile);
        }
        return tile;
    }

    public static Color GetColourFromInt(int value)
    {
        return Color.Lerp(Color.red, Color.blue, value / LevelGrid.Instance.size.x);

    }
}