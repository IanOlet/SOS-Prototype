using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class director : MonoBehaviour
{

    public static director instance; //There is only one spawn director

    float budget = 0; //How much the director has saved up to spend on enemies
    float scaling = 0; //Increase to budget and spawn rates over time in a stage, at 1 per second.
    float level; //Modifies spawns based on how many levels have been completed
    float savingsGoal; //Randomly sets a goal for the director to save up to before spending.

    Coroutine spawnCo; //The coroutine for spawning enemies. Reference here is to cancel it early on level transition.

    //Enemy references
    public GameObject bobber; //50
    public GameObject blaster; //150
    public GameObject bombobber; //100
    public GameObject bishop; //500

    List<enemy> foeList; //The list of all enemies in the level. Used for cleanup.

    public TextMeshProUGUI debugBudgetText;

    private void Awake() //Used to make a singleton for the director
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
        foeList = new List<enemy>();
        levelReset();
    }

    // Update is called once per frame
    void FixedUpdate() //Making this fixed update so scaling doesn't increase when timescale is 0
    {
        budget += (10f + (scaling/10)) * Time.deltaTime * 5f; //At base, the director gets 10 points per second. 50 is enough for one basic enemy.
        debugBudgetText.text = budget+" / "+savingsGoal;
        scaling += Time.deltaTime; //Scaling increases over time.
        //scaling += Time.deltaTime * 5f; //Debug, increasing scaling a lot
        if(budget >= savingsGoal) //Once the savings goal is reached, spend budget until it's gone.
        {
            spawnCo = StartCoroutine(spawnEnemies());
            savingsGoal = Random.Range(150f + (4f * scaling), 450f + (5f * scaling)); //Set the new savings goal.
        }
    }

    void levelReset()
    {
        if(spawnCo != null)
            StopCoroutine(spawnCo);
        budget = 0;
        scaling = 0;
        savingsGoal = Random.Range(150f + (2f*scaling), 450f + (5f * scaling)); //Set the initial savings goal.
        foreach(enemy e in foeList)
        {
            Destroy(e.gameObject); //Clear all enemies
        }
        foeList.Clear();

        //Modify the level modifier here
    }

    IEnumerator spawnEnemies() //Spend the budget on enemies
    {
        Debug.Log("Budget of "+budget+" reached. Spawning batch.");

        float givenChance = 0.5f; //The given chance for any one option to be picked. Put here for easy adjustment. As a rule, spawn variety decreases as givenChance increases.

        string debugSpawnList = "Spawned ";

        while (budget >= 50) //Keep spawning until all budget is used, prioritizing more expensive choices. Current system is a 50% chance to take each option, from most to least expensive.
        {
            Vector3 spawnPos = playerFlight.instance.transform.position;
            Vector3 direction = Random.insideUnitCircle * spawnPos; //Get a direction relative to the player
            direction = direction.normalized;
            spawnPos = spawnPos + (direction * Random.Range(10f, 30f)); //Get a position between 10 and 40 units away from the player

            if(budget >= 1500 && Random.value > givenChance) //Bishop trio
            {
                budget -= 1500;
                for (int x = 0; x < 3; x++) //spawn 3 bishops. We use the same position to keep spawned groups together
                {
                    Vector2 spread = Random.insideUnitCircle * 2f; //Used to spread out groups a bit
                    enemy e = Instantiate(bobber, spawnPos + new Vector3(spread.x, spread.y), transform.rotation).GetComponent<enemy>();
                    e.directorSpawn();
                    foeList.Add(e);
                }
                debugSpawnList += "three bishops, ";
            }
            else if (budget >= 750 && Random.value > givenChance) //Five blasters
            {
                budget -= 750;
                for (int x = 0; x < 5; x++)
                {
                    Vector2 spread = Random.insideUnitCircle * 2f; //Used to spread out groups a bit
                    enemy e = Instantiate(bobber, spawnPos + new Vector3(spread.x, spread.y), transform.rotation).GetComponent<enemy>();
                    e.directorSpawn();
                    foeList.Add(e);
                }
                debugSpawnList += "five blasters, ";
            }
            else if (budget >= 500 && Random.value > givenChance) //Bishop
            {
                budget -= 500;
                enemy e = Instantiate(bishop, spawnPos, transform.rotation).GetComponent<enemy>();
                e.directorSpawn();
                foeList.Add(e);
                debugSpawnList += "a bishop, ";

            }
            else if (budget >= 500 && Random.value > givenChance) //Ten bobbers
            {
                budget -= 500;
                for (int x = 0; x < 10; x++)
                {
                    Vector2 spread = Random.insideUnitCircle * 2f; //Used to spread out groups a bit
                    enemy e = Instantiate(bobber, spawnPos+ new Vector3(spread.x, spread.y), transform.rotation).GetComponent<enemy>();
                    e.directorSpawn();
                    foeList.Add(e);
                }
                debugSpawnList += "ten bobbers, ";
            }
            else if (budget >= 500 && Random.value > givenChance) //Five bombobbers
            {
                budget -= 500;
                for (int x = 0; x < 5; x++)
                {
                    Vector2 spread = Random.insideUnitCircle * 2f; //Used to spread out groups a bit
                    enemy e = Instantiate(bobber, spawnPos + new Vector3(spread.x, spread.y), transform.rotation).GetComponent<enemy>();
                    e.directorSpawn();
                    foeList.Add(e);
                }
                debugSpawnList += "five bombobbers, ";
            }
            else if (budget >= 450 && Random.value > givenChance) //Three blasters
            {
                budget -= 450;
                for (int x = 0; x < 3; x++)
                {
                    Vector2 spread = Random.insideUnitCircle * 2f; //Used to spread out groups a bit
                    enemy e = Instantiate(bobber, spawnPos + new Vector3(spread.x, spread.y), transform.rotation).GetComponent<enemy>();
                    e.directorSpawn();
                    foeList.Add(e);
                }
                debugSpawnList += "three blasters, ";
            }
            else if (budget >= 250 && Random.value > givenChance) //Five bobbers
            {
                budget -= 250;
                for (int x = 0; x < 5; x++)
                {
                    Vector2 spread = Random.insideUnitCircle * 2f; //Used to spread out groups a bit
                    enemy e = Instantiate(bobber, spawnPos + new Vector3(spread.x, spread.y), transform.rotation).GetComponent<enemy>();
                    e.directorSpawn();
                    foeList.Add(e);
                }
                debugSpawnList += "five bobbers, ";
            }
            else if (budget >= 150 && Random.value > givenChance) //One blaster
            {
                budget -= 150;
                enemy e = Instantiate(blaster, spawnPos, transform.rotation).GetComponent<enemy>();
                e.directorSpawn();
                foeList.Add(e);
                debugSpawnList += "one blaster, ";
            }
            else if (budget >= 100 && Random.value > givenChance) //One bombobber
            {
                budget -= 100;
                enemy e = Instantiate(bombobber, spawnPos, transform.rotation).GetComponent<enemy>();
                e.directorSpawn();
                foeList.Add(e);
                debugSpawnList += "one bombobber, ";
            }
            else if (budget > 50) //One bobber
            {
                budget -= 50;
                enemy e = Instantiate(bobber, spawnPos, transform.rotation).GetComponent<enemy>();
                e.directorSpawn();
                foeList.Add(e);
                debugSpawnList += "one bobber, ";
            }
            yield return new WaitForSeconds(0.1f); //With coroutines, we can stagger the spawns.
        }

        Debug.Log(debugSpawnList);
    }
}
