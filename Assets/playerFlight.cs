using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerFlight : MonoBehaviour //Controls player movement. The goal is to make the ship follow space physics, so it keeps all momentum and has to accelerate in different directions to turn
{
    public static playerFlight instance; //The player is a singleton

    bool accelerating = false; //Whether or not the ship is accelerating, used to change turn velocity
    float accelCooldown = 0f; //Cooldown on the acceleration boost that occurs when initally moving forward.
    float maxCooldown = 0.2f; //The amount of time that needs to pass before the acceleration boost cools down.

    float accelForce = 15f; //How much force to apply when accelerating.
    float maxSpeed = 50f; //The limit to speed.

    float turnSpeed = 140f; //The turning speed, in degrees. When not accelerating, the player turns twice as fast. Used to be 180, changing to see if it helps accuracy

    TrailRenderer tr; //A trailrenderer childed to this object

    Rigidbody2D r; //This object's rigidbody

    bool aimMode = false; //Whether the player is aiming carefully by pressing shift or right click

    private void Awake() //Used to make a singleton for the player
    {
        if (instance != null)
            Destroy(gameObject);
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody2D>();
        tr = GetComponentInChildren<TrailRenderer>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(Input.GetMouseButton(1) || Input.GetKey(KeyCode.LeftShift)) //Turn on aimmode if either right mouse or left shift is pressed
        {
            aimMode = true;
        }
        else //Turn off aimmode otherwise
        {
            aimMode = false;
        }

        Motion();
    }

    private void Motion() //Handles all motion for the ship.
    {
        //VERY IMPORTANT! Ideally these deadzones should be configurable. At the very least test with a controller to see if they're good.
        if (Input.GetAxis("Vertical") > 0.5f)
        {
            if (accelCooldown >= maxCooldown && !accelerating) //The first moment you accelerate in a direction, you get a boost while also decreasing momentum in other directions.
            {
                r.velocity = Vector2.Lerp(r.velocity, Vector2.zero, 0.5f);
                r.AddForce(transform.right * accelForce, ForceMode2D.Impulse);
                accelCooldown = 0f;
                tr.widthMultiplier = 2f; //Bump up the trail size for boosts.
            }
            else //Otherwise just accelerate as normal, slightly reducing momentum from other directions.
            {
                r.velocity = Vector2.Lerp(r.velocity, Vector2.zero, 0.02f);
                r.AddForce(transform.right * accelForce);
            }
            accelerating = true;
            tr.emitting = true; //Turn on the trail

            if (tr.widthMultiplier > 1f) //The trail goes to a normal width as you accelerate
            {
                tr.widthMultiplier = Mathf.Lerp(tr.widthMultiplier, 1f, 0.1f);
            }
        }
        else //If you aren't accelerating
        {
            accelerating = false;

            tr.emitting = false; //No trail when not accelerating.
        }

        if (accelCooldown < maxCooldown) //The cooldown keeps going even if you're still accelerating
        {
            accelCooldown += Time.deltaTime;
        }

        if (Input.GetAxis("Vertical") < -0.2f) //Slow down. Braking shouldn't be as effective as retrograde acceleration for changing direction, mostly should just be convenience.
        {
            r.velocity = Vector2.Lerp(r.velocity, Vector2.zero, 0.05f);
            if (r.velocity.magnitude < 0.1f) //If you're moving very slow then just stop
                r.velocity = Vector2.zero;
        }

        if (Input.GetAxisRaw("Horizontal") < -0.1f) //Rotate left. Consider rotating slower if shooting.
        {
            //if(accelerating)
            //transform.Rotate(new Vector3(0f, 0f, turnSpeed * Time.deltaTime));
            //else

            r.angularVelocity = 0f; //Reset angular velocity if you start turning.
            if(aimMode)
                transform.Rotate(new Vector3(0f, 0f, turnSpeed * 0.8f * Time.deltaTime)); //Turn slower in aimmode
            else
                transform.Rotate(new Vector3(0f, 0f, turnSpeed * 2f * Time.deltaTime));
        }
        if (Input.GetAxisRaw("Horizontal") > 0.1f) //Rotate right.
        {
            //if(accelerating)
            //transform.Rotate(new Vector3(0f, 0f, turnSpeed * -1f * Time.deltaTime));
            //else

            r.angularVelocity = 0f;
            if(aimMode)
                transform.Rotate(new Vector3(0f, 0f, turnSpeed * -0.8f * Time.deltaTime));
            else
                transform.Rotate(new Vector3(0f, 0f, turnSpeed * -2f * Time.deltaTime));
        }

        r.velocity = Vector2.ClampMagnitude(r.velocity, maxSpeed); //Adheres to the max speed
    }
}
