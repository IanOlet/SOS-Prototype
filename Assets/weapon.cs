using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class weapon : MonoBehaviour
{
    private GameObject projectilePrefab; //The prefab for the weapon's projectile
    private Rigidbody2D parentRB;

    protected string name; //The name of the weapon
    protected string[] weaponTags; //The list of tags for the weapon, such as kinetic, missile, energy, etc

    protected float fireRate = 1; //base firerate
    protected float damage = 1; //base damage
    protected float shotSpeed = 1; //base projectile speed
    protected float accuracy = 1; //base maximum inaccuracy, in degrees
    protected int piercing = 1; //base piercing
    protected float bonusFireRate = 0; //firerate added by items
    protected float bonusDamage = 0; //damage added by items
    protected float bonusShotSpeed = 0; //projectile speed added by items
    protected float bonusAccuracy = 0; //accuracy added by items (better bonus accuracy should be negative)
    protected int bonusPiercing = 0; //piercing added by items

    float cooldown = 0; //The cooldown on the firerate.
    float spread = 0; //The weapon's spread
    float spreadcooldown = 0; //The cooldown on spread reduction
    float finalAccuracy; //Max inaccuracy after modifiers

    bool firing = false; //Test bool to directly fire weapons

    // Start is called before the first frame update
    void Start()
    {
        parentRB = GetComponentInParent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (cooldown > 0) //Cooldown always lowers.
            cooldown -= Time.deltaTime;
        if (spreadcooldown > 0) //Lower cooldown on spread reduction until it reaches 0
            spreadcooldown -= Time.deltaTime;
        else if (spread > 0) //Spread lowers over time, resets to 0 after a second of not firing
        {
            spread -= Time.deltaTime * finalAccuracy;
            if (spread < 0)
                spread = 0;
        }

        //TEST FUNCTION
        /*finalAccuracy = accuracy;
        if(Input.GetMouseButtonDown(0))
        {
            firing = true;
        }
        if(Input.GetMouseButtonUp(0))
        {
            firing = false;
        }
        if (firing)
            Fire();*/
    }

    public void Fire() //Fires the weapon by instantiating a projectile
    {
        if (cooldown <= 0)
        {
            Quaternion deviation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + Random.Range(-1*(spread/2),spread/2));
            projectile p = Instantiate(projectilePrefab, transform.position, deviation).GetComponent<projectile>(); //Create the projectile
            p.giveStats(damage + bonusDamage, shotSpeed + bonusShotSpeed, piercing + bonusPiercing, parentRB.velocity); //Give the projectile its stats
            cooldown = 1/(fireRate+bonusFireRate); //Reset the cooldown

            spreadcooldown = 0.1f; //Wait a tenth of a second to start reducing spread, so spread doesn't reduce during continual fire
            spread += finalAccuracy / (fireRate + bonusFireRate); //Will reach full spread after one second of firing
            if (spread > finalAccuracy) //Spread caps out at finalAccuracy
                spread = finalAccuracy;
            //Debug.Log(finalAccuracy / (fireRate+bonusFireRate));
        }
    }

    public void statSet(float d, float f, float s, float a, int p, GameObject projectilePre) //changes the stats of the weapon, used when initially creating
    {
        damage = d;
        fireRate = f;
        shotSpeed = s;
        accuracy = a;
        piercing = p;
        projectilePrefab = projectilePre;
    }

    public void bonusSet(float d, float f, float s, float a, int p) //Sets the total bonus damage of the weapon. This version is used for general damage modifiers applied to all weapons. Call this for all weapons at the start of every level, even if the bonus is 0 for both.
    {
        bonusDamage = d;
        bonusFireRate = f;
        bonusShotSpeed = s;
        bonusAccuracy = a;
        bonusPiercing = p;

        finalAccuracy = accuracy + bonusAccuracy;
        if (finalAccuracy < 0) //Check if accuracy bonuses push the degree deviation into the negatives, if so just set deviation to 0
            finalAccuracy = 0;
    }

    public void bonusSet(float d, float f, float s, float a, int p, string t) //Alternate bonus set, for specific weapon type bonuses. Adds the bonus damage to the base, so call this after the above bonusSet.
    {
        foreach(string tag in weaponTags) //If the weapon has the appropriate tag, apply the modifier.
        {
            if (tag.Equals(t))
            {
                bonusDamage += d;
                bonusFireRate += f;
                bonusShotSpeed += s;
                bonusAccuracy += a;
                bonusPiercing += p;
            }
        }
        finalAccuracy = accuracy + bonusAccuracy;
        if (finalAccuracy < 0) //Check if accuracy bonuses push the degree deviation into the negatives, if so just set deviation to 0
            finalAccuracy = 0;
    }



    
}
