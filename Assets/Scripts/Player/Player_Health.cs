using System.Collections;
using UnityEngine;

public class Player_Health : MonoBehaviour // by Samuel White
{
    //========================================
    // Stores the players Health.
    // It will handle events for assigning damage, healing and lives.
    //========================================

    [Header("Player Data")]
    public int playerNumber = 0; // 0 = Player 1, 1 = Player 2

    [Header("Health Settings")]
    public int maxHealth = 5;
    public int health;

    [Header("Visual Effects")]
    [SerializeField] Material characterMaterial;
    [SerializeField] MeshRenderer characterMeshRenderer;
    [SerializeField] SpriteRenderer characterSpriteRenderer;

    [SerializeField] float damageFlashDuration = 0.1f;
    [SerializeField] float respawnInvincibilityDuration = 3f;
    [SerializeField] AnimationCurve damageFlashCurve;
    [SerializeField] AnimationCurve respawnInvincibilityCurve;

    [Header("Player Character")]
    [SerializeField] GameObject character;
    [SerializeField] BoxCollider characterCollider;

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
        if (GameManager.playerData[playerNumber].isDead || GameManager.playerData[playerNumber].isInvincible) return;

        health -= value;
        DamageFlash();
        AudioManager.PlayPlayerSound(PlayerCategory.PlayerSoundTypes.Damage, 1);
        if(health <= 0) 
        {
            GameManager.playerData[playerNumber].lives--;
            StartCoroutine(DamageInvicibility());
            AudioManager.PlayPlayerSound(PlayerCategory.PlayerSoundTypes.Deaths, 1);
            if (GameManager.playerData[playerNumber].lives < 1 ) 
            {
                GameManager.playerData[playerNumber].lives = 0;
                GameManager.instance.GameOver();
                // TODO Handle player death here
            }
            else
            {
                health = maxHealth;
                // TODO Handle player respawn here
            }
        }
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
            yield return null;
        }
        flashing = false;
        yield break;
    }

    IEnumerator DamageInvicibility()
    {
        character.SetActive(false);
        yield return new WaitForSeconds(2f);
        character.SetActive(true);
        characterCollider.enabled = false;

        flashing = true;

        float t = 0;
        while (t < 1)
        {
            t += Global_Game_Speed.GetDeltaTime() / respawnInvincibilityDuration;
            characterMaterial.SetFloat("_Flash", respawnInvincibilityCurve.Evaluate(Mathf.InverseLerp(0, Settings_Manager.damageFlashIntensity, t)));
            yield return null;
        }
        flashing = false;
        characterCollider.enabled = true;
        yield break;
    }
}
