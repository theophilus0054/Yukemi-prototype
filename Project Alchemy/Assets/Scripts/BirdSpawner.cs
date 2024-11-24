using System.Collections;
using UnityEngine;

public class BirdSpawner : MonoBehaviour
{
    // Array of bird slots (Bird1, Bird2, Bird3)
    public GameObject[] birdSlots = new GameObject[3];

    // Array of bird prefabs to randomly choose from
    public GameObject[] birdPrefabs;

    // Flag to track if birds can be spawned
    private bool canSpawnBirds = true;
    private bool spawningInProgress = false;

    void Start()
    {
        // Fill the birdSlots array with the references to Bird1, Bird2, and Bird3
        birdSlots[0] = transform.Find("Bird1").gameObject;
        birdSlots[1] = transform.Find("Bird2").gameObject;
        birdSlots[2] = transform.Find("Bird3").gameObject;

        // Start the spawning process
        StartCoroutine(SpawnBirds());
    }

    void Update()
    {
        // Only attempt to spawn birds immediately if there are no birds and no other spawn is in progress
        if (NoBirdsInSlots() && !spawningInProgress)
        {
            StartCoroutine(SpawnBirdsImmediately());
        }
    }

    // Coroutine to spawn birds every 10 seconds, only if there's space available
    IEnumerator SpawnBirds()
    {
        while (true)
        {
            // If spawning is disabled or we're already spawning, wait
            if (!canSpawnBirds || spawningInProgress || AreAllSlotsFull())
            {
                yield return null; // Wait for the next frame without stopping the loop
            }
            else
            {
                //Debug.Log("Waiting for 10 seconds to spawn a new bird...");
                yield return new WaitForSeconds(5f);  // Wait for 10 seconds before trying to spawn a bird
                SpawnBird(); // Spawn a bird if there is space
            }
        }
    }

    // Coroutine to spawn a bird immediately if there are no birds
    IEnumerator SpawnBirdsImmediately()
    {
        spawningInProgress = true;

        yield return new WaitForSeconds(1f);  // Wait for 1 second to spawn the first bird

        if (NoBirdsInSlots()) // Check again if no birds are present
        {
            SpawnBird();
        }

        spawningInProgress = false;
    }

    public void ResetSlot(int slotIndex)
    {
        GameObject slotObject = birdSlots[slotIndex];  // Get the GameObject from the birdSlots array
        Transform slotTransform = slotObject.transform;

        // Wait a frame for Unity to properly update the slot after the bird is destroyed
        StartCoroutine(ClearSlotAndCheck(slotTransform, slotIndex));
    }

    private IEnumerator ClearSlotAndCheck(Transform slotTransform, int slotIndex)
    {
        // Wait for a short period to allow Unity to update the transform hierarchy
        yield return null;  // Wait for the end of the frame to give Unity time to update

        //Debug.Log("After removing bird, slot " + slotIndex + " has " + slotTransform.childCount + " child.");
        // Check if the slot is empty now
        if (slotTransform.childCount == 0)
        {
            //Debug.Log("Slot " + slotIndex + " is now empty. Attempting to spawn a new bird.");
            // If the slot is empty, attempt to spawn a new bird

            StartCoroutine(SpawnBirds()); // Spawn a new bird
            
        }
        else
        {
            //Debug.Log("Slot " + slotIndex + " still has a child. Not spawning yet.");
        }
    }

    public void CheckForAvailableSlots()
    {
        if (canSpawnBirds)
        {
            //Debug.Log("Slot is available, resuming bird spawning.");
            // Ensure that the spawning process continues if there's space
            if (NoBirdsInSlots() && !spawningInProgress)
            {
                StartCoroutine(SpawnBirdsImmediately()); // Restart spawning immediately if necessary
            }
        }
    }

    // Function to check if there are no birds in the slots
    bool NoBirdsInSlots()
    {
        foreach (GameObject slot in birdSlots)
        {
            if (slot.transform.childCount > 0)  // Check if there is any bird in the slot
            {
                return false;
            }
        }
        return true;
    }

    bool AreAllSlotsFull()
    {
        foreach (var slot in birdSlots)
        {
            if (slot.transform.childCount == 0) // If any slot is empty
            {
                return false; // There’s at least one empty slot
            }
        }
        return true; // All slots are full
    }

    // Function to spawn a bird in an available slot
    void SpawnBird()
    {
        // Check for the first available slot
        for (int i = 0; i < birdSlots.Length; i++)
        {
            if (birdSlots[i].transform.childCount == 0) // No bird in this slot
            {
                // Randomly select a bird prefab from the array
                int randomIndex = Random.Range(0, birdPrefabs.Length);
                GameObject selectedBirdPrefab = birdPrefabs[randomIndex];

                // Instantiate and set the bird in the slot
                GameObject newBird = Instantiate(selectedBirdPrefab); // Create a new bird from the selected prefab
                newBird.transform.SetParent(birdSlots[i].transform); // Set the parent to the slot (Bird1/Bird2/Bird3)
                newBird.transform.localPosition = Vector3.zero; // Reset position to the slot's center

                newBird.transform.localScale = new Vector2(2f, 2f);

                Bird bird = newBird.GetComponent<Bird>();
                if (bird != null)
                {
                    bird.slotIndex = i;  // Assign the slot index directly to the bird
                }

                Debug.Log("New bird spawned in Slot " + (i + 1) + " with prefab: " + selectedBirdPrefab.name);

                break; // Exit the loop after spawning one bird
            }
        }
    }
}
