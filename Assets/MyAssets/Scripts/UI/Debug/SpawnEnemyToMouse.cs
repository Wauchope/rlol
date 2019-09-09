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
        //Creates a raycast from the camera to the mouse position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        //If the raycast ray collides with an object
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            //Set the hoverposition to the collision point
            hoverPos = hit.point;
        }
        

        //Debug.Log(mousePos + "" + hoverPos);
    }

    private void MoveGhostCursor()
    {
        //If the ghost cursor is active in the heirarchy
        if (ghostCursor.activeInHierarchy)
        {
            //Move the ghost object to the where the cursor is hovering
            CheckCursorPosition();
            ghostCursor.transform.position = hoverPos;
        }
    }


    private void CreateGhostObject(GameObject objectToCreate)
    {
        //Creates the selected object at the current mouse hover position
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
