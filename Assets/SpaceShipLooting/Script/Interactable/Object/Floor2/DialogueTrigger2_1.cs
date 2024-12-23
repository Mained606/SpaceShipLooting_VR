using System.Collections;
using UnityEngine;

public class DialogueTrigger2_1 : MonoBehaviour
{
    public GameObject Trigger2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
        JsonTextManager.instance.OnDialogue("stage2-2");
        Trigger2.SetActive(true);
        this.gameObject.SetActive(false);
    }
   
}
