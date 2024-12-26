using UnityEngine;

public class CardKey : MonoBehaviour
{
    private void Awake()
    {
        JsonTextManager.instance.OnDialogue("stage2-8");
        AudioManager.Instance.Play("KeyCard", false);
    }
}
