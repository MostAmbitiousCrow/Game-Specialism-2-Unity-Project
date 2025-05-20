using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Runtime.CompilerServices;

public class Settings_Menu_Manager : MonoBehaviour // By Samuel White
{
    //========================================
    // Settings Menu Content. 
    // Shared with Main Menu Manager and the Gameplay UI.
    //========================================
    
    [Header("Volume Scrollers")]
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider enemyVolumeSlider;
    [SerializeField] private Slider playerVolumeSlider;
    [SerializeField] private Slider interfaceVolumeSlider;

    [Header("Accessibility Components")]
    [SerializeField] private Slider gameSpeedSlider;
    [SerializeField] private Slider damageFlashSlider;
    [SerializeField] private Toggle gamepadVibrationToggle;
    [SerializeField] private Toggle autoshootToggle;
    [SerializeField] private Toggle dyslexiaFontToggle;
    [SerializeField] private Toggle playerInvincibleToggle;
    [SerializeField] private TMP_Dropdown colourblindDropDown;

    [Header("Menus")]
    [SerializeField] private GameObject settingsAudioMenu;
    [SerializeField] private GameObject settingsAccessibilityMenu;
    [SerializeField] private GameObject settingsButtonMappingMenu;

    void Start()
    {
        UpdateUI();
        if(GameData.currentLevel == Scene_Loader_Transition.SceneNames.Main_Menu) gameObject.SetActive(false);
    }

    #region Settings Menu Content
    // ============================= Settings Menu Content =============================

    public void SaveSettings() => Settings_Manager.SaveSettings();

    // Volume Control

    public void ToggleVolumeMenu(bool state)
    {
        settingsAudioMenu.SetActive(state);
        settingsAccessibilityMenu.SetActive(!state);
        settingsButtonMappingMenu.SetActive(!state);
    }

    public void MasterVolumeSlider()
    {
        Settings_Manager.masterVolume = Mathf.Clamp(masterVolumeSlider.value / 10, .0001f, 1);
        AudioManager.UpdateAudioManagerVolume();
    }

    public void MusicVolumeSlider()
    {
        Settings_Manager.musicVolume = Mathf.Clamp(musicVolumeSlider.value / 10, .0001f, 1);
        AudioManager.UpdateAudioManagerVolume();
    }

    public void EnemyVolumeSlider()
    {
        Settings_Manager.enemyVolume = Mathf.Clamp(enemyVolumeSlider.value / 10, .0001f, 1);
        AudioManager.UpdateAudioManagerVolume();
    }

    public void PlayerVolumeSlider()
    {
        Settings_Manager.playerVolume = Mathf.Clamp(playerVolumeSlider.value / 10, .0001f, 1);
        AudioManager.UpdateAudioManagerVolume();
    }

    public void InterfaceVolumeSlider()
    {
        Settings_Manager.interfaceVolume = Mathf.Clamp(interfaceVolumeSlider.value / 10, .0001f, 1);
        AudioManager.UpdateAudioManagerVolume();
    }

    // Accessiblity Triggers

    public void ToggleAccessibilityMenu(bool state)
    {
        settingsAccessibilityMenu.SetActive(state);
        settingsAudioMenu.SetActive(!state);
        settingsButtonMappingMenu.SetActive(!state);
    }

        public void GameSpeedSlider()
        {
            Settings_Manager.SetGameSpeed(Mathf.Clamp(gameSpeedSlider.value / 10, .1f, 1f));
        }

        public void DamageFlashSlider()
        {
            Settings_Manager.SetDamageFlashIntensity(Mathf.Clamp(damageFlashSlider.value / 10, 0.001f, 1f));
        }

        public void ControllerVibration()
        {
            Settings_Manager.SetControllerVibration(gamepadVibrationToggle.isOn);
            foreach (var item in GameData.controllerRumbles) item.StartRumble(1, .2f, 1);
        }

        public void AutoShoot()
        {
            Settings_Manager.SetPlayerAutoShoot(autoshootToggle.isOn);
        }

        public void DyslexiaFont()
        {
            Settings_Manager.SetDyslexiaFont(dyslexiaFontToggle.isOn);
        }

        public void PlayerInvincible()
        {
            Settings_Manager.SetPlayerInvincibility(playerInvincibleToggle.isOn);
        }

        public void SelectColourBlindMode()
        {
            Settings_Manager.SetColourBlindMode((Settings_Manager.ColourBlindMode)colourblindDropDown.value);
        }

    #region Settings Menu Content
    // ============================= Settings Menu Content =============================

    public void ToggleInputMappingMenu(bool state) // TODO Unfinished
    {
        settingsButtonMappingMenu.SetActive(state);
        settingsAccessibilityMenu.SetActive(!state);
        settingsAudioMenu.SetActive(!state);
    }
    #endregion

    // ============================= Other Stuff =============================

    // Reset Settings
    public void ResetSettings()
    {
        Settings_Manager.SetDefaultSettings();
        UpdateUI();
    }
    #endregion

    public void UpdateUI()
    {
        // Update UI to Current Settings
        masterVolumeSlider.value = (int)Settings_Manager.masterVolume * 10;
        musicVolumeSlider.value = (int)Settings_Manager.musicVolume * 10;
        enemyVolumeSlider.value = (int)Settings_Manager.enemyVolume * 10;
        playerVolumeSlider.value = (int)Settings_Manager.playerVolume * 10;
        interfaceVolumeSlider.value = (int)Settings_Manager.interfaceVolume * 10;

        gameSpeedSlider.value = Mathf.Clamp((int)Settings_Manager.gameSpeed * 10, 1f, 10f);
        damageFlashSlider.value = Mathf.Clamp((int)Settings_Manager.damageFlashIntensity * 10, 1f, 10f);
        gamepadVibrationToggle.isOn = Settings_Manager.controllerVibration;
        autoshootToggle.isOn = Settings_Manager.playerAutoShoot;
        dyslexiaFontToggle.isOn = Settings_Manager.dyslexiaFont;
        playerInvincibleToggle.isOn = Settings_Manager.playerInvicible;
        colourblindDropDown.value = (int)Settings_Manager.colourBlindMode;
    }
}
