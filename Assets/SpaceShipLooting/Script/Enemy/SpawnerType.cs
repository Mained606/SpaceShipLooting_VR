using UnityEngine;

public enum SpawnType
{
    WayPointPatrol,
    RandomPatrol,
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
        }
        else
        {
            currentSpawnerType = SpawnType.WayPointPatrol;
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
        }

    }
}
