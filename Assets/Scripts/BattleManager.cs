using Cysharp.Threading.Tasks;
using MessagePipe;
using System;
using System.Threading;
using Random = System.Random;

public class BattleManager
{
    private PlayerManager _playerManager;
    private GameDatabaseSO _database;
    private IAsyncPublisher<StartBattleMessage> _startBattle;
    private IPublisher<UpdateUIInBattleMessage> _updateUIInBattle;
    private IPublisher<BattleVictoryMessage> _battleVictory;
    private IPublisher<GameOverMessage> _gameOver;
    private IPublisher<SetBattleStatusMessage> _setBattleStatus;

    private readonly Random _random = new();
    private int _battleCounter = 1;
    private int _winCount = 0;
    private const int TurnDelay = 200; // milliseconds

    private BattleManager(PlayerManager playerManager, GameDatabaseSO database,
        IAsyncPublisher<StartBattleMessage> startBattle, IPublisher<UpdateUIInBattleMessage> updateUIInBattle,
        IPublisher<BattleVictoryMessage> battleVictory, IPublisher<GameOverMessage> gameOver,
        IPublisher<SetBattleStatusMessage> setBattleStatus)
    {
        _playerManager = playerManager;
        _database = database;
        _startBattle = startBattle;
        _updateUIInBattle = updateUIInBattle;
        _battleVictory = battleVictory;
        _gameOver = gameOver;
        _setBattleStatus = setBattleStatus;
    }

    public async UniTask StartBattle(CancellationToken cancellationToken = default)
    {
        if (!_playerManager.IsReadyToBattle(_battleCounter)) return;
        _battleCounter++;

        BattleCharacter player = new BattlePlayer(_playerManager.GetPlayerSnapshot());
        BattleCharacter enemy = new BattleEnemy(_database.GetRandomEnemy());

        _setBattleStatus.Publish(new SetBattleStatusMessage($"{player.Id} vs {enemy.Id}"));
        await _startBattle.PublishAsync(new StartBattleMessage(player, enemy));
        await UniTask.Delay(TurnDelay, cancellationToken: cancellationToken);

        BattleCharacter attacker = player.Stats.Dexterity >= enemy.Stats.Dexterity ? player : enemy;
        BattleCharacter target = attacker == player ? enemy : player;

        int playerTurnCount = 0;
        int enemyTurnCount = 0;

        while (player.Health > 0 && enemy.Health > 0)
        {
            int currentTurnCount = (attacker == player) ? ++playerTurnCount : ++enemyTurnCount;

            _setBattleStatus.Publish(new SetBattleStatusMessage($"{attacker.Id}'s turn"));
            await UniTask.Delay(TurnDelay, cancellationToken: cancellationToken);

            if (IsAttackSuccessful(attacker.Stats.Dexterity, target.Stats.Dexterity))
            {
                int damage = CalculateDamage(attacker, target, currentTurnCount);
                target.TakeDamage(damage);
                _updateUIInBattle.Publish(new UpdateUIInBattleMessage(player.Health, enemy.Health));
                _setBattleStatus.Publish(new SetBattleStatusMessage($"Success: {damage} dmg"));
            }
            else
            {
                _setBattleStatus.Publish(new SetBattleStatusMessage($"Miss"));
            }

            await UniTask.Delay(TurnDelay, cancellationToken: cancellationToken);
            (attacker, target) = (target, attacker);
        }

        await HandleBattleResult(player as BattlePlayer, enemy as BattleEnemy, cancellationToken);
    }

    private bool IsAttackSuccessful(int attackerDex, int targetDex)
    {
        return _random.Next(1, attackerDex + targetDex + 1) > targetDex;
    }

    private int CalculateDamage(BattleCharacter attacker, BattleCharacter target, int turn)
    {
        int damage = attacker.Weapon.Damage + attacker.Stats.Strength;
        damage += attacker.CalculateBonusDamage(attacker, target, turn, BonusType.Attack);
        damage += target.CalculateBonusDamage(attacker, target, turn, BonusType.Defence);
        return Math.Max(1, damage);
    }

    private async UniTask HandleBattleResult(BattlePlayer player, BattleEnemy enemy, CancellationToken token)
    {
        if (player.Health > 0)
        {
            _setBattleStatus.Publish(new SetBattleStatusMessage($"Victory"));
            await UniTask.Delay(TurnDelay, cancellationToken: token);

            _winCount++;
            if (_winCount == 5)
            {
                _gameOver.Publish(new GameOverMessage());
            }
            else
            {
                _playerManager.SaveSnapshot();
                _battleVictory.Publish(new BattleVictoryMessage(_battleCounter, enemy.Reward, () => {
                    _playerManager.EquipWeapon(enemy.Reward);
                }));
            }
        }
        else
        {
            _setBattleStatus.Publish(new SetBattleStatusMessage($"Game Over"));
            await UniTask.Delay(TurnDelay, cancellationToken: token);
            _gameOver.Publish(new GameOverMessage());
        }
    }
}
