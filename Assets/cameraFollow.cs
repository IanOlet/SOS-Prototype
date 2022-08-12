using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    //Simple script to get the camera to follow the player. May be updated or replaced eventually.

    private Transform player; //The player, the target for the camera to follow.
    Vector2 pos; //The position of the camera
    float zoom = -10f; //The z position of the camera, change it to change the zoom. Default is -10f.
    private Vector3 v = Vector3.zero; //Velocity of the camera, used in damping. Must be 3d to preserve the camera's zoom
    Camera c;

    // Start is called before the first frame update
    void Start()
    {
        c = GetComponent<Camera>();
        player = playerFlight.instance.transform;
        pos = transform.position;
    }

    // Update is called once per frame
    void FixedUpdate() //The whole time the only problem was not having FixedUpdate
    {
        pos = transform.position;

        Vector3 point = c.WorldToViewportPoint(player.position);
        Vector3 delta = player.position - c.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z)); //The 0.5fs here refer to screen position, so it's the middle of the screen.
        Vector3 destination = transform.position + delta;
        transform.position = Vector3.SmoothDamp(transform.position, destination, ref v, 0.15f); //Increasing the float at the end here makes the camera follow slower.
    }
}
