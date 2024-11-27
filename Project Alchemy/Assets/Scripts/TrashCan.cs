using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCan : MonoBehaviour, IDropHandler
{
    // This method is called when an object is dropped onto the trash can
    public void OnDrop(PointerEventData eventData)
    {
        // Get the object being dropped
        GameObject draggedObject = eventData.pointerDrag;

        // Check if the dragged object is tagged as "MixingPot"
        if (draggedObject != null && draggedObject.CompareTag("MixingPot"))
        {
            // Find the MixingPot component on the dragged object
            MixingPot mixingPot = draggedObject.GetComponent<MixingPot>();

            // If the MixingPot component is found, empty the pot
            if (mixingPot != null)
            {
                mixingPot.EmptyPot(); // Call the method to empty the pot
            }
        }
    }
}
