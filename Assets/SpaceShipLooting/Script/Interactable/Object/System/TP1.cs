using UnityEngine.SceneManagement;
using UnityEngine;

public class TP1 : MonoBehaviour
{
    public SceneFader fader;

    [SerializeField] private string loadToScene = "ProtoScene2";

   
    private void OnTriggerEnter(Collider other)
    {
     fader.FadeTo(loadToScene);
    }
}

