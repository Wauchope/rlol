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

    private List<Node> nodes;
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

    private void OnDrawGizmos()
    {
        if (Application.isPlaying)
        {
            //foreach (Node node in nodes)
            for (int i = 0; i < nodes.Count - 2; i++)
            {
                Node node = nodes[i];
                Vector2 pos = node.GetPosition();
                Gizmos.color = Color.magenta;
                Gizmos.DrawSphere(new Vector3(pos.x * tileWidth, 0, pos.y * tileWidth), 10f);
                if (node.GetConnections().Count != 0)
                {
                    foreach (NodeConnection connection in node.GetConnections())
                    {
                        Node[] points = connection.GetNodes();
                        Vector3 worldPosA = new Vector3(points[0].GetPosition().x * tileWidth, 0, points[0].GetPosition().y * tileWidth);
                        Vector3 worldPosB = new Vector3(points[1].GetPosition().x * tileWidth, 0, points[1].GetPosition().y * tileWidth);
                        Gizmos.color = Color.cyan;
                        Gizmos.DrawLine(worldPosA, worldPosB);
                    }
                }
            }
        }

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
        CreateLevel();

        //3 WITH ROOMS
        //CreateRooms(25);
    }

    public void GenerateNewLevel()
    {
        grid.ClearGrid();

        nodes = new List<Node>();

        CreateStartArea(3, 3);
        CreateLevel();
    }

    private void CreateLevel()
    {
        CreateNodes(25, 10);
        CreateNodeConnections();

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

    //Create Nodes
    private void CreateNodes(int maxNodes, int buffer)
    {
        nodes = new List<Node>();
        nodes.Add(new Node(new Vector2(0, 0)));
        nodes.Add(new Node(new Vector2(grid.size.x, grid.size.y)));

        for (int i = 0; i < maxNodes; i++)
        {
            int x = Mathf.FloorToInt(Random.Range(5, grid.size.x));
            int y = Mathf.FloorToInt(Random.Range(5, grid.size.y));

            Vector2 newNodePos = new Vector2(x, y);

            bool failed = false;

            if (nodes.Count != 0)
            {
                foreach (Node node in nodes)
                {
                    if (node.GetDirectDistance(newNodePos) <= buffer)
                    {
                        failed = true;
                        break;
                    }
                }
            }

            if (!failed)
            {
                Node newNode = new Node(newNodePos);
                nodes.Add(newNode);
                Debug.Log(newNodePos);
            }
        }
    }

    private void CreateNodeConnections()
    {
        List<NodeConnection> newConnections = new List<NodeConnection>();

        Node[] nodesToConnect;

        //for each node
        for (int i = 0; i < nodes.Count; i++)
        {
            nodesToConnect = new Node[2] { nodes[1], nodes[2] };

            //compare the distance of each node with the nodes stored in nodesToConnect
            for (int j = 0; j < nodes.Count - i - 1; j++)
            {
                // remainingNodes[i] is the current node
                // remainingNodes[j] is the node being compared

                bool actionTaken;
                
                if (nodes[i].GetDistance(nodes[j]) < nodes[i].GetDistance(nodesToConnect[0]))
                {
                    nodesToConnect[0] = nodes[j];
                    actionTaken = true;
                }
                else if (nodes[i].GetDistance(nodes[j]) < nodes[i].GetDistance(nodesToConnect[1]))
                {
                    nodesToConnect[1] = nodes[j];
                    actionTaken = true;
                }
                

            }

            //add NodeConnections from nodesToConnect to connectionsToAdd

            for (int z = 0; z < nodesToConnect.Length; z++)
            {
                NodeConnection outwardConnection = new NodeConnection(nodes[i], nodesToConnect[z]);
                NodeConnection inwardConnection = new NodeConnection(nodesToConnect[z], nodes[i]);

                //check if connectionsToAdd doesnt contain the connection or its inverse
                if (!newConnections.Contains(outwardConnection) || !newConnections.Contains(inwardConnection))
                {
                    nodes[i].AddConnection(outwardConnection);
                    newConnections.Add(outwardConnection);
                }
            }
        }
    }

    private void CreateMinimumSpanningTree()
    {

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

    //Bubble sort algorithm
    private List<Node> SortNodesListByDistance(Vector2 control, List<Node> nodesToSort)
    {
        int n = nodesToSort.Count;

        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                Node node1 = nodesToSort[j];
                Node node2 = nodesToSort[j + 1];

                if (/*nodesToSort[j].GetDistance(control) < nodesToSort[j + 1].GetDistance(control)) && */
                    nodesToSort[j].GetDirectDistance(control) < nodesToSort[j + 1].GetDirectDistance(control))
                //if (nodes[j].GetPosition().x < nodes[j + 1].GetPosition().x || nodes[j].GetPosition().y < nodes[j + 1].GetPosition().y)
                //if (Mathf.Pow(Mathf.Pow(node1.GetPosition().x, 2) + Mathf.Pow(node1.GetPosition().y, 2), 0.5f) < Mathf.Pow(Mathf.Pow(node2.GetPosition().x, 2) + Mathf.Pow(node2.GetPosition().y, 2), 0.5f))
                {
                    
                    Node temp = nodesToSort[j];
                    nodesToSort[j] = nodesToSort[j + 1];
                    nodesToSort[j + 1] = temp;
                }
            }
        }

        foreach (Node node in nodesToSort)
        {
            Debug.Log(node.GetDistance(new Vector2(0, 0)));
        }

        return nodesToSort;
    }
}