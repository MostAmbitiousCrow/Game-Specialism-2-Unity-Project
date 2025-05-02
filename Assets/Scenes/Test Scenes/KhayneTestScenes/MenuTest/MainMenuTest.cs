using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuTest : MonoBehaviour // Made by Khayne Lutchmun
{   
    [SerializeField] private GameObject FadeObject; // The button to play the game.
    [SerializeField] private Image FadeOut ; // The image that will fade out the screen.

    private void Start()
    {
        FadeOut.color = new Color(0, 0, 0, 0); // Set initial color transparent
        FadeObject.SetActive(false); // Hide the fade object at the start 
    }

    public void playLevel()
    {
        //loads the first level.
        StartCoroutine(AysncPlay());
        Debug.Log("Player loaded Level 1");
    }
    
    IEnumerator AysncPlay()
    {
        FadeObject.SetActive(true); // Show the fade object
        //fade out the screen
        float fadeTime = 1f; // Time to fade out
        
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            FadeOut.color = new Color(0, 0, 0, Mathf.Clamp01(t / fadeTime)); // Fade to black
            yield return null;
        }
        
        FadeOut.color = new Color(0, 0, 0, 1); // Ensure it's fully black

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Level1"); //loads the scene asynchronously.
        asyncLoad.allowSceneActivation = false; // Prevents the scene from activating immediately
        
        //while the scene is loading, wait until it is done.
        while (!asyncLoad.isDone)
        {
            if (asyncLoad.progress >= 0.9f)
            {// If the scene is almost loaded
                asyncLoad.allowSceneActivation = true; // Activate the scene
            }
            yield return null;
        }

        
    }

        public void quit()
    {
        //quits the game, will output a message to the console to show that the the function has worked
        Application.Quit();
        Debug.Log("Player has quit");
    }
}
