using System.Collections.Generic;
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
                    playerData[i].score = 0;
                    playerData[i].highScore = PlayerPrefs.GetFloat($"P{i + 1}HighScore", 0);
                    playerData[i].lives = 3;
                    playerData[i].kills = 0;
                    playerData[i].deaths = 0;
                    playerData[i].isDead = false;
                }  
                Debug.Log($"Multiplayer detected: Created {playerData.Count} players");
            }
            else
            {
                // Create 1 Player
                playerData.Add(new PlayerData{ playerObject = Instantiate(playerOne)});

                // Set Player Data
                playerData[0].score = 0;
                playerData[0].highScore = PlayerPrefs.GetFloat("HighScore", 0);
                playerData[0].lives = 3;
                playerData[0].kills = 0;
                playerData[0].deaths = 0;
                playerData[0].isDead = false;
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
}

public class PlayerData
{
    public GameObject playerObject;
    public float score;
    public float highScore;
    public int lives;
    public int kills;
    public int deaths;
    public bool isDead;
    public bool isInvincible;
}