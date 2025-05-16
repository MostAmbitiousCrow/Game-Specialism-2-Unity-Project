using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.InputSystem;
using System;
using TMPro;

public class Main_Menu_Manager : MonoBehaviour // By Samuel White
{
    //========================================
    // Main Menu Manager:
    // Manages the main menu, including the settings menu, credits menu, and multiplayer menu.
    // Also triggers the Start Game Functions
    //========================================

    #region Variables
    [Header("Transition Components")]
    [SerializeField] private RectTransform shutter;
    [SerializeField] private float shutterTransitionTime = 1f;
    private Vector3 shutterStartPos;
    private Vector3 shutterEndPos;
    [Space(10)]
    [SerializeField] private Image clickBlocker;
    [Space(10)]
    [SerializeField] Settings_Menu_Manager settingsMenuManager;

    [Header("Menu Components")]

    [SerializeField] MenuData[] menuDatas; // 0 = Settings | 1 = Credits | 2 = Multiplayer | 3 = Main | 4 = none
    [Serializable]
    public struct MenuData
    {
        public string name;
        public GameObject menu;
        public GameObject enterButton;
    }
    [Space(10)]
    [SerializeField] private GameObject creditsMenu;
    [SerializeField] private GameObject multiplayerMenu;

    #endregion

    void Start()
    {
        shutterStartPos = shutter.anchoredPosition;
        shutterEndPos = new Vector3(shutterStartPos.x, 0, shutterStartPos.z); // Move up by 200
        GameManager.instance.eventSystem.SetSelectedGameObject(menuDatas[3].enterButton);
        playerInputManager.JoinPlayer();
        AudioManager.PlayMusic(AudioManager.MusicOptions.Play, 1, 2, MusicCategory.MusicSoundTypes.MainMenu);
        settingsMenuManager.UpdateUI();
        GameManager.UpdateGlobalFonts();
    }

    #region Menu Navigation
    // ============================= Menu Navigation =============================
    // 0 = Settings | 1 = Credits | 2 = Multiplayer | 3 = Main | 4 = none

    public void OpenMainMenu(int oldMenu) 
    {
        StartCoroutine(ShutterTransition(3, oldMenu));
    }
    public void OpenSettingsMenu()
    {
        StartCoroutine(ShutterTransition(0, 3));
    }
    public void OpenCreditsMenu()
    {
        StartCoroutine(ShutterTransition(1, 3));
    }
    public void OpenMultiplayerMenu()
    {
        StartCoroutine(ShutterTransition(2, 3));
    }

    // Quit Game
    public void QuitGame()
    {
        Application.Quit(); // Quit the game
        Debug.Log("Player has quit the game.");
    }
    #endregion
    #region Multiplayer Menu Content
    // ============================= Multiplayer Menu Content=============================

    [Header("Multiplayer Menu Components")]
    [SerializeField] private GameObject[] playerBoxes; // 0 = Player 1, 1 = Player 2
    [SerializeField] private bool multiplayerMenuOpen;
    [SerializeField] private Button startButton;
    [SerializeField] TMP_Dropdown difficultyDropDown;

    [Header("Player Input")]
    [SerializeField] PlayerInputManager playerInputManager;
    [SerializeField] int playerCount = 0;

    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player Joined");
        AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Player_Joined);
        playerBoxes[Mathf.Clamp(playerCount, 0, 1)].SetActive(true);
        playerInput.ActivateInput();
        GameData.playerInputs.Add(playerInput);
        playerInput.transform.SetParent(GameManager.instance.playersFolder.transform);
        playerCount = GameData.playerInputs.Count;
        playerInput.gameObject.name = $"Player {playerCount}";
        playerInput.neverAutoSwitchControlSchemes = true;
        if (playerCount >= 1) startButton.interactable = true;
    }

    public void DisconnectAllPlayers()
    {
        int loops = playerCount;
        Debug.Log($"Disconnected {GameData.playerInputs.Count} PLayers");
        if (GameData.playerInputs.Count > 0)
        {
            for (int i = 0; i < loops; i++)
            {
                if (GameData.playerInputs[0] == null)
                {
                    GameData.playerInputs.RemoveAt(0);
                    return;
                }
                Debug.Log($"Destroyed { GameData.playerInputs[0] }");
                Destroy(GameData.playerInputs[0].gameObject);
                playerBoxes[i].SetActive(false);
            }
        }
        GameData.playerInputs.Clear();
        foreach (var item in playerBoxes) item.SetActive(false);

        playerCount = 0;
        GameManager.instance.eventSystem.SetSelectedGameObject(menuDatas[2].enterButton);
    }

    public void OnPlayerLeft(PlayerInput playerInput)
    {
        Debug.Log("Player Disconnected");
        AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Player_Left);
        playerBoxes[Mathf.Clamp(playerCount, 0, 1)].SetActive(false);
        Destroy(playerInput.gameObject);
        GameData.playerInputs.Remove(playerInput);
        playerCount = GameData.playerInputs.Count;
        if (playerCount < 1) startButton.interactable = false;
    }

    public void PlayGame()
    {
        if (!multiplayerMenuOpen) return;
        GameData.isMultiplayer = playerCount > 1; // Decide if it's multiplayer or not based on player count

        // Load the game scene
        AudioManager.PlayMusic(AudioManager.MusicOptions.Stop, 1, 0, MusicCategory.MusicSoundTypes.None);
        Scene_Loader_Transition.LoadScene(Scene_Loader_Transition.SceneNames.Level_1);
    }

    public void AddPlayer()
    {
        if (!multiplayerMenuOpen) return;
        playerCount++;
        playerBoxes[Mathf.Clamp(playerCount - 1, 0, 1)].SetActive(true);
        if (playerCount >= 1) startButton.interactable = true;
    }

    #endregion

    #region Difficulty Select

    public void SelectDifficulty()
    {
        GameManager.instance.SetGameDifficulty((GameData.Difficulty)difficultyDropDown.value);
    }

    #endregion

    #region Transition Function
    // ============================= Transition Function =============================

    IEnumerator ShutterTransition(int newMenu, int oldMenu)
    {
        clickBlocker.raycastTarget = true;
        GameManager.instance.eventSystem.SetSelectedGameObject(null);

        for (float t = 0; t < shutterTransitionTime; t += Time.unscaledDeltaTime)
        {
            float alpha = Mathf.Clamp01(t / shutterTransitionTime);
            shutter.anchoredPosition = Vector3.Lerp(shutterStartPos, shutterEndPos, alpha);
            yield return null;
        }
        shutter.anchoredPosition = shutterEndPos; // Set the shutter position to the end position
        yield return new WaitForSecondsRealtime(0.5f);

        if (newMenu != 4) menuDatas[newMenu].menu.SetActive(true);
        if (oldMenu != 4) menuDatas[oldMenu].menu.SetActive(false);

        if (newMenu == 2) // 0 = Settings | 1 = Credits | 2 = Multiplayer | 3 = Main | 4 = none
        {
            multiplayerMenuOpen = true;
            playerInputManager.EnableJoining();
        }
        else
        {
            multiplayerMenuOpen = false;
            playerInputManager.DisableJoining();
            // RemoveAllPlayers();
        }

        for (float t = 0; t < shutterTransitionTime; t += Time.unscaledDeltaTime)
        {
            float alpha = Mathf.Clamp01(t / shutterTransitionTime);
            shutter.anchoredPosition = Vector3.Lerp(shutterEndPos, shutterStartPos, alpha);
            yield return null;
        }
        shutter.anchoredPosition = shutterStartPos; // Reset the shutter position
        clickBlocker.raycastTarget = false;
        GameManager.instance.eventSystem.SetSelectedGameObject(menuDatas[newMenu].enterButton);
        yield break;
    }
    #endregion

    #region Play Sounds
    public void PlaySound_UIHover() => AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Button_Hover, .5f);
    public void PlaySound_UIPress() => AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Button_Press, .5f);
    public void PlaySound_UIBack() => AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Button_Back, .5f);
    public void PlaySound_UIStartGame() => AudioManager.PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes.Button_GameStart, .5f);

    #endregion
}
