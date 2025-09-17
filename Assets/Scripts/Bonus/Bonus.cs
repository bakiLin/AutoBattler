using UnityEngine;

public abstract class Bonus : ScriptableObject
{
    [SerializeField]
    private BonusType _bonusType;
}

public enum BonusType 
{ 
    Attack, 
    Defence 
}
