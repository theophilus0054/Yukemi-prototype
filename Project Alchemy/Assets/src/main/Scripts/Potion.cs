using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion
{
    public string potionName;
    public int potionID; // Unique identifier for the potion
    public Sprite potionSprite; // Sprite for the potion
    public List<string> ingredients; // List of ingredients required for this potion

    // Constructor with sprite and ingredients
    public Potion(string name, int id, Sprite sprite, List<string> ingredientsList)
    {
        potionName = name;
        potionID = id;
        potionSprite = sprite;
        ingredients = ingredientsList;
    }
}
