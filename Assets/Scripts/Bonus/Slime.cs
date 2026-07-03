using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Slime", fileName = "Slime")]
public class Slime : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        if (attacker.Weapon.Type == WeaponType.Slashing)
            return -attacker.Weapon.Damage;
        return 0;
    }
}
