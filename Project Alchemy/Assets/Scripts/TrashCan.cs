using UnityEngine;
using UnityEngine.EventSystems;

public class TrashCan : MonoBehaviour, IDropHandler
{
    public StorageUnit storageUnit;
    

    // This method is called when an object is dropped onto the trash can
    public void OnDrop(PointerEventData eventData)
    {
        // Get the object being dropped
        GameObject draggedObject = eventData.pointerDrag;
        //Debug.Log("Object dropped: " + draggedObject.name + " with tag: " + draggedObject.tag);

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

        if (draggedObject != null && draggedObject.CompareTag("Potion"))
        {
            //Debug.Log("Potion Ran");
            // Find the Potion component on the dragged object (which holds the potion data
            DragPotion dragPotion = draggedObject.GetComponent<DragPotion>();
            if (dragPotion != null && dragPotion.currentPotion != null)
            {
                // Remove the potion from storage
                //Debug.Log("Potion Ran2");
                storageUnit.RemovePotion(dragPotion.currentPotion);
                Debug.Log("Potion removed from storage: " + dragPotion.currentPotion.potionName);
                NotificationText.Instance.ShowNotification(dragPotion.currentPotion.potionName + " removed from storage");
            }
        }
    }
}
