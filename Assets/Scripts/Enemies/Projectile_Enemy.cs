using UnityEngine;

public class Projectile_Enemy : BulletManager // By Samuel White
{
    // The global script for enemy projectiles.

    public SO_Proj_Eni_Bas scriptable_Object;
    public Transform Target { private get; set; }
    private float time = 0;

    #region Active States
    void Awake() => gameObject.SetActive(false);

    void OnEnable()
    {
        if (scriptable_Object == null)
        {
            gameObject.SetActive(false);
            Debug.LogWarning("No Scriptable Object assigned to the projectile");
            return;
        }

        float f = scriptable_Object.projectileSize;
        transform.localScale = new(f,f,f);
        time = 0;
    }
    #endregion

    #region Update Bullet Functions
    public override void UpdateBullet() // Updated by the Bullet Manager
    {
        Move();
        if (scriptable_Object.useRotation) Rotate();
        else if (scriptable_Object.useHoming) Home();
        
        if (time > 1) 
        { gameObject.SetActive(false); Deactivate(); }
        else time += Global_Game_Speed.GetDeltaTime() / scriptable_Object.projectileLifeTime;
    }

    private void Move() // Move the projectile
    {
        if (scriptable_Object.useMoveAcceleration)
        {
            float speed = Mathf.Lerp(scriptable_Object.moveStartSpeed, scriptable_Object.moveEndSpeed,
                scriptable_Object.moveAccelerationCurve.Evaluate(time));
            transform.position += speed * Global_Game_Speed.GetDeltaTime() * transform.forward;
        }
        else
        {
            transform.position += scriptable_Object.moveStartSpeed * Global_Game_Speed.GetDeltaTime() * transform.forward;
        }
    }
    private void Rotate() // Rotate the projectile
    {
        if (scriptable_Object.useAngularAcceleration)
        {
            float speedX = Mathf.Lerp(scriptable_Object.rotateStartSpeedX, scriptable_Object.rotateEndSpeedX,
                scriptable_Object.rotateAccelerationCurveX.Evaluate(time));

            float speedY = Mathf.Lerp(scriptable_Object.rotateStartSpeedY, scriptable_Object.rotateEndSpeedY,
                scriptable_Object.rotateAccelerationCurveY.Evaluate(time));

            transform.Rotate(speedX * Global_Game_Speed.GetDeltaTime(), speedY * Global_Game_Speed.GetDeltaTime(), 0);
        }
        else
        {
            transform.Rotate(scriptable_Object.rotateStartSpeedX * Global_Game_Speed.GetDeltaTime(), scriptable_Object.rotateStartSpeedY * Global_Game_Speed.GetDeltaTime(), 0);
        }
    }
    #endregion

    private void Home()
    {
        if (scriptable_Object.useHomingAcceleration)
        {
            float speed = Mathf.Lerp(scriptable_Object.homeStartStrength, scriptable_Object.homeEndStrength,
                scriptable_Object.homeAccelerationCurve.Evaluate(time));
            Vector3 direction = Target.position - transform.position; // Get direction from this projectile to the closest enemy
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, speed * Global_Game_Speed.GetDeltaTime(), 0.0F);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), 1 - speed * Global_Game_Speed.GetDeltaTime());
        }
        else
        {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(Target.position - transform.position),
                scriptable_Object.homeStartStrength * Global_Game_Speed.GetDeltaTime());
        }
    }

    #region Collision Detection
    private void OnTriggerEnter(Collider c)
    {
        c.GetComponent<Player_Health>().Damage(scriptable_Object.projectileDamage); // Damage the target
        AudioManager.PlayEnemySound(scriptable_Object.destroySound, 1); // Play the hit sound
        Deactivate();
    }

    public void Deactivate() // Return the projectile to the pool and deactivate this bullet
    {
        gameObject.SetActive(false);
        Bullet_Pool_System.instance.ReturnEnemyBullet(this, scriptable_Object.ID); // Return the projectile to the pool
    }
    #endregion
}