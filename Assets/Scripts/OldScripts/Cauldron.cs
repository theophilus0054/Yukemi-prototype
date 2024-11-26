using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class Cauldron : MonoBehaviour, IDropHandler
{
    [SerializeField] private Transform summonPoint; // Assign a position for summoning potions
    [SerializeField] private GameObject highPotionPrefab; // Assign your potion prefabs in the Inspector
    [SerializeField] private GameObject potionNameTextPrefab; // A prefab for displaying text above the potion
    [SerializeField] private GameObject manaPotionPrefab;
    [SerializeField] private GameObject harmPotionPrefab;


    [SerializeField] private TextMeshProUGUI ingredientListText; // Assign via Inspector
    private List<string> ingredients = new List<string>();

    private Dictionary<string[], (GameObject prefab, string potionName)> recipes;

    private void Start()
    {
        // Initialize recipes
        recipes = new Dictionary<string[], (GameObject, string)>
        {
            { new string[] { "glowstone", "sugar" }, (highPotionPrefab, "High Potion") },
            { new string[] { "redstone", "sugar" }, (manaPotionPrefab, "Mana Potion") },
            { new string[] { "glowstone", "redstone", "sugar" }, (harmPotionPrefab, "Harmy Potion") }
        };
    }

    public void OnDrop(PointerEventData eventData)
    {
        Debug.Log("Item dropped!");

        if (eventData.pointerDrag != null)
        {
            // Get the ingredient name from the draggable object's clone
            string ingredientName = eventData.pointerDrag.gameObject.name;

            // Add the ingredient to the cauldron list
            ingredients.Add(ingredientName);
            Debug.Log($"Added {ingredientName} to the cauldron!");

            // Update the UI text
            UpdateIngredientList();
        }
    }

    public void ResetCauldron()
    {
        ingredients.Clear();
        Debug.Log("Cauldron reset!");
        UpdateIngredientList(); // Clear the UI text as well
    }

    public void CheckRecipe()
    {
        foreach (var recipe in recipes)
        {
            if (recipe.Key.All(ingredients.Contains) && recipe.Key.Length == ingredients.Count)
            {
                Debug.Log($"Potion Created: {recipe.Value.potionName}!");
                ResetCauldron();
                return;
            }
        }

        Debug.Log("No valid potion recipe!");
    }

    private void UpdateIngredientList()
    {
        if (ingredients.Count == 0)
        {
            ingredientListText.text = "Cauldron Contents: Empty";
        }
        else
        {
            ingredientListText.text = "Cauldron Contents: " + string.Join(", ", ingredients);
        }
    }
}
