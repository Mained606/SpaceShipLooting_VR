using UnityEngine;

public class DialogueTrigger3 : MonoBehaviour
{

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        JsonTextManager.instance.OnDialogue("stage1-5");
        this.gameObject.SetActive(false);
    }


}
