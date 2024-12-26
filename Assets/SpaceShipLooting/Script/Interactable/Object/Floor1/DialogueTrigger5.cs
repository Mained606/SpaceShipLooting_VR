using UnityEngine;

public class DialogueTrigger5 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        JsonTextManager.instance.OnDialogue("stage1-1");
        this.gameObject.SetActive(false);
    }
}
