using UnityEngine;

[CreateAssetMenu(menuName = "SO/Bonus/Rage", fileName = "Rage")]
public class Rage : Bonus, IBonus
{
    public int Bonus(BattleData battleData)
    {
        if (battleData.Turn > 0 && battleData.Turn < 4)
            return 2;
        return -1;
    }
}
