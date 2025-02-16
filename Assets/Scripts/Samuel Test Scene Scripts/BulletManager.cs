using UnityEngine;

public abstract class BulletManager : MonoBehaviour
{
    private void Update() 
    {
        UpdateBullet();
    }
    
    public abstract void UpdateBullet();
}