using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Floor1Console : MonoBehaviour, ISignal
{
    public static UnityEvent<bool> consoleCheck = new UnityEvent<bool>();
    private int Scount = 0;
    private int Fcount = 0;

    [SerializeField] private string Tag;
    private MeshRenderer[] render;
    private GameObject Particle;

    [Header("Object Settings")]
    [SerializeField] private bool Succesce; // Inspector에서 체크 여부 설정
    private HashSet<GameObject> nono = new HashSet<GameObject>(); // 중복 사격 방지

    void Start()
    {
        // Particle 오브젝트와 렌더러 배열 초기화
        Particle = transform.Find("Particle")?.gameObject;
        render = new MeshRenderer[3];
        render[0] = transform.Find("Screen_A")?.GetComponent<MeshRenderer>();
        render[1] = transform.Find("Screen_B")?.GetComponent<MeshRenderer>();
        render[2] = transform.Find("Screen_C")?.GetComponent<MeshRenderer>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (nono.Contains(collision.gameObject) || !collision.gameObject.CompareTag(Tag)) return; // 중복 처리 방지 및 태그 확인

        nono.Add(collision.gameObject);
        EffectGo();

        if (Succesce)
        {
            Sender(true);
            Scount++;
            if (Scount >= 3)
            {
                Clear(consoleCheck);
            }
        }
        else
        {
            Sender(false);
            Fcount++;
            if (Fcount >= 2)
            {
                Clear(consoleCheck);
            }
        }
    }

    private void EffectGo()
    {
        // 파티클 활성화 및 하위 파티클 실행
        if (Particle != null)
        {
            Particle.SetActive(true);
            var particleSystems = Particle.GetComponentsInChildren<ParticleSystem>();
            foreach (var ps in particleSystems)
            {
                ps.Play();
            }
        }

        // 렌더러 색상 변경
        foreach (var renderer in render)
        {
            if (renderer != null)
            {
                var mat = renderer.material;
                if (mat.HasProperty("_BaseColor"))
                {
                    mat.SetColor("_BaseColor", Color.black);
                }
                else if (mat.HasProperty("_Color"))
                {
                    mat.SetColor("_Color", Color.black);
                }
                else if (mat.HasProperty("_EmissionColor"))
                {
                    mat.SetColor("_EmissionColor", Color.black);
                }
            }
        }
    }

    public void Clear(UnityEvent<bool> signal)
    {
        signal.RemoveAllListeners();
    }

    public void Sender(bool state)
    {
        consoleCheck?.Invoke(state);
    }

    public void Receiver(bool state) { }
}
