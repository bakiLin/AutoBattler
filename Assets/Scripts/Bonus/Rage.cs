using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Rage", fileName = "Rage")]
public class Rage : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        if (turn > 0 && turn < 4)
            return 2;
        return -1;
    }
}
