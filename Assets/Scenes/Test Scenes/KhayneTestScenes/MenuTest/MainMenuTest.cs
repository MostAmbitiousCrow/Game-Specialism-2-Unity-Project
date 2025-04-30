using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuTest : MonoBehaviour // Made by Khayne Lutchmun
{   
    public void playLevel()
    {
        //loads the first level, you can copy this line and change the scene name to load other levels
        SceneManager.LoadScene("Level1");
        Debug.Log("Player loaded Level 1");
    }
    
        public void quit()
    {
        //quits the game, will output a message to the console to show that the the function has worked
        Application.Quit();
        Debug.Log("Player has quit");
    }
}
