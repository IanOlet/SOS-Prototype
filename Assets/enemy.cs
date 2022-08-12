using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemy : MonoBehaviour
{
    protected float health;
    protected int damage;
    protected int speed;

    protected Rigidbody2D rb;

    protected GameObject player;

    protected ParticleSystem ps; //particle system, attached to a child so we can rotate it

    // Start is called before the first frame update
    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        //player = GameObject.FindGameObjectWithTag("Player");
        player = playerFlight.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0) //If the enemy has no health, run the appropriate method
            Die();
        Motion();
    }

    virtual protected void Motion()
    {

    }

    public void TakeDamage(float d, Quaternion direction) //Take damage from a source. We take the direction as well for the particle system.
    {
        health -= d;

        //Vector2 v = new Vector2(ps.transform.position.x, ps.transform.position.y);
        //ps.transform.rotation = Quaternion.Euler(0, 0, Vector2.Angle(v, direction)); //Should set the particles to emit based on the damage source
        //float a = Mathf.Atan2(direction.x-v.x, direction.y-v.y) * Mathf.Rad2Deg; //This should get the angle to line up the particle with the incoming damage
        //ps.transform.rotation = Quaternion.Euler(0, 0, a-180);

        //ps.transform.right = direction - new Vector2(transform.position.x, transform.position.y);

        ps.transform.rotation = direction; //New system, the particles are just in whatever direciton the projectile was facing
        
        ps.Play(); //Emit particles based on damage
    }

    virtual protected void Die()
    {
        Destroy(this.gameObject);
        //Maybe also particle effects
    }
}
