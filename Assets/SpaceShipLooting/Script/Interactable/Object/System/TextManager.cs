using TMPro;
using UnityEngine;
using System.Collections;

public class TextManager : MonoBehaviour
{
    public static TextManager Instance { get; private set; }
    [TextArea(1, 5)]
    public string[] Scripts;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this; 
    }

    public void textNow(int index, TextMeshProUGUI textDisplay)
    {
        textDisplay.text = Scripts[index];
    }

    public IEnumerator textGo(int index, TextMeshProUGUI textDisplay, float befole, float after)
    {
        yield return new WaitForSeconds(befole);
        textDisplay.text = Scripts[index]; 
        yield return new WaitForSeconds(after); 
        textDisplay.text = ""; 
    }
}
 //   TextManager.Instance.textNow(0,textDisplay) -> 즉시 출력 대사 순서, 출력ui

 //   StartCoroutine(TextManager.Instance.textGo(0, textDisplay,0f, 0f)) -> 코루틴 대사 출력전, 후 



