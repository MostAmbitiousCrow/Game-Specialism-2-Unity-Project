using UnityEngine;

public class Projectile_Enemy : BulletManager // By Samuel White
{
    // The global script for enemy projectiles.

    public SO_Proj_Eni_Bas scriptable_Object;
    private float time = 0;
    [SerializeField] private int ID;

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

        transform.LookAt(GameData.playerOne.position);
    }
    #endregion

    #region Update Bullet Functions
    public override void UpdateBullet() // Updated by the Bullet Manager
    {
        Move();
        if (scriptable_Object.useRotation) Rotate();
        
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
            float speed = Mathf.Lerp(scriptable_Object.rotateStartSpeed, scriptable_Object.rotateEndSpeed,
                scriptable_Object.rotateAccelerationCurve.Evaluate(time));
            transform.Rotate(0, 0, speed * Global_Game_Speed.GetDeltaTime());
        }
        else
        {
            transform.Rotate(0, 0, scriptable_Object.rotateStartSpeed * Global_Game_Speed.GetDeltaTime());
        }
    }
    #endregion

    #region Collision Detection
    private void OnTriggerEnter(Collider c)
    {
        c.GetComponent<Character_Health_Script>().Damage(scriptable_Object.projectileDamage); // Damage the target
        AudioManager.PlayEnemySound(scriptable_Object.destroySound, 1); // Play the hit sound
        Deactivate();
    }

    public void Deactivate() // Return the projectile to the pool and deactivate this bullet
    {
        gameObject.SetActive(false);
        Bullet_Pool_System.instance.ReturnBullet(gameObject, ID, GetType().GetField("scriptable_Object"));
    }
    #endregion
}