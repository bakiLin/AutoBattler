using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Slime", fileName = "Slime")]
public class Slime : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.Weapon.Type == WeaponType.Slashing)
            return -battleData.Weapon.Damage;
        return 0;
    }
}
