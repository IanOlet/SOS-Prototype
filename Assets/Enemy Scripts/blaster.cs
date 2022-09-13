using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class blaster : enemy
{
    public GameObject blasterProjectile; //The projectile the blaster will shoot

    float aggroRange; //How close the player has to be for the enemy to be active
    float shootRange; //How close the blaster has to be before it shoots
    float maxHealth = 10;
    float shootCooldown = 2f; //How many seconds between shots
    float cooling; //Current cooldown on shots
    float warmupTime = 1f; //How long the blaster waits until it shoots once the player is in range.
    float warmup = 0;

    float shotSpeed = 10f; //The speed of the enemy's projectile

    // Start is called before the first frame update
    protected override void Start()
    {
        health = maxHealth; //Set the values for the enemy.
        speed = 1;
        damage = 10;
        aggroRange = 20f; //Temporary, blasters will have higher aggro later
        shootRange = 20f;

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
            Vector2 v = Vector2.Lerp(transform.position, player.transform.position, 0.01f * speed); //Blasters will lerp towards the player, always staying some distance away
            transform.position = v;

            if (Vector2.Distance(transform.position, player.transform.position) <= shootRange) //If the cooldown is over and the player is in range, start warming up
            {
                warmup += Time.deltaTime; //Increment warmup
                if (warmup >= warmupTime && cooling >= shootCooldown) //Once the warmup time is over, shoot whenever the cooldown is ready
                {
                    cooling = 0;
                    projectile p = Instantiate(blasterProjectile, transform.position, transform.rotation).GetComponent<projectile>(); //Create the projectile
                    p.giveStats(damage, shotSpeed, 1, rb.velocity); //Give stats to the enemy projectile (Blasters don't have piercing)
                }
            }
            else
            {
                warmup = 0; //If the player isn't in range, reset the warmup
            }
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.05f); //Slow down
            if (rb.velocity.magnitude < 0.1f) //If you're moving very slow then just stop
                rb.velocity = Vector2.zero;
        }

        if (cooling < shootCooldown) //Cool down between shots
            cooling += Time.deltaTime;
    }
}
