public class StartBattleMessage : EventMessage
{
    public BattleCharacter Player { get; }
    public BattleCharacter Enemy { get; }

    public StartBattleMessage(BattleCharacter player, BattleCharacter enemy)
    {
        Player = player;
        Enemy = enemy;
    }
}