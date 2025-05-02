using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Character_Health_Script : MonoBehaviour // by Samuel White
{
    //========================================
    // Stores the Characters Health.
    // It will handle events for assigning damage and healing.
    //========================================

    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    [SerializeReference] float health;

    [Header("Visual Effects")]
    [SerializeField] Material characterMaterial;
    [SerializeField] MeshRenderer characterMeshRenderer;

    [SerializeField] float damageFlashDuration = 0.1f;
    [SerializeField] AnimationCurve damageFlashCurve;

    public UnityEvent deathEvent;
    private float flashT;
    private bool flashing;

    // Start is called before the first frame update
    void Awake()
    {
        health = maxHealth;
        characterMaterial = characterMeshRenderer.material;
        // characterMaterial = characterMeshRenderer.material = new Material(characterMeshRenderer.material);
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void Damage(int value)
    {
        health -= value;
        DamageFlash();
        if(health <= 0) deathEvent.Invoke();
    }

    public void Heal(int value)
    {
        if(health < maxHealth) health += value;
    }

    void DamageFlash()
    {
        Debug.Log($"{name} Flashed");
        if (!flashing) StartCoroutine(DamageFlashCoroutine());
        else flashT = 0;
    }

    IEnumerator DamageFlashCoroutine()
    {
        flashT = 0;
        flashing = true;
        while (flashT < 1)
        {
            flashT += Global_Game_Speed.GetDeltaTime() / damageFlashDuration;
            characterMaterial.SetFloat("_Flash", damageFlashCurve.Evaluate(Mathf.InverseLerp(0, Settings_Manager.damageFlashIntensity, flashT)));
            yield return new WaitForEndOfFrame();
        }
        flashing = false;
        yield break;
    }
}
