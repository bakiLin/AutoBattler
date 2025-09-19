using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Skeleton", fileName = "Skeleton")]
public class Skeleton : BonusBase, IBonus
{
    public override int Bonus(BattleData battleData)
    {
        if (battleData.WeaponType == WeaponType.Bludgeoning)
            return battleData.AttackStats.Strength + battleData.WeaponDamage;
        return 0;
    }
}
