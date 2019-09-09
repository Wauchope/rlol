using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NonPlayerCharacter : MonoBehaviour
{
    public CharacterData characterData;

    public NavMeshAgent agent;

    public Vector3 restPosition;

    public Vector3 goal;
    public Vector3 Goal
    {
        set
        {
            goal = value;
            agent.SetDestination(goal);
        }
    }

    public Player player;
    private bool playerInRange = false;

    public Health health;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

    }

    private void SetupCharacter()
    { 
        //Sets the max speed for the NavAgent using data from the scriptable object
        agent.speed = characterData.Speed;
        //Ensures the NavAgent reaches max speed (in theory) instantaneously
        agent.acceleration = agent.speed;
    }


    public void SetPath(Vector3 targetPos)
    {
        if (!(targetPos == goal))
        {
            Goal = targetPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerEnter(other);
    }

    public virtual void TriggerEnter(Collider other)
    {

    }

    public void Die()
    {
        gameObject.SetActive(false);
        GameManager.Instance.EnemyPool.ReleaseObject(gameObject);
    }

    private void OnEnable()
    {
        Respawn();
    }

    private void Respawn()
    {
        health = GetComponent<Health>();
        health.SetCurrentHealthPercentage(100);
    }

}
