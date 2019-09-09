using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ObjectPool : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> activePool;
    [SerializeField]
    private List<GameObject> inactivePool;

    // Start is called before the first frame update
    void Start()
    {
        //Perhaps call in Awake()
        //Initialize the pool lists (avoids null reference error)
        activePool = new List<GameObject>();
        inactivePool = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Change so it returns the created object? might be useful
    public GameObject CreateObject(GameObject objectToCreate, Vector3 position)
    {
        GameObject foundObject;
        foundObject = SearchForObjectInPool(objectToCreate, inactivePool);
        //If an object of the requested type does not exist in the object pool
        if (foundObject == null)
        {

            //Create the new object
            foundObject = Instantiate(objectToCreate, position, Quaternion.identity, transform);
            //Name the new object
            foundObject.name = "Enemy";
            //Set it to be active in the heirarchy
            foundObject.SetActive(true);
            //Add the object to the active pool
            activePool.Add(foundObject);
        }
        else
        {
            //Remove the found object from the inactive pool and adds it to the active pool
            inactivePool.Remove(foundObject);
            activePool.Add(foundObject);
            //Moves the object to the given Vector3 position
            foundObject.transform.position = position;
            //Sets the object to active in the heirarchy 
            foundObject.SetActive(true);

            //Sets the objects start position to the clicked position
            foundObject.GetComponent<NavMeshAgent>().Warp(foundObject.transform.position);
        }
        return foundObject;
    }

    public void ReleaseObject(GameObject objectToDisable)
    {
        if (SearchForObjectInPool(objectToDisable, activePool) != null)
        {
            //Swaps the pool that the object is in
            activePool.Remove(objectToDisable);
            inactivePool.Add(objectToDisable);
        }
        else
        {
            //An object was in the wrong pool, im doing something seriously wrong
            Debug.Log("Object is not currently active. WTF");
        }
    }

    private GameObject SearchForObjectInPool(GameObject objectToFind, List<GameObject> pool)
    {
        GameObject foundObject = null;
        Debug.Log(objectToFind.name);

        //Returns null if the given pool or the gameobject containing this script is empty.
        if (gameObject.transform.childCount == 0 || pool.Count == 0)
        {
            return foundObject;
        }

        //Iterates over each child object of the gameobject containing this script
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            //If object with same name is found in the given pool
            if (objectToFind.name == pool[i].name)
            {
                foundObject = pool[i];
                break;
            }
        }

        return foundObject;
    }
}
