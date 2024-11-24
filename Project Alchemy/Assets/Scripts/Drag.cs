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


    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        image = GetComponent<Image>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (canvas == null)
        {
            canvas = FindObjectOfType<Canvas>(); // Find the canvas at runtime
        }

        //Debug.Log("BeginDrag");

        // Create a clone of the gameObject
        currentClone = Instantiate(gameObject, transform.parent);
        
        // Get the RectTransform and Image of the clone
        RectTransform cloneRectTransform = currentClone.GetComponent<RectTransform>();
        Image cloneImage = currentClone.GetComponent<Image>();
        
        // Set the initial position to match the pointer
        cloneRectTransform.anchoredPosition = rectTransform.anchoredPosition;
        
        // Disable the Draggable script on the clone to prevent recursive cloning
        Destroy(currentClone.GetComponent<Draggable>());

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