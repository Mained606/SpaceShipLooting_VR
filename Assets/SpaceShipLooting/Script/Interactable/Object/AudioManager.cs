using UnityEngine;
using UnityEngine.Audio;

// 오디오를 관리하는 클래스
public class AudioManager : MonoBehaviour
{
    #region Variables

    public static AudioManager Instance { get; private set; }

    [Header("Player Sounds")]
    public Sound[] PlayerSounds;

    [Header("Enemy Sounds")]
    public Sound[] EnemySounds;

    [Header("Interact Sounds")]
    public Sound[] InteractSounds;

    [Header("Interact Sounds")]
    public Sound[] BossSounds;

    private string bgmSound = ""; // 현재 플레이 되는 배경음 이름
    public string BgmSound
    {
        get { return bgmSound; }
    }

    public AudioMixer audioMixer;

    #endregion

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioMixer
        AudioMixerGroup[] audioMixerGroups = audioMixer.FindMatchingGroups("Master");

        // AudioManager 초기화
        InitializeSounds(PlayerSounds, audioMixerGroups);
        InitializeSounds(EnemySounds, audioMixerGroups);
        InitializeSounds(InteractSounds, audioMixerGroups);
    }

    private void InitializeSounds(Sound[] sounds, AudioMixerGroup[] audioMixerGroups)
    {
        foreach (var sound in sounds)
        {
            sound.source = this.gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;

            if (sound.loop)
            {
                sound.source.outputAudioMixerGroup = audioMixerGroups[1]; // bgm
            }
            else
            {
                sound.source.outputAudioMixerGroup = audioMixerGroups[2]; // sfx
            }
        }
    }

    public void Play(string name)
    {
        Sound sound = FindSound(name);
        if (sound == null)
        {
            Debug.Log($"Cannot Find {name}");
            return;
        }
        sound.source.Play();
    }

    public void Stop(string name)
    {
        Sound sound = FindSound(name);
        if (sound == null)
        {
            return;
        }
        if (sound.name == bgmSound)
        {
            bgmSound = "";
        }
        sound.source.Stop();
    }

    // 배경음 재생
    public void PlayBgm(string name)
    {
        if (bgmSound == name) return;

        StopBgm();

        Sound sound = FindSound(name);
        if (sound == null)
        {
            Debug.Log($"Cannot Find {name}");
            return;
        }
        bgmSound = sound.name;
        sound.source.Play();
    }

    public void StopBgm()
    {
        Stop(bgmSound);
    }

    private Sound FindSound(string name)
    {
        Sound sound = null;
        sound = FindSoundInArray(PlayerSounds, name) ??
                FindSoundInArray(EnemySounds, name) ??
                FindSoundInArray(InteractSounds, name);
        return sound;
    }

    private Sound FindSoundInArray(Sound[] sounds, string name)
    {
        foreach (var s in sounds)
        {
            if (s.name == name) return s;
        }
        return null;
    }
}
