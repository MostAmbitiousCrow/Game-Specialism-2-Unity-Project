using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuTest : MonoBehaviour
{   // This script is used to load the different scenes in the game.

    // All it needs is to be attached to an object, then you can call the functions from a button, if needed I can just show how.

    // All the scene names NEED to be the same in the code as they are in unity otherwise they WILL NOT work.

    // Lastly the scene need to be added into the build settings, you can do this by going to File -> Build Settings -> and then drag and drop them in or do add open scenes if you have them open.

//  Here is an example of how to do level 2!

//     public void playLevel2()
//    {
//        SceneManager.LoadScene("Level2");
//        Debug.Log("Player loaded Level2");
//    }

    public void playLevel1()
    {
        //loads the first level, you can copy this line and change the scene name to load other levels
        SceneManager.LoadScene("TestGameScene");
        Debug.Log("Player loaded Level1");
    }

    public void playMainMenu()
    { 
        //loads the main menu, this can be tied to a button to return to the main menu such as for a pause menu
        SceneManager.LoadScene("Test1Khayne");
        Debug.Log("Player loaded MainMenu");
    }

    public void quit()
    {
        //quits the game, will output a message to the console to show that the the function has worked
        Application.Quit();
        Debug.Log("Player has quit!");
    }
}
