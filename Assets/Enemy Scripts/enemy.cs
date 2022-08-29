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
    //protected ParticleSystem.ShapeModule pShape; //the shape module for the particle system.

    // Start is called before the first frame update
    virtual protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        player = playerFlight.instance.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (health <= 0) //If the enemy has no health, run the appropriate method
            Die();
        else
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

        //The old system didn't look right, a shot moving towards an enemy but hitting their side creates particles out the opposite side instead of in the direction of motion.

        ps.transform.rotation = direction; //New system, the particles are just in whatever direciton the projectile was facing

        rb.AddForce(direction * Vector2.right * d, ForceMode2D.Impulse); //Knockback the enemy based on damage taken.
        
        ps.Play(); //Emit particles based on damage
    }

    public void TakeDamage(float d) //Alternate TakeDamage for directionless damage, such as explosions.
    {
        health -= d;

        //pShape.shapeType = ParticleSystemShapeType.Sphere; //Switch to a sphere shape for particles

        ps.Play(); //Emit particles based on damage

        //pShape.shapeType = ParticleSystemShapeType.Cone; //Return to standard cone shape
    }

    virtual protected void Die()
    {
        //pShape.shapeType = ParticleSystemShapeType.Sphere;
        ps.Play(); //Scatters particles in all directions on death

        Destroy(this.gameObject);
        //Maybe also particle effects
    }
}
