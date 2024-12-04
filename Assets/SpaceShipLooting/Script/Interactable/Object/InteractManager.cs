using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    private Dictionary<ISignal, ISignalReceiver> signalMappings = new Dictionary<ISignal, ISignalReceiver>();

    private void Start()
    {
        AutoBindSignals(); // 자동 바인딩 호출
    }

    private void AutoBindSignals()
    {
        var senders = FindObjectsOfType<MonoBehaviour>().OfType<ISignal>();
        var receivers = FindObjectsOfType<MonoBehaviour>().OfType<ISignalReceiver>();

        foreach (var sender in senders)
        {
            bool isBound = false;

            foreach (var receiver in receivers)
            {
                if (IsMatching(sender, receiver))
                {
                    BindSignal(sender, receiver);
                    isBound = true;
                    break; // 매칭 완료 시 루프 종료
                }
            }

            if (!isBound)
            {
                Debug.LogWarning($"No matching receiver found for sender: {sender.GetGameObject()?.name}");
            }
        }
    }

    private void BindSignal(ISignal sender, ISignalReceiver receiver)
    {
        if (signalMappings.TryGetValue(sender, out var existingReceiver))
        {
            if (existingReceiver != receiver)
            {
                sender.OnSignal.RemoveListener(existingReceiver.ReceiveSignal); // 기존 연결 제거
                sender.OnSignal.AddListener(receiver.ReceiveSignal);           // 새로운 연결 추가
                signalMappings[sender] = receiver;                            // 매핑 갱신
            }
        }
        else
        {
            sender.OnSignal.AddListener(receiver.ReceiveSignal);
            signalMappings[sender] = receiver;
        }
    }

    private bool IsMatching(ISignal sender, ISignalReceiver receiver)
    {
        var senderObject = sender.GetGameObject();
        var receiverObject = (receiver as MonoBehaviour)?.gameObject;

        // 이름 기반 비교
        return senderObject != null && receiverObject != null &&
               senderObject.name == receiverObject.name; // 이름이 동일하면 매칭
    }

    private void OnDestroy()
    {
        foreach (var sender in signalMappings.Keys)
        {
            sender.ClearListeners(); // 발신자에 연결된 모든 리스너 제거
        }
    }
}
