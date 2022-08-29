using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bombobber : enemy //Same AI as a normal bobber, but it explodes if the player stays close for too long or on death
{
    float aggroRange; //How close the player has to be for the enemy to be active
    bool active = false; //If the bobber has seen the player
    float maxHealth = 10;

    float blastRadius; //The range of the explosion's damage effect
    float fuseRange; //How close the player has to be for the fuse to count down
    float maxFuse; //Maximum time until explosion
    float fuse; //Current time until explosion

    bool lit = false; //Whether the fuse is currently lit

    public GameObject blastTemplate; //The effect used for explosions

    SpriteRenderer sr;
    Color baseColor;

    Coroutine fuseCo; //The coroutine for the fuse. Used when cancelling the fuse coroutine

    // Start is called before the first frame update
    protected override void Start()
    {
        health = maxHealth; //Set the values for the enemy.
        speed = 1;
        damage = 1;
        aggroRange = 20f;

        blastRadius = 5;
        fuseRange = 8f;
        maxFuse = 1f;
        fuse = maxFuse;

        sr = GetComponent<SpriteRenderer>();
        baseColor = sr.color;

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
            if (Vector2.Distance(transform.position, player.transform.position) <= fuseRange) //Check if player is within fuse range
            {
                if (!lit) //If fuse isn't already lit, light it
                {
                    fuseCo = StartCoroutine(fuseCountdown());
                    lit = true;
                }
            }
            else //Cancel the fuse otherwise
            {
                if (lit)
                {
                    StopCoroutine(fuseCo);
                    lit = false;
                }
                if (fuse < maxFuse) //The fuse restores over time if not exploding
                {
                    fuse += Time.deltaTime;
                    sr.color = new Color(fuse / maxFuse, fuse / maxFuse, fuse / maxFuse); //Color reverts as fuse returns to normal
                    if (fuse > maxFuse)
                        fuse = maxFuse;
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

    protected override void Die()
    {
        fuseCo = StartCoroutine(fuseCountdown()); //If taking enough damage to die, start the fuse to explode
    }

    IEnumerator fuseCountdown()
    {
        while (fuse > 0) //Count the fuse down until it hits 0, upon which the bombobber explodes
        {
            fuse -= 0.1f;
            sr.color = new Color(fuse / maxFuse, fuse / maxFuse, fuse / maxFuse); //Color approaches black as fuse counts down
            if (fuse <= 0)
                explode();
            yield return new WaitForSeconds(0.1f);
        }
    }

    void explode()
    {
        foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, blastRadius)) //Explode, damaging anything in the radius
        {
            if (c.tag == "Enemy")
                c.GetComponent<enemy>().TakeDamage(damage);
            else if (c.tag == "Player")
                Debug.Log("Player in explosion. Replace this once damage is added");
        }
        GameObject b = Instantiate(blastTemplate, transform.position, transform.rotation); //Create blast template visual
        Destroy(b, 0.2f); //Destroy the blast template
        base.Die(); //Die normally
    }
}
