using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Poison", fileName = "Poison")]
public class Poison : Bonus, IBonus
{
    public int Bonus(BattleData battleData)
    {
        return battleData.Turn - 1;
    }
}
