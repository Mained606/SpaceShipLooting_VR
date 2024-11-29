using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    private float rotation;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(transform.localRotation);
        transform.localRotation = Quaternion.Euler(transform.localEulerAngles.x, transform.localEulerAngles.y + 1f, transform.localEulerAngles.z);
    }
}
