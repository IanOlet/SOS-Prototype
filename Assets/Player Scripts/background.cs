using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class background : MonoBehaviour
{
    MeshRenderer mr; //The mesh renderer for the background quad
    Material material; //The material for the mesh
    GameObject player; //The player, used to get parallax

    float parallaxAmount = 0.001f; //Multiplies the player's position to make the parallax more subtle

    // Start is called before the first frame update
    void Start()
    {
        mr = GetComponent<MeshRenderer>();
        material = mr.material;
        player = playerFlight.instance.gameObject;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 offset = material.mainTextureOffset;

        offset = new Vector2(player.transform.position.x * parallaxAmount, player.transform.position.y * parallaxAmount); //Set the texture offset according to the player's position

        material.mainTextureOffset = offset; //Apply the offset
    }
}
