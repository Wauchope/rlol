using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemyToMouse : MonoBehaviour
{
    [SerializeField]
    private GameObject ghostCursor;
    [SerializeField]
    private GameObject ghostObject;

    //The world position which the cursor is hovering over
    Vector3 hoverPos = new Vector3();
    //The screen position of the mouse cursor
    Vector2 mousePos = new Vector2();

    bool active;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (active)
        {
            if (Input.GetMouseButtonDown(0))
            {
                CreateGhostObject(ghostObject);
                ghostCursor.SetActive(false);
                active = false;
            }
        }
    }

    private void FixedUpdate()
    {
        if (active)
        {
            MoveGhostCursor();
        }
    }

    //Create ghost object
    //Have GhostObject follow cursor
    //OnMouseDown Place Enemy at Ghost Object position

    private void CheckCursorPosition()
    {        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            hoverPos = hit.point;
        }
        

        //Debug.Log(mousePos + "" + hoverPos);
    }

    private void MoveGhostCursor()
    {
        if (ghostCursor.activeInHierarchy)
        {
            CheckCursorPosition();
            ghostCursor.transform.position = hoverPos;
        }
    }


    private void CreateGhostObject(GameObject objectToCreate)
    {
        //Instantiate(objectToCreate, hoverPos, Quaternion.identity);
        GameObject newObject = GameManager.Instance.EnemyPool.CreateObject(objectToCreate, hoverPos);
    }

    public void ButtonPressed()
    {
        if (!active)
        {
            active = true;
            ghostCursor.SetActive(true);
        }
    }
}
