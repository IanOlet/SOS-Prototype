using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bobber : enemy
{
    float aggroRange; //How close the player has to be for the enemy to be active
    float maxHealth = 10;

    // Start is called before the first frame update
    protected override void Start()
    {
        health = maxHealth; //Set the values for the enemy.
        speed = 1;
        damage = 5;
        aggroRange = 20f;

        base.Start();
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
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.01f); //Reduce velocity in other directions, helps it change directions a bit better.
            rb.AddForce(transform.right * speed * .15f, ForceMode2D.Impulse); //Push the bobber in the direction it's facing
            rb.velocity = Vector2.ClampMagnitude(rb.velocity, 30f); //Adheres to the max speed
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.05f); //Slow down
            if (rb.velocity.magnitude < 0.1f) //If you're moving very slow then just stop
                rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player") //Bobbers deal damage on contact
        {
            playerHealth.instance.takeDamage(damage);
        }
    }
}
