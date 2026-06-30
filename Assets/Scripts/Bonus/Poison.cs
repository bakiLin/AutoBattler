using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Poison", fileName = "Poison")]
public class Poison : BonusBase
{
    public override int Use(TurnData battleData)
    {
        return battleData.Turn - 1;
    }
}
