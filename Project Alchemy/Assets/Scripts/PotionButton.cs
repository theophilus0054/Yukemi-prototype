using UnityEngine;          // For MonoBehaviour, GameObject, etc.
using UnityEngine.UI;       // For UI components like Button, Text, Image
using TMPro;

public class PotionButton : MonoBehaviour
{
    // Reference to the Text component of the button
    public TMP_Text buttonText;

    // Reference to the Image component of the button
    public Image buttonImage;

    // Array of potion sprites that correspond to the potionID
    public Sprite[] potionSprites;  // Corrected to use Sprite[] instead of Image[]

    // Method to update the button's text and image
    public void UpdateButtonUI(string potionName, int potionID)
    {
        if (buttonText != null)
        {
            buttonText.text = potionName; // Set the text to the potion's name
        }

        if (buttonImage != null && potionID >= 0 && potionID < potionSprites.Length)
        {
            buttonImage.sprite = potionSprites[potionID]; // Set the image to the potion's sprite
            RectTransform imageRectTransform = buttonImage.GetComponent<RectTransform>();
            imageRectTransform.sizeDelta = new Vector2(70, 70);
        }
    }
}
