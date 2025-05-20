using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour // By Samuel White
{
    //========================================
    // The Audio Manager class.
    // Used to play specific sounds and music.
    //========================================
    
    public static AudioManager instance;
    [SerializeField] private PlayerCategory playerCategory;
    [SerializeField] private EnemyCategory enemyCategory;
    [SerializeField] private InterfaceCategory interfaceCategory;
    [SerializeField] private MusicCategory musicCategory;
 
    [SerializeField] private AudioSource musicAudioSource;

    [Space(10)]

    [SerializeField] private AudioMixer audioMixer;
    private Coroutine musicCoroutine;

    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #region Play Sounds

    public static void PlayPlayerSound(PlayerCategory.PlayerSoundTypes type, float volume = 1)
    {
        AudioClip[] clips = instance.playerCategory.soundList[(int)type].sounds;
        if (clips.Length == 0) return;

        AudioClip randomclip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.playerCategory.audioSource.PlayOneShot(randomclip, volume);
    }

    public static void PlayEnemySound(EnemyCategory.EnemySoundTypes type, float volume = 1)
    {
        AudioClip[] clips = instance.enemyCategory.soundList[(int)type].sounds;
        if (clips.Length == 0) return;

        AudioClip randomclip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.enemyCategory.audioSource.PlayOneShot(randomclip, volume);
    }
    public static void PlayInterfaceSound(InterfaceCategory.InterfaceSoundTypes type, float volume = 1)
    {
        AudioClip[] clips = instance.interfaceCategory.soundList[(int)type].sounds;
        if (clips.Length == 0) return;

        AudioClip randomclip = clips[UnityEngine.Random.Range(0, clips.Length)];
        instance.interfaceCategory.audioSource.PlayOneShot(randomclip, volume);
    }
    #endregion

    #region Update Volume
    public static void UpdateAudioManagerVolume()
    {
        instance.audioMixer.SetFloat("Master", Mathf.Log10(Settings_Manager.masterVolume) * 20);
        instance.audioMixer.SetFloat("Music", Mathf.Log10(Settings_Manager.musicVolume) * 20);
        instance.audioMixer.SetFloat("Player", Mathf.Log10(Settings_Manager.playerVolume) * 20);
        instance.audioMixer.SetFloat("Enemies", Mathf.Log10(Settings_Manager.enemyVolume) * 20);
        instance.audioMixer.SetFloat("Interface", Mathf.Log10(Settings_Manager.interfaceVolume) * 20);
    }
    #endregion

    [HideInInspector]
    public enum MusicOptions
    {
        Play, Pause, Stop, Resume
    }
    public static void PlayMusic(MusicOptions option , float volume, float volumeTime, MusicCategory.MusicSoundTypes music)
    {
        AudioSource a = instance.musicAudioSource;
        if (music != MusicCategory.MusicSoundTypes.None) a.clip = instance.musicCategory.soundList[(int)music].music;
        Debug.Log($"Played Music: Option: {option} | Music: {music} | Volume: {volume} | Transition Time: {volumeTime}");

        if (volumeTime > 0)
        {
            instance.StartCoroutine(instance.MusicTransition(volume, volumeTime, option));
            if (music == MusicCategory.MusicSoundTypes.Game_Intro) instance.StartCoroutine(instance.GameMusicLoop()); // Loop Gameplay Music
        }
        else
        {
            a.volume = volume;
            if(music == MusicCategory.MusicSoundTypes.None)
            {
                a.clip = null;
                a.Stop();
            }
            else if (music == MusicCategory.MusicSoundTypes.Game_Intro) instance.StartCoroutine(instance.GameMusicLoop()); // Loop Gameplay Music
            switch (option)
            {
                case MusicOptions.Play:
                    a.Play();
                    break;
                case MusicOptions.Pause:
                    a.Pause();
                    return;
                case MusicOptions.Stop:
                    a.Stop();
                    break;
                case MusicOptions.Resume:
                    a.UnPause();
                    break;
            }
        }
    }

    public static void UpdateMusic(MusicOptions option)
    {
        AudioSource a = instance.musicAudioSource;
        switch (option)
        {
            case MusicOptions.Play:
                a.Play();
                break;
            case MusicOptions.Pause:
                a.Pause();
                break;
            case MusicOptions.Stop:
                a.Stop();
                if(instance.musicCoroutine != null) instance.StopCoroutine(instance.musicCoroutine);
                break;
            case MusicOptions.Resume:
                a.UnPause();
                break;
        }
    }

    private IEnumerator MusicTransition(float tv, float time, MusicOptions option) // Target volume, Time
    {
        float t = 0;
        float v = musicAudioSource.volume;
        if (option == MusicOptions.Play) musicAudioSource.Play();
        while (t < 1)
        {
            t += Time.deltaTime / time;
            musicAudioSource.volume = Mathf.Lerp(v, tv, t);
            yield return null;
        }
        if (option == MusicOptions.Stop) musicAudioSource.Stop();
        else if (option == MusicOptions.Pause) musicAudioSource.Pause();
    }

    private IEnumerator GameMusicLoop()
    {
        yield return new WaitForSeconds(musicCategory.soundList[(int)MusicCategory.MusicSoundTypes.Game_Intro].music.length);
        PlayMusic(MusicOptions.Play, 1, 0, MusicCategory.MusicSoundTypes.Game_Loop);
        yield break;
    }

    public enum AudioDataTypes
    {
        Gameplay_Sounds, MainMenu_Sounds
    }

    #region Load Audio
    public static void LoadAudioData(bool unload, AudioDataTypes dataType) // For Memory Purposes: Function to load or unload necessary sound clips
    {
        MusicCategory.SoundList[] musicList;
        switch (dataType)
        {
            case AudioDataTypes.Gameplay_Sounds:

            // --- Player Sounds---

            foreach (var item in instance.playerCategory.soundList)
            {
                foreach (var sound in item.sounds)
                {
                    if (!unload)sound.LoadAudioData();
                    else sound.UnloadAudioData();
                }
            }
            // --- Enemy Sounds ---

            foreach (var item in instance.enemyCategory.soundList)
            {
                foreach (var sound in item.sounds)
                {
                    if (sound == null) return;
                    if (!unload)sound.LoadAudioData();
                    else sound.UnloadAudioData();
                }
            }
            // --- UI Sounds ---

            // Button_Hover
            foreach (var sound in instance.interfaceCategory.soundList[0].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // Button_Press
            foreach (var sound in instance.interfaceCategory.soundList[1].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // Button_Back
            foreach (var sound in instance.interfaceCategory.soundList[2].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // Pause
            foreach (var sound in instance.interfaceCategory.soundList[3].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // Unpause
            foreach (var sound in instance.interfaceCategory.soundList[4].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // --- Music ---

            // Game_Intro
            musicList = instance.musicCategory.soundList;
            if (!unload) musicList[1].music.LoadAudioData();
            else musicList[1].music.UnloadAudioData();

            // Game Loop
            if (!unload) musicList[2].music.LoadAudioData();
            else musicList[2].music.UnloadAudioData();
            
            // Boss (Not Provided)
            // if (!unload) musicList[3].music.LoadAudioData();
            // else musicList[3].music.UnloadAudioData();
            break;

            case AudioDataTypes.MainMenu_Sounds:

            // --- Interface ---
            
            // Button_Hover
            foreach (var sound in instance.interfaceCategory.soundList[0].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // Button_Press
            foreach (var sound in instance.interfaceCategory.soundList[1].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // Button_Back
            foreach (var sound in instance.interfaceCategory.soundList[2].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // GameStart
            foreach (var sound in instance.interfaceCategory.soundList[5].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // Player Joined
            foreach (var sound in instance.interfaceCategory.soundList[6].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // Player Left
            foreach (var sound in instance.interfaceCategory.soundList[7].sounds)
            {
                if (!unload) sound.LoadAudioData();
                else sound.UnloadAudioData();   
            }

            // --- Music ---

            musicList = instance.musicCategory.soundList;
            // Main Menu
            if (!unload) musicList[0].music.LoadAudioData();
            else musicList[0].music.UnloadAudioData();
            break;
        }
    }
#endregion

#region Display Inspector Sounds
// #if UNITY_EDITOR
    // Creates and names the sound lists in the inspector
    private void OnDrawGizmos()
    {
        string[] pNames = Enum.GetNames(typeof(PlayerCategory.PlayerSoundTypes));
        Array.Resize(ref playerCategory.soundList, pNames.Length);
        for (int i = 0; i < playerCategory.soundList.Length; i++)
        {
            playerCategory.soundList[i].listName = pNames[i];

            // string[] slNames = Enum.GetNames(typeof(PlayerCategory.SoundList));
            // Array.Resize(ref playerCategory[i].soundList, slNames.Length);
            // for (int j = 0; j < categoryList[i].soundList.Length; j++)
            // {
            //    categoryList[i].soundList[j].listName = slNames[j];
            // }
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
// #endif
}

[Serializable]
public struct PlayerCategory
{
    [HideInInspector] public string categoryName; //  Name of Sound Category
    public AudioSource audioSource;
    public enum PlayerSoundTypes { Took_Damage, Fire_Bullet, Fire_Frozen_Shot, Shot_Hit, Frozen_Shot_Hit, FreezeMode_Activate, Deaths, Indicators, Powerup_Obtain, 
    PU_Boomerang_Throw, PU_Boomerang_Hit, PU_Boomerang_Catch, PU_Flake, PU_Sprinkles, PU_BubbleGum, }
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
    Chef_Attack, Chef_Death, Chef_Spawn, Limb_Attack, Limb_Spin, Limb_Death, Limb_Spawn, Oni_Attack, Oni_Death, Oni_Spawn, 
    Enemy_Hit, Enemy_Frozen, Enemy_Frozen_Smashed, Enemy_Defeated }
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
    public enum InterfaceSoundTypes { Button_Hover, Button_Press, Button_Back, Pause, Unpause, Button_GameStart, Player_Joined, Player_Left }
    [SerializeField] public SoundList[] soundList; // List of Types of Sounds
    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string listName;
        [SerializeField] public InterfaceSoundTypes audioType;
        [SerializeField] public AudioClip[] sounds;
    }
}

[Serializable]
public struct MusicCategory
{
    [HideInInspector] public string categoryName; //  Name of Sound Category
    public AudioSource audioSource;
    public enum MusicSoundTypes { MainMenu, Game_Intro, Game_Loop, Boss, None}
    [SerializeField] public SoundList[] soundList; // List of Types of Sounds
    [Serializable]
    public struct SoundList
    {
        [HideInInspector] public string listName;
        [SerializeField] public MusicSoundTypes musicType;
        [SerializeField] public AudioClip music;
    }
    #endregion
}