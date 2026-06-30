using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Slime", fileName = "Slime")]
public class Slime : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.WeaponType == WeaponType.Slashing)
            return -battleData.WeaponDamage;
        return 0;
    }
}
