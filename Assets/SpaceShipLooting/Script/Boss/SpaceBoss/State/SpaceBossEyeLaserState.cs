using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class SpaceBossEyeLaserState : State<BossController>
{
    private SpaceBossController boss;

    public override void OnInitialized()
    {
        // context를 SpaceBossController로 캐스팅
        boss = context as SpaceBossController;
        if (boss == null)
        {
            Debug.LogError("SpaceBossController를 초기화할 수 없습니다.");
        }
    }

    public override void OnEnter()
    {
        Debug.Log("보스 레이저 공격 상태 진입");
        if (boss == null) return;

        boss.StopAllSkillCoroutines(); // 이전 상태의 모든 코루틴 종료


        boss.canvas.gameObject.SetActive(true);
        boss.textbox.text = "Laser State...";

        // 레이저 공격 시작 이벤트 실행
        boss.OnLaserStateStarted?.Invoke();
        TriggerLaserStartEffects();

        // 레이저 공격 시작 효과 실행
        boss.StartSkillCoroutine(LaserAttackSequence());
    }

    private IEnumerator LaserAttackSequence()
    {
        // 1. 플레이어를 따라다니며 추적하는 단계 (3초)
        float trackingDuration = boss.TrackingDuration;
        float trackingTime = 0f;

        while (trackingTime < trackingDuration)
        {
            trackingTime += Time.deltaTime;

            if (boss.Target != null)
            {
                // 눈이 플레이어의 현재 위치를 계속 추적
                boss.AdjustEyePosition(true, boss.Target.position);
            }

            yield return null; // 매 프레임 대기
        }

        // 2. 레이저 공격 준비 단계
        boss.textbox.text = "Laser Charging...";
        boss.AdjustEyePosition(true, boss.Target.position); // 마지막 추적 위치로 눈 고정
        yield return new WaitForSeconds(boss.LaserChargeDuration); // 레이저 충전 시간

        // 3. 레이저 발사
        boss.textbox.text = "Laser Attack!";
        if (boss.Target != null)
        {
            boss.FireLaser(boss.Target.position); // 플레이어의 최종 위치를 목표로 레이저 발사
        }

        // 4. 레이저 쿨다운 후 디펜스 상태로 전환
        yield return new WaitForSeconds(2f);
        boss.SpaceBossDefenceState();
    }

    public override void Update(float deltaTime)
    {

    }

    public override void OnExit()
    {
        Debug.Log("보스 레이저 공격 상태 종료");

        boss.canvas.gameObject.SetActive(false);

        // 레이저 공격 종료 이벤트 실행
        boss.OnLaserStateEnded?.Invoke();
        TriggerLaserEndEffects();
    }

    private void TriggerLaserStartEffects()
    {
        boss.AdjustEyePosition(true);
    }

    private void TriggerLaserEndEffects()
    {
        if (!boss.AllCoresDestroyed)
        {
            boss.AdjustEyePosition(false);
        }
    }
}
