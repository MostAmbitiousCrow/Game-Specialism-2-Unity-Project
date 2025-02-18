using UnityEngine;

public class Projectile_Player_Flight : BulletManager
{
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
    }
    #endregion

    #region Update Bullet Functions
    public override void UpdateBullet() // Updated by the Bullet Manager
    {
        Move();
        if (scriptable_Object.useRotation) Rotate();
        
        if (time > 1) 
        { gameObject.SetActive(false); Deactivate(); }
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
    private void Rotate() // Rotate the projectile
    {
        if (scriptable_Object.useAngularAcceleration)
        {
            float speed = Mathf.Lerp(scriptable_Object.rotateStartSpeed, scriptable_Object.rotateEndSpeed, scriptable_Object.rotateAccelerationCurve.Evaluate(time));
            transform.Rotate(0, 0, speed * Time.deltaTime);
        }
        else
        {
            transform.Rotate(0, 0, scriptable_Object.rotateStartSpeed * Time.deltaTime);
        }
    }
    #endregion

    #region Collision Detection
    public void Deactivate() // Return the projectile to the pool and deactivate this bullet
    {
        Bullet_Pool_System.instance.ReturnBullet(gameObject, ID, GetType().GetField("scriptable_Object"));

    }
    #endregion
}
