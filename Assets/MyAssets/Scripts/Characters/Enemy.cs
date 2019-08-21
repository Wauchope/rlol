using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : NonPlayerCharacter
{
    // Start is called before the first frame update
    void Start()
    {
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
        TargetPlayer();
    }

    public override void TriggerEnter(Collider other)
    {
        if (other.CompareTag("PlayerSword"))
        {
            if (player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("SwordSlash_01"))
            {
                health.TakeDamage(player.GetDamage());

                if (health.CurrentHealth <= 0)
                {
                    Die();
                }
            }
        }
    }
}
