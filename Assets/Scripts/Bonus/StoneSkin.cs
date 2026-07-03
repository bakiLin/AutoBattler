using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Stone Skin", fileName = "Stone Skin")]
public class StoneSkin : BonusBase
{
    public override int Use(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        return -target.Stats.Endurance;
    }
}
