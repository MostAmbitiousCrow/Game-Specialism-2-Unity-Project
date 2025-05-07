using System.Collections;
using UnityEngine;

public class Enemy_Health : Enemy_Character_Data // by Samuel White
{
    //========================================
    // Stores the enemy Health.
    // It will handle events for assigning damage, score and freezing.
    //========================================

    [Header("Health Settings")]
    [SerializeField] int maxHealth = 5;
    [SerializeReference] float health;

    [Header("Visual Effects")]
    [SerializeField] Material characterMaterial;
    [SerializeField] MeshRenderer characterMeshRenderer;
    [SerializeField] SpriteRenderer characterSpriteRenderer;

    [SerializeField] float damageFlashDuration = 0.1f;
    [SerializeField] AnimationCurve damageFlashCurve;

    private float flashT;
    private bool flashing;

    void Awake()
    {
        health = maxHealth;
        if (characterMeshRenderer != null) characterMaterial = characterMeshRenderer.material;
        else if (characterSpriteRenderer != null) characterMaterial = characterSpriteRenderer.material;
    }

    private void OnEnable()
    {
        health = maxHealth;
    }

    public void Damage(int value)
    {
        health -= value;
        DamageFlash();
        if(health <= 0) ReturnEnemy();
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
