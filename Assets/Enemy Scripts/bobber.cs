using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobber : enemy
{
    float aggroRange; //How close the player has to be for the enemy to be active
    bool active = false; //If the bobber has seen the player
    float maxHealth = 10;

    // Start is called before the first frame update
    protected override void Start()
    {
        health = maxHealth; //Set the values for the enemy.
        speed = 1;
        damage = 1;
        aggroRange = 20f;

        rb = GetComponent<Rigidbody2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        player = playerFlight.instance.gameObject;
    }

    protected override void Motion()
    {
        if (Vector2.Distance(transform.position, player.transform.position) <= aggroRange || health < maxHealth) //If close to the player or if taking damage, become active
        {
            active = true;
        }
        if (active) //After seeing the player once
        {
            transform.right = player.transform.position - transform.position; //Face the player
            rb.AddForce(transform.right * speed * 0.005f, ForceMode2D.Impulse); //Push the bobber in the direction it's facing
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 30f); //Adheres to the max speed
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.05f); //Slow down
            if (rb.velocity.magnitude < 0.1f) //If you're moving very slow then just stop
                rb.velocity = Vector2.zero;
        }
    }
}
