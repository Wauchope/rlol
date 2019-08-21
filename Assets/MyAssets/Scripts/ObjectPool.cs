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
        GameObject foundObject = SearchForObjectInPool(objectToCreate, inactivePool);
        if (foundObject == null)
        {
            GameObject __newObject = Instantiate(objectToCreate, position, Quaternion.identity, transform);
            __newObject.name = "Enemy";
            __newObject.SetActive(true);
            activePool.Add(__newObject);
        }
        else
        {
            inactivePool.Remove(foundObject);
            activePool.Add(foundObject);
            foundObject.transform.position = position;
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
            activePool.Remove(objectToDisable);
            inactivePool.Add(objectToDisable);
        }
        else
        {
            Debug.Log("Object is not currently active. WTF");
        }
    }

    private GameObject SearchForObjectInPool(GameObject objectToFind, List<GameObject> pool)
    {
        GameObject foundObject = null;
        Debug.Log(objectToFind.name);

        if (gameObject.transform.childCount == 0 || pool.Count == 0)
        {
            return foundObject;
        }

        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Debug.Log(pool[i].name);
            //If object with same name is found AND found object is not active in the hierarchy
            if (objectToFind.name == pool[i].name)
            {
                foundObject = pool[i];
                break;
            }
        }

        return foundObject;
    }
}
