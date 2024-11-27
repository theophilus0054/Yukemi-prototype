using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DragPotion : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image image; // The Image component of the object (the potion slot's sprite)
    private RectTransform rectTransform; // The RectTransform of the object
    private GameObject currentClone; // The clone being dragged
    private Canvas canvas; // Reference to the canvas, set dynamically at runtime

    private int potionSlotIndex; // Track which slot the potion belongs to
    private Image potionSlotImage; // Reference to the potion slot image that the potion sprite belongs to
    public Potion currentPotion; // Reference to the potion data in the storage unit

    private Vector2 cursorOffset;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        potionSlotImage = GetComponent<Image>(); // Assuming the potion slot is the image component
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Set the potionSlotIndex dynamically based on the slot that was clicked
        potionSlotIndex = Array.IndexOf(StorageUnit.Instance.potionSlots, potionSlotImage);

        // Ensure that currentPotion is properly set based on the clicked slot
        if (potionSlotIndex >= 0 && potionSlotIndex < StorageUnit.Instance.potions.Length)
        {
            currentPotion = StorageUnit.Instance.potions[potionSlotIndex];
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>(); // Find the canvas at runtime
        }

        // Check if there's a potion assigned to the slot
        if (potionSlotImage.sprite != null) // Ensure that there's an actual sprite assigned
        {
            // Create a clone of the potion sprite (not the whole object)
            currentClone = new GameObject("PotionClone", typeof(Image));
            Image cloneImage = currentClone.GetComponent<Image>();

            // Assign the potion slot's sprite to the clone image (instead of currentPotion.potionSprite)
            cloneImage.sprite = potionSlotImage.sprite;
            cloneImage.raycastTarget = false; // Make it non-interactive during drag

            // Set the initial position of the clone to match the potion slot
            RectTransform cloneRectTransform = currentClone.GetComponent<RectTransform>();
            cloneRectTransform.sizeDelta = new Vector2(200, 200);
            cloneRectTransform.SetParent(canvas.transform, false); // Set canvas as parent

            Vector2 cursorPos;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.transform as RectTransform, eventData.position, canvas.worldCamera, out cursorPos);

            // Offset the clone to be directly under the cursor
            cursorOffset = cursorPos - cloneRectTransform.anchoredPosition;

            // Move the clone to the cursor position
            cloneRectTransform.anchoredPosition = cursorPos;

            // Make the clone 
            Color cloneColor = cloneImage.color;
            cloneColor.a = 1f;  // Set alpha to 50% transparency
            cloneImage.color = cloneColor;

            // Set the clone as the last sibling to appear on top
            currentClone.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the clone while dragging
        if (currentClone != null)
        {
            RectTransform cloneRectTransform = currentClone.GetComponent<RectTransform>();
            cloneRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // When dragging ends, destroy the clone
        if (currentClone != null)
        {
            Destroy(currentClone);
        }
    }

    // Set the potion data (potion sprite and slot index) for this drag behavior
    public void SetPotionData(Potion potion, int index)
    {
        currentPotion = potion;
        potionSlotIndex = index;
    }
}
