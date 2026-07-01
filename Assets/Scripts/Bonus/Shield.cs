using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Shield", fileName = "Shield")]
public class Shield : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.Target.Strength > battleData.Attacker.Strength)
            return -3;
        return 0;
    }
}
