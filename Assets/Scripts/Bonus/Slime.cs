using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Slime", fileName = "Slime")]
public class Slime : Bonus, IBonus
{
    public int Bonus(BattleData battleData)
    {
        if (battleData.WeaponType == WeaponType.Slashing)
            return -battleData.WeaponDamage;
        return 0;
    }
}
