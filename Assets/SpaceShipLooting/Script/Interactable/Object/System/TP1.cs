using UnityEngine;

public class TP1 : MonoBehaviour
{
    public SceneFader fader;
    [SerializeField] private string loadToScene = "ProtoScene2";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        fader.FadeTo(loadToScene);
    }

}
