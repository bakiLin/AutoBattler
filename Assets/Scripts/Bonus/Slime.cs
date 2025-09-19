using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Slime", fileName = "Slime")]
public class Slime : BonusBase, IBonus
{
    public override int Bonus(BattleData battleData)
    {
        if (battleData.WeaponType == WeaponType.Slashing)
            return -battleData.WeaponDamage;
        return 0;
    }
}
