using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InteractManager : MonoBehaviour
{
    private Dictionary<ISignal, ISignalReceiver> signalMappings = new Dictionary<ISignal, ISignalReceiver>();

    private void Start()
    {
        AutoBindSignals();
    }

    private void AutoBindSignals()
    {
        // 모든 ISignal 및 ISignalReceiver 가져오기
        var senders = FindObjectsOfType<MonoBehaviour>().OfType<ISignal>();
        var receivers = FindObjectsOfType<MonoBehaviour>().OfType<ISignalReceiver>();

        foreach (var sender in senders)
        {
            foreach (var receiver in receivers)
            {
                // 이름 기반 매칭
                if (sender.GetGameObject().name == receiver.GetGameObject().name)
                {
                    sender.OnSignal.AddListener(receiver.ReceiveSignal);
                    signalMappings[sender] = receiver;

                    Debug.Log($"Mapped: {sender.GetGameObject().name} -> {receiver.GetGameObject().name}");
                }
            }
        }
    }

    private void OnDestroy()
    {
        foreach (var sender in signalMappings.Keys)
        {
            sender.ClearListeners(); // 모든 리스너 제거
        }
    }
}
