using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Hidden Attack", fileName = "Hidden Attack")]
public class HiddenAttack : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        if (attacker.Stats.Dexterity > target.Stats.Dexterity)
            return 1;
        return 0;
    }
}
