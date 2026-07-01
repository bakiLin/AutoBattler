using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Action Rush", fileName = "Action Rush")]
public class ActionRush : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.Turn == 1)
            return battleData.Weapon.Damage;
        return 0;
    }
}
