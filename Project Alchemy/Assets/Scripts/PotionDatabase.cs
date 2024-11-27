using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class PotionDatabase : MonoBehaviour
{
    // Singleton instance of PotionDatabase
    public static PotionDatabase Instance;

    // List of all possible potions (this could include names, types, etc.)
    public Potion[] potionList;

    // Array of corresponding sprites for each potion
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

        // Initialize potion data and recipes
        PopulatePotionList();
    }

    // Initialize the potion list with static values, including ingredient lists
    void PopulatePotionList()
    {
        potionList = new Potion[]
        {
            new Potion("Restorative Remedy", 0, potionSprites[0], new List<string>{
                "Purity Blossom", "Dragon's Blood"
            }),
            new Potion("Mixture of Mana", 1, potionSprites[1], new List<string>{
                "Southern Star Silt", "Faerie Dust"
            }),
            new Potion("Salve of Strength", 2, potionSprites[2], new List<string>{
                "Southern Star Silt", "Dragon's Blood"
            }),
            new Potion("Teacher's Tonic", 3, potionSprites[3], new List<string>{
                "Purity Blossom", "Southern Star Silt"
            }),
            new Potion("Eclipse Elixir", 4, potionSprites[4], new List<string>{
                "Purity Blossom", "Fragment of Night"
            }),
            new Potion("Liquid Lie", 5, potionSprites[5], new List<string>{
                "Forbidden Fruit", "Faerie Dust"
            }),
            new Potion("Acidic Afterburn", 6, potionSprites[6], new List<string>{
                "Forbidden Fruit", "Fragment of Night"
            }),
            new Potion("Combustive Concoction", 7, potionSprites[7], new List<string>{
                "Forbidden Fruit", "Dragon's Blood"
            }),
            new Potion("Potion of Poison", 8, potionSprites[8], new List<string>{
                "Purity Blossom", "Forbidden Fruit"
            }),
            new Potion("Paralyzing Philter", 9, potionSprites[9], new List<string>{
                "Dragon's Blood", "Faerie Dust"
            }),
            new Potion("Draught of Dreams", 10, potionSprites[10], new List<string>{
                "Southern Star Silt", "Fragment of Night"
            }),
            new Potion("Blinding Brew", 11, potionSprites[11], new List<string>{
                "Faerie Dust", "Fragment of Night"
            })
        };
    }
}

