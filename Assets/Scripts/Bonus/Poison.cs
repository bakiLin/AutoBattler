using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Poison", fileName = "Poison")]
public class Poison : BonusBase, IBonus
{
    public override int Bonus(BattleData battleData)
    {
        return battleData.Turn - 1;
    }
}
