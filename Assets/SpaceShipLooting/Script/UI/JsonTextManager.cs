using System.Collections;
using NUnit.Framework;
using UnityEngine;
using TMPro;

public class JsonTextManager : MonoBehaviour
{
    public static JsonTextManager instance;

    [SerializeField] private TMP_Text text;
    Coroutine onDialogueCor;
    private void Awake()
    {
        instance = this;
    }

    public void OnDialogue(string stringKey)
    {
        TextManagerJsonData.GetInstance().LoadDatas();
        var dicString = TextManagerJsonData.GetInstance().dicString_Table;

        if (dicString.ContainsKey(stringKey))
        {
            if (onDialogueCor == null)
            {
                onDialogueCor = StartCoroutine(PlayDialogue(dicString[stringKey].desc));
            }
            else
            {
                Debug.Log("Already Dialogue lines");
            }

        }
        else
        {
            Debug.Log("Key Value Error");
        }
    }

    IEnumerator PlayDialogue(string[] stringDesc)
    {
        foreach (var playDesc in stringDesc)
        {
            text.text = playDesc;
            yield return new WaitForSeconds(1f);
        }

        text.text = "";
        onDialogueCor = null;
        yield return null;
    }
}
