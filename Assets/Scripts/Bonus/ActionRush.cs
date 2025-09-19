using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Action Rush", fileName = "Action Rush")]
public class ActionRush : BonusBase, IBonus
{
    public override int Bonus(BattleData battleData)
    {
        if (battleData.Turn == 1)
            return battleData.WeaponDamage;
        return 0;
    }
}
