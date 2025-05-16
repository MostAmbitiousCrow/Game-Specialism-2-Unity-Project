using UnityEngine;

public class WaffleBullet : MonoBehaviour
{
    [Header("Bullet Data")]
    [SerializeField] float speed = 40f;
    [SerializeField] float lifetime = 4f;
    [SerializeField] int damage = 1;
    [SerializeField] float returnSpeed = 20f;
    //[SerializeField] float maxDistance = 20f;

    private Vector3 startPosition;
    private bool returning = false;
    private Transform player;
    private BoomerangPowerUp boomerangPowerUpScript;

    void Start()
    {
        startPosition = transform.position;
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        if (!returning)
        {
            MoveForward();
        }
        else
        {
            ReturnToPlayer();
        }
    }

    void MoveForward()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        //if (Vector3.Distance(startPosition, transform.position) >= maxDistance)
        //{
        //    returning = true;
        //}
    }

    void ReturnToPlayer()
    {
        if (player != null)
        {
            Vector3 direction = (player.position - transform.position).normalized;
            transform.position = Vector3.MoveTowards(transform.position, player.position, returnSpeed * Time.deltaTime);
            if (Vector3.Distance(transform.position, player.position) < 1f)
            {
                // Boomerang caught by player
                returning = false;
                boomerangPowerUpScript.ResetFireCount(); // Notify the power-up script to reset the fire count
                Destroy(gameObject); // Or handle reuse logic
            }
        }
    }

    private void OnTriggerEnter(Collider c)
    {
        if (c.CompareTag("EnemyB"))
        {
            c.GetComponent<Enemy_Character_Data>().Damage(damage); // Damage the target
            returning = true; // Start returning to the player
        }
        else if (c.CompareTag("Box"))
        {
            c.GetComponent<Character_Health_Script>().Damage(damage); // Damage the target
            returning = true; // Start returning to the player
        }
        else if (c.CompareTag("Player"))
        {
            // If the player collides with the boomerang, it should return to the player
            boomerangPowerUpScript.ResetFireCount(); // Notify the power-up script to reset the fire count
        }
    }

    public void Initialize(Transform playerTransform, BoomerangPowerUp powerUp)
    {
        player = playerTransform;
        boomerangPowerUpScript = powerUp;
    }

    public void Deactivate() // Destroy the bullet
    {
        Destroy(gameObject);
    }
}
