using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class NotificationText : MonoBehaviour
{
    public TMP_Text notificationTextPrefab;  // Reference to the TextMeshPro prefab (set this in the inspector)
    public float fadeDuration = 2f;         // Duration for fading in and out
    public float displayDuration = 2f;      // How long the notification stays before fading out
    public Canvas canvas;                  // The canvas where notifications will be displayed
    public float verticalSpacing = 10f;     // Space between stacked notifications
    public int maxNotifications = 3;        // Maximum number of notifications to be displayed at once

    private static NotificationText instance;

    private Queue<TMP_Text> notificationQueue = new Queue<TMP_Text>();  // Queue to manage active notifications
    private float yOffset = 0f;             // Vertical offset for stacking notifications

    // Singleton pattern to access NotificationText globally
    public static NotificationText Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<NotificationText>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("NotificationText");
                    instance = obj.AddComponent<NotificationText>();
                }
            }
            return instance;
        }
    }

    // Show the notification at the upper middle of the screen
    public void ShowNotification(string message)
    {
        StartCoroutine(ShowNotificationCoroutine(message));
    }

    private IEnumerator ShowNotificationCoroutine(string message)
    {
        // Create a new TextMeshPro notification
        TMP_Text newNotification = Instantiate(notificationTextPrefab, canvas.transform);
        newNotification.text = message;
        newNotification.alpha = 0;  // Start with transparent

        // Set the position to the upper middle of the screen with vertical offset
        RectTransform rectTransform = newNotification.rectTransform;
        rectTransform.pivot = new Vector2(0.5f, 1f);  // Pivot set to upper-middle
        rectTransform.anchorMin = new Vector2(0.5f, 1f);  // Anchor set to upper-middle
        rectTransform.anchorMax = new Vector2(0.5f, 1f);  // Anchor set to upper-middle
        float topOffset = 50f;
        float canvasHeight = canvas.GetComponent<RectTransform>().rect.height;
        Vector2 screenCenter = new Vector2(0, canvasHeight / 2 - topOffset);
        rectTransform.anchoredPosition = screenCenter;  // Upper middle + vertical offset

        // Fade In
        yield return StartCoroutine(Fade(newNotification, 1f));

        // Add the notification to the queue
        notificationQueue.Enqueue(newNotification);

        // If the queue exceeds the max notifications, remove the oldest
        if (notificationQueue.Count > maxNotifications)
        {
            TMP_Text oldestNotification = notificationQueue.Dequeue();
            if (oldestNotification != null)  // Check if it's not null before accessing
            {
                StartCoroutine(FadeOutAndDestroy(oldestNotification));  // Fade and destroy the oldest notification
            }
        }

        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out the new notification after it's been displayed
        yield return StartCoroutine(Fade(newNotification, 0f));

        // Destroy the notification object after it fades out
        if (newNotification != null)
        {
            Destroy(newNotification.gameObject);
        }

        // Update the offset for the next notification
        UpdateYOffset();

        // Update positions of the active notifications
        UpdateNotificationPositions();
    }

    // Coroutine for fading effect
    private IEnumerator Fade(TMP_Text notification, float targetAlpha)
    {
        if (notification == null) yield break;  // Ensure the notification exists before fading

        float currentAlpha = notification.alpha;
        float timeElapsed = 0f;

        while (timeElapsed < fadeDuration)
        {
            notification.alpha = Mathf.Lerp(currentAlpha, targetAlpha, timeElapsed / fadeDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure the final alpha is exactly the target value
        notification.alpha = targetAlpha;
    }

    // Handle the fade out and destruction of the notification safely
    private IEnumerator FadeOutAndDestroy(TMP_Text notification)
    {
        if (notification == null) yield break;

        // Fade out the notification first
        yield return StartCoroutine(Fade(notification, 0f));

        // Now, destroy the notification after it's faded out
        if (notification != null)
        {
            Destroy(notification.gameObject);
        }
    }

    // Update the positions of active notifications
    private void UpdateNotificationPositions()
    {
        float currentYOffset = Screen.height * 0.25f; // Starting position for the first notification

        // Loop through each active notification and update its position
        List<TMP_Text> validNotifications = new List<TMP_Text>(notificationQueue);

        foreach (TMP_Text notification in validNotifications)
        {
            if (notification == null) continue;  // Skip destroyed notifications

            RectTransform rectTransform = notification.rectTransform;

            // Update position based on the current Y offset and the height of each notification
            rectTransform.anchoredPosition = new Vector2(0, currentYOffset);

            // Update the offset for the next notification (include spacing between notifications)
            currentYOffset -= rectTransform.rect.height + verticalSpacing;
            if(currentYOffset < 0)
            {
                break; // Stop positioning notifications off the screen
            }
        }
    }

    // Update the offset for the next notification based on the current notifications
    private void UpdateYOffset()
    {
        if (notificationQueue.Count > 0)
        {
            // After each notification fades out and is removed, update the Y offset
            TMP_Text lastNotification = notificationQueue.Peek();  // Get the most recent notification
            if (lastNotification != null) // Ensure the notification exists before accessing it
            {
                RectTransform rectTransform = lastNotification.rectTransform;
                yOffset = rectTransform.anchoredPosition.y - rectTransform.rect.height - verticalSpacing;
            }
        }
        else
        {
            yOffset = 0f;  // Reset offset when no notifications are present
        }
    }
}
