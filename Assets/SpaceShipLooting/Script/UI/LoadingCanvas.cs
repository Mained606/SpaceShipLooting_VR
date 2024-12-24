using System;
using System.Collections;
using UnityEngine;

public class LoadingCanvas : MonoBehaviour
{
    [SerializeField] private SceneFader fader;

    private void Start()
    {
        StartCoroutine(StartLoading());
    }

    private IEnumerator StartLoading()
    {
        yield return new WaitForSeconds(2f);

        fader.FadeTo(1);
    }
}
