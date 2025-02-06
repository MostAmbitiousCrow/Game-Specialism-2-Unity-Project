using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    // Static variable to track if the game is paused
    public static bool Paused = false;
    
    // Reference to the Pause Menu Canvas GameObject
    public GameObject PauseMenuCanvas;

    public GameObject GameUICanvas;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the game is running at normal speed when it starts
        Time.timeScale = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the Escape key is pressed
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle between paused and playing states
            if(Paused)
            {
                Play();
            }
            else
            {
                Stop();
            }
        }
    }

    // Method to pause the game
    void Stop()
    {
        // Activate the Pause Menu Canvas
        PauseMenuCanvas.SetActive(true);

        // Deactivate the Game UI Canvas
        GameUICanvas.SetActive(false);
        
        // Stop the game time
        Time.timeScale = 0f;
        
        // Set the paused state to true
        Paused = true;
    }

    // Method to resume the game
    public void Play()
    {
        // Deactivate the Pause Menu Canvas
        PauseMenuCanvas.SetActive(false);

        // Activate the Game UI Canvas
        GameUICanvas.SetActive(true);
        
        // Resume the game time
        Time.timeScale = 1f;
        
        // Set the paused state to false
        Paused = false;
    }

    // Method to load the main menu scene
    public void MainMenuButton()
    {
        // Load the previous scene in the build index (assumed to be the main menu)
        //TBI: Change this to the main menu scene
    }
}

