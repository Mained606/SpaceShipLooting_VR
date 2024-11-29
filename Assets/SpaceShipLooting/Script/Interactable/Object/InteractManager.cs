using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class SignalPair
{
    public MonoBehaviour sender;   // A 역할 (SelectObject 또는 CollisionObject & GrabObject)
    public MonoBehaviour receiver; // B 역할 (ISignal 인터페이스 상속)
}

public class InteractManager : MonoBehaviour
{
    public List<SignalPair> signalPairs = new List<SignalPair>(); // A-B 페어 리스트

    private Dictionary<MonoBehaviour, ISignal> signalMappings = new Dictionary<MonoBehaviour, ISignal>();

    private void Start()
    {
        // SignalPair 리스트를 기반으로 딕셔너리 초기화
        foreach (var pair in signalPairs)
        {
            if (pair.sender != null && pair.receiver != null && pair.receiver is ISignal)
            {
                signalMappings[pair.sender] = (ISignal)pair.receiver;
                // UnityEvent 리스너 추가
                if (pair.sender is ISignal senderSignal)
                {
                    senderSignal.OnSignal.AddListener((signal) => SignalReceive(signal, pair.sender));
                }
            }
        }
    }

    public void SignalReceive(string signal, MonoBehaviour sender)
    {
    //    Debug.Log($"Signal received: {signal} from {sender.name}");

        // 딕셔너리에서 Sender와 연결된 Receiver 찾기
        if (signalMappings.TryGetValue(sender, out ISignal receiver))
        {
     //       Debug.Log($"Forwarding signal to: {receiver}");
            receiver.ReceiveSignal();
        }
        else
        {
      //      Debug.LogWarning($"No receiver found for sender: {sender.name}");
        }
    }
}