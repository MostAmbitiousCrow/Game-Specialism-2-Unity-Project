using UnityEngine;

public class Test_Enemy_Projectile : MonoBehaviour
{
    [SerializeField] Basic_Projectile_Scriptable_Object scriptable_Object;

    void Awake() => gameObject.SetActive(false);

    void OnEnable()
    {
        float f = scriptable_Object.projectileSize;
        transform.localScale = new(f,f,f);
    }

    private void Move()
    {
        transform.position += scriptable_Object.projectileStartSpeed * Time.deltaTime * transform.right;
    }

    private void Update()
    {
        Move();        
    }
}