using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Hidden Attack", fileName = "Hidden Attack")]
public class HiddenAttack : BonusBase
{
    public override int Use(TurnData battleData)
    {
        if (battleData.Attacker.Dexterity > battleData.Target.Dexterity)
            return 1;
        return 0;
    }
}
