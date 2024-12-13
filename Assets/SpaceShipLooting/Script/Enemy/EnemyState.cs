using System.Collections.Generic;
using System;
using UnityEngine;

public enum EnemyState
{
    E_Idle,
    E_Patrol,
    E_Chase,
    E_BusterCall,
    E_Attack,
    E_Death
}

[Serializable]
public class InterActEventData
{
    public InterActType interActType;
    public List<Transform> interActPosition;
}

public enum InterActType
{
    PipeExpolde,
    Dispatch,
    BusterCall,
    None
}
