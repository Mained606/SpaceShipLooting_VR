using UnityEngine;

public class DialogueTrigger2_3 : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        JsonTextManager.instance.OnDialogue("stage2-6");
        this.gameObject.SetActive(false);
    }
}
