using UnityEngine;

public abstract class BonusBase : ScriptableObject
{
    [field: SerializeField] public BonusType Type { get; private set; }

    public abstract int Use(TurnData battleData);
}

public enum BonusType 
{ 
    Attack, 
    Defence 
}
