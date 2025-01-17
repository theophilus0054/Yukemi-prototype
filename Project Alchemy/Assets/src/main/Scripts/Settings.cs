using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  // To access SceneManager
using UnityEngine.UI;  // To access Button components

public class Settings : MonoBehaviour
{
    // References to the buttons and exit panel
    public GameObject exitPanel;  // The panel that you want to hide when "Start" is clicked
    public Button startButton;    // The Start button
    public Button exitButton;     // The Exit button
    public Button settingsButton; // The Settings button that brings up the panel
    public Button overlay;         // The black overlay behind the exit panel

    // Start is called before the first frame update
    void Start()
    {
        // Hide the exit panel and overlay at the start
        if (exitPanel != null)
        {
            exitPanel.SetActive(false);
        }

        // Ensure overlay is initially hidden
        if (overlay != null)
        {
            overlay.gameObject.SetActive(false);
        }

        // Add listeners to the buttons
        startButton.onClick.AddListener(OnStartButtonClicked);
        exitButton.onClick.AddListener(OnExitButtonClicked);
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
        overlay.onClick.AddListener(OnOverlayClicked);
    }

    // Update is called once per frame
    void Update()
    {
        // Any other update logic can go here, if needed
    }

    // Called when the "Start" button is clicked
    void OnStartButtonClicked()
    {
        // Hide the exit panel
        if (exitPanel != null)
        {
            exitPanel.SetActive(false);
        }

        // Hide the overlay as well
        if (overlay != null)
        {
            overlay.gameObject.SetActive(false);
        }
        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }

    // Called when the "Exit" button is clicked
    void OnExitButtonClicked()
    {
        // Load the first scene (Scene 0)
        SceneManager.LoadScene(0);
    }

    // Called when the "Settings" button is clicked to show the exit panel and overlay
    void OnSettingsButtonClicked()
    {
        // Show the exit panel and overlay
        if (exitPanel != null)
        {
            exitPanel.SetActive(true);
        }

        if (overlay != null)
        {
            overlay.gameObject.SetActive(true);
        }
        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(OnOverlayClicked);
    }

    // Called when the overlay is clicked to close the exit panel and overlay
    public void OnOverlayClicked()
    {
        // Hide the exit panel and overlay
        if (exitPanel != null)
        {
            exitPanel.SetActive(false);
        }

        if (overlay != null)
        {
            overlay.gameObject.SetActive(false);
        }
        settingsButton.onClick.RemoveAllListeners();
        settingsButton.onClick.AddListener(OnSettingsButtonClicked);
    }
}
