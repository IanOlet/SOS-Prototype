using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobber : enemy
{
    float aggroRange; //How close the player has to be for the enemy to be active

    // Start is called before the first frame update
    protected override void Start()
    {
        health = 10; //Set the values for the enemy.
        speed = 1;
        damage = 1;
        aggroRange = 20f;

        rb = GetComponent<Rigidbody2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        player = playerFlight.instance.gameObject;
    }

    protected override void Motion()
    {
        if(Vector2.Distance(transform.position, player.transform.position) > aggroRange) //If out of aggro range, slow down
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.05f);
            if (rb.velocity.magnitude < 0.1f) //If you're moving very slow then just stop
                rb.velocity = Vector2.zero;
        }
        else
        {
            //Vector2 v = new Vector2(transform.position.x, transform.position.y);
            //float a = Mathf.Atan2(player.transform.position.x - v.x, player.transform.position.y - v.y) * Mathf.Rad2Deg; //Get the degree angle to face the player
            //transform.rotation = Quaternion.Euler(0, 0, a); //Bobbers always face the player if in range.
            transform.right = player.transform.position - transform.position; //Face the player if in range
            rb.AddForce(transform.right * speed * 0.005f, ForceMode2D.Impulse); //Push the bobber in the direction it's facing
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 30f); //Adheres to the max speed
        }
    }
}
