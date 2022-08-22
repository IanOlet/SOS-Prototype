using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bomb : projectile
{
    public GameObject blastTemplate; //The visual effect for explosions

    protected float blastRadius = 5f; //The radius of explosions created by the bomb
    protected float fuse = 2f; //How long until the bomb explodes
    protected Vector2 startVelocity; //The bomb's initial velocity, used to slow it down as the fuse runs out
    protected bool startVelSet = false; //If start velocity has been set

    SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
        lifetime = fuse;
        startVelocity = new Vector2(1000, 1000); //Set startvelocity
    }

    // Update is called once per frame
    protected override void FixedUpdate()
    {
        if (lifetime < fuse / 2)
        {
            if (!startVelSet) //Set the start velocity if it hasn't already
                startVelocity = rb.velocity;
            rb.velocity = Vector2.Lerp(startVelocity, Vector2.zero, Mathf.SmoothStep(0, 1f/2f, (fuse/2f)-lifetime)); //Bombs slow down after the fuse is halfway
        }

        lifetime -= Time.deltaTime;

        Color col = sr.color;
        col = new Color(col.r, col.g, ((fuse-lifetime)/fuse)); //Color approaches white as fuse runs out
        sr.color = col;

        if (lifetime <= 0) //After a few seconds delete the projectile and create an explosion
        {
            foreach (Collider2D c in Physics2D.OverlapCircleAll(transform.position, blastRadius)) //Explode when it runs out, damaging all enemies in radius
            {
                if (c.tag == "Enemy")
                    c.GetComponent<enemy>().TakeDamage(damage);
            }
            GameObject b = Instantiate(blastTemplate, transform.position, transform.rotation); //Create blast template visual
            Destroy(b, 0.2f); //Destroy the blast template
            Destroy(this.gameObject); //Destroy the projectile
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision) //Explode earlier if touched.
    {
        if (collision.tag == "Enemy")
        {
            if (lifetime > 0.3f)
                lifetime = 0.3f;
        }

    }
}
