using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Skeleton", fileName = "Skeleton")]
public class Skeleton : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        if (attacker.Weapon.Type == WeaponType.Bludgeoning)
            return attacker.Stats.Strength + attacker.Weapon.Damage;
        return 0;
    }
}
