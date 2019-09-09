using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NonPlayerCharacter
{
    // Start is called before the first frame update
    void Start()
    {
        //Init
        agent = GetComponent<NavMeshAgent>();
        agent.Warp(gameObject.transform.position);

        restPosition = gameObject.transform.position;

        player = Player.Instance;
        health = GetComponent<Health>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        FollowPlayer();
    }


    //Checks if the player is in the radius of the nonplayer character
    public void FollowPlayer(float searchRadius = 25)
    {

        if (Vector3.Distance(player.transform.position, gameObject.transform.position) < searchRadius)
        {
            SetPath(player.transform.position);
        }
        else
        {
            SetPath(restPosition);
        }
    }

    //Allows the enemy to use triggers
    public override void TriggerEnter(Collider other)
    {
        //If the trigger is the players sword
        if (other.CompareTag("PlayerSword"))
        {
            //If the player is in an attack animation
            //Create custom method returning bool to simplify
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordSlash_01"))
            {
                //Take damage
                health.TakeDamage(player.GetDamage());

                //If health is below or equal to zero
                if (health.CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }
    }
}
