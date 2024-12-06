using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// InteractManager는 Sender와 Receiver를 자동으로 매핑하고, 신호를 전달합니다.
public class InteractManager : MonoBehaviour
{
    // Sender와 Receiver의 매핑 정보를 저장
    private Dictionary<ISignal, ISignalReceiver> signalMappings = new Dictionary<ISignal, ISignalReceiver>();

    private void Start()
    {
        // 시작 시 모든 Sender와 Receiver를 검색해 매핑
        AutoBindSignals();
    }

    // 모든 Sender와 Receiver를 검색해 자동으로 연결합니다.
    private void AutoBindSignals()
    {
        // 모든 Sender와 Receiver를 찾음
        var senders = FindObjectsOfType<MonoBehaviour>().OfType<ISignal>();
        var receivers = FindObjectsOfType<MonoBehaviour>().OfType<ISignalReceiver>();

        foreach (var sender in senders)
        {
            foreach (var receiver in receivers)
            {
                // Sender와 Receiver의 이름 접두사를 추출
                string senderPrefix = sender.GetGameObject().name.Split('_')[0];
                string receiverPrefix = receiver.GetGameObject().name.Split('_')[0];

                // 접두사를 비교해 매핑
                if (senderPrefix == receiverPrefix)
                {
                    // Sender의 이벤트에 Receiver의 동작을 연결
                    sender.OnSignal.AddListener(signal => HandleSignal(sender, signal));
                    signalMappings[sender] = receiver;

                    Debug.Log($"Mapped: {sender.GetGameObject().name} -> {receiver.GetGameObject().name}");
                }
            }
        }
    }

    // Sender로부터 신호를 받아 처리합니다.
    private void HandleSignal(ISignal sender, string signal)
    {
        Debug.Log($"Received Signal: {signal} from {sender.GetGameObject().name}");

        // Sender와 매핑된 Receiver를 찾아 신호를 전달
        if (signalMappings.TryGetValue(sender, out ISignalReceiver receiver))
        {
            receiver.ReceiveSignal(signal);
        }
        else
        {
            Debug.LogWarning($"No Receiver found for Sender: {sender.GetGameObject().name}");
        }
    }

    // 매니저가 파괴될 때 리스너를 정리합니다.
    private void OnDestroy()
    {
        foreach (var sender in signalMappings.Keys)
        {
            sender.ClearListeners();
        }
    }
}
