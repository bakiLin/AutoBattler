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
    private EnemyHolder _enemyHolder;

    [SerializeField]
    private TextMeshProUGUI _status;

    [SerializeField]
    private float _statusChangeTime;

    private bool _ready, _isPlayerTurn;

    private EnemySO _enemy;

    private Random _random = new Random();

    // Events
    public Action OnStartBattle, OnLevelUpChangeUI;
    public Action<EnemySO> OnChangeEnemyUI; 

    private void OnEnable()
    {
        _player.OnReady += ReadyToBattle;
    }

    private void OnDisable()
    {
        _player.OnReady -= ReadyToBattle;
    }

    private void ReadyToBattle()
    {
        _ready = true;
    }

    public void StartBattle()
    {
        if (_ready)
        {
            _enemy = new EnemySO(_enemyHolder.GetRandom());
            _enemy.Health += _enemy.Endurance;
            _player.Health += _player.Stats.Endurance;

            OnStartBattle?.Invoke();
            OnChangeEnemyUI?.Invoke(_enemy);
            
            StartCoroutine(BattleCoroutine());
        }
    }

    private IEnumerator BattleCoroutine()
    {
        yield return new WaitForSeconds(_statusChangeTime);
        int counter = 0;

        while (true)
        {
            if (counter == 0) IsPlayerFirst();

            if (_isPlayerTurn) _status.text = "Player turn";
            else _status.text = $"{_enemy.name} turn";

            yield return new WaitForSeconds(_statusChangeTime);
            _status.text = $"Calculating attack chance";
            yield return new WaitForSeconds(_statusChangeTime);

            if (!Attack()) break;

            counter++;
            _isPlayerTurn = !_isPlayerTurn;
            yield return new WaitForSeconds(_statusChangeTime);
        }

        yield return new WaitForSeconds(_statusChangeTime);

        if (_player.Health > 0)
        {
            _player.RestoreHealth();
            _player.CopyDictionary();
            OnLevelUpChangeUI?.Invoke();
        }
        else _status.text = "Game Over";
    }

    private bool Attack()
    {
        if (_isPlayerTurn)
        {
            if (IsAttackSuccessful(_player.Stats.Dexterity, _enemy.Dexterity))
            {
                _enemy.Health -= _player.Weapon.Damage + _player.Stats.Strength;
                OnChangeEnemyUI.Invoke(_enemy);

                if (_enemy.Health == 0)
                {
                    _status.text = $"Player won";
                    return false;
                }
            }
        }
        else
        {
            if (IsAttackSuccessful(_enemy.Dexterity, _player.Stats.Dexterity))
            {
                _player.Health -= _enemy.Damage + _enemy.Strength;
                OnStartBattle.Invoke();

                if (_player.Health == 0)
                {
                    _status.text = $"{_enemy.name} won";
                    return false;
                }
            }
        }
        return true;
    }

    private bool IsPlayerFirst()
    {
        if (_player.Stats.Dexterity >= _enemy.Dexterity)
            _isPlayerTurn = true;
        return _isPlayerTurn;
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
