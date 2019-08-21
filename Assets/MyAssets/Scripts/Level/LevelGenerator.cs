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
    [SerializeField] public GameObject tileObject;
    [SerializeField] private GameObject walkerObject;

    NavMeshSurface navMeshSurface;

    int tileWidth = 25;
    LevelGrid grid;

    // Start is called before the first frame update
    void Start()
    {
        navMeshSurface = GetComponentInParent<NavMeshSurface>();
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
        grid = CreateGrid(50, 50);
        CreateStartArea(3, 3);

        StartCoroutine(CreateLevel());
    }

    private IEnumerator CreateLevel()
    {
        for (int i = 0; i < 10; i++)
        {
            CreateRandomBranch(out Walker walker);
            yield return new WaitWhile(() => walker.walking);
        }
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

        //Implement pathfinding
        //Make walker place tiles

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
        LevelGrid.Instance.GetRandomTile(out Vector2 goalPosition);

        //3c
        walker.StartWalking(startPosition, goalPosition, tileWidth);
    }

    //4
    private void BuildNavMesh()
    {
        if (navMeshSurface != null)
        {
            navMeshSurface.BuildNavMesh();
        }
    }
}