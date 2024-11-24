using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class MixingPot : MonoBehaviour, IDropHandler
{
    // List to store the ingredients in the mixing pot
    private List<GameObject> ingredients = new List<GameObject>();

    // Maximum number of ingredients the mixing pot can hold
    private const int maxIngredients = 2;

    // Reference to the "Make Potion" button (set this in the inspector)
    public Button makePotionButton;
    public StorageUnit storageUnit;

    // Define valid potion combinations (This is just an example, you can define as many as you need)
    private List<PotionRecipe> validPotionRecipes = new List<PotionRecipe>()
    {
        new PotionRecipe(new List<string> { "Purity Blossom", "Dragon's Blood" }, "Restorative Remedy"), 
        new PotionRecipe(new List<string> { "Southern Star Silt", "Faerie Dust" }, "Mixture of Mana"),
        new PotionRecipe(new List<string> { "Southern Star Silt", "Dragon's Blood" }, "Salve of Strength"),
        new PotionRecipe(new List<string> { "Purity Blossom", "Southern Star Silt" }, "Teacher's Tonic"),
        new PotionRecipe(new List<string> { "Purity Blossom", "Fragment of Night" }, "Eclipse Elixir"),
        new PotionRecipe(new List<string> { "Forbidden Fruit", "Faerie Dust" }, "Liquid Lie"),
        new PotionRecipe(new List<string> { "Forbidden Fruit", "Fragment of Night" }, "Acidic Afterburn"),
        new PotionRecipe(new List<string> { "Forbidden Fruit", "Dragon's Blood" }, "Combustive Concoction"),
        new PotionRecipe(new List<string> { "Purity Blossom", "Forbidden Fruit" }, "Potion of Poison"),
        new PotionRecipe(new List<string> { "Dragon's Blood", "Faerie Dust" }, "Paralyzing Philter"),
        new PotionRecipe(new List<string> { "Southern Star Silt", "Fragment of Night" }, "Draught of Dreams"),
        new PotionRecipe(new List<string> { "Faerie Dust", "Fragment of Night" }, "Blinding Brew")
    };
    void Start()
    {
        makePotionButton.gameObject.SetActive(false); 
        makePotionButton.onClick.AddListener(MakePotion); 
    }

    // This method is called when an object is dropped onto the mixing pot
    public void OnDrop(PointerEventData eventData)
    {
        // Get the object being dropped
        GameObject ingredient = eventData.pointerDrag;

        // Check if the dropped object is a valid ingredient
        if (ingredient != null && ingredient.CompareTag("Ingredient"))
        {
            // If the pot has space for another ingredient
            if (ingredients.Count < maxIngredients)
            {
                // Add the ingredient to the mixing pot
                ingredients.Add(ingredient);
                Debug.Log("Added: " + ingredient.name);
                NotificationText.Instance.ShowNotification("Added: " + ingredient.name);
                PrintIngredients(); 
                CheckPotionCombination();
            }
            else
            {
                NotificationText.Instance.ShowNotification("Mixing Pot is full!");
                Debug.Log("Mixing Pot is full!");
                CheckPotionCombination();
            }
        }
    }
    // Method to check if the current ingredients form a valid potion recipe
    private void CheckPotionCombination()
    {
        if (ingredients.Count == maxIngredients)
        {
            bool isValidRecipe = false;
            foreach (var recipe in validPotionRecipes)
            {
                // Check if current ingredients match a valid recipe
                if (IsValidRecipe(recipe))
                {
                    isValidRecipe = true;
                    Debug.Log("Valid potion combination: " + string.Join(" + ", recipe.potionName));
                    makePotionButton.gameObject.SetActive(true); // Show the button to make potion
                    PotionButton potionButton = makePotionButton.GetComponent<PotionButton>();

                    if (potionButton != null)
                    {
                        potionButton.UpdateButtonUI(recipe.potionName, validPotionRecipes.IndexOf(recipe));
                    }
                    else
                    {
                        Debug.LogError("PotionButton component not found on makePotionButton.");
                    }
                    return;
                }
            }

            if (!isValidRecipe)
            {
                Debug.Log("Invalid combination! The ingredients cannot make a potion.");
                NotificationText.Instance.ShowNotification("Invalid ingredient combination!");
                makePotionButton.gameObject.SetActive(false); // Hide the button if invalid
            }
        }
    }
    // Helper method to check if current ingredients match the recipe
    private bool IsValidRecipe(PotionRecipe recipe)
    {
        List<string> ingredientNames = new List<string>();

        // Gather the names of the current ingredients in the pot
        foreach (var ingredient in ingredients)
        {
            ingredientNames.Add(ingredient.name);
        }

        // Sort both lists to make the order irrelevant
        ingredientNames.Sort();
        recipe.ingredients.Sort();

        // Check if the sorted ingredient list matches the recipe
        return ingredientNames.SequenceEqual(recipe.ingredients);
    }

    // Method to create the potion when the button is pressed
    private void MakePotion()
    {
        
        // Logic to create the potion (e.g., instantiate a potion object, play animation, etc.)
        string potionName = GetPotionNameBasedOnIngredients(); // Combine ingredient names
        int potionID = GetPotionIDBasedOnIngredients(potionName);

        // Call the method in StorageUnit to add this potion to storage
        storageUnit.AddPotionToStorageFromMixingPot(potionName, potionID);

        // Empty the pot after potion is created
        ingredients.Clear();
        makePotionButton.gameObject.SetActive(false); // Hide the button
    }
    // Define the logic to return a potionID based on the potion name
    private string GetPotionNameBasedOnIngredients()
    {
        foreach (var recipe in validPotionRecipes)
        {
            List<string> ingredientNames = new List<string>();

            // Gather the names of the current ingredients in the pot
            foreach (var ingredient in ingredients)
            {
                ingredientNames.Add(ingredient.name);
            }

            // Sort both lists to make the order irrelevant
            ingredientNames.Sort();
            recipe.ingredients.Sort();

            // Check if the sorted ingredient list matches the recipe
            if (ingredientNames.SequenceEqual(recipe.ingredients))
            {
                return recipe.potionName; // Return the matching potion name
            }
        }

        return "Unknown Potion"; // If no recipe matched
    }
    // Define the logic to return a potionID based on the potion name
    private int GetPotionIDBasedOnIngredients(string potionName)
    {
        for (int i = 0; i < validPotionRecipes.Count; i++)
        {
            if (validPotionRecipes[i].potionName == potionName)
            {
                return i; // Return the index of the matching potion in the list
            }
        }

        return -1; // Invalid potion
    }
    // Method to empty the mixing pot
    public void EmptyPot()
    {
        ingredients.Clear();
        NotificationText.Instance.ShowNotification("Mixing Pot has been emptied.");
        Debug.Log("Mixing Pot has been emptied.");
    }
    // Print the current ingredients in the mixing pot
    private void PrintIngredients()
    {
        Debug.Log("Current contents of the Mixing Pot:");
        foreach (var ingredient in ingredients)
        {
            Debug.Log("- " + ingredient.name);
        }
    }
}

public class PotionRecipe
{
    public List<string> ingredients;
    public string potionName;

    public PotionRecipe(List<string> ingredients, string potionName)
    {
        this.ingredients = ingredients;
        this.potionName = potionName;
    }
}