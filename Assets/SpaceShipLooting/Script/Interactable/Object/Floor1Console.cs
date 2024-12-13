using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Floor1Console: MonoBehaviour, ISignal
{
    public static UnityEvent<bool> consoleCheck = new UnityEvent<bool>();
    private int Scount = 0;
    private int Fcount = 0;
    private ParticleSystem boom;
    [SerializeField] private string Tag;
    private Renderer render;

    [Header("Object Settings")]
    [SerializeField] private bool Succesce; // Inspector에서 체크 여부 설정
    private HashSet<GameObject> nono = new HashSet<GameObject>(); // 중복 사격 방지

    void Start()
    {
        render = GetComponentInChildren<Renderer>();

        if(render == null) Debug.Log(" 렌더러가 없잖아 ");
        else boom = GetComponent<ParticleSystem>();

        if (boom == null)
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
        boom.Play();

        render.material = new Material(render.material); // 개별 마테리얼 생성 
        render.material.SetColor("BaseMap", Color.black);    // 색상 변경 
    }
   
    public void Clear(UnityEvent<bool> signal) => signal.RemoveAllListeners();

    public void Sender(bool state) => consoleCheck?.Invoke(state);

    public void Receiver(bool state) { }
}
