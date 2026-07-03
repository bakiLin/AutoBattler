using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Poison", fileName = "Poison")]
public class Poison : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        return turn - 1;
    }
}
