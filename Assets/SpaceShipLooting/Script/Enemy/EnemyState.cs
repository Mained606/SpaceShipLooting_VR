using System.Collections.Generic;
using System;
using UnityEngine;

public enum EnemyState
{
    E_Idle,
    E_Move,
    E_Chase,
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
    None
}
