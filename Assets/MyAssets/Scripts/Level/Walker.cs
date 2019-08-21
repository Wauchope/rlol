using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Walker : MonoBehaviour
{
    private Vector2 targetPos;

    private List<Vector2> visited;

    private int tileWidth;

    public bool walking { get; private set; } = false;

    int yOffset = 5;

    [SerializeField]
    private int age = 0;
    private int lifespan = 500;

    float fuzziness = 0.5f;
    Vector2 fuzziness_vector;

    int maxNeighbours = 3;
    float maxNeighbourDeathChance = 10f;


    private Vector2 currentGridPos;
    public Vector2 CurrentGridPos
    {
        get
        {
            return currentGridPos;
        }
        private set
        {
            currentGridPos = new Vector2(Mathf.Clamp(value.x, 0, LevelGrid.Instance.size.x - 1), Mathf.Clamp(value.y, 0, LevelGrid.Instance.size.y - 1));
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        fuzziness_vector = new Vector2(fuzziness, fuzziness);
    }

    // Update is called once per frame
    void Update()
    {
        if (walking)
        {
            TakeStep();
        }

        Age();
    }

    private void FixedUpdate()
    {

    }

    private void Age()
    {
        if (age < lifespan)
        {
            age++;
        }
        else Die();

        //Check neighbours - if 3 exist then die
        /*
        if (CheckNeighbourCount() > maxNeighbours)
        {
            if (Random.Range(0f, 100f) < maxNeighbourDeathChance)
            {
                Die();
            }
        }*/
    }

    private void Die()
    {
        walking = false;
        Destroy(gameObject);
    }

    private int CheckNeighbourCount()
    {
        return LevelGrid.Instance.GetNeighbourCount(CurrentGridPos);
    }

    public void StartWalking(Vector2 start, Vector2 target, int tileWidth)
    {
        CurrentGridPos = start;
        targetPos = target;
        this.tileWidth = tileWidth;

        Debug.Log(targetPos);

        visited = new List<Vector2>();
        walking = true;
    }

    public void TakeStep()
    {
        bool doStep = false;

        Vector2 nextPos = GetRandomAdjacentTilePos();

        nextPos = new Vector2(Mathf.Clamp(nextPos.x, 0, LevelGrid.Instance.size.x), Mathf.Clamp(nextPos.y, 0, LevelGrid.Instance.size.y));

        if ((fuzziness_vector + nextPos - targetPos).magnitude <= (CurrentGridPos - targetPos).magnitude)
        {
            doStep = true;
        }

        if (!visited.Contains(nextPos) && doStep)
        {
            visited.Add(nextPos);

            transform.position = new Vector3(nextPos.x, 0, nextPos.y) * tileWidth;
            if (!LevelGrid.Instance.DoesTileExistAtPoint(nextPos))
            {
                Tile tile = Instantiate(GetComponentInParent<LevelGenerator>().tileObject, transform.position, Quaternion.identity, transform.parent).GetComponent<Tile>();
                LevelGrid.Instance.AddPosToGrid(nextPos, tile);
            }
            transform.position += Vector3.up * yOffset;
            CurrentGridPos = nextPos;
        }

        if (CurrentGridPos == targetPos)
        {
            Debug.Log("Target Found");
            walking = false;
        }
    }

    public Vector2 GetRandomAdjacentTilePos()
    {
        //Add check against level grid
        Vector2 randomPosition;
        switch (Random.Range(0, 4))
        {
            //y+1
            case 0:
                randomPosition = CurrentGridPos + Vector2.up;
                break;
            case 1:
                randomPosition = CurrentGridPos + Vector2.right;
                break;
            case 2:
                randomPosition = CurrentGridPos + Vector2.down;
                break;
            case 3:
                randomPosition = CurrentGridPos + Vector2.left;
                break;
            default:
                randomPosition = Vector2.zero;
                break;
        }

        return randomPosition;
    }
}
