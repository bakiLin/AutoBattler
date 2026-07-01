public class UpdateUIInBattleMessage : EventMessage
{
    public int PlayerHealth { get; private set; }
    public int EnemyHealth { get; private set; }

    public UpdateUIInBattleMessage(int playerHealth, int enemyHealth)
    {
        PlayerHealth = playerHealth;
        EnemyHealth = enemyHealth;
    }
}