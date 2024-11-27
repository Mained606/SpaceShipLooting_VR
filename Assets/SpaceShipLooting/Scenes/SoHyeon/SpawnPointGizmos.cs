using UnityEngine;

public class SpawnPointGizmos : MonoBehaviour
{
    private float patrolRange = Enemy.patrolRange;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, patrolRange);
    }
}
