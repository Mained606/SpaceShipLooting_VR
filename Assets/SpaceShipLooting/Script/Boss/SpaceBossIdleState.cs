using Unity.VisualScripting;
using UnityEngine;

public class SpaceBossIdleState : State<BossController>
{
    private float searchRange = 30f;

    public override void OnInitialized()
    {

    }

    public override void OnEnter()
    {
        
    }

    public override void Update(float deltaTime)
    {
        // if()
    }

    public override void OnExit()
    {

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(context.transform.position, searchRange);
    }  
}
