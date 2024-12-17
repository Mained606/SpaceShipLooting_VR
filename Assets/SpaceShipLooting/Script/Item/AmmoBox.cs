using UnityEngine;

public class AmmoBox : MonoBehaviour
{
    [SerializeField] private int ammoCount = 7;

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("총알 지급");
            GameManager.Instance.PlayerStatsData.AddAmmo(ammoCount);

            Debug.Log(GameManager.Instance.PlayerStatsData.maxAmmo);

            Destroy(gameObject);
        }
    }
}
