using UnityEngine;

public abstract class BonusBase : ScriptableObject, IBonus
{
    [SerializeField]
    private int _id;

    public int Id { get => _id; }

    [SerializeField]
    private BonusType _bonusType;

    public BonusType BonusType { get => _bonusType; }

    public virtual int Bonus(BattleData battleData) => 0;
}

public enum BonusType 
{ 
    Attack, 
    Defence 
}
