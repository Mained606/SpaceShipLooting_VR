using UnityEngine;

public class BodyObjectPositionSetting : MonoBehaviour
{
    public Transform vrCamera;
    public Transform socket;
    public Transform vest;
    private float vestOffsetZ = -0.105f;


    void Update()
    {
        if (Mathf.Abs(socket.localPosition.x - vrCamera.localPosition.x) > 0f || 
        Mathf.Abs(socket.localPosition.z - vrCamera.position.z) > 0f)
        {
            socket.localPosition = new Vector3(vrCamera.localPosition.x, socket.localPosition.y, vrCamera.localPosition.z);
        }

        if (Mathf.Abs(vest.localPosition.x - vrCamera.localPosition.x) > 0f || 
        Mathf.Abs(vest.localPosition.z - vrCamera.position.z) > 0f)
        {
            vest.localPosition = new Vector3(vrCamera.localPosition.x, vest.localPosition.y, vrCamera.localPosition.z + vestOffsetZ);
        }
    }   
}
