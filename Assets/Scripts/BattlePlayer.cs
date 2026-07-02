public class BattlePlayer : BattleCharacter
{
    public BattlePlayer(PlayerData data)
    {
        Id = "Player";
        Stats = data.Stats;
        Weapon = data.Weapon;
        Health = data.Health;
        Bonuses = data.Bonuses;
    }
}
