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

        // 이름을 기준으로 매칭
        foreach (var sender in senders)
        {
            foreach (var receiver in receivers)
            {
                // 이름이 같은 경우 매핑
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
        // 모든 발신자의 리스너 제거
        foreach (var sender in signalMappings.Keys)
        {
            sender.ClearListeners();
        }
    }
}
