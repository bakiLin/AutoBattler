using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Dragon", fileName = "Dragon")]
public class Dragon : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.Turn % 3 == 0)
            return 3;
        return 0;
    }
}
