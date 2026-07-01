using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Skeleton", fileName = "Skeleton")]
public class Skeleton : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.Weapon.Type == WeaponType.Bludgeoning)
            return battleData.Attacker.Strength + battleData.Weapon.Damage;
        return 0;
    }
}
