using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Stone Skin", fileName = "Stone Skin")]
public class StoneSkin : Bonus, IBonus
{
    public int Bonus(BattleData battleData)
    {
        return -battleData.TargetStats.Endurance;
    }
}
