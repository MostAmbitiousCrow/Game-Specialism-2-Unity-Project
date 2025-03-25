using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Projectile_Player_Flight : BulletManager // By Samuel White
{
    public PlayerProjectileData scriptable_Object;
    public Transform target;
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
        transform.rotation = new();
        target = Player_Shoot_Flight.targetEnemy;
        time = 0;
    }
    #endregion

    #region Update Bullet Functions
    public override void UpdateBullet() // Updated by the Bullet Manager
    {
        Move();
        if (scriptable_Object.canHome && target != null) Home();
        
        if (time > 1) 
        { Deactivate(); }
        else time += Time.deltaTime / scriptable_Object.projectileLifeTime;
    }

    private void Move() // Move the projectile
    {
        if (scriptable_Object.useMoveAcceleration)
        {
            float speed = Mathf.Lerp(scriptable_Object.moveStartSpeed, scriptable_Object.moveEndSpeed, scriptable_Object.moveAccelerationCurve.Evaluate(time));
            transform.position += speed * Time.deltaTime * transform.forward;
        }
        else
        {
            transform.position += scriptable_Object.moveStartSpeed * Time.deltaTime * transform.forward;
        }
    }
    private void Home() // Rotate the projectile
    {
        if (time > scriptable_Object.timeTilHome) //TODO Adjust this to match lerp
        {
            Vector3 direction = target.position - transform.position; // Get direction from this projectile to the closest enemy
            //Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, scriptable_Object.homingStrength * Time.deltaTime, 0.0F);
            Vector3 newDirection = Vector3.RotateTowards(transform.forward, direction, scriptable_Object.homingStrength * Time.deltaTime, 0.0F);

            //transform.rotation = Quaternion.LookRotation(newDirection);
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(newDirection), 1 - scriptable_Object.homingStrength * Time.deltaTime);
        }
    }
    #endregion

    #region Collision Detection
    public void Deactivate() // Return the projectile to the pool and deactivate this bullet
    {
        target = null;
        gameObject.SetActive(false);
        Bullet_Pool_System.instance.ReturnBullet(gameObject, ID, GetType().GetField("scriptable_Object"));
    }
    #endregion
}
