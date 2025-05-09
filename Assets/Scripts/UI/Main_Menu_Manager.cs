using UnityEngine;
using UnityEngine.UI;

public class Main_Menu_Manager : MonoBehaviour
{
    [SerializeField] private RectTransform shutter;

    private Vector3 shutterStartPos;
    private Vector3 shutterEndPos;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject multiplayerMenu;

    // Start is called before the first frame update
    void Start()
    {
        shutterStartPos = shutter.anchoredPosition;
        shutterEndPos = new Vector3(shutterStartPos.x, shutterStartPos.y + 200, shutterStartPos.z); // Move up by 200
    }

    public void PlayGame()
    {
        // Load the game scene
        Scene_Loader_Transition.LoadScene(Scene_Loader_Transition.SceneNames.Level_1);
    }
    public void ShowSettingsMenu()
    {
        settingsMenu.SetActive(true);
        creditsMenu.SetActive(false);
        multiplayerMenu.SetActive(false);
    }
    public void ShowCreditsMenu()
    {
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(true);
        multiplayerMenu.SetActive(false);
    }
    public void ShowMultiplayerMenu()
    {
        settingsMenu.SetActive(false);
        creditsMenu.SetActive(false);
        multiplayerMenu.SetActive(true);
    }
    public void QuitGame()
    {
        Application.Quit(); // Quit the game
        Debug.Log("Player has quit the game.");
    }
}
