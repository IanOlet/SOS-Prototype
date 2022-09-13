using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bishop : enemy
{
    public GameObject bishopProjectile; //The projectile the blaster will shoot
    LineRenderer lr;

    float aggroRange; //How close the player has to be for the enemy to be active
    float maxHealth = 50;
    float warmupTime = 3f; //Time before the bishop fires once it starts
    float warmup = 0;

    int jumps = 0; //Bishops move in three jumps between firing.
    float jumpProgress = 0; //Used to keep track of how far into the lerp the bishop is
    Vector2 jumpDestination; //Where the bishop is jumping to
    Vector2 oldDestination; //Where the bishop is jumping from
    float jumpTimer = 0; //How long it's been since last jump
    float jumpCooldown = 0.5f; //Time between jumps
    bool betweenJump = true; //If the bishop is between jumps

    Vector3[] laserEnds;

    float shotSpeed = 50f; //The speed of the enemy's projectile

    // Start is called before the first frame update
    protected override void Start()
    {
        health = maxHealth; //Set the values for the enemy.
        speed = 1;
        damage = 20;
        aggroRange = 20f; //Temporary, blasters will have higher aggro later
        
        lr = GetComponent<LineRenderer>();
        lr.positionCount = 2; //Only 2 vertices in our line, the bishop and where it's facing
        laserEnds = new Vector3[2];
        lr.enabled = false;

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
            if (warmup <= warmupTime - 0.5f)
            {
                transform.right = player.transform.position - transform.position; //Track the player until a half second before it fires.
                laserEnds[1] = player.transform.position;
            }
            if (jumps < 3)
            {
                if (betweenJump)
                {
                    if (jumpTimer < jumpCooldown)
                    {
                        jumpTimer += Time.deltaTime;
                    }
                    else
                    {
                        betweenJump = false;
                        int xMod = 1;
                        int yMod = 1;
                        if (Random.value < 0.5f) //Coinflip to decide if it goes left or right
                            xMod = -1;
                        if (Random.value < 0.5f) //Coinflip to decide if it goes up or down
                            yMod = -1;
                        jumpDestination = new Vector2(transform.position.x + (5 * xMod), transform.position.y + (5 * yMod)); //Set the random destination
                        oldDestination = transform.position;
                        jumpProgress = 0; //Reset jump progress
                    }
                }
                else
                {
                    if(jumpProgress < 1)
                    {
                        transform.position = Vector2.Lerp(oldDestination, jumpDestination, Mathf.SmoothStep(0, 1, jumpProgress)); //Smoothly jumps from A to B
                        jumpProgress += Time.deltaTime;
                    }
                    else
                    {
                        jumps += 1; //Increment jumps and set up for next jump
                        betweenJump = true;
                        jumpTimer = 0;
                        rb.velocity = Vector2.zero; //Stop moving between jumps
                    }
                }
            }
            else
            {
                rb.velocity = Vector2.zero; //Stop moving while firing
                lr.enabled = true; //Turn on the attack indicator
                laserEnds[0] = transform.position; //Laser starts on the bishop
                
                lr.SetPositions(laserEnds);
                if (warmup < warmupTime)
                {
                    warmup += Time.deltaTime;
                    lr.endWidth = (10 - (10 * (warmup / warmupTime)));
                }
                else
                {
                    projectile p = Instantiate(bishopProjectile, transform.position, transform.rotation).GetComponent<projectile>();
                    p.giveStats(damage, shotSpeed, 10, Vector2.zero); //Bishop projectiles don't inherit velocity since bishops stand still when firing anyway
                    jumps = 0; //Go back to jumping
                    betweenJump = true;
                    jumpTimer = -1f; //Extra time before the first jump
                    lr.enabled = false; //Turn off the indicator
                    warmup = 0;
                }
            }
            
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.05f); //Slow down
            if (rb.velocity.magnitude < 0.1f) //If you're moving very slow then just stop
                rb.velocity = Vector2.zero;
        }
    }
}
