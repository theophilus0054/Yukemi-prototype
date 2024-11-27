using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Drag : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Image image; // The Image component of the object
    private RectTransform rectTransform; // The RectTransform of the object
    private GameObject currentClone; // The clone being dragged
    private Canvas canvas; // Reference to the canvas, set dynamically at runtime

    public Sprite ingredientSprite;  // Public field to assign the ingredient sprite


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        //Debug.Log("BeginDrag");
        if (gameObject.CompareTag("Ingredient"))
        {
            // Create a new gameObject for the ingredient sprite
            currentClone = new GameObject("IngredientClone");

            // Attach the Image component and set the sprite to the one for this ingredient
            Image cloneImage = currentClone.AddComponent<Image>();
            cloneImage.sprite = ingredientSprite; // Assign the ingredient sprite to the clone

            // Set the size of the clone to match the original
            RectTransform cloneRectTransform = currentClone.GetComponent<RectTransform>();
            cloneRectTransform.sizeDelta = rectTransform.sizeDelta * 0.5f;
            cloneRectTransform.localScale = Vector2.one * 0.5f;

            // Position it where the original item is being dragged from
            Vector2 localPointerPosition;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas.GetComponent<RectTransform>(), eventData.position, canvas.worldCamera, out localPointerPosition);
            cloneRectTransform.anchoredPosition = localPointerPosition;

            // Make the clone semi-transparent
            Color cloneColor = cloneImage.color;
            cloneColor.a = 1f;  // Set alpha to 50% transparency
            cloneImage.color = cloneColor;

            // Set the clone to be a child of the canvas
            currentClone.transform.SetParent(canvas.transform, false);

            // Disable the raycast target on the clone so it doesn't block UI interactions
            cloneImage.raycastTarget = false;

            // Bring the clone to the front (last sibling)
            currentClone.transform.SetAsLastSibling();
        }
        else
        {
            // Default behavior: Clone the object as usual
            currentClone = Instantiate(gameObject, transform.parent);
            RectTransform cloneRectTransform = currentClone.GetComponent<RectTransform>();
            Image cloneImage = currentClone.GetComponent<Image>();

            // Set the initial position to match the pointer
            cloneRectTransform.anchoredPosition = rectTransform.anchoredPosition;

            // Disable the Draggable script on the clone to prevent recursive cloning
            Destroy(currentClone.GetComponent<Drag>());

            // Make the clone's image non-interactive for raycasts while dragging
            cloneImage.raycastTarget = false;

            // Make the clone more transparent
            if (cloneImage != null)
            {
                Color cloneColor = cloneImage.color;
                cloneColor.a = 0.5f;  // Set alpha to 50% transparency
                cloneImage.color = cloneColor;
            }

            // Set the clone as the last sibling to appear on top
            currentClone.transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        //Debug.Log("Dragging");
        if (currentClone != null)
        {
            // Move the clone instead of the original
            RectTransform cloneRectTransform = currentClone.GetComponent<RectTransform>();
            cloneRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //Debug.Log("EndDrag");
        if (currentClone != null)
        {
            // Enable raycast target on the clone's image
            Image cloneImage = currentClone.GetComponent<Image>();
            cloneImage.raycastTarget = true;
            
            // Clear the reference to the current clone
            Destroy(currentClone);
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("PointerDown");
    }
}