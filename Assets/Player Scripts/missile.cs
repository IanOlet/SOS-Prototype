using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class missile : projectile
{
    public GameObject blastTemplate;
    public GameObject target; //Missiles lock on to the first enemy they encounter

    protected float blastRadius = 1f; //The radius of explosions created by the missile
    protected float seekStrength = 3f; //How fast missile can turn to seek a target
    protected float seekRadius = 7f;

    LayerMask enemyMask;

    private void Awake()
    {
        LayerMask.GetMask("Enemy"); //Get the layer for enemies for overlapCircle calls
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (target == null) //If no target found yet, look for nearby enemies
        {
            float targetDistance = seekRadius+0.01f;
            foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, seekRadius)) //Look for enemies within 3 units of the missile
            {
                if(c.tag == "Enemy" && Vector2.Distance(transform.position, c.transform.position) < targetDistance) //Check all nearby enemies to find which one is closest
                {
                    target = c.gameObject;
                    targetDistance = Vector2.Distance(transform.position, target.transform.position);
                }
            }
        }
        else //If target found, seek it out
        {
            //float targetAngle = Vector2.SignedAngle(transform.right, target.transform.position-transform.position); //Find the angle between the two
            //Quaternion tA = Quaternion.Euler(0, 0, targetAngle);
            //transform.rotation = Quaternion.Lerp(transform.rotation, tA, 0.2f * seekStrength);
            transform.right = Vector2.Lerp(transform.right, target.transform.position - transform.position, 0.008f * seekStrength); //New calculation, should cause missiles to gradually turn

            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, 0.2f); //Reduce velocity in other directions, helps it change directions a bit better.
            rb.AddForce(transform.right * speed * .15f, ForceMode2D.Impulse); //Accellerate
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

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy")) //Damage enemies that are hit
        {
            collision.GetComponent<enemy>().TakeDamage(damage, transform.rotation);
            GameObject b = Instantiate(blastTemplate, transform.position, transform.rotation); //Create the visual of the blast template
            Destroy(b, 0.2f); //Destroy the blast template
        }
        foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, blastRadius)) //Explode on a hit, damaging all enemies in radius
        {
            if(c.tag == "Enemy")
                c.GetComponent<enemy>().TakeDamage(damage);
        }
        piercing -= 1; //Decrement piercing
        if (piercing <= 0) //If piercing runs out, delete the projectile
            Destroy(this.gameObject); //Delete the projectile
    }
}
