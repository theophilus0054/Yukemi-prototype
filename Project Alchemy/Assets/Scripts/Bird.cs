using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // For IPointerClickHandler

public class Bird : MonoBehaviour, IPointerClickHandler, IDropHandler // Implement the interface for detecting clicks
{
    public int slotIndex;
    public Sprite scrollSprite;          // Sprite when the bird is waiting (scroll)
    public Sprite noScrollSprite;        // Sprite when the bird is no longer waiting (without scroll)
    public Image potionRequestImage;     // Image component for displaying the potion request
    public StorageUnit storageUnit;
    public Slider timerSlider;

    private bool isClicked = false;      // To track if the bird has been clicked
    private float timer = 45f;           // Timer for 60 seconds after the bird is clicked
    private bool isTimerRunning = false; // To track if the timer is running
    private Potion requestedPotion;      // The potion that the bird is requesting
    private bool hasReceivedPotion = false; // Flag to track if the bird received the correct potion

    void Start()
    {
        if (storageUnit == null)
        {
            storageUnit = FindObjectOfType<StorageUnit>();  // Find the first StorageUnit in the scene
        }

        // Initially set the bird's sprite to the "scroll" state
        GetComponent<Image>().sprite = scrollSprite;

        // Hide the potion request UI initially
        potionRequestImage.gameObject.SetActive(false); // Hide the potion request image

        isTimerRunning = true;
        if (timerSlider != null)
        {
            timerSlider.maxValue = timer;  // Set the maximum value based on the timer duration
            timerSlider.value = timer;     // Set the initial value to the timer's value
        }
    }

    // This method is called when the bird is clicked
    public void OnPointerClick(PointerEventData eventData)
    {
        OnClick(); // Trigger the OnClick method when the bird is clicked
    }

    // Called when the bird is clicked
    public void OnClick()
    {
        if (!isClicked)
        {
            isClicked = true;
            //Debug.Log("Bird has been clicked!");

            // Change the bird's sprite to "no scroll"
            GetComponent<Image>().sprite = noScrollSprite;

            // Start the timer

            // Generate a random potion request for the bird
            GeneratePotionRequest();
        }
    }

    // Generate a random potion request (you can adjust the potion data accordingly)
    void GeneratePotionRequest()
    {
        // Create a random potion ID (you can change this to your own logic)
        int randomPotionIndex = Random.Range(0, PotionDatabase.Instance.potionSprites.Length);
        requestedPotion = PotionDatabase.Instance.potionList[randomPotionIndex];  // Assume PotionDatabase holds the list of potions

        Debug.Log("Bird requested: " + requestedPotion.potionName);
        QuestManager.Instance.AddQuest(requestedPotion.potionName);
        QuestManager.Instance.ShowQuestByIndex(slotIndex);
        //NotificationText.Instance.ShowNotification("Bird requested: " + requestedPotion.potionName);
        // Show the potion request above the bird
        potionRequestImage.gameObject.SetActive(true); // Make the potion request image visible
        potionRequestImage.sprite = PotionDatabase.Instance.potionSprites[randomPotionIndex]; // Set the requested potion sprite
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerRunning)
        {
            timer -= Time.deltaTime;
            if (timerSlider != null)
            {
                timerSlider.value = timer;  // Update the slider's value based on the timer
            }

            // If the timer runs out, remove the bird
            if (timer <= 0f)
            {
                //Debug.Log("Bird's timer expired.");
                // Remove the bird from the scene
                //NotificationText.Instance.ShowNotification("Bird got fed up of waiting, -10 points");
                QuestManager.Instance.RemoveQuest(slotIndex);
                RemoveBird();
            }
        }
    }

    // This method is called when a potion is dragged and dropped onto the bird
    public void OnDrop(PointerEventData eventData)
    {
        // Check if the object being dropped is tagged as "Potion"
        GameObject droppedObject = eventData.pointerDrag;  // Get the object being dragged

        if (droppedObject != null && droppedObject.CompareTag("Potion"))
        {
            DragPotion dragPotion = droppedObject.GetComponent<DragPotion>();

            if (dragPotion != null && !hasReceivedPotion)
            {
                Debug.Log("dragged: " + dragPotion.currentPotion.GetType());
                Debug.Log("dragged name: " + dragPotion.currentPotion.potionName);
                Debug.Log("dragged id: " + dragPotion.currentPotion.potionID);
                Debug.Log("requested: " + requestedPotion.GetType());
                Debug.Log("requested name: " + requestedPotion.potionName);
                Debug.Log("requested: " + requestedPotion.potionID);
                // Check if the dropped potion is the correct one
                if (dragPotion.currentPotion.potionName == requestedPotion.potionName)
                {
                    Debug.Log("Correct potion received!");
                    //NotificationText.Instance.ShowNotification("Bird received the right potion, +10 points");
                    hasReceivedPotion = true;
                    storageUnit.RemovePotion(dragPotion.currentPotion);
                    QuestManager.Instance.RemoveQuest(slotIndex);
                    RemoveBird();
                }
                else
                {
                    //Debug.Log("Incorrect potion.");
                }
            }
        }
    }

    public void RemoveBird()
    {
        // Remove the potion and bird from the scene
        potionRequestImage.gameObject.SetActive(false);
        BirdSpawner spawner = FindObjectOfType<BirdSpawner>();
        //Debug.Log("Before removing bird, slot " + slotIndex + " has " + spawner.birdSlots[slotIndex].transform.childCount + " child.");
        Destroy(transform.parent.gameObject);

        
        if (spawner != null)
        {
            spawner.ResetSlot(slotIndex); // Reset the correct slot based on the bird's slot index
        }
    }
}
