using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_Enemy_Select_Manager : MonoBehaviour // By Samuel White
{
    [SerializeField] private List<GameObject> enemies = new List<GameObject>();
    private int lastID = 0;

    void Awake()
    {
        for (int i = 1; i < enemies.Count; i++)
        {
            enemies[i].SetActive(false);
        }
    }

    public void SelectEnemy(int ID)
    {
        enemies[lastID].SetActive(false);
        Debug.Log($"Enemy {ID} Selected");
        enemies[ID].SetActive(true);
        lastID = ID;
    }
}
