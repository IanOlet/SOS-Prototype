using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testDummy : enemy //For testing purposes
{
    float waitTime = 0; //Time until it reappears

    protected override void Start()
    {
        waitTime = 0;
        health = 100;
        rb = GetComponent<Rigidbody2D>();
        ps = GetComponentInChildren<ParticleSystem>();
        //player = GameObject.FindGameObjectWithTag("Player");
        player = playerFlight.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0 && waitTime == 0)
            Die();
        else if (health <=0)
        {
            waitTime += Time.deltaTime;
            if(waitTime > 2f)
            {
                GetComponent<SpriteRenderer>().enabled = true;
                GetComponent<BoxCollider2D>().enabled = true;
                health = 100;
                waitTime = 0;
            }
        }
        Motion();
    }

    protected override void Die()
    {
        GetComponent<SpriteRenderer>().enabled = false; //The test dummy disappears for a while before respawning
        GetComponent<BoxCollider2D>().enabled = false;
        waitTime += Time.deltaTime;
    }
}
