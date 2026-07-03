using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Action Rush", fileName = "Action Rush")]
public class ActionRush : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        if (turn == 1)
            return attacker.Weapon.Damage;
        return 0;
    }
}
