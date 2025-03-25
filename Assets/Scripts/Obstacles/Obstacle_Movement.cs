using UnityEngine;

public class Obstacle_Movement : BulletManager // By Samuel White
{
    public override void UpdateBullet() // Updated by the Bullet Manager
    {

    }


    private void OnCollisionEnter(Collision collision) // Obstacle Collider only detects Enemy and Player Layer
    {
        collision.gameObject.GetComponent<Character_Health_Script>().Damage(0); // TODO Temporary Damage until GDD is fuffilled

    }
}
