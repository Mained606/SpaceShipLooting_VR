using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Floor1Console: MonoBehaviour, ISignal
{
    public static UnityEvent<bool> consoleCheck = new UnityEvent<bool>();
    private int Scount = 0;
    private int Fcount = 0;
 //   private ParticleSystem boom;
    [SerializeField] private string Tag;
    private MeshRenderer[] render;
    private GameObject Particle;

    [Header("Object Settings")]
    [SerializeField] private bool Succesce; // Inspector에서 체크 여부 설정
    private HashSet<GameObject> nono = new HashSet<GameObject>(); // 중복 사격 방지

    void Start()
    {
        Debug.Log($"This script is attached to: {gameObject.name}");

        Particle = transform.Find("Particle")?.gameObject;
        render = new MeshRenderer[3];
        render[0] = transform.Find("Screen_A")?.GetComponent<MeshRenderer>();
        render[1] = transform.Find("Screen_B")?.GetComponent<MeshRenderer>();
        render[2] = transform.Find("Screen_C")?.GetComponent<MeshRenderer>();

        foreach (var renderer in render)
        {
            if (renderer == null)
            {
                Debug.LogError("렌더러가 없잖아");
            }
        }

    //    boom = GetComponent<ParticleSystem>();

        if (Particle == null)
        {
            Debug.Log(" 파티클이 없다고 ");
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (nono.Contains(collision.gameObject) || !collision.gameObject.CompareTag(Tag)) return; // 이미 처리된 오브젝트는 무시
        nono.Add(collision.gameObject);

        if (collision.gameObject.CompareTag(Tag))
        {
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
    }
    private void EffectGo()
    {
        // boom.Play();

        if (Particle != null)
        {
            Particle.SetActive(true);
        }

        foreach (var renderer in render)
        {
            if (renderer != null)
            {
                var mat = renderer.material;

                if (mat.name.Contains("HUD_Screen_01"))
                {
                    if (mat.HasProperty("_BaseColor"))
                    {
                        mat.SetColor("_BaseColor", Color.black);
                    }
                    else if (mat.HasProperty("_Color"))
                    {
                        mat.SetColor("_Color", Color.black);
                    }
                    else
                    {
                        Debug.LogError("HUD_Screen_01 does not support _BaseColor or _Color");
                    }
                }
                else if (mat.name.Contains("HUD_Screen_02"))
                {
                    if (mat.HasProperty("_EmissionColor"))
                    {
                        mat.SetColor("_EmissionColor", Color.black);
                    }
                    else if (mat.HasProperty("_Color"))
                    {
                        mat.SetColor("_Color", Color.black);
                    }
                    else
                    {
                        Debug.LogError("HUD_Screen_02 does not support _EmissionColor or _Color");
                    }
                }
                else
                {
                    Debug.LogError($"Unknown material: {mat.name}");
                }
            }
        }
    }
   
    public void Clear(UnityEvent<bool> signal) => signal.RemoveAllListeners();

    public void Sender(bool state) => consoleCheck?.Invoke(state);

    public void Receiver(bool state) { }
}
