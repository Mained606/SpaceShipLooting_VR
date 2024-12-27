using UnityEngine;

public class EnemyCheck : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 15f; // 일반 탐지 반경
    [SerializeField] private float detectionCloseRadius = 5f; // 가까운 탐지 반경
    public string enemyTag = "Enemy"; // 적 태그

    private bool isHeartSoundPlaying = false; // 일반 심장 소리 재생 상태
    private bool isFastHeartSoundPlaying = false; // 빠른 심장 소리 재생 상태

    private void Update()
    {
        // 탐지 반경 내의 적 확인
        Collider[] collidersInRange = Physics.OverlapSphere(transform.position, detectionRadius);

        // 범위 안에 Enemy 태그를 가진 적이 있는지 확인
        Transform closestEnemy = FindClosestEnemy(collidersInRange);
        if (closestEnemy != null)
        {
            float distanceToEnemy = Vector3.Distance(transform.position, closestEnemy.position);

            if (distanceToEnemy <= detectionCloseRadius)
            {
                PlayFastHeartBeatSound(); // 빠른 심장 소리 재생
            }
            else
            {
                PlayHeartBeatSound(); // 일반 심장 소리 재생
            }
        }
        else
        {
            StopHeartBeatSound(); // 소리 멈춤
            PlayFastHeartBeatSound();
        }
    }

    private Transform FindClosestEnemy(Collider[] colliders)
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag(enemyTag)) // Enemy 태그 확인
            {
                float distance = Vector3.Distance(transform.position, collider.transform.position);
                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    closest = collider.transform;
                }
            }
        }

        return closest;
    }

    private void PlayHeartBeatSound()
    {
        if (!isHeartSoundPlaying)
        {
            StopFastHeartBeatSound(); // 빠른 심장 소리를 멈춤
            isHeartSoundPlaying = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play("HeartBeatStrongMid", true, 1f, 0.8f); // 일반 심장 소리 재생
            }
        }
    }

    private void PlayFastHeartBeatSound()
    {
        if (!isFastHeartSoundPlaying)
        {
            StopHeartBeatSound(); // 일반 심장 소리를 멈춤
            isFastHeartSoundPlaying = true;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Play("HeartBeatStrongFast", true, 1.4f, 0.8f); // 빠른 심장 소리 재생
            }
        }
    }

    private void StopHeartBeatSound()
    {
        if (isHeartSoundPlaying)
        {
            isHeartSoundPlaying = false;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Stop("HeartBeatStrongMid"); // 일반 심장 소리 멈춤
            }
        }
    }

    private void StopFastHeartBeatSound()
    {
        if (isFastHeartSoundPlaying)
        {
            isFastHeartSoundPlaying = false;

            if (AudioManager.Instance != null)
            {
                AudioManager.Instance.Stop("HeartBeatStrongFast"); // 빠른 심장 소리 멈춤
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        // 탐지 반경을 시각적으로 확인하기 위한 Gizmo
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionCloseRadius);
    }
}