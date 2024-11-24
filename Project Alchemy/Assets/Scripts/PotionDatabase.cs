using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PotionDatabase : MonoBehaviour
{
    // Singleton instance of PotionDatabase
    public static PotionDatabase Instance;

    // List of all possible potions (this could include names, types, etc.)
    public List<Potion> potionList = new List<Potion>();

    // List of corresponding sprites for each potion
    public Sprite[] potionSprites;

    void Awake()
    {
        // Ensure only one instance exists and it is accessible globally
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy any other instances
        }

        // Optionally, you can populate the potionList here based on the available sprites or other data
        PopulatePotionList();
    }

    void PopulatePotionList()
    {
        // For demonstration, create some sample potions
        potionList.Add(new Potion("Restorative Remedy", 0));
        potionList.Add(new Potion("Mixture of Mana", 1));
        potionList.Add(new Potion("Salve of Strength", 2));
        potionList.Add(new Potion("Teacher's Tonic", 3));
        potionList.Add(new Potion("Eclipse Elixir", 4));
        potionList.Add(new Potion("Liquid Lie", 5));
        potionList.Add(new Potion("Acidic Afterburn", 6));
        potionList.Add(new Potion("Combustive Concoction", 7));
        potionList.Add(new Potion("Potion of Poison", 8));
        potionList.Add(new Potion("Paralyzing Philter", 9));
        potionList.Add(new Potion("Draught of Dreams", 10));
        potionList.Add(new Potion("Blinding Brew", 11));
        // You can add more potions or populate from external data, if needed
    }
}

