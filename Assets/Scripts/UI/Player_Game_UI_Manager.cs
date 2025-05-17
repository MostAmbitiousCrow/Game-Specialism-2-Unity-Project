using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.UI;

public class Player_Game_UI_Manager : MonoBehaviour // By Samuel White
{
    //========================================
    // Used to manage the in-game gameplay and menu UI.
    // Pause menu, results menu, Game UI
    //========================================

    public static Player_Game_UI_Manager instance;

    [Header("===========Player UI Content===========")]
    [SerializeField] GameObject[] playerGameUI;

    [Space(10)]

    [SerializeField] Image[] playerHealthBars; // 0 = Player 1, 1 = Player 2
    [SerializeField] Image[] playerFreezeBars; // 0 = Player 1, 1 = Player 2
    [SerializeField] Image[] playerPowerUpIcons; // 0 = Player 1, 1 = Player 2
    
    [Space(10)]
    
    [SerializeField] GameObject[] playerTwoStats;

    [Space(10)]

    [Header("Power-up Icons")]
    [SerializeField] Sprite[] powerUpIcons;
    public enum PowerUpIcons { None, Gum, Flake, Boomerang, Sprinkles }
    public PowerUpIcons PowerUpIcon {private get; set;}

    [Header("===========Game UI Content===========")]
    [SerializeField] GameObject pauseMenu;
    [Space(10)]
    [SerializeField] GameObject resultsMenu;
    [SerializeField] GameObject[] resultTitles; // 0 = Game Over, 1 = Win
    [Space(10)]
    [SerializeField] GameObject settingsMenu;

    [Space(10)]

    [Header("===========Settings UI Content===========")]
    [SerializeField] GameObject settingsExitButton;

    [Header("Player Stats")]
    public PlayerStatContent[] playerStats = new PlayerStatContent[2]; // 0 = Player 1, 1 = Player 2
    [System.Serializable]
    public struct PlayerStatContent
    {
        public TextMeshProUGUI Kills, Lives, Time, Score;
    }

<<<<<<< HEAD

=======
    private int playerNum; // The Player Number who currently has the UI open
>>>>>>> Level-Editor-Prototype

    void Awake()
    {
        instance = this;
        ResetGameUI();
    }

    // Update is called once per frame
    void Update()
    {
<<<<<<< HEAD
        if (!GameData.isGameStarted) return; // Don't update if the game hasn't started yet
        UpdatePlayerHealth();
        UpdatePlayerFreeze();
=======
        if (GameData.isGameStarted) // Update when the game has started
        {
            UpdateUI();
        }
>>>>>>> Level-Editor-Prototype
    }

    #region Update Player Health

    public void UpdateUI()
    {
        int count = Mathf.Min(playerHealthBars.Length, GameManager.playerData.Count);
        for (int i = 0; i < count; i++)
        {
            // Update Health
            var minHealth = GameManager.playerData[i].characterData.playerHealth.health;
            var maxHealth = GameManager.playerData[i].characterData.playerHealth.maxHealth;
            playerHealthBars[i].fillAmount = minHealth / maxHealth;
            if (minHealth <= 0) playerHealthBars[i].fillAmount = 0f;

            // Update Freeze Meter
            var minMeter = GameManager.playerData[i].characterData.playerShoot.freezeMeter;
            var maxMeter = GameManager.playerData[i].characterData.playerShoot.freezeMeterMax;
            playerFreezeBars[i].fillAmount = minMeter / maxMeter;
            if (minMeter <= 0) playerFreezeBars[i].fillAmount = 0f;

            // Update Score Gameplay UI
            if (GameManager.playerData[i].characterData.playerShoot.freezeMeter <= 0)
            {
                playerFreezeBars[i].fillAmount = 0f;
            }

            playerScoreTexts[i].text = $"p{i} Score: {GameManager.playerData[i].score}";

<<<<<<< HEAD
    public void UpdatePlayerFreeze()
    {
        if (GameData.isMultiplayer)
        {
            for (int i = 0; i < playerFreezeBars.Length - 1; i++)
            {
                if (!GameManager.playerData[i].characterData.playerShoot.freezeModeActive) return; // Return if freeze mode is not active

                playerFreezeBars[i].fillAmount = GameManager.playerData[i].characterData.playerShoot.freezeMeter / GameManager.playerData[i].characterData.playerShoot.freezeMeterMax;
                if (GameManager.playerData[i].characterData.playerShoot.freezeMeter <= 0)
                {
                    playerFreezeBars[i].fillAmount = 0f;
                }
            }
        }
        else
        {
            if (!GameManager.playerData[0].characterData.playerShoot.freezeModeActive) return; // Return if freeze mode is not active

            playerFreezeBars[0].fillAmount = GameManager.playerData[0].characterData.playerShoot.freezeMeter / GameManager.playerData[0].characterData.playerShoot.freezeMeterMax;
            if (GameManager.playerData[0].characterData.playerShoot.freezeMeter <= 0)
            {
                playerFreezeBars[0].fillAmount = 0f;
            }
=======
>>>>>>> Level-Editor-Prototype
        }
    }
    #endregion

    #region Update Player Power-up UI Icon

    public static void UpdatePlayerPowerUp(int playerID, PowerUpIcons icon)
    {
        if (GameData.isMultiplayer)
        {
            instance.playerPowerUpIcons[playerID].sprite = instance.powerUpIcons[(int)icon];
        }
        else
        {
            instance.playerPowerUpIcons[0].sprite = instance.powerUpIcons[(int)icon];
        }
    }
    #endregion

    #region Show Menus

    public void UI_ShowPauseMenu(bool show)
    {
        ShowPauseMenu(show, playerNum);
    }

    public void ShowPauseMenu(bool show, int pNum)
    {
        playerNum = pNum;

        if (!GameData.canPause) return;
        pauseMenu.SetActive(show);
        GameManager.instance.PauseGame(show);
<<<<<<< HEAD
=======
        GameManager.instance.eventSystem.GetComponent<InputSystemUIInputModule>().actionsAsset 
            = GameData.playerInputs[playerNum].actions;
        GameData.playerInputs[playerNum].SwitchCurrentActionMap(show ? "UI" : "Player Movement");
        GameManager.instance.EventSystem_SelectUIButton(pause_resumeButton);
        AudioManager.UpdateMusic(show ? AudioManager.MusicOptions.Pause : AudioManager.MusicOptions.Resume);
>>>>>>> Level-Editor-Prototype
    }

    public void ShowResultsMenu(bool show)
    {
        resultsMenu.SetActive(show);
        GameData.playerInputs[playerNum].SwitchCurrentActionMap(show ? "UI" : "Player Movement");
        if (show)
        {
            if (GameData.isGameOver) resultTitles[0].SetActive(true);
            else resultTitles[1].SetActive(true);
            if (GameData.isMultiplayer) // Show Player 2 stats if Multiplayer is enabled
            {
                for (int i = 0; i < playerTwoStats.Length; i++)
                {
                    playerTwoStats[i].SetActive(true); // Show Player 2 stats
                }
                for (int i = 0; i < playerStats.Length; i++)
                {
                    playerStats[i].Kills.text = GameManager.playerData[i].kills.ToString();
                    playerStats[i].Lives.text = GameManager.playerData[i].lives.ToString();
                    // playerStats[i].Time.text = GameData.gameTime.ToString(); //TODO Get time remaining from the Level Manager
                    playerStats[i].Score.text = GameManager.playerData[i].score.ToString();
                }
            }
            else
            {
                for (int i = 0; i < playerTwoStats.Length; i++)
                {
                    playerTwoStats[i].SetActive(false); // Hide Player 2 stats
                }
            }
        }
        else
        {
            foreach (var title in resultTitles) // Hide all titles
            {
                title.SetActive(false);
            }
        }
    }
    #endregion

    public void ShowSettings(bool show) //TODO
    {
        settingsMenu.SetActive(show);
        GameManager.instance.EventSystem_SelectUIButton(settingsExitButton);
    }

    #region Reset UI

    public void ResetGameUI()
    {
        foreach (var healthBar in playerHealthBars)
        {
            healthBar.fillAmount = 1f;
        }

        foreach (var freezeBar in playerFreezeBars)
        {
            freezeBar.fillAmount = 0f;
        }

        foreach (var powerUpIcon in playerPowerUpIcons)
        {
            powerUpIcon.sprite = null;
        }

        playerGameUI[1].SetActive(GameData.isMultiplayer);
    }
    #endregion

    public void NextLevel()
    {
        //Scene_Loader_Transition.SceneNames scene = GameData.currentLevel++;
        GameData.currentLevel = GameData.currentLevel++;
        Scene_Loader_Transition.LoadScene(GameData.currentLevel);
        Debug.Log($"Next Level: {GameData.currentLevel}");
    }

    public void ReturnToMainMenu()
    {
        Scene_Loader_Transition.LoadScene(Scene_Loader_Transition.SceneNames.Main_Menu);
        GameManager.instance.DestroyPlayers();
    }
}
