public class BattleEnemy : BattleCharacter
{
    public Weapon Reward { get; private set; }

    public BattleEnemy(Enemy enemy)
    {
        Id = enemy.Id;
        Health = enemy.Health + enemy.Stats.Endurance;
        Stats = enemy.Stats;
        Weapon = new Weapon(enemy.Damage);
        Bonuses = enemy.Bonus is { } bonus ? new() { bonus } : new();
        Reward = enemy.Reward.Weapon;
    }
}
