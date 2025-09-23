using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Stone Skin", fileName = "Stone Skin")]
public class StoneSkin : BonusBase, IBonus
{
    public override int Bonus(BattleData battleData)
    {
        return -battleData.TargetStats.Endurance;
    }
}
