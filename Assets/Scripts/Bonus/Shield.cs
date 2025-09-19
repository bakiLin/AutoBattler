using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Shield", fileName = "Shield")]
public class Shield : BonusBase, IBonus
{
    public override int Bonus(BattleData battleData)
    {
        if (battleData.TargetStats.Strength > battleData.AttackStats.Strength)
            return -3;
        return 0;
    }
}
