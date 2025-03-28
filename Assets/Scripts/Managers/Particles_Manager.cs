using UnityEngine;

public class ParticleManager : MonoBehaviour // By Samuel White
{
    public static ParticleManager instance;
    public ParticleSystem enemyParticles;
    public ParticleSystem playerParticles;
    public void Awake()
    {
        instance = this;
    }

    public void ExplodeEnemyParticles(Vector3 pos)
    {
        enemyParticles.transform.position = pos;
        enemyParticles.Play();
    }
    public void ExplodePlayerParticles(Vector3 pos)
    {
        playerParticles.transform.position = pos;
        playerParticles.Play();
    }
}
