using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Shield", fileName = "Shield")]
public class Shield : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        if (target.Stats.Strength > attacker.Stats.Strength)
            return -3;
        return 0;
    }
}
