using UnityEngine;
using UnityEngine.EventSystems;

public class CursorChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // When the pointer enters the button, change to the hand cursor
    public void OnPointerEnter(PointerEventData eventData)
    {
        // Change the cursor to the "hand" when hovering over the button
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto); // Use default pointer (hand icon)
    }

    // When the pointer exits the button, reset to default cursor
    public void OnPointerExit(PointerEventData eventData)
    {
        // Reset cursor to default system pointer
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}

