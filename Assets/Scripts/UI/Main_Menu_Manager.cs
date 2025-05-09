using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Main_Menu_Manager : MonoBehaviour
{
    [SerializeField] private RectTransform shutter;

    [Header("Volume Scrollers")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider enemyVolumeSlider;
    [SerializeField] private Slider playerVolumeSlider;
    [SerializeField] private Slider interfaceVolumeSlider;

    [Header("Accessibility Components")]
    [SerializeField] private Slider gameSpeedSlider;
    [SerializeField] private Toggle gamepadVibrationToggle;
    [SerializeField] private Toggle autoshootToggle;
    [SerializeField] private Toggle dyslexiaFontToggle;
    [SerializeField] private TMP_Dropdown colourblindDropDown;


    private Vector3 shutterStartPos;
    private Vector3 shutterEndPos;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private GameObject settingsAudioMenu;
    [SerializeField] private GameObject settingsAccessibilityMenu;
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

    // Show Menus
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

    // Quit Game
    public void QuitGame()
    {
        Application.Quit(); // Quit the game
        Debug.Log("Player has quit the game.");
    }

    // Multiplayer Menu Content
    public void AddPlayer()
    {

    }

    // Settings Menu Content

        // Volume Control

    public void ToggleVolumeMenu(bool state)
    {
        settingsAudioMenu.SetActive(state);
        settingsAccessibilityMenu.SetActive(!state);
    }

        public void MasterVolumeSlider()
        {
            Settings_Manager.masterVolume = masterVolumeSlider.value;
        }

        public void MusicVolumeSlider()
        {
            Settings_Manager.musicVolume = musicVolumeSlider.value;
        }

        public void EnemyVolumeSlider()
        {
            Settings_Manager.enemyVolume = enemyVolumeSlider.value;
        }

        public void PlayerVolumeSlider()
        {
        Settings_Manager.playerVolume = playerVolumeSlider.value;
        }

        public void InterfaceVolumeSlider()
        {
        Settings_Manager.interfaceVolume = interfaceVolumeSlider.value;
        }

    // Accessiblity Triggers

    public void ToggleAccessibilityMenu(bool state)
    {
        settingsAccessibilityMenu.SetActive(state);
        settingsAudioMenu.SetActive(!state);
    }

        public void GameSpeedSlider()
        {
            Settings_Manager.gameSpeed = gameSpeedSlider.value;
        }

        public void ControllerVibration()
        {
            Settings_Manager.controllerVibration = gamepadVibrationToggle.isOn;
        }

        public void AutoShoot()
        {
            Settings_Manager.playerAutoShoot = autoshootToggle.isOn;
        }

        public void DyslexiaFont()
        {
            Settings_Manager.SetDyslexiaFont(dyslexiaFontToggle.isOn);
        }

        public void SelectColourBlindMode()
        {
            Settings_Manager.SetColourBlindMode((Settings_Manager.ColourBlindMode)colourblindDropDown.value);
        }

    // Reset Options
    
    public void ResetSettings()
    {
        Settings_Manager.SetDefaultSettings();

        masterVolumeSlider.value = Settings_Manager.masterVolume;

    }

}
