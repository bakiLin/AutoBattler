using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Hidden Attack", fileName = "Hidden Attack")]
public class HiddenAttack : BonusBase, IBonus
{
    public override int Bonus(BattleData battleData)
    {
        if (battleData.AttackStats.Dexterity > battleData.TargetStats.Dexterity)
            return 1;
        return 0;
    }
}
