using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MixingPot : MonoBehaviour, IDropHandler
{
    // List to store the ingredients in the mixing pot
    private List<GameObject> ingredients = new List<GameObject>();

    // Maximum number of ingredients the mixing pot can hold
    private const int maxIngredients = 2;

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
                PrintIngredients(); // Print the current ingredients in the pot
            }
            else
            {
                Debug.Log("Mixing Pot is full!");
            }
        }
    }
    // Method to empty the mixing pot
    public void EmptyPot()
    {
        ingredients.Clear();
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
