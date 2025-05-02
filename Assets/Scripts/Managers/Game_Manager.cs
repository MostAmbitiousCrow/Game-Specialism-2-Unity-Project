using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour // By Samuel White
{
    //========================================
    // The Game Manager class.
    // This class is used to store the game data.
    //========================================

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
        GameData.highScore = PlayerPrefs.GetFloat("HighScore", 0);
    }

    private void Start()
    {
        if (GameData.isMultiplayer)
        {
            GameData.playerOne = GameObject.FindWithTag("Player").transform;
            GameData.playerTwo = GameObject.FindWithTag("Player2").transform;

            GameData.players = new List<Transform> { GameData.playerOne, GameData.playerTwo };
        }
        else
        {
            Transform player = GameObject.FindWithTag("Player").transform;
            GameData.players = new List<Transform> { player };
            GameData.playerOne = player;
            GameData.playerTwo = null;
        }
    }

    public void SetHighScore(float value)
    {
        if (value > GameData.highScore)
        {
            GameData.highScore = value;
            PlayerPrefs.SetFloat("HighScore", GameData.highScore);
        }
    }

    public void ResetHighScore()
    {
        PlayerPrefs.DeleteKey("HighScore");
        GameData.highScore = 0;
    }
}

public static class GameData
{
    public static bool isMultiplayer;

    public static Transform playerOne;
    public static Transform playerTwo;

    public static List<Transform> players;

    public static float score;
    public static float highScore;

    public static int playerOneScore;
    public static int playerTwoScore;

    public static int playerOneLives;
    public static int playerTwoLives;

    public static bool isGameOver;
    public static bool isPaused;
    public static bool isGameStarted;
    public static bool isGameFinished;
}