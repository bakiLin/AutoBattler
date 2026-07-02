public class UpdateUIInBattleMessage : EventMessage
{
    public int PlayerCurrentHealth { get; }
    public int EnemyCurrentHealth { get; }

    public UpdateUIInBattleMessage(int playerCurrentHealth, int enemyCurrentHealth)
    {
        PlayerCurrentHealth = playerCurrentHealth;
        EnemyCurrentHealth = enemyCurrentHealth;
    }
}