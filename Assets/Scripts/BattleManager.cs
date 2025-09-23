using System;
using System.Collections;
using TMPro;
using UnityEngine;
using Random = System.Random;

public class BattleManager : MonoBehaviour
{
    [SerializeField]
    private PlayerSO _player;

    [SerializeField]
    private ScriptableObjectHolder _scriptableObjectHolder;

    [SerializeField]
    private TextMeshProUGUI _status;

    [SerializeField]
    private float _statusChangeTime, _startDelay;

    private bool _isPlayerTurn;

    private int _requiredLevel = 1;

    private int _turnCount, _playerTurnCount, _enemyTurnCount, _winCount;

    private EnemySO _enemy;

    private Random _random = new Random();

    private AudioManager _audioManager;

    public Action OnStartBattle, OnGameOver;

    public Action<WeaponSO> OnEndBattle;

    public Action<EnemySO> OnUpdateEnemyUI;

    private void Start()
    {
        _audioManager = AudioManager.Instance;
    }

    public void StartBattle()
    {
        if (_player.IsReadyToBattle(_requiredLevel))
        {
            if (_requiredLevel < 3) _requiredLevel++;

            GenerateEnemy();
            _enemy.Health += _enemy.Stats.Endurance;
            _player.Health += _player.Stats.Endurance;

            OnStartBattle?.Invoke();
            OnUpdateEnemyUI?.Invoke(_enemy);
            
            StartCoroutine(BattleCoroutine());
        }
    }

    private void GenerateEnemy()
    {
        if (_enemy != null)
        {
            EnemySO enemy;
            do enemy = new EnemySO(_scriptableObjectHolder.GetRandom());
            while (enemy.name == _enemy.name);
            _enemy = enemy;
        }
        else
            _enemy = new EnemySO(_scriptableObjectHolder.GetRandom());
    }

    private IEnumerator BattleCoroutine()
    {
        _turnCount = 0;
        _status.text = "Battle";
        yield return new WaitForSeconds(_startDelay);

        while (true)
        {
            if (_turnCount == 0) IsPlayerFirst();
            else if (_turnCount == 20)
            {
                _status.text = "We'll call it a draw";
                GameOver();
                yield break;
            }
            _turnCount++;

            if (_isPlayerTurn) _status.text = "Player turn";
            else _status.text = $"{_enemy.name} turn";

            yield return new WaitForSeconds(_statusChangeTime);
            _status.text = $"Calculating attack chance";
            yield return new WaitForSeconds(_statusChangeTime);

            if (!Attack()) break;

            _isPlayerTurn = !_isPlayerTurn;
            yield return new WaitForSeconds(_statusChangeTime);
        }

        yield return new WaitForSeconds(_statusChangeTime);

        if (_player.Health > 0)
        {
            _winCount++;

            if (_winCount == 5)
            {
                _status.text = $"Victory";
                yield return new WaitForSeconds(_statusChangeTime);
                GameOver();
                yield break;
            }

            _player.RestoreHealth();
            _player.CopyPlayerData();
            OnEndBattle?.Invoke(_enemy.Reward);
        }
        else
        {
            _status.text = "Game Over";
            yield return new WaitForSeconds(_statusChangeTime);
            GameOver();
        }
    }

    private void GameOver()
    {
        _player.ResetCharacter();
        OnGameOver?.Invoke();
    }

    private bool IsPlayerFirst()
    {
        if (_player.Stats.Dexterity >= _enemy.Stats.Dexterity)
            _isPlayerTurn = true;
        return _isPlayerTurn;
    }

    private bool Attack()
    {
        if (_isPlayerTurn)
        {
            if (IsAttackSuccessful(_player.Stats.Dexterity, _enemy.Stats.Dexterity))
            {
                _playerTurnCount++;
                _enemy.Health -= CalculateDamage(_player, _enemy, _player.Weapon.Damage, _playerTurnCount, _player.Weapon.Type);
                OnUpdateEnemyUI.Invoke(_enemy);
                if (IsDead(_enemy.Health, "Player won")) return false;
            }
        }
        else
        {
            if (IsAttackSuccessful(_enemy.Stats.Dexterity, _player.Stats.Dexterity))
            {
                _enemyTurnCount++;
                _player.Health -= CalculateDamage(_enemy, _player, _enemy.Damage, _enemyTurnCount, WeaponType.None);
                if (IsDead(_player.Health, $"{_enemy.name} won")) return false;
            }
        }
        return true;
    }

    private int CalculateDamage(ICharacter attack, ICharacter target, int weaponDamage, int turnCount, WeaponType weaponType)
    {
        var battleData = new BattleData(attack.Stats, target.Stats, weaponDamage, turnCount, weaponType);
        int damage = weaponDamage + attack.Stats.Strength;
        damage += attack.ActivateBonus(battleData, BonusType.Attack);
        damage += target.ActivateBonus(battleData, BonusType.Defence);
        return damage;
    }

    private bool IsDead(int health, string status)
    {
        if (health == 0)
        {
            _playerTurnCount = 0;
            _enemyTurnCount = 0;

            _status.text = status;
            return true;
        }
        return false;
    }

    private bool IsAttackSuccessful(int attackDexterity, int targetDexterity)
    {
        int dexteritySum = attackDexterity + targetDexterity;
        int chance = _random.Next(1, dexteritySum + 1);

        if (chance <= targetDexterity)
        {
            _audioManager.Play("miss");
            _status.text = "Miss";
            return false;
        }

        _audioManager.Play("hit");
        _status.text = "Success";
        return true;
    }
}
