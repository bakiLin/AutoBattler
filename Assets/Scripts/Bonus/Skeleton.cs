using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Skeleton", fileName = "Skeleton")]
public class Skeleton : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.WeaponType == WeaponType.Bludgeoning)
            return battleData.AttackStats.Strength + battleData.WeaponDamage;
        return 0;
    }
}
