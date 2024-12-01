using UnityEngine;

public class AspectRatioManager : MonoBehaviour
{
    private float targetAspect = 16f / 10f; // Target 16:10 aspect ratio
    private Camera mainCamera;

    private float previousWidth = -1;
    private float previousHeight = -1;

    private float tolerance = 0.005f;

    void Start()
    {
        mainCamera = Camera.main;
        SetAspectRatio(); // Initialize at the start
    }

    void Update()
    {
        // Only update if the screen size changes
        if (Screen.width != previousWidth || Screen.height != previousHeight)
        {
            previousWidth = Screen.width;
            previousHeight = Screen.height;

            SetAspectRatio(); // Recalculate aspect ratio when the screen size changes
        }
    }

    void SetAspectRatio()
    {
        // Get the current screen aspect ratio
        float windowAspect = (float)Screen.width / (float)Screen.height;

        // Log the current aspect ratio and comparison to target
        Debug.Log("Window Aspect: " + windowAspect);
        Debug.Log("Target Aspect (16:10): " + targetAspect);
        Debug.Log("Difference: " + Mathf.Abs(windowAspect - targetAspect));

        // Calculate the scale factor for height and width based on the target aspect ratio
        float scaleHeight = windowAspect / targetAspect;
        float scaleWidth = targetAspect / windowAspect;

        // If the aspect ratio is approximately 16:10, do not apply any adjustments
        if (Mathf.Abs(windowAspect - targetAspect) < tolerance)
        {
            Debug.Log("Aspect ratio is close to 16:10, no adjustment needed.");
            mainCamera.rect = new Rect(0, 0, 1, 1); // Full screen, no black bars
            return;
        }

        // If the screen is wider than 16:10 (letterboxing)
        if (scaleHeight < 1.0f)
        {
            Debug.Log("Wider than 16:10, applying letterboxing (black bars top/bottom).");
            // Add black bars to the top and bottom (letterboxing)
            float y = (1.0f - scaleHeight) / 2.0f;
            mainCamera.rect = new Rect(0, y, 1, scaleHeight); // Top and bottom black bars
        }
        // If the screen is taller than 16:10 (pillarboxing)
        else
        {
            Debug.Log("Taller than 16:10, applying pillarboxing (black bars left/right).");
            // Add black bars to the left and right (pillarboxing)
            float x = (1.0f - scaleWidth) / 2.0f;
            mainCamera.rect = new Rect(x, 0, scaleWidth, 1); // Left and right black bars
        }
    }
}
