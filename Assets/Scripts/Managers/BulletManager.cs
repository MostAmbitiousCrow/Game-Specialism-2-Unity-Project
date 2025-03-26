using UnityEngine;

public abstract class BulletManager : MonoBehaviour // By Samuel White
{
    private void Update() 
    {
        UpdateBullet();
    }
    
    public abstract void UpdateBullet();
}