using UnityEngine;

public class Projectile_Enemy : BulletManager // By Samuel White
{
    // The global script for enemy projectiles.

    public SO_Proj_Eni_Bas data;
    public Transform Target { private get; set; }
    [SerializeField] SpriteRenderer spriteRenderer;
    private float time = 0;

    #region Active States
    void Start()
    {
        // gameObject.SetActive(false);
        spriteRenderer.sprite = data.sprite;
    }

    void OnEnable()
    {
        if (data == null)
        {
            // gameObject.SetActive(false);
            // Debug.LogWarning("No Scriptable Object assigned to the projectile");
            return;
        }

        float f = data.projectileSize;
        transform.localScale = new(f,f,f);
        time = 0;
    }
    #endregion

    #region Update Bullet Functions
    public override void UpdateBullet() // Updated by the Bullet Manager
    {
        Move();
        if (data.useRotation) Rotate();
        else if (data.useHoming) Home();
        
        if (time > 1) 
        { gameObject.SetActive(false); Deactivate(); }
        else time += Global_Game_Speed.GetDeltaTime() / data.projectileLifeTime;
    }

    private void Move() // Move the projectile
    {
        if (data.useMoveAcceleration)
        {
            float speed = Mathf.Lerp(data.moveStartSpeed, data.moveEndSpeed,
                data.moveAccelerationCurve.Evaluate(time));
            transform.position += speed * Global_Game_Speed.GetDeltaTime() * transform.forward;
        }
        else
        {
            transform.position += data.moveStartSpeed * Global_Game_Speed.GetDeltaTime() * transform.forward;
        }
    }
    private void Rotate() // Rotate the projectile
    {
        if (data.useAngularAcceleration)
        {
            float speedX = Mathf.Lerp(data.rotateStartSpeedX, data.rotateEndSpeedX,
                data.rotateAccelerationCurveX.Evaluate(time));

            float speedY = Mathf.Lerp(data.rotateStartSpeedY, data.rotateEndSpeedY,
                data.rotateAccelerationCurveY.Evaluate(time));

            transform.Rotate(speedX * Global_Game_Speed.GetDeltaTime(), speedY * Global_Game_Speed.GetDeltaTime(), 0);
        }
        else
        {
            transform.Rotate(data.rotateStartSpeedX * Global_Game_Speed.GetDeltaTime(), data.rotateStartSpeedY * Global_Game_Speed.GetDeltaTime(), 0);
        }
    }
    #endregion

    private void Home()
    {
        if (data.useHomingAcceleration)
        {
            float speed = Mathf.Lerp(data.homeStartStrength, data.homeEndStrength,
                data.homeAccelerationCurve.Evaluate(time));
            Vector3 direction = Target.position - transform.position; // Get direction from this projectile to the closest enemy
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, speed * Global_Game_Speed.GetDeltaTime(), 0.0F);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), 1 - speed * Global_Game_Speed.GetDeltaTime());
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Target.position - transform.position),
                data.homeStartStrength * Global_Game_Speed.GetDeltaTime());
        }
    }

    #region Collision Detection
    private void OnTriggerEnter(Collider c)
    {
        c.GetComponent<Player_Health>().Damage(data.projectileDamage); // Damage the target
        AudioManager.PlayEnemySound(data.destroySound, .25f); // Play the hit sound
        Deactivate();
    }

    public void Deactivate() // Return the projectile to the pool and deactivate this bullet
    {
        gameObject.SetActive(false);
        Bullet_Pool_System.instance.ReturnEnemyBullet(this, data.ID); // Return the projectile to the pool
    }
    #endregion
}