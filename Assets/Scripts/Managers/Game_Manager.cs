using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour // By Samuel White
{
    //========================================
    // The Game Manager class.
    // This class is used to store the game data.
    //========================================

    public static GameManager instance;
    public static List<PlayerData> playerData = new();

    public GameObject playerOne;
    public GameObject playerTwo;

    public PlayerInputManager playerInputManager;

    [SerializeField] private Transform playerCamera;

    public enum ScoreContext { Enemy_Hit, Enemy_Defeated, Player_Hit, Player_Defeated, PowerUp_Obtained, Powerup_Hit, Powerup_Crate_Smashed, Enemy_Frozen, Enemy_Frozen_Smashed, }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform.root);
        GameData.isMultiplayer = true;
    }

    private void Start()
    {
        if(playerCamera == null) playerCamera = Camera.main.transform;
        CreateInitialPlayers();
    }

    #region Create Players
    public void CreateInitialPlayers()
    {
        playerData.Clear();

        // Create Player Data

            // Create Players - Add to playerData list
            if (GameData.isMultiplayer && playerInputManager.playerCount > 1)
            {
                // Create 2 Players | Add to player list
                GameObject o = new PlayerData{ playerObject = Instantiate(playerOne)}.playerObject;
                o.name = "Player 1";
                Player_Data c = o.GetComponent<Player_Data>();
                c.playerNumber = 0;
                c.view = playerCamera;
                GameData.players.Add(o.transform);

                o = new PlayerData{ playerObject = Instantiate(playerTwo)}.playerObject;
                o.name = "Player 2";
                c = o.GetComponent<Player_Data>();
                c.playerNumber = 0;
                c.view = playerCamera;
                GameData.players.Add(o.transform);

                // Set Player Data
                for (int i = 0; i < playerData.Count; i++)
                {
                    playerData[i].playerData = playerData[i].playerObject.GetComponent<Player_Data>();
                    playerData[i].score = 0;
                    playerData[i].highScore = PlayerPrefs.GetFloat($"P{i + 1}HighScore", 0);
                    playerData[i].lives = 3;
                    playerData[i].kills = 0;
                    playerData[i].deaths = 0;
                    playerData[i].isDead = false;
                    playerData[i].playerData.playerNumber = i;
                }  
                Debug.Log($"Multiplayer detected: Created {playerData.Count} players");
            }
            else
            {
                // Create 1 Player
                playerData.Add(new PlayerData{ playerObject = Instantiate(playerOne)});

                // Set Player Data
                playerData[0].playerData = playerData[0].playerObject.GetComponent<Player_Data>();
                playerData[0].score = 0;
                playerData[0].highScore = PlayerPrefs.GetFloat("HighScore", 0);
                playerData[0].lives = 3;
                playerData[0].kills = 0;
                playerData[0].deaths = 0;
                playerData[0].isDead = false;
                playerData[0].playerData.playerNumber = 0;
                Debug.Log($"Singleplayer detected: Created {playerData.Count} player");
            }

        StartGame(); // Start the game (Temporary) // TODO remove once Khayne has implemented loading sequence
    }
    #endregion

    #region Player Join Event
    // public void PlayerJoinEvent()
    // {

    // }

    public void OnPlayerJoined()
    {
        Debug.Log("Player Joined");
    }
    #endregion

    #region Set Game Difficulty
    public void SetGameDifficulty(GameData.Difficulty difficulty)
    {
        GameData.gameDifficulty = difficulty;

        switch (difficulty)
        {
            case GameData.Difficulty.Easy:
                // TODO - Update Variables based on Easy Difficulty
                break;
            case GameData.Difficulty.Normal:
                // TODO - Update Variables based on Normal Difficulty
                break;
            case GameData.Difficulty.Hard:
                // TODO - Update Variables based on Hard Difficulty
                break;
            default:
                break;
        }
    }
    #endregion

    #region Award Score
    public void AwardScore(int playerID, ScoreContext context)
    {
        switch (context)
        {
            case ScoreContext.Enemy_Hit:
                playerData[playerID].score += 10 * playerData[playerID].scoreMultiplier; // Add 10 points for hitting an enemy
                break;
            case ScoreContext.Enemy_Defeated:
                playerData[playerID].score += 100 * playerData[playerID].scoreMultiplier; // Add 100 points for defeating an enemy
                break;
            case ScoreContext.Player_Hit:
                playerData[playerID].score -= 20 * playerData[playerID].scoreMultiplier; // Subtract 20 points for hitting a player
                break;
            case ScoreContext.Player_Defeated:
                playerData[playerID].score -= 200 * playerData[playerID].scoreMultiplier; // Subtract 200 points for player defeat
                break;
            case ScoreContext.PowerUp_Obtained:
                playerData[playerID].score += 50 * playerData[playerID].scoreMultiplier; // Add 50 points for obtaining a power-up
                break;
            case ScoreContext.Powerup_Hit:
                playerData[playerID].score += 20 * playerData[playerID].scoreMultiplier; // Add 20 points for hitting an enemy with a power-up
                break;
            case ScoreContext.Powerup_Crate_Smashed:
                playerData[playerID].score += 50 * playerData[playerID].scoreMultiplier; // Add 50 points for smashing a power-up crate
                break;
            case ScoreContext.Enemy_Frozen:
                playerData[playerID].score += 50 * playerData[playerID].scoreMultiplier; // Add 50 points for freezing an enemy
                break;
            case ScoreContext.Enemy_Frozen_Smashed:
                playerData[playerID].score += 200 * playerData[playerID].scoreMultiplier; // Add 200 points for smashing a frozen enemy
                break;
        }
    }
    #endregion

    #region Set High Scores
    public void SetHighScore()
    {
        if (GameData.isMultiplayer)
        {
            for (int i = 0; i < playerData.Count; i++)
            {
                if (playerData[i].score > playerData[i].highScore)
                {
                    playerData[i].highScore = playerData[i].score;
                    PlayerPrefs.SetFloat($"P{i + 1}HighScore", playerData[i].highScore);
                }
            }
        }
        else
        {
            if (playerData[0].score > GameData.highScore)
            {
                GameData.highScore = playerData[0].score;
                PlayerPrefs.SetFloat("HighScore", GameData.highScore);
            }
        }
    }
    #endregion

    #region Reset High Scores
    public void ResetHighScores()
    {
        if (GameData.isMultiplayer)
        {
            for (int i = 0; i < playerData.Count; i++)
            {
                playerData[i].highScore = 0;
                PlayerPrefs.SetFloat($"P{i + 1}HighScore", playerData[i].highScore);
            }
        }
        else
        {
            GameData.highScore = 0;
            PlayerPrefs.SetFloat("HighScore", GameData.highScore);
        }
    }
    #endregion

    #region Start Game
    public void StartGame()
    {
        GameData.isGameStarted = true;
        GameData.isGameOver = false;
        GameData.isGameFinished = false;
        GameData.isPaused = false;
    }
    #endregion

    #region Pause Game
    public void PauseGame(bool pause)
    {
        if (!pause)
        {
            GameData.isPaused = false;
            Time.timeScale = 1;
            // TODO - Toggle Pause Menu
            // UIManager.instance.PauseMenu();
        }
        else
        {
            GameData.isPaused = true;
            Time.timeScale = 0;
        }
    }
    #endregion

    #region Game Over
    public void GameOver()
    {
        GameData.isGameOver = true;
        GameData.isGameFinished = true;
        GameData.isPaused = true;
        // TODO - Toggle Game Over Menu
        // UIManager.instance.GameOverMenu();
    }
    #endregion

    #region Restart Game
    public void RestartGame()
    {
        GameData.isGameOver = false;
        GameData.isGameFinished = false;
        GameData.isPaused = false;

        if (GameData.isMultiplayer)
        {
            foreach (var item in playerData)
            {
                item.score = 0;
                item.lives = 3;
                item.kills = 0;
                item.deaths = 0;
                item.isDead = false;
            }
        }
        else
        {
            playerData[0].score = 0;
            playerData[0].lives = 3;
            playerData[0].kills = 0;
            playerData[0].deaths = 0;
            playerData[0].isDead = false;
        }

        // TODO - Restart the game
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion
}

#region Game Data

public static class GameData
{
    public static bool isMultiplayer;

    public static bool isGameOver;
    public static bool isPaused;
    public static bool isGameStarted;
    public static bool isGameFinished;

    public enum Difficulty { Easy, Normal, Hard }
    public static Difficulty gameDifficulty;

    public static float highScore;
    public static List<Transform> players = new();

    public static int currentLevel = 0;
}
#endregion

#region Player Data

public class PlayerData
{
    public GameObject playerObject;

    public Player_Data playerData;

    public float score;
    public float highScore;
    public float scoreMultiplier = 1;

    public int lives;
    public int kills;
    public int deaths;
    
    public bool isDead;
    public bool isInvincible;
}
#endregion

#region Global Text Data
public static class GlobalTextData
{
    public static List<TextMeshProUGUI> textComponents;
    public static TMP_FontAsset DyslexFont { get; set; }
    public static TMP_FontAsset DefaultFont { get; set; }

    public static void UpdateGlobalFonts()
    {
        foreach (var item in textComponents)
        {
            item.font = Settings_Manager.dyslexiaFont ? DyslexFont : DefaultFont;
        }
    }
}
#endregion