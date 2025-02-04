using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Character_Health_Script : MonoBehaviour
{
    [SerializeField] int maxHealth = 5;
    [SerializeReference] float health;

    public UnityEvent deathEvent;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
    }

    public void Damage(int value)
    {
        health -= value;

    }

    public void Heal(int value)
    {
        health += value;

    }
}
