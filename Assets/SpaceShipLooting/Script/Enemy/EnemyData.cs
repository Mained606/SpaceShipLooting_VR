using UnityEngine;

[System.Serializable]
public class EnemyData
{
    public float health;
    public EnemyType enemyType;
    public float moveSpeed;
    public float runPerceptionRange;
    public float stealthPerceptionRange;
    public float attackRange;
    public float attackDamage;
    public float attackInterval;
    public float chaseInterval;
    public GameObject targetEncounterUI;
    public GameObject item;
}

public enum EnemyType
{
    RandomPatrol,
    WaypointPatrol,
    NonePatrol
}
