using Cysharp.Threading.Tasks;
using MessagePipe;
using Random = System.Random;

public class BattleManager
{
    private PlayerManager _playerManager;
    private GameDatabaseSO _database;
    private Random _random = new Random();
    private int _requiredLevel = 1;
    private int _winCount = 0;
    private IPublisher<StartBattleMessage> _startBattle;
    private IPublisher<UpdateUIInBattleMessage> _updateUIInBattle;

    private BattleManager(PlayerManager playerManager, GameDatabaseSO database,
        IPublisher<StartBattleMessage> startBattle, IPublisher<UpdateUIInBattleMessage> updateUIInBattle)
    {
        _playerManager = playerManager;
        _database = database;
        _startBattle = startBattle;
        _updateUIInBattle = updateUIInBattle;
    }

    public async UniTask StartBattle()
    {
        if (_playerManager.IsReadyToBattle(_requiredLevel))
        {
            var enemy = _database.GetRandomEnemy();
            enemy.SetMaxHealth();
            var player = _playerManager.GetPlayer();
            _startBattle.Publish(new StartBattleMessage());

            int playerTurnCount = 0;
            int enemyTurnCount = 0;
            int totalTurnCount = 0;
            string status = "Battle";
            bool playerTurn = player.Stats.Dexterity >= enemy.Stats.Dexterity;

            // delay before start

            while (true)
            {
                if (playerTurn) status = "Player turn";
                else status = $"{enemy.Id} turn";

                // delay before attack

                if (playerTurn)
                {
                    if (IsAttackSuccessful(player.Stats.Dexterity, enemy.Stats.Dexterity))
                    {
                        playerTurnCount++;
                        int damage = CalculateDamage(player, enemy, player.Weapon, playerTurnCount);
                        enemy.SetHealth(enemy.Health - damage);

                        _updateUIInBattle.Publish(new UpdateUIInBattleMessage(
                            player.Health, enemy.Health));

                        if (enemy.Health == 0)
                        {
                            status = "Player won";
                            break;
                        }
                    }
                }
                else
                {
                    if (IsAttackSuccessful(enemy.Stats.Dexterity, player.Stats.Dexterity))
                    {
                        enemyTurnCount++;
                        int damage = CalculateDamage(enemy, player, new Weapon(enemy.Damage), enemyTurnCount);
                        player.SetHealth(player.Health - damage);

                        _updateUIInBattle.Publish(new UpdateUIInBattleMessage(
                            player.Health, enemy.Health));

                        if (player.Health == 0)
                        {
                            status = $"{enemy.Id} won";
                            break;
                        }
                    }
                }

                playerTurn = !playerTurn;
                // delay
            }
            // delay

            if (player.Health > 0)
            {
                _winCount++;

                if (_winCount == 5)
                {
                    status = "Victory";
                    // delay
                    // game over
                }

                // copy player data to save state
                // end battle
            }
            else
            {
                status = "Game Over";
                // delay
                // game over
            }
        }
    }

    private bool IsAttackSuccessful(int attackerDex, int targetDex)
    {
        int chance = _random.Next(1, attackerDex + targetDex + 1);

        if (chance <= targetDex)
        {
            //_audioManager.Play("miss");
            //_status.text = "Miss";
            return false;
        }

        //_audioManager.Play("hit");
        //_status.text = "Success";
        return true;
    }

    private int CalculateDamage(ICharacter attacker, ICharacter target, Weapon weapon, int turn)
    {
        var battleData = new TurnData(attacker.Stats, target.Stats, weapon, turn);
        int damage = weapon.Damage + attacker.Stats.Strength;
        damage += attacker.CalculateBonusDamage(battleData, BonusType.Attack);
        damage += target.CalculateBonusDamage(battleData, BonusType.Defence);
        return damage;
    }
}
