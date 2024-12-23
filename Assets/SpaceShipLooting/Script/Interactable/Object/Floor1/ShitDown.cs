using UnityEngine;

public class ShitDown : MonoBehaviour
{
    private Collider col;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        col = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        JsonTextManager.instance.OnDialogue("stage1-9");
        this.gameObject.SetActive(false);
    }
}
