using UnityEngine;

public abstract class BonusBase : ScriptableObject
{
    [field: SerializeField] public BonusType Type { get; protected set; }

    public abstract int Use(BattleCharacter attacker, BattleCharacter target, int turn);
}

public enum BonusType 
{ 
    Attack, 
    Defence 
}
