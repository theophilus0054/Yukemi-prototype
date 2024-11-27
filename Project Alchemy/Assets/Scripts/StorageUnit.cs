using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StorageUnit : MonoBehaviour
{
    public static StorageUnit Instance;

    // Storage for up to 3 potions
    public Potion[] potions = new Potion[3];

    // For keeping track of the buttons or slots to hold potion images
    public Image[] potionSlots = new Image[3];

    public Sprite emptyStorageSprite;  // Sprite when no potions are in the storage unit
    public Sprite oneStorageSprite;
    public Sprite twoStorageSprite;
    public Sprite threeStorageSprite;
    private Image storageUnitImage;

    private void Awake()
    {
        // Ensure that only one instance of StorageUnit exists and assign it to Instance
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicates if there's already an instance
        }

        // Ensure arrays are initialized if not already
        if (potions == null)
            potions = new Potion[3];
        if (potionSlots == null)
            potionSlots = new Image[3];
    }


    void Start()
    {
        // Get the Image component from the Storage Unit
        storageUnitImage = GetComponent<Image>();

        // Initially update the storage unit sprite based on whether it has any potions
    }

    void Update()
    {
        UpdateStorageUnitSprite();
    }

    public void AddPotionToStorageFromMixingPot(string potionName, int potionID, Sprite potionSprite, List<string> ingredients)
    {
        bool isFull = true;
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i] == null) // If there's an empty slot
            {
                isFull = false;
                break;
            }
        }

        if (isFull)
        {
            // If the storage is full, print a message and return without adding the potion
            //NotificationText.Instance.ShowNotification("Storage is full!");
            Debug.Log("Storage is full!");
            return;
        }

        // Find the first empty spot in the potions array
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i] == null)
            {
                // Create the new potion based on the mixing pot result
                Potion newPotion = new Potion(potionName, potionID, potionSprite, ingredients);

                // Add it to the storage
                potions[i] = newPotion;

                // Update the slot image to match the potion sprite
                potionSlots[i].sprite = newPotion.potionSprite;

                //DragPotion dragPotion = potionSlots[i].GetComponent<DragPotion>();
                //if (dragPotion != null)
                //{
                //    dragPotion.SetPotionData(newPotion, i); // Pass the potion data to DragPotion
                //}
                //NotificationText.Instance.ShowNotification(newPotion.potionName + " created and was put in storage");
                //Debug.Log("Potion created: " + newPotion.potionName);
                // Break out of the loop as we've added a potion
                break;
            }
        }
        //PrintPotionsInStorage();
    }


    public void RemovePotion(Potion potionToRemove)
    {
        // Loop through the potions array to find and remove the specified potion
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i] != null && potions[i] == potionToRemove)
            {
                potions[i] = null;
                potionSlots[i].sprite = null;

                // Shift the potions in the array to the left
                for (int j = i; j < 2; j++)
                {
                    potions[j] = potions[j + 1];  // Shift the potion to the left
                    potionSlots[j].sprite = potionSlots[j + 1].sprite;  // Move the sprite to the left
                    potions[j + 1] = null;
                    potionSlots[j + 1].sprite = null;
                }
                // Optionally, you can add some visual feedback or additional actions here
                //Debug.Log("Potion removed from storage slot " + i);
                Debug.Log("potion 1: " + potions[0]);
                Debug.Log("potion 2: " + potions[1]);
                Debug.Log("potion 3: " + potions[2]);
                break;
            }
        }
        //PrintPotionsInStorage();
    }
    public void PrintPotionsInStorage()
    {
        Debug.Log("Potions in Storage:");

        // Loop through the potions array and print each potion's name and ID if it's not null
        for (int i = 0; i < potions.Length; i++)
        {
            if (potions[i] != null)
            {
                Debug.Log("Potion Slot " + i + ": " + potions[i].potionName + " (ID: " + potions[i].potionID + ")");
            }
            else
            {
                Debug.Log("Potion Slot " + i + ": Empty");
            }
        }
    }

    private void UpdateStorageUnitSprite()
    {
        int potionCount = 0;
        for (int i = 0;i < potions.Length;i++)
        {
            if (potions[i] != null)
            {
                potionCount++;
            }
        }

        if (potionCount == 1)
        {
            storageUnitImage.sprite = oneStorageSprite;
        }
        else if (potionCount == 2)
        {
            storageUnitImage.sprite = twoStorageSprite;
        }
        else if (potionCount == 3)
        {
            storageUnitImage.sprite = threeStorageSprite;
        }
        else
        {
            storageUnitImage.sprite = emptyStorageSprite;
        }
    }

}
