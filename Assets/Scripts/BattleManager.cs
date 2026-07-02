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
    private IPublisher<CharacterMissedMessage> _characterMissed;

    private readonly Random _random = new();
    private int _battleCounter = 1;
    private int _winCount = 0;

    private BattleManager(PlayerManager playerManager, GameDatabaseSO database,
        IAsyncPublisher<StartBattleMessage> startBattle, IPublisher<UpdateUIInBattleMessage> updateUIInBattle,
        IPublisher<BattleVictoryMessage> battleVictory, IPublisher<GameOverMessage> gameOver,
        IPublisher<SetBattleStatusMessage> setBattleStatus, IPublisher<CharacterMissedMessage> characterMissed)
    {
        _playerManager = playerManager;
        _database = database;
        _startBattle = startBattle;
        _updateUIInBattle = updateUIInBattle;
        _battleVictory = battleVictory;
        _gameOver = gameOver;
        _setBattleStatus = setBattleStatus;
        _characterMissed = characterMissed;
    }

    public async UniTask StartBattle(CancellationToken cancellationToken = default)
    {
        if (!_playerManager.IsReadyToBattle(_battleCounter)) return;
        _battleCounter++;

        BattleCharacter player = new BattlePlayer(_playerManager.GetPlayerSnapshot());
        BattleCharacter enemy = new BattleEnemy(_database.GetRandomEnemy());

        _setBattleStatus.Publish(new SetBattleStatusMessage($"{player.Id} vs {enemy.Id}"));
        await _startBattle.PublishAsync(new StartBattleMessage(player, enemy), cancellationToken);
        await UniTask.Delay(_database.TurnDelay, cancellationToken: cancellationToken, cancelImmediately: true);

        BattleCharacter attacker = player.Stats.Dexterity >= enemy.Stats.Dexterity ? player : enemy;
        BattleCharacter target = attacker == player ? enemy : player;

        int playerTurnCount = 0;
        int enemyTurnCount = 0;

        while (player.Health > 0 && enemy.Health > 0)
        {
            int currentTurnCount = (attacker == player) ? ++playerTurnCount : ++enemyTurnCount;

            _setBattleStatus.Publish(new SetBattleStatusMessage($"{attacker.Id}'s turn"));
            await UniTask.Delay(_database.TurnDelay, cancellationToken: cancellationToken, cancelImmediately: true);

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
                _characterMissed.Publish(new CharacterMissedMessage(target == player));
            }

            await UniTask.Delay(_database.TurnDelay, cancellationToken: cancellationToken, cancelImmediately: true);
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
            await UniTask.Delay(_database.TurnDelay, cancellationToken: token, cancelImmediately: true);

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
            await UniTask.Delay(_database.TurnDelay, cancellationToken: token, cancelImmediately: true);
            _gameOver.Publish(new GameOverMessage());
        }
    }
}
