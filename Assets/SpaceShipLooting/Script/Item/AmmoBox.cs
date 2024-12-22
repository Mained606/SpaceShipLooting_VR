using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] private int ammoCount = 7;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("총알 지급");
            GameManager.Instance.PlayerStatsData.AddAmmo(ammoCount);

            Debug.Log(GameManager.Instance.PlayerStatsData.maxAmmo);

            Destroy(transform.parent.gameObject);
        }
    }
}
