using UnityEngine;
using UnityEngine.Events;

public class Character_Health_Script : MonoBehaviour // by Samuel
{
    [SerializeField] int maxHealth = 5;
    [SerializeReference] float health;

    public UnityEvent deathEvent;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void Damage(int value)
    {
        health -= value;
        if(health <= 0) deathEvent.Invoke();
    }

    public void Heal(int value)
    {
        if(health < maxHealth) health += value;
    }
}
