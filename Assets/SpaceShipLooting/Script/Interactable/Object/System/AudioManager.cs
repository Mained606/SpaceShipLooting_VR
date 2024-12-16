using UnityEngine;
using UnityEngine.Audio;

    // 오디오를 관리하는 클래스 
    public class AudioManager : MonoBehaviour
{
    #region Variables

    public static AudioManager Instance { get; private set; }
    public Sound[] sounds;
        private string bgmSound = "";   // 현재 플레이 되는 배경음 이름
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
        AudioMixerGroup[] audioMixerGroups = audioMixer.FindMatchingGroups("Master"); //  마스터 하위 그룹들까지 땡겨서 매치됨

            // AudioManager 초기화
            foreach(var sound in sounds)
            {
                sound.source= this.gameObject.AddComponent<AudioSource>();
                sound.source.clip= sound.clip;
                sound.source.volume= sound.volume;
                sound.source.pitch= sound.pitch;
                sound.source.loop = sound.loop;

                if(sound.loop)
                {
                    sound.source.outputAudioMixerGroup = audioMixerGroups[1];  // bgm
                }
                else
                {
                    sound.source.outputAudioMixerGroup = audioMixerGroups[2];  // sfx
                }
            }
        }
        public void Play(string name)
        {
            Sound sound = null;
            // 매개변수 이름과 같은 클립 찾기 
            foreach (var s in sounds)
            {
                if(s.name == name)
                {
                    sound = s;
                    break;
                }
            }
            // 매개변수 이름과 같은 클립이 없으면
            if(sound == null)
            {
                Debug.Log($"Cannot Find {name}");
                return;
            }
            sound.source.Play();
        }

        public void Stop(string name)
        {
            Sound sound = null;

            // 매개변수 이름과 같은 클립 찾기 
            foreach (var s in sounds)
            {
                if (s.name == name)
                {
                    sound = s;

                    //
                    if(s.name == bgmSound)
                    {
                        bgmSound = "";
                    }
                    break;
                }
            }
            // 매개변수 이름과 같은 클립이 없으면
            if (sound == null)
            {
             //   Debug.Log($"Cannot Find {name}");
                return;
            }
            sound.source.Stop();
        }

        // 배경음 재생
        public void PlayBgm(string name)
        {
            // 배경음 이름 체크 
            if(bgmSound == name)
            {
                return;
            }
            // 배경음 정지
            StopBgm();

            Sound sound = null;

            foreach (var s in sounds)
            {
                if(s.name == name)
                {
                    bgmSound = s.name;
                    sound = s;
                    break;
                }
            }
            // 매개변수 이름과 같은 클립이 없으면
            if (sound == null)
            {
                Debug.Log($"Cannot Find {name}");
                return;
            }
            sound.source.Play();
        }
        public void StopBgm()
        {
            Stop(bgmSound);
        }
    }
