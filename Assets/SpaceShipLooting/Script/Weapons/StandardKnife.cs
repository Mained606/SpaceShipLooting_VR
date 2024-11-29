using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class StandardKnife : XRGrabInteractable
{
    [SerializeField] private Transform rayOrigin; // 칼날 끝에서 레이를 발사할 위치
    
    [SerializeField] private float damage = 20f;  // 적에게 줄 데미지
    
    [SerializeField] private float rayLength = 1f; // 레이의 길이
    
    [SerializeField] private LayerMask enemyLayer;  // 적을 식별하기 위한 레이어
    
    [SerializeField] private float swingSpeedThreshold = 1f; // 스윙으로 간주할 최소 속도

    private Vector3 previousPosition; // 이전 프레임에서의 위치를 저장
    private bool isSwinging = false;  // 스윙 상태를 나타냄

    // protected override void Start()
    // {
    //     base.Start();
    //     previousPosition = transform.position; // 초기 위치 설정
    // }

    private void Update()
    {
        DetectEnemiesWithRay();
        // 칼의 현재 이동 속도 계산
        // float speed = (transform.position - previousPosition).magnitude / Time.deltaTime;

        // // 스윙 속도가 임계값을 넘었는지 확인
        // if (speed > swingSpeedThreshold)
        // {
        //     if (!isSwinging)
        //     {
        //         isSwinging = true; // 스윙 시작
        //         DetectEnemiesWithRay(); // 레이로 적 탐지
        //     }
        // }
        // else
        // {
        //     isSwinging = false; // 스윙 중단
        // }

        // 다음 프레임을 위해 이전 위치 업데이트
        // previousPosition = transform.position;
    }

    private void DetectEnemiesWithRay()
    {
        // 칼날 끝에서 레이를 발사하여 적 탐지
        if (Physics.Raycast(rayOrigin.position, rayOrigin.forward, out RaycastHit hit, rayLength, enemyLayer))
        {
            Debug.Log(hit.collider.gameObject.name);
            // 충돌한 오브젝트에 Damageable 컴포넌트가 있는지 확인
            Damageable damageable = hit.collider.GetComponent<Damageable>();
            if (damageable != null)
            {
                // 적에게 데미지 적용
                damageable.InflictDamage(damage);
                Debug.Log($"적을 타격함: {hit.collider.name}, {damage} 데미지 입힘.");
            }
        }
    }

    private void OnDrawGizmos()
    {
        // 디버깅을 위해 에디터에서 레이를 시각화
        if (rayOrigin != null)
        {
            Gizmos.color = isSwinging ? Color.green : Color.red; // 스윙 중이면 초록색, 아니면 빨간색
            Gizmos.DrawRay(rayOrigin.position, rayOrigin.forward * rayLength); // 레이 그리기
        }
    }
}