using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Rage", fileName = "Rage")]
public class Rage : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.Turn > 0 && battleData.Turn < 4)
            return 2;
        return -1;
    }
}
