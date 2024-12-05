using UnityEngine;

public interface ISignalReceiver
{
    void ReceiveSignal(string signal);
    GameObject GetGameObject();
}
