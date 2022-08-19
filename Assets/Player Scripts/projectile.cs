using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectile : MonoBehaviour
{
    protected Rigidbody2D rb;

    protected float damage;
    protected float speed;
    protected int piercing;

    protected float lifetime = 5;

    //explosive as a bool
    //explosion prefab to create when destroyed

    // Start is called before the first frame update
    void Start()
    {
        if(rb == null) //Set reference to the rigidbody if giveStats didn't do it first
            rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    protected virtual void FixedUpdate()
    {
        //rb.velocity = transform.right * speed; //Set speed. Will have to do something different for accelerating projectiles
        lifetime -= Time.deltaTime;
        if (lifetime <= 0) //After a few seconds delete the projectile
            Destroy(this.gameObject);
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag.Equals("Enemy")) //Damage enemies that are hit
        {
            //collision.GetComponent<enemy>().TakeDamage(damage, new Vector2(transform.position.x, transform.position.y));
            collision.GetComponent<enemy>().TakeDamage(damage, transform.rotation);
        }
        piercing -= 1; //Decrement piercing
        if (piercing <= 0) //If piercing runs out, delete the projectile
            Destroy(this.gameObject); //Delete the projectile
    }

    public void giveStats(float d, float s, int p, Vector2 v)
    {
        if (rb == null) //Set reference to the rigidbody if needed
            rb = GetComponent<Rigidbody2D>();
        damage = d;
        speed = s; //Is this necessary?
        piercing = p;
        rb.velocity = (transform.right * speed); //Set velocity
        rb.velocity += (v); //Projectiles inherit velocity
        //Consider having it inheret less velocity
    }
}
