using UnityEngine;

public abstract class BulletManager : MonoBehaviour // By Samuel
{
    private void Update() 
    {
        UpdateBullet();
    }
    
    public abstract void UpdateBullet();
}