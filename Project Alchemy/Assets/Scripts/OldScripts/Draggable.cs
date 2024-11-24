using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Draggable : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Canvas canvas;
    private RectTransform rectTransform;
    public Image image;
    private GameObject currentClone; // Reference to the current clone being dragged
    
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("BeginDrag");
        
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
        
        // Set the clone as the last sibling to appear on top
        currentClone.transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        Debug.Log("Dragging");
        if (currentClone != null)
        {
            // Move the clone instead of the original
            RectTransform cloneRectTransform = currentClone.GetComponent<RectTransform>();
            cloneRectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Debug.Log("EndDrag");
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
        Debug.Log("PointerDown");
    }
}