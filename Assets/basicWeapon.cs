using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class basicWeapon : weapon
{

    //Obsolete script, now every weapon uses the weapon script with the projectile being set by weaponController
    //This means we only need 5 weapon scripts on the player instead of a script for every weapon type.

    //projectile should be BasicBullet

    void Start()
    {
        damage = 1;
        fireRate = 20;
        piercing = 1; //Testing for now
        accuracy = 5;
        shotSpeed = 10;
    }
}
