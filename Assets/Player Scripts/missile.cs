using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : projectile
{
    public GameObject blastTemplate;
    public GameObject target; //Missiles lock on to the first enemy they encounter

    protected float blastRadius = 1f; //The radius of explosions created by the missile
    protected float seekStrength = 3f; //How fast missile can turn to seek a target (3 is standard for missiles)
    protected float seekRadius = 7f; //7 is standard for missiles

    bool findingTarget = false; //Whether the target finding coroutine is already running

    LayerMask enemyMask;

    private void Awake()
    {
        LayerMask.GetMask("Enemy"); //Get the layer for enemies for overlapCircle calls
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (target != null)//If target found, seek it out
        {
            //float targetAngle = Vector2.SignedAngle(transform.right, target.transform.position-transform.position); //Find the angle between the two
            //Quaternion tA = Quaternion.Euler(0, 0, targetAngle);
            //transform.rotation = Quaternion.Lerp(transform.rotation, tA, 0.2f * seekStrength);
            transform.right = Vector2.Lerp(transform.right, target.transform.position - transform.position, 0.008f * seekStrength); //New calculation, should cause missiles to gradually turn

            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.2f); //Reduce velocity in other directions, helps it change directions a bit better.
            rb.AddForce(transform.right * speed * .15f, ForceMode2D.Impulse); //Accellerate
        }
        else //Otherwise find a target
        {
            if (!findingTarget)
            {
                StartCoroutine(acquireTarget());
                findingTarget = true;
            }
        }

        lifetime -= Time.deltaTime;
        if (lifetime <= 0) //After a few seconds delete the projectile
        {
            foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, blastRadius)) //Explode when it runs out, damaging all enemies in radius
            {
                if(c.tag == "Enemy")
                    c.GetComponent<enemy>().TakeDamage(damage);
            }
            GameObject b = Instantiate(blastTemplate, transform.position, transform.rotation); //Create blast template visual
            Destroy(b, 0.2f); //Destroy the blast template
            Destroy(this.gameObject); //Destroy the projectile
        }
    }

    IEnumerator acquireTarget() //Coroutine for finding targets
    {
        while (target == null)
        {
            float targetDistance = seekRadius + 0.01f;
            foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, seekRadius)) //Look for enemies within the seek radius of the missile
            {
                if (c.tag == "Enemy" && Vector2.Distance(transform.position, c.transform.position) < targetDistance) //Check all nearby enemies to find which one is closest
                {
                    target = c.gameObject;
                    targetDistance = Vector2.Distance(transform.position, target.transform.position);
                    findingTarget = false; //Target found, reset the bool for next time
                }
            }
            yield return new WaitForSeconds(0.2f); //Searches for targets every .2 seconds
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy")) //Damage enemies that are hit
        {
            collision.GetComponent<enemy>().TakeDamage(damage, transform.rotation);
            GameObject b = Instantiate(blastTemplate, transform.position, transform.rotation); //Create the visual of the blast template
            Destroy(b, 0.2f); //Destroy the blast template
        }
        foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, blastRadius)) //Explode on a hit, damaging all enemies in radius. Note that direct hits do double damage because of this.
        {
            if(c.tag == "Enemy")
                c.GetComponent<enemy>().TakeDamage(damage);
        }
        piercing -= 1; //Decrement piercing
        if (piercing <= 0) //If piercing runs out, delete the projectile
            Destroy(this.gameObject); //Delete the projectile
    }
}
