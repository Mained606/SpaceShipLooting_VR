using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CoreExplosionSound : MonoBehaviour
{
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.Log("AudioSource를 찾을 수 없습니다");
        }
    }

    private void OnEnable()
    {
        // 오브젝트가 활성화되면 사운드 재생
        PlaySound();
    }

    private void OnDestroy()
    {
        // 오브젝트가 파괴될 때 사운드 중지
        StopSound();
    }

    private void PlaySound()
    {
        if (audioSource != null)
        {
            audioSource.Play(); // 사운드 재생
        }
    }

    private void StopSound()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop(); // 사운드 중지
        }
    }
}