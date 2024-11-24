using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
    public string potionName;
    public int potionID; // Unique identifier for the potion (if needed)

    public Potion(string name, int id)
    {
        potionName = name;
        potionID = id;
    }
}
