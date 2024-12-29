using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Management;

public class BodyObjectPositionSetting : MonoBehaviour
{

    public Transform vrCamera;
    public Transform socket;
    public Transform vest;
    public Transform nightVisions;
    [SerializeField] private float vestOffsetY = 1.2f;
    [SerializeField] private float vestOffsetZ = -0.105f;
    [SerializeField] private float socketOffsetY = 1.2f;
    [SerializeField] private float nightVisionsOffsetZ = 0.1f;

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

        if (Mathf.Abs(nightVisions.localPosition.x - vrCamera.localPosition.x) > 0f ||
        Mathf.Abs(nightVisions.localPosition.z - vrCamera.position.z) > 0f)
        {
            nightVisions.localPosition = new Vector3(vrCamera.localPosition.x, vrCamera.localPosition.y, vrCamera.localPosition.z + nightVisionsOffsetZ);
        }
    }
}
