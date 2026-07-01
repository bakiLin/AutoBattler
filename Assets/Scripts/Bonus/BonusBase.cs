using UnityEngine;

public abstract class BonusBase : ScriptableObject
{
    [field: SerializeField] public BonusType Type { get; private set; }

    public abstract int Use(TurnData data);
}

public enum BonusType 
{ 
    Attack, 
    Defence 
}
