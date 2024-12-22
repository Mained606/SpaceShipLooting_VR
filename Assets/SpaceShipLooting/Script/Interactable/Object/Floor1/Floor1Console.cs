using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Floor1Console : MonoBehaviour, ISignal
{
    public static UnityEvent<bool> consoleCheck = new UnityEvent<bool>();
    public static UnityEvent<bool> consoleFalse = new UnityEvent<bool>();

    private int Scount = 0;
    private int Fcount = 0;

    [Header("Object Settings")]
    [SerializeField] private List<string> Tags; // 여러 태그를 받을 리스트
    private MeshRenderer[] render;
    private GameObject Particle;

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
        // 충돌한 오브젝트가 이미 처리되었거나, 지정된 태그에 포함되지 않으면 return
        if (nono.Contains(collision.gameObject) || !Tags.Contains(collision.gameObject.tag)) return;

        nono.Add(collision.gameObject);

        // Succesce 값에 따라 색상을 변경하고 로직 처리
        EffectGo();

        if (Succesce)
        {
            Sender(true); // Success 체크된 경우, consoleCheck 호출
            Debug.Log("트루 발사");
            Scount++;
            if (Scount >= 3)
            {
                Clear(consoleCheck);
            }
        }
        else
        {
            Sender(false); // Success 체크되지 않은 경우, consoleFalse 호출
            Debug.Log("페일 발사");
            Fcount++;
            if (Fcount >= 2)
            {
                Clear(consoleFalse);
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
        Color targetColor = Succesce ? Color.black : Color.red; // 성공 여부에 따라 색상 결정
        foreach (var renderer in render)
        {
            if (renderer != null)
            {
                var mat = renderer.material;
                if (mat.HasProperty("_BaseColor"))
                {
                    mat.SetColor("_BaseColor", targetColor);
                }
                else if (mat.HasProperty("_Color"))
                {
                    mat.SetColor("_Color", targetColor);
                }
                else if (mat.HasProperty("_EmissionColor"))
                {
                    mat.SetColor("_EmissionColor", targetColor);
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
        if (Succesce)
        {
            consoleCheck?.Invoke(state); // Success가 true일 때 consoleCheck 호출
        }
        else
        {
            consoleFalse?.Invoke(state); // Success가 false일 때 consoleFalse 호출
        }
    }

    public void Receiver(bool state) { }
}
