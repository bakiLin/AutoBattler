using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Dragon", fileName = "Dragon")]
public class Dragon : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        if (turn % 3 == 0)
            return 3;
        return 0;
    }
}
