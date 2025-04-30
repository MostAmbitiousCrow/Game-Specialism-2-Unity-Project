using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Transform player;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        if (player == null) player = GameObject.FindWithTag("Player").transform;
    }
}
