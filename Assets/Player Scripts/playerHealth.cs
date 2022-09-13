using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class playerHealth : MonoBehaviour
{
    public static playerHealth instance; //PlayerHealth is a singleton

    float currentHealth;
    float maxHealth = 100; //Player starts with 100 health

    public TextMeshProUGUI healthText;
    public TextMeshProUGUI maxHealthText;
    public Slider healthBar;

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
        currentHealth = maxHealth; //Set current health at the start
        healthText.text = ""+currentHealth; //Set up health UI
        maxHealthText.text = ""+maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void scrapHeal(float h) //When scrapping items, increase current and max health by the heal value. Consider adding less to max health.
    {
        maxHealth += h;
        currentHealth += h;
        healthText.text = "" + currentHealth; //Set up health UI again
        maxHealthText.text = "" + maxHealth;
        healthBar.maxValue = maxHealth;
        healthBar.value = currentHealth;
    }

    public void takeDamage(float d)
    {
        currentHealth -= d; //Take damage

        healthText.text = "" + currentHealth; //Update UI
        healthBar.value = currentHealth;

        if (currentHealth <= 0f)
        {
            //When all health is gone the player is destroyed.
            Destroy(this.gameObject); //For now, just delete the gameobject.
            //Add code to offer restarting or going back to the menu, displaying score, etc.
        }
    }
}
