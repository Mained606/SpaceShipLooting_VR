using UnityEngine;
using System.Collections;

public class SpaceBossEMPAttackState : State<BossController>
{
    private SpaceBossController boss;

    private bool isPlayerInStealthMode;
    private bool hasTriggeredEMP; // EMP 공격 트리거 여부

    private ParticleSystem empEffect;
    private ParticleSystem vfx_Implosion;


    public override void OnInitialized()
    {
        // context를 SpaceBossController로 캐스팅
        boss = context as SpaceBossController;
        if (boss == null)
        {
            Debug.LogError("SpaceBossController를 초기화할 수 없습니다.");
        }
        else
        {
            PlayerStateManager.Instance.OnStealthStateChanged.AddListener(PlayerStealthCheck);
        }
    }

    public override void OnEnter()
    {
        Debug.Log("보스 EMP 공격 상태 진입");

        if (boss == null) return;

        boss.StopAllSkillCoroutines(); // 이전 상태의 모든 코루틴 종료

        hasTriggeredEMP = false;

        boss.canvas.gameObject.SetActive(true);
        boss.textbox.text = "EMP Attack...";

        empEffect = boss.empEffect;
        vfx_Implosion = boss.vfx_Implosion;

        Coroutine coroutine = boss.StartSkillCoroutine(TriggerEMPAttackEffects());
    }

    public override void Update(float deltaTime)
    {

    }

    public override void OnExit()
    {
        Debug.Log("보스 EMP 공격 상태 종료");

        boss.canvas.gameObject.SetActive(false);
    }

    // 플레이어 앉은 상태 체크
    private void PlayerStealthCheck(bool isStealth)
    {
        isPlayerInStealthMode = isStealth;
    }

    // EMP 공격 트리거
    private IEnumerator TriggerEMPAttackEffects()
    {
        if (hasTriggeredEMP) yield break;

        hasTriggeredEMP = true;

        //보스 EMP 모으는 사운드 및 이펙트 재생
        AudioManager.Instance.Play("BossEmpCharging");

        vfx_Implosion.gameObject.SetActive(true);
        vfx_Implosion.Play();

        yield return new WaitForSeconds(boss.EMPAttackDuration);

        vfx_Implosion.gameObject.SetActive(false);
        vfx_Implosion.Stop();

        // //EMP 발사 사운드 및 이펙트 재생
        AudioManager.Instance.Play("BossEmp_");

        boss.empEffect.gameObject.SetActive(true);
        boss.empEffect.Play();



        // 범위 내의 모든 콜라이더를 가져옴
        Collider[] hitColliders = Physics.OverlapSphere(boss.transform.position, boss.EMPAttackRadius);
        foreach (var hitCollider in hitColliders)
        {
            // 데미지를 줄 수 있는 객체인지 확인
            if (hitCollider.gameObject.CompareTag("Player"))
            {
                if (isPlayerInStealthMode)
                {
                    Debug.Log("플레이어가 스텔스 모드에 있어 EMP 공격을 피했습니다.");
                    continue;
                }

                Damageable damageable = hitCollider.GetComponent<Damageable>();
                if (damageable != null)
                {
                    // 데미지 적용
                    damageable.InflictDamage(boss.EMPAttackDamage);
                    Debug.Log($"범위 공격이 {hitCollider.gameObject.name}에 {boss.EMPAttackDamage}의 데미지를 주었습니다.");
                }
            }
        }

        yield return new WaitForSeconds(1f);

        empEffect.Stop();
        empEffect.gameObject.SetActive(false);

        // 디펜스 상태로 전환
        boss.SpaceBossDefenceState();
    }
}
