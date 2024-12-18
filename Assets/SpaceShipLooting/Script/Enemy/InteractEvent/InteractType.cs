using System;
using UnityEngine;

public enum InteractType
{
    PipeExplosion,
    Dispatch,
    BusterCall,
    None
}

[Serializable]
public class InteractEventData
{
    public InteractType interactType;
    public Transform interactPosition;
}

