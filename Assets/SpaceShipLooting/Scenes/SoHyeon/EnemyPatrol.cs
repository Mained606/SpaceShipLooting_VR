using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
namespace test
{

    public class Test22 : MonoBehaviour
    {
        private float rotationTime = 2f; // 전체 회전에 걸리는 시간
        private bool rotatingLeft = true; // 현재 좌측 회전 중인지 여부
        private Quaternion startRotation; // 초기 회전값
        private Quaternion targetRotation; // 목표 회전값
        private Dictionary<string, float> timers = new Dictionary<string, float>();

        public NavMeshAgent player;

        public GameObject[] arr1;
        float speed = 2f;
        int index = 0;

        bool flag = false;

        void Start()
        {
            startRotation = transform.rotation; // 초기 회전값 저장
            timers.Add("rotationTimer", rotationTime); // 회전 타이머 초기화
            player = GetComponent<NavMeshAgent>();
            player.SetDestination(arr1[index].transform.position);
        }

        private void Update()
        {
            UpdateTimer();
            Check();
            

        }

        void UpdateTimer()
        {
            // Dictionary의 값을 키로 접근하여 업데이트
            var keys = new List<string>(timers.Keys);
            foreach (string key in keys)
            {
                if (timers[key] > 0)
                {
                    timers[key] -= Time.deltaTime; // 타이머 감소
                }
            }
        }

        void LookAround(float leftAngle, float rightAngle)
        {
            if (timers["rotationTimer"] > 0) // 회전 중
            {
                // Lerp로 부드럽게 회전
                transform.rotation = Quaternion.Lerp(
                    startRotation,
                    targetRotation,
                    (rotationTime - timers["rotationTimer"]) / rotationTime
                );
            }
            else // 타이머가 종료되었을 때
            {
                // 회전 방향 반전
                timers["rotationTimer"] = rotationTime;
                rotatingLeft = !rotatingLeft;

                if (rotatingLeft) // 좌측으로 회전
                {
                    startRotation = transform.rotation;
                    targetRotation = Quaternion.LookRotation(
                        Quaternion.AngleAxis(leftAngle, Vector3.up) * transform.forward
                    );
                }
                else // 우측으로 회전
                {
                    startRotation = transform.rotation;
                    targetRotation = Quaternion.LookRotation(
                        Quaternion.AngleAxis(rightAngle, Vector3.up) * transform.forward
                    );
                    flag = false;
                }
                
            }
        }

        void Check()
        {
            
            if(Vector3.Distance(transform.position, arr1[index].transform.position) < 2f)
            {
                player.enabled = false;
                flag = true;
                if(flag)
                {
                    LookAround(-90, 90);
                }
                else
                {
                    index++;
                    if (index == 2)
                    {
                        index = 0;
                    }
                    player.enabled = true;
                    player.SetDestination(arr1[index].transform.position);
                }
                
            }
        }
    }
}