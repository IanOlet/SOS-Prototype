using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class nameGenerator : MonoBehaviour
{

    //Ship name component files
    public TextAsset standard;
    public TextAsset order;
    public TextAsset slime;
    public TextAsset elf;
    public TextAsset dwarf;
    public TextAsset fiend;

    string shipName;

    public TextMeshProUGUI wreck;
    public TextMeshProUGUI shipNameUI;

    // Start is called before the first frame update
    void Start()
    {
        makeName();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y)) //Debug, make a new name whenever the y key is pressed
            makeName();
    }

    string makeName()
    {
        switch (Random.Range(0, 10)) //Randomly pick ship name
        {
            case < 5: //Any result less than 5 results in a standard ship name, so they're more likely to be used
                //The files contain two lists of names. The lists are deliminated by a single semicolon, while individual entries are deliminated by commas
                string[] Stemp = standard.text.Split(";"); //Switch statements don't let me declare the same local variable in multiple cases. Current solution is to have different arrays for each case
                string[] Sfirst = Stemp[0].Split(",");
                string[] Ssecond = Stemp[1].Split(",");
                shipName = Sfirst[Random.Range(0, Sfirst.Length)] + " " + Ssecond[Random.Range(0, Ssecond.Length)]; //Adjective noun
                break;
            case 5: //Order
                string[] Otemp = order.text.Split(";");
                string[] Ofirst = Otemp[0].Split(",");
                string[] Osecond = Otemp[1].Split(",");
                shipName = Ofirst[Random.Range(0, Ofirst.Length)] + " of " + Osecond[Random.Range(0, Osecond.Length)]; //Noun of noun
                break;
            case 6: //Slime
                string[] Slimenames = slime.text.Split(","); //Slime names are a jumble of all possible words
                shipName = Slimenames[Random.Range(0, Slimenames.Length)] + " " + Slimenames[Random.Range(0, Slimenames.Length)]; //Adjective/noun adjective/noun
                break;
            case 7: //Elf
                string[] Etemp = elf.text.Split(";");
                string[] Efirst = Etemp[0].Split(",");
                string[] Esecond = Etemp[1].Split(",");
                shipName = Efirst[Random.Range(0, Efirst.Length)] + " in " + Esecond[Random.Range(0, Esecond.Length)]; //Noun in nouns
                break;
            case 8: //Dwarf
                string[] Dtemp = dwarf.text.Split(";");
                string[] Dfirst = Dtemp[0].Split(",");
                string[] Dsecond = Dtemp[1].Split(",");
                shipName = Dfirst[Random.Range(0, Dfirst.Length)] + " and " + Dsecond[Random.Range(0, Dsecond.Length)]; //Noun and noun
                break;
            case 9: //Fiend
                string[] Ftemp = fiend.text.Split(";");
                string[] Ffirst = Ftemp[0].Split(",");
                string[] Fsecond = Ftemp[1].Split(",");
                shipName = Ffirst[Random.Range(0, Ffirst.Length)] + " " + Fsecond[Random.Range(0, Fsecond.Length)]; //Noun adjective
                break;
        }
        Debug.Log(shipName);
        shipNameUI.text = shipName;
        return shipName;
    }
}
