using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Game_UI_Manager : MonoBehaviour // By Samuel White
{
    //========================================
    // Used to manage the in-game gameplay and menu UI.
    // Pause menu, results menu, Game UI
    //========================================

    public static Player_Game_UI_Manager instance;

    [Header("===========Player UI Content===========")]
    [SerializeField] Image[] playerHealthBars; // 0 = Player 1, 1 = Player 2
    [SerializeField] Image[] playerFreezeBars; // 0 = Player 1, 1 = Player 2
    [SerializeField] Image[] playerPowerUpIcons;
    
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

    [Header("Player Stats")]
    public PlayerStatContent[] playerStats = new PlayerStatContent[2]; // 0 = Player 1, 1 = Player 2
    [System.Serializable]
    public struct PlayerStatContent
    {
        public TextMeshProUGUI Kills, Lives, Time, Score;
    }



    void Awake()
    {
        instance = this;
        ResetGameUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameData.isGameStarted) return; // Don't update if the game hasn't started yet
        UpdatePlayerHealth();
        UpdatePlayerFreeze();
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

    public void ShowPauseMenu(bool show)
    {
        if (!GameData.canPause) return;
        pauseMenu.SetActive(show);
        GameManager.instance.PauseGame(show);
    }

    public void ShowResultsMenu(bool show)
    {
        resultsMenu.SetActive(show);
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
    }
    #endregion

    public void NextLevel()
    {
        Scene_Loader_Transition.LoadScene(GameData.currentLevel++);
    }

    public void ReturnToMainMenu()
    {
        Scene_Loader_Transition.LoadScene(Scene_Loader_Transition.SceneNames.Main_Menu);
        GameManager.instance.DestroyPlayers();
    }
}
