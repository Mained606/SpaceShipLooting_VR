using System.Collections;
using UnityEngine;

public class DialogueTrigger2_2 : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(kalbbang());
    }

    IEnumerator kalbbang()
    {
        JsonTextManager.instance.OnDialogue("stage2-3");
        yield return new WaitForSeconds(8f);
        JsonTextManager.instance.OnDialogue("stage2-4");
        this.gameObject.SetActive(false);
    }

}
