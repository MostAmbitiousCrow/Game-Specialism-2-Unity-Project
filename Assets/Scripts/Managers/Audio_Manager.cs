using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnemyCategory;


public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    [SerializeField] private PlayerCategory playerCategory;
    [SerializeField] private EnemyCategory enemyCategory;
    [SerializeField] private InterfaceCategory interfaceCategory;

    [SerializeField] private AudioSource musicAudioSource;

    private void Awake()
    {
        instance = this;
    }

    public static void PlayPlayerSound(PlayerCategory.PlayerSoundTypes type, float volume = 1)
    {
        AudioClip[] clips = instance.playerCategory.soundList[(int)type].sounds;

        AudioClip randomclip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.playerCategory.audioSource.PlayOneShot(randomclip, volume);
    }

    public static void PlayEnemySound(EnemyCategory.EnemySoundTypes type, float volume = 1)
    {
        AudioClip[] clips = instance.enemyCategory.soundList[(int)type].sounds;

        AudioClip randomclip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.enemyCategory.audioSource.PlayOneShot(randomclip, volume);
    }
    public static void PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes type, float volume = 1)
    {
        AudioClip[] clips = instance.interfaceCategory.soundList[(int)type].sounds;

        AudioClip randomclip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.interfaceCategory.audioSource.PlayOneShot(randomclip, volume);
    }

    [HideInInspector]
    public enum MusicOptions
    {
        Play, Pause, 
    }
    public static void EditMusic(bool pause , float volume, float volumeTime, AudioClip music)
    {
        AudioSource a = instance.musicAudioSource;
        if (music != null)
        {
            a.clip = music;
            a.Play();
        }

        if (pause) a.Pause();
        else a.UnPause();

        if (volumeTime > 0)
            instance.StartCoroutine(instance.MusicTransition(volume, volumeTime));
        else
            a.volume = volume;
    }

    private IEnumerator MusicTransition(float tv, float time) // Target volume, Time
    {
        float t = 0;
        float v = musicAudioSource.volume;
        while (t < 1)
        {
            t += Time.deltaTime / time;
            musicAudioSource.volume = Mathf.Lerp(v, tv, t);
            yield return null;
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        string[] pNames = Enum.GetNames(typeof(PlayerCategory.PlayerSoundTypes));
        Array.Resize(ref playerCategory.soundList, pNames.Length);
        for (int i = 0; i < playerCategory.soundList.Length; i++)
        {
            playerCategory.soundList[i].listName = pNames[i];

            //string[] slNames = Enum.GetNames(typeof(PlayerCategory.SoundList));
            //Array.Resize(ref playerCategory[i].soundList, slNames.Length);
            //for (int j = 0; j < categoryList[i].soundList.Length; j++)
            //{
            //    categoryList[i].soundList[j].listName = slNames[j];
            //}
        }
        string[] eNames = Enum.GetNames(typeof(EnemyCategory.EnemySoundTypes));
        Array.Resize(ref enemyCategory.soundList, eNames.Length);
        for (int i = 0; i < enemyCategory.soundList.Length; i++)
        {
            enemyCategory.soundList[i].listName = eNames[i];
        }
        string[] iNames = Enum.GetNames(typeof(InterfaceCategory.InterfaceSoundTypes));
        Array.Resize(ref interfaceCategory.soundList, iNames.Length);
        for (int i = 0; i < interfaceCategory.soundList.Length; i++)
        {
            interfaceCategory.soundList[i].listName = iNames[i];
        }
    }
#endif
}

[Serializable]
public struct PlayerCategory
{
    [HideInInspector] public string categoryName; //  Name of Sound Category
    public AudioSource audioSource;
    public enum PlayerSoundTypes { Damage, Attack, Deaths, Indicators, Boomerang, }
    [SerializeField] public SoundList[] soundList; // List of Types of Sounds
    [Serializable]
    public struct SoundList
    {
        [SerializeField] public string listName;
        [SerializeField] public PlayerSoundTypes playerSoundType;
        [SerializeField] public AudioClip[] sounds;
    }
}

[Serializable]
public struct EnemyCategory
{
    [HideInInspector] public string categoryName; //  Name of Sound Category
    public AudioSource audioSource;
    public enum EnemySoundTypes { Imp_Attack, Imp_Death, Imp_Spawn, Succubus_Attack, Succubus_Death, Succubus_Spawn,
    Chef_Attack, Chef_Death, Chef_Spawn, Limb_Attack, Limb_Death, Limb_Spawn, Oni_Attack, Oni_Death, Oni_Spawn, Enemy_Hit }
    [SerializeField] public SoundList[] soundList; // List of Types of Sounds
    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string listName;
        [SerializeField] public EnemySoundTypes enemySoundType;
        [SerializeField] public AudioClip[] sounds;
    }
}

[Serializable]
public struct InterfaceCategory
{
    [HideInInspector] public string categoryName; //  Name of Sound Category
    public AudioSource audioSource;
    public enum InterfaceSoundTypes { Buttons, Exit, Pause, Unpause }
    [SerializeField] public SoundList[] soundList; // List of Types of Sounds
    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string listName;
        [SerializeField] public InterfaceSoundTypes audioType;
        [SerializeField] public AudioClip[] sounds;
    }
}