using UnityEngine;
using TMPro;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class SpaceBossCoreExplosionState : State<BossController>
{
    private SpaceBossController boss;
    private Dictionary<GameObject, Coroutine> activeCoreCoroutines = new Dictionary<GameObject, Coroutine>();

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
        Debug.Log("보스 코어 폭발 상태 진입");

        boss.canvas.gameObject.SetActive(true);
        boss.textbox.text = "Core Explosion...";

        ActivateRandomCoreAttack();
    }

    public override void Update(float deltaTime)
    {
        if (activeCoreCoroutines.Count == 0)
        {
            boss.SpaceBossDefenceState(); // 디펜스 상태로 전환
        }
    }

    public override void OnExit()
    {
        Debug.Log("보스 코어 폭발 상태 종료");
        StopAllCoreCoroutines();

        boss.canvas.gameObject.SetActive(false);
    }

    private void ActivateRandomCoreAttack()
    {
        if (boss.cores == null || boss.cores.Length < 1)
        {
            Debug.LogError("코어가 충분하지 않습니다.");
            return;
        }

        var selectedCores = boss.cores
            .OrderBy(x => Random.value)
            .Take(Mathf.Min(boss.cores.Length, 2))
            .ToArray();

        foreach (var core in selectedCores)
        {
            ActivateWire(core, true);
            Coroutine coroutine = boss.StartSkillCoroutine(PerformAreaAttack(core));
            activeCoreCoroutines[core] = coroutine;
        }
    }

    private void ActivateWire(GameObject core, bool isActive)
    {
        var wire = core.transform.Find("Wire");
        if (wire != null)
        {
            wire.gameObject.SetActive(isActive);

            if (isActive)
            {
                var wireScript = wire.GetComponent<Wire>();
                if (wireScript != null)
                {
                    wireScript.Initialize(this); // 현재 스테이트를 전달
                }
            }
        }
        else
        {
            Debug.LogWarning("와이어 오브젝트를 찾을 수 없습니다.");
        }
    }

    private IEnumerator PerformAreaAttack(GameObject core)
    {
        yield return new WaitForSeconds(boss.ExplosionDelay);

        if (IsWireActive(core))
        {
            Vector3 position = core.transform.position;

            Collider[] hitColliders = Physics.OverlapSphere(position, boss.ExplosionRadius);
            foreach (var collider in hitColliders)
            {
                if (collider.gameObject.CompareTag("Player"))
                {
                    Damageable damageable = collider.GetComponent<Damageable>();
                    if (damageable != null)
                    {
                        damageable.InflictDamage(boss.ExplosionDamage);
                        Debug.Log($"범위 공격이 {collider.gameObject.name}에 {boss.ExplosionDamage}의 데미지를 주었습니다.");
                    }
                }
            }
        }

        ActivateWire(core, false);
        activeCoreCoroutines.Remove(core);
    }

    private bool IsWireActive(GameObject core)
    {
        var wire = core.transform.Find("Wire");
        return wire != null && wire.gameObject.activeSelf;
    }

    private void StopAllCoreCoroutines()
    {
        foreach (var coroutine in activeCoreCoroutines.Values)
        {
            boss.StopCoroutine(coroutine);
        }
        activeCoreCoroutines.Clear();
    }

    public void OnWireTriggerEnter(GameObject wire)
    {
        var core = wire.transform.parent.gameObject;
        if (activeCoreCoroutines.TryGetValue(core, out var coroutine))
        {
            boss.StopCoroutine(coroutine);
            activeCoreCoroutines.Remove(core);
            ActivateWire(core, false);
            Debug.Log($"{core.name}의 폭발이 취소되었습니다.");
        }
    }
}