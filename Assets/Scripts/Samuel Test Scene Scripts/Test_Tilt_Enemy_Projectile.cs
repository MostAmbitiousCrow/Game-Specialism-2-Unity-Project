using UnityEngine;

public class TiltProjectile : MonoBehaviour
{
    public Basic_Projectile_Scriptable_Object scriptable_Object;
    private float t = 0;

    void Awake() => gameObject.SetActive(false);

    void OnEnable() => t = 0;

    void Move()
    {
        transform.position += scriptable_Object.projectileStartSpeed * Time.deltaTime * transform.right;
    }

    private void Update()
    {
        Move();

        t += Time.deltaTime;
        if (t > scriptable_Object.projectileLifeTime)
        {
            gameObject.SetActive(false);
        }
    }
}