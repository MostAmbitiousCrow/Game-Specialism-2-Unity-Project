using UnityEngine;

public class Game_Manager : MonoBehaviour // By Samuel White
{
    public static Game_Manager instance;
    public ParticleSystem enemyParticles;
    public void Awake()
    {
        instance = this;
    }

    public void ExplodeParticles(Vector3 pos)
    {
        enemyParticles.Play();
    }
}
