using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class weaponController : MonoBehaviour
{

    public weapon[] unusedWeapon; //The "empty" weapon scripts that will be filled out on weapon creation.
    int nextWeapon; //The index of the next weapon to be created
    List<weapon> weaponList;

    public Button wep1; //The UI buttons for the weapon options
    public Button wep2;
    public Button wep3;
    int chosenweapon = 1; //The index of the chosen weapon
    bool choosing = false; //If the player is choosing a weapon

    weapon[] newWeapons; //An array of the three weapon options for choosing new ones

    float bonusDamage = 0; //All stat bonuses, applied to each weapon at the start of every level.
    float bonusFirerate = 0;
    float bonusShotspeed = 0;
    float bonusAccuracy = 0;
    int bonusPiercing = 0;

    public GameObject[] projectiles; //All possible projectile prefabs
    public GameObject basicBullet;
    //0 is basic bullet

    public weapon basic;

    // Start is called before the first frame update
    void Start()
    {
        weaponList = new List<weapon>();
        newWeapons = new weapon[3];

        unusedWeapon = GetComponents<weapon>(); //Gets references to all waiting weapon scripts
        unusedWeapon[0].statSet(1, 20, 20, 5, 1, projectiles[0]); //The player always starts with a basic gun.
        nextWeapon = 1; //The player's second weapon will eventually be unusedWeapon[1]
        weaponList.Add(unusedWeapon[0]);
        applyBonuses();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!choosing && (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))) //Fire when either mouse1 or space are pressed, while not choosing an item
        {
            foreach(weapon w in weaponList)
            {
                w.Fire();
            }
        }
    }

    public void CreateWeapons(int budget) //Present a choice of three weapons with randomly generated stats within the given minimum budget
    {
        choosing = true;
        for(int x = 0; x < 3; x++)
        {
            //Set up random choice of weapon type once you have more.
            //Stats are damage, firerate, shotspeed, accuracy, and piercing
        }
    }

    public void ChooseWeapon(int x) //Choose a weapon, with the parameter's value being the index of the chosen weapon
    {

    }

    public void applyBonuses() //Updates the stat bonuses on all weapons, should be called at the start of every stage or whenever bonuses are changed otherwise
    {
        foreach(weapon w in weaponList)
        {
            w.bonusSet(bonusDamage, bonusFirerate, bonusShotspeed, bonusAccuracy, bonusPiercing);
            //Use the alternate bonusSet that specifies type afterward to add conditional bonus damage from items
        }
    }
}
