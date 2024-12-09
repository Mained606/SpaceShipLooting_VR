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
        // 1. 플레이어를 따라다니는 3초
        float trackingDuration = boss.TrackingDuration;
        float trackingTime = 0f;

        while (trackingTime < trackingDuration)
        {
            trackingTime += Time.deltaTime;
            if (boss.Target != null)
            {
                // 눈알을 플레이어의 현재 위치로 회전
                boss.AdjustEyePosition(true, boss.Target.position);
            }
            yield return null;
        }

        // 2. 공격 위치 고정 및 에너지 모으기
        Vector3 fixedAttackPosition = boss.EyeTransform.position; // 공격 위치 고정
        
        // boss.SetChargeEffect(true); // 에너지 모으는 이펙트 활성화

        yield return new WaitForSeconds(boss.LaserChargeDuration); // 3초간 에너지 모으기
        boss.textbox.text = "Laser Charge...";  


        // boss.SetChargeEffect(false); // 에너지 모으는 이펙트 비활성화

        // 3. 고정된 위치에 레이저 발사
        boss.textbox.text = "Laser Attack...";
        boss.FireLaser(fixedAttackPosition);

        yield return new WaitForSeconds(2f);

        // yield return new WaitForSeconds(boss.LaserCooldown); // 쿨다운

        // 디펜스 상태로 전환
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
