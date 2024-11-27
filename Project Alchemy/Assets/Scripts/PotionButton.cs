using UnityEngine;          // For MonoBehaviour, GameObject, etc.
using UnityEngine.UI;       // For UI components like Button, Text, Image
using TMPro;

public class PotionButton : MonoBehaviour
{

    // Reference to the Image component of the button
    public Image buttonImage;

    // Method to update the button's text and image
    public void UpdateButtonUI(string potionName, int potionID)
    {

        if (buttonImage != null && potionID >= 0 && potionID < 12)
        {
            buttonImage.sprite = PotionDatabase.Instance.potionList[potionID].potionSprite; // Set the image to the potion's sprite
            RectTransform imageRectTransform = buttonImage.GetComponent<RectTransform>();
            imageRectTransform.sizeDelta = new Vector2(230, 230);
        }
    }
}
