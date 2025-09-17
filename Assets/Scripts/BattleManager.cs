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

    private EnemySO _enemy;

    private Random _random = new Random();

    public Action OnStartBattle;

    public Action<WeaponSO> OnEndBattle;

    public Action<EnemySO> OnUpdateEnemyUI; 

    public void StartBattle()
    {
        if (_player.IsReadyToBattle(_requiredLevel))
        {
            _requiredLevel++;

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
        int counter = 0;
        yield return new WaitForSeconds(_statusChangeTime);

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
            OnEndBattle?.Invoke(_enemy.Reward);
        }
        else _status.text = "Game Over";
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
                _enemy.Health -= _player.Weapon.Damage + _player.Stats.Strength;
                OnUpdateEnemyUI.Invoke(_enemy);
                if (IsDead(_enemy.Health, "Player won")) return false;
            }
        }
        else
        {
            if (IsAttackSuccessful(_enemy.Stats.Dexterity, _player.Stats.Dexterity))
            {
                _player.Health -= _enemy.Damage + _enemy.Stats.Strength;
                if (IsDead(_player.Health, $"{_enemy.name} won")) return false;
            }
        }
        return true;
    }

    private bool IsDead(int health, string status)
    {
        if (health == 0)
        {
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
