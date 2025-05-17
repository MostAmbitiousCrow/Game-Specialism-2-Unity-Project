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
    [SerializeField] TextMeshProUGUI[] playerScoreTexts; // 0 = Player 1, 1 = Player 2
    
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

    [Header("===========Game UI Content===========")]
    [SerializeField] GameObject pause_resumeButton;
    [Space(10)]
    [SerializeField] GameObject results_NextLevel;
    [SerializeField] GameObject results_RestartGameButton;
    [SerializeField] GameObject results_MainMenuButton;

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

    private int playerNum; // The Player Number who currently has the UI open

    void Awake()
    {
        if (instance == null) instance = this;
        ResetGameUI();
    }

    private void Update()
    {
        if (GameData.isGameStarted) // Update when the game has started
        {
            UpdatePlayerHealth();
            UpdatePlayerFreeze();
            UpdatePlayerScore();
        }
    }

    #region Update Player Health

    public void UpdatePlayerHealth()
    {
        if (GameData.isMultiplayer)
        {
            for (int i = 0; i < playerHealthBars.Length - 1; i++)
            {
                playerHealthBars[i].fillAmount = GameManager.playerData[i].characterData.playerHealth.health / GameManager.playerData[i].characterData.playerHealth.maxHealth;
                if (GameManager.playerData[i].characterData.playerHealth.health <= 0) { playerHealthBars[i].fillAmount = 0f; }
            }
        }
        else
        {
            playerHealthBars[0].fillAmount = GameManager.playerData[0].characterData.playerHealth.health / GameManager.playerData[0].characterData.playerHealth.maxHealth;
            if (GameManager.playerData[0].characterData.playerHealth.health <= 0) { playerHealthBars[0].fillAmount = 0f; }
        }
    }
    #endregion

    #region Update Player Freeze Bars

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
        }
    }
    #endregion

    #region Update Player Score

    public void UpdatePlayerScore()
    {
        if (GameData.isMultiplayer)
        {
            for (int i = 0; i < playerScoreTexts.Length; i++)
            {
                playerScoreTexts[i].text = $"P{i} Score: {GameManager.playerData[0].score}";
            }
        }
        else
        {
            playerScoreTexts[0].text = $"P1 Score: {GameManager.playerData[0].score}";
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
        GameManager.instance.eventSystem.GetComponent<InputSystemUIInputModule>().actionsAsset 
            = GameData.playerInputs[playerNum].actions;
        GameData.playerInputs[playerNum].SwitchCurrentActionMap(show ? "UI" : "Player Movement");
        GameManager.instance.EventSystem_SelectUIButton(pause_resumeButton);
        AudioManager.UpdateMusic(show ? AudioManager.MusicOptions.Pause : AudioManager.MusicOptions.Resume);
    }

    public void ShowResultsMenu(bool show)
    {
        resultsMenu.SetActive(show);
        if (show)
        {
            GameData.canPause = false;

            if (GameData.isGameOver)
            {
                resultTitles[0].SetActive(true);
                GameManager.instance.EventSystem_SelectUIButton(results_RestartGameButton);
            }
            else
            {
                resultTitles[1].SetActive(true);
                GameManager.instance.EventSystem_SelectUIButton(results_NextLevel);
            }

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
                playerStats[0].Kills.text = GameManager.playerData[0].kills.ToString();
                playerStats[0].Lives.text = GameManager.playerData[0].lives.ToString();
                // playerStats[0].Time.text = GameData.gameTime.ToString(); //TODO Get time remaining from the Level Manager
                playerStats[0].Score.text = GameManager.playerData[0].score.ToString();
            }
        }
        else
        {
            GameManager.instance.EventSystem_SelectUIButton(null);
            GameManager.instance.PauseGame(false);
            foreach (var title in resultTitles) // Hide all titles
            {
                title.SetActive(false);
            }
        }
    }
    #endregion

    public void ShowSettings(bool show)
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
    }
}
