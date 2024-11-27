using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
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
                //Debug.Log("Added: " + ingredient.name);
                // NotificationText.Instance.ShowNotification("Added: " + ingredient.name);
                PrintIngredients(); 
                CheckPotionCombination();
            }
            else
            {
                // NotificationText.Instance.ShowNotification("Mixing Pot is full!");
                //Debug.Log("Mixing Pot is full!");
            }
        }
    }
    // Method to check if the current ingredients form a valid potion recipe
    private void CheckPotionCombination()
    {
        if (ingredients.Count == maxIngredients)
        {
            bool isValidRecipe = false;
            foreach (var recipe in PotionDatabase.Instance.potionList)
            {
                // Check if current ingredients match a valid recipe
                if (IsValidRecipe(recipe))
                {
                    isValidRecipe = true;
                    //Debug.Log("Valid potion combination: " + string.Join(" + ", recipe.potionName));
                    makePotionButton.gameObject.SetActive(true); // Show the button to make potion
                    PotionButton potionButton = makePotionButton.GetComponent<PotionButton>();

                    if (potionButton != null)
                    {
                        potionButton.UpdateButtonUI(recipe.potionName, recipe.potionID);
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
                //Debug.Log("Invalid combination! The ingredients cannot make a potion.");
                NotificationText.Instance.ShowNotification("Invalid ingredient combination!");
                makePotionButton.gameObject.SetActive(false); // Hide the button if invalid
            }
        }
    }
    // Helper method to check if current ingredients match the recipe
    private bool IsValidRecipe(Potion potion)
    {
        List<string> ingredientNames = new List<string>();

        // Gather the names of the current ingredients in the pot
        foreach (var ingredient in ingredients)
        {
            ingredientNames.Add(ingredient.name);
        }

        // Sort both lists to make the order irrelevant
        ingredientNames.Sort();
        potion.ingredients.Sort();

        // Check if the sorted ingredient list matches the recipe
        return ingredientNames.SequenceEqual(potion.ingredients);
    }

    // Method to create the potion when the button is pressed
    private void MakePotion()
    {
        
        // Logic to create the potion (e.g., instantiate a potion object, play animation, etc.)
        Potion potion = GetPotionBasedOnIngredients();

        // Call the method in StorageUnit to add this potion to storage
       storageUnit.AddPotionToStorageFromMixingPot(potion.potionName, potion.potionID, potion.potionSprite, potion.ingredients);

        // Empty the pot after potion is created
        ingredients.Clear();
        makePotionButton.gameObject.SetActive(false); // Hide the button
    }
    // Define the logic to return a potionID based on the potion name
     private Potion GetPotionBasedOnIngredients()
    {
        foreach (var recipe in PotionDatabase.Instance.potionList)
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
                return recipe; // Return the matching potion name
            } 
        }
        return null;
    }
   
    // Method to empty the mixing pot
    public void EmptyPot()
    {
        ingredients.Clear();
        // NotificationText.Instance.ShowNotification("Mixing Pot has been emptied.");
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
