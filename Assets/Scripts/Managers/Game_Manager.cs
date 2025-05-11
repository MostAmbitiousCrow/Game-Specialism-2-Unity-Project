using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour // By Samuel White
{
    //========================================
    // The Game Manager class.
    // This class is used to store the game data.
    //========================================

    public static GameManager instance;
    public static List<PlayerData> playerData = new();

    public GameObject[] playerPrefabs;

    public EventSystem eventSystem;

    [SerializeField] private Transform playerCamera;

    public enum ScoreContext { Enemy_Hit, Enemy_Defeated, Player_Hit, Player_Defeated, PowerUp_Obtained, Powerup_Hit, Powerup_Crate_Smashed, Enemy_Frozen, Enemy_Frozen_Smashed, }

    public GameObject playersFolder;

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(transform.root);
        GameData.isMultiplayer = true;
    }

    private void Start()
    {
        playersFolder = new()
        {
            name = "----Players Folder----"
        };
        playersFolder.transform.parent = transform.root;
        Settings_Manager.instance.LoadSettings();
    }

    #region Create Players
    // ======================================== Create Players ========================================
    public void CreateInitialPlayers()
    {
        playerData.Clear();

        // Create Player Data
        Vector3[] spawnPositions = new Vector3[]
        {
            new (-4, 0, 0),
            new (4, 0, 0)
        };

        // Create Players - Add to playerData list
        if (GameData.isMultiplayer)
        {
            // Create the 2 Players
            for (int i = 0; i < 2; i++)
            {
                PlayerData data = new();
                GameObject o = Instantiate(playerPrefabs[i]);
                data.playerObject = o;
                o.name = $"Player {i + 1}";
                o.transform.position = spawnPositions[i];
                DontDestroyOnLoad(o);

                Player_Character_Data character_Data = o.GetComponent<Player_Character_Data>();
                character_Data.playerNumber = i;
                character_Data.view = playerCamera;

                PlayerInput playerInput = GameData.playerInputs[i];
                o.transform.SetParent(playerInput.transform);
                // newPlayerInput.actions = playerInput.actions;
                // newPlayerInput.defaultControlScheme = playerInput.defaultControlScheme;
                playerInput.notificationBehavior = PlayerNotifications.BroadcastMessages;
                playerInput.SwitchCurrentActionMap("Player Movement");
                data.playerInput = playerInput;
                data.playerInput.neverAutoSwitchControlSchemes = true;

                GameData.players.Add(o.transform);
                playerData.Add(data);

                // Set Player Data
                playerData[i].characterData = playerData[i].playerObject.GetComponent<Player_Character_Data>();
                playerData[i].score = 0;
                playerData[i].highScore = PlayerPrefs.GetFloat($"P{i + 1}HighScore", 0);
                playerData[i].lives = 3;
                playerData[i].kills = 0;
                playerData[i].deaths = 0;
                playerData[i].isDead = false;
                playerData[i].characterData.playerNumber = i;
                playerData[i].playerInput = GameData.playerInputs[i];
            }
            Debug.Log($"Multiplayer detected: Created {playerData.Count} players");
        }
        else
        {
            // Create 1 Player
            PlayerData data = new();
            GameObject o = Instantiate(playerPrefabs[0]);
            data.playerObject = o;
            o.name = $"Player 1";
            playerData.Add(data);

            Player_Character_Data pData = o.GetComponent<Player_Character_Data>();
            pData.playerNumber = 0;
            pData.view = playerCamera;

            PlayerInput playerInput = GameData.playerInputs[0];
            PlayerInput newPlayerInput = o.AddComponent<PlayerInput>();
            newPlayerInput.actions = playerInput.actions;
            newPlayerInput.defaultControlScheme = playerInput.defaultControlScheme;
            newPlayerInput.SwitchCurrentActionMap("Player Movement");
            playerData[0].playerInput = newPlayerInput;

            GameData.players.Add(o.transform);

            // Set Player Data
            playerData[0].characterData = playerData[0].playerObject.GetComponent<Player_Character_Data>();
            playerData[0].score = 0;
            playerData[0].highScore = PlayerPrefs.GetFloat("HighScore", 0);
            playerData[0].lives = 3;
            playerData[0].kills = 0;
            playerData[0].deaths = 0;
            playerData[0].isDead = false;
            playerData[0].characterData.playerNumber = 0;
            playerData[0].playerInput = GameData.playerInputs[0];
            Debug.Log($"Singleplayer detected: Created {playerData.Count} player");
        }
    }
    #endregion

    #region Reset Players
    public void DestroyPlayers()
    {
        for (int i = 0; i < GameData.playerInputs.Count; i++)
        {
            Destroy(GameData.playerInputs[0].gameObject);
        }
    }
    #endregion

    #region Set Game Difficulty
    // ======================================== Set Game Difficulty ========================================
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

    #region Global Text Data
    // ======================================== Global Text Data ========================================
    public List<TextMeshProUGUI> textComponents = new();
    public TMP_FontAsset DyslexFont;
    public TMP_FontAsset DefaultFont;

    public static void UpdateGlobalFonts()
    {
        foreach (var item in instance.textComponents)
        {
            item.font = Settings_Manager.dyslexiaFont ? instance.DyslexFont : instance.DefaultFont;
        }
    }
#endregion

    #region Award Score
    // ======================================== Award Score ========================================
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
                playerData[playerID].score -= 20 * playerData[playerID].scoreMultiplier; // Subtract 20 points for player getting hit
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
    // ======================================== Set High Scores ========================================
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
    // ======================================== Reset High Scores ========================================
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
    // ======================================== Start Game ========================================
    public void StartGame()
    {
        GameData.isGameStarted = true;
        GameData.isGameOver = false;
        GameData.isGameFinished = false;
        GameData.isPaused = false;
        GameData.canPause = true;
        CreateInitialPlayers();
        AudioManager.PlayMusic(AudioManager.MusicOptions.Play, 1, .5f, MusicCategory.MusicSoundTypes.Game_Intro);
    }
    #endregion

    #region Pause Game
    // ======================================== Pause Game ========================================
    public void PauseGame(bool pause)
    {
        if (!GameData.canPause) return;
        if (!pause)
        {
            GameData.isPaused = false;
            Time.timeScale = 1;
            // TODO - Toggle Pause Menu
            // UIManager.instance.OpenPauseMenu(false);
        }
        else
        {
            GameData.isPaused = true;
            Time.timeScale = 0;
            // TODO - Toggle Pause Menu
            // UIManager.instance.OpenPauseMenu(true);
        }
    }
    #endregion

    #region Game Over
    // ======================================== Game Over ========================================
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
    // ======================================== Restart Game ========================================
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

    #region UI Button Select

    public void EventSystem_SelectUIButton(GameObject button)
    {
        eventSystem.SetSelectedGameObject(button);
    }
    #endregion
}

#region Game Data

public static class GameData
{
    public static bool isMultiplayer;

    public static bool isGameOver;
    public static bool isPaused;
    public static bool canPause;
    public static bool isGameStarted;
    public static bool isGameFinished;

    public enum Difficulty { Easy, Normal, Hard }
    public static Difficulty gameDifficulty;

    public static float highScore;
    public static List<Transform> players = new();

    public static Scene_Loader_Transition.SceneNames currentLevel;
    public static int playerCount = 0;

    public static List<PlayerInput> playerInputs = new();
}
#endregion

#region Player Data

public class PlayerData
{
    public GameObject playerObject;

    public Player_Character_Data characterData;
    public PlayerInput playerInput;

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