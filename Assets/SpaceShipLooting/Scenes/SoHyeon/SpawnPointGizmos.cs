using UnityEngine;

public class SpawnPointGizmos : MonoBehaviour
{
    private float patrolRange = 10f;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
    }
}
