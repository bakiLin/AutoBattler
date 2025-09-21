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
    private float _statusChangeTime;

    private bool _isPlayerTurn;

    private int _requiredLevel = 1;

    private int _turnCount, _playerTurnCount, _enemyTurnCount, _winCount;

    private EnemySO _enemy;

    private Random _random = new Random();

    public Action OnStartBattle, OnGameOver;

    public Action<WeaponSO> OnEndBattle;

    public Action<EnemySO> OnUpdateEnemyUI; 

    public void StartBattle()
    {
        if (_player.IsReadyToBattle(_requiredLevel))
        {
            if (_requiredLevel < 3) _requiredLevel++;

            _enemy = new EnemySO(_scriptableObjectHolder.GetRandom());
            _enemy.Health += _enemy.Stats.Endurance;
            _player.Health += _player.Stats.Endurance;

            OnStartBattle?.Invoke();
            OnUpdateEnemyUI?.Invoke(_enemy);
            
            StartCoroutine(BattleCoroutine());
        }
    }

    private IEnumerator BattleCoroutine()
    {
        _status.text = "";
        yield return new WaitForSeconds(_statusChangeTime);

        while (true)
        {
            if (_turnCount == 0) IsPlayerFirst();
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
                _player.ResetCharacter();
                OnGameOver?.Invoke();
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
            _player.ResetCharacter();
            OnGameOver?.Invoke();
        }
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
                var battleData = new BattleData(_player.Stats, _enemy.Stats, _player.Weapon.Damage, _playerTurnCount, _player.Weapon.Type);
                int damage = _player.Weapon.Damage + _player.Stats.Strength;
                damage += _player.ActivateBonus(battleData, BonusType.Attack);
                damage += _enemy.ActivateBonus(battleData, BonusType.Defence);
                _enemy.Health -= damage;

                OnUpdateEnemyUI.Invoke(_enemy);
                if (IsDead(_enemy.Health, "Player won")) return false;
            }
        }
        else
        {
            if (IsAttackSuccessful(_enemy.Stats.Dexterity, _player.Stats.Dexterity))
            {
                _enemyTurnCount++;
                var battleData = new BattleData(_enemy.Stats, _player.Stats, _enemy.Damage, _enemyTurnCount, WeaponType.None);
                int damage = _enemy.Damage + _enemy.Stats.Strength;
                damage += _enemy.ActivateBonus(battleData, BonusType.Attack);
                damage += _player.ActivateBonus(battleData, BonusType.Defence);
                _player.Health -= damage;

                if (IsDead(_player.Health, $"{_enemy.name} won")) return false;
            }
        }
        return true;
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
            _status.text = "Miss";
            return false;
        }

        _status.text = "Success";
        return true;
    }
}
