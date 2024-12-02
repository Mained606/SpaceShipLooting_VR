using UnityEngine;

public enum SpawnType
{
    RandomPatrol,
    WayPointPatrol,
    normal
}

public class SpawnerType : MonoBehaviour
{

    [SerializeField] Transform[] wayPoints;
    SpawnType currentSpawnerType;
    EnemyPatrol[] currentEnemies;

    private void Start()
    {
        if(wayPoints == null || wayPoints.Length == 0)
        {
            currentSpawnerType = SpawnType.RandomPatrol;
            Debug.Log("SpawnType : Random");
        }
        else if(wayPoints != null && wayPoints.Length >= 2)
        {
            Debug.Log("SpawnType : WayPoint");
            currentSpawnerType = SpawnType.WayPointPatrol;
        }
        else if(wayPoints != null && wayPoints.Length == 1)
        {
            Debug.Log("SpawnType : Normal");
            currentSpawnerType = SpawnType.normal;
        }

        if(transform.childCount > 0)
        {
            currentEnemies = new EnemyPatrol[transform.childCount];
            wayPoints = new Transform[transform.GetChild(0).childCount];
            for(int j = 0; j < transform.GetChild(0).childCount; j++)
            {
                wayPoints[j] = transform.GetChild(0).GetChild(j);
            } 
            if (currentSpawnerType == SpawnType.WayPointPatrol)
            {
                for(int i =1; i < transform.childCount; i++)
                {
                   
                    currentEnemies[i] = transform.GetChild(i).GetComponent<EnemyPatrol>();
                    Debug.Log(currentEnemies[i] + " , " + currentSpawnerType.ToString() + " , " + wayPoints.Length);

                    currentEnemies[i].SetSpawnType(currentSpawnerType, wayPoints);
                    i++;
                }
            }
            else if(currentSpawnerType == SpawnType.RandomPatrol || currentSpawnerType == SpawnType.normal)
            {
                for(int i =1; i < transform.childCount; i++)
                {
                    currentEnemies[i] = transform.GetChild(i).GetComponent<EnemyPatrol>();
                    currentEnemies[i].SetSpawnType(currentSpawnerType, wayPoints);
                }
            }
        }

    }
}
