using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Stone Skin", fileName = "Stone Skin")]
public class StoneSkin : BonusBase
{
    public override int Use(TurnData battleData)
    {
        return -battleData.Target.Endurance;
    }
}
