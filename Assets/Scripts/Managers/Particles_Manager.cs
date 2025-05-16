using UnityEngine;

public class ParticleManager : MonoBehaviour // By Samuel White
{
    public static ParticleManager instance;

    public enum EnemyParticlesType
    {
        EnemyShoot,
        EnemyCharacter_Hit,
        EnemyBullet_Hit,
        EnemyDeath,
        EnemyFreeze,
        EnemyFreeze_Hit,
        EnemyFreeze_Explode,
        EnemyPortal_Trickle,
        EnemyPortal_Close
    }

    public enum PlayerParticlesType
    {
        PlayerBullet_Shoot,
        PlayerCharacter_Hit,
        PlayerBullet_Hit,
        PlayerDeath,
        PlayerActivateFreezeMode
    }

    public enum MiscParticlesType
    {
        PowerUpBox_Destroy,
        PowerUpBox_Spawn,
        PowerUpBox_Collect
    }

    public ParticleSystem[] ParticleSystems { get; private set; }

    public void Awake()
    {
        instance = this;
        ParticleSystems = transform.GetComponentsInChildren<ParticleSystem>(true); // Obtain all particle systems in children
    }

    public void PlayPlayerParticle(PlayerParticlesType type, Vector3 pos)
    {
        if (!Settings_Manager.enableParticles) return;
        ParticleSystem particle = ParticleSystems[(int)type];
        if (particle == null) return;

        particle.transform.position = pos;
        particle.Play();
    }

    public void StopPlayerParticle(PlayerParticlesType type)
    {
        ParticleSystem particle = ParticleSystems[(int)type];
        if (particle == null) return;

        particle.Stop();
    }

    public void PlayEnemyParticle(EnemyParticlesType type, Vector3 pos)
    {
        if (!Settings_Manager.enableParticles) return;
        ParticleSystem particle = ParticleSystems[(int)type];
        if (particle == null) return;

        particle.transform.position = pos;
        particle.Play();
    }

    public void StopEnemyParticle(EnemyParticlesType type)
    {
        ParticleSystem particle = ParticleSystems[(int)type];
        if (particle == null) return;

        particle.Stop();
    }

    public void PlayMiscParticle(MiscParticlesType type, Vector3 pos)
    {
        if (!Settings_Manager.enableParticles) return;
        ParticleSystem particle = ParticleSystems[(int)type];
        if (particle == null) return;

        particle.transform.position = pos;
        particle.Play();
    }

    public void StopMiscParticle(MiscParticlesType type)
    {
        ParticleSystem particle = ParticleSystems[(int)type];
        if (particle == null) return;

        particle.Stop();
    }
}
