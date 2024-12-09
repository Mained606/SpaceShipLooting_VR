using UnityEngine;

public class BossRoomWall : MonoBehaviour
{
    public Animator animator; // Animator 컴포넌트

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            animator.SetTrigger("StartAnimation");
        }
    }
}
