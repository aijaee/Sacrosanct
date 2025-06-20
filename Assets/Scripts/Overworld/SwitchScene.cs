using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene switching

public class SwitchScene : MonoBehaviour
{
    public string triggerKey = "TriggerActivated"; // Unique key for this trigger

    void Start()
    {
        // Check if the trigger has already been activated
        if (PlayerPrefs.GetInt(triggerKey, 0) == 1)
        {
            gameObject.SetActive(false); // Disable the trigger if it has already been used
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (PlayerPrefs.GetInt(triggerKey, 0) == 0) // Check if the trigger is not yet activated
        {
            PlayerPrefs.SetInt(triggerKey, 1); // Mark the trigger as activated
            PlayerPrefs.Save(); // Ensure the data is saved

            SceneManager.LoadScene(2); // Switch to scene 2
        }
    }
}
