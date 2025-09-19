public class BattleData
{
    public Stats AttackStats, TargetStats;

    public int WeaponDamage, Turn;

    public WeaponType WeaponType;

    public BattleData(Stats attackStats, Stats targetStats, int weaponDamage, int turn, WeaponType weaponType)
    {
        AttackStats = attackStats;
        TargetStats = targetStats;
        WeaponDamage = weaponDamage;
        Turn = turn;
        WeaponType = weaponType;
    }
}
