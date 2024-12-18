using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class MainScene1 : MonoBehaviour
{
    public SceneFader Fader;
    public TextMeshProUGUI textBox;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator Main1play()
    {
        Fader.FromFade(3f);  // 5초 동안 페이드 효과

        yield return null;
    }

}
