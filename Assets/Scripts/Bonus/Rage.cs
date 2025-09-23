using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Rage", fileName = "Rage")]
public class Rage : BonusBase, IBonus
{
    public override int Bonus(BattleData battleData)
    {
        if (battleData.Turn > 0 && battleData.Turn < 4)
            return 2;
        return -1;
    }
}
