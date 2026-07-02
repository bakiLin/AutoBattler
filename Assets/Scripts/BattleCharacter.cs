using System.Collections.Generic;
using UnityEngine;

public abstract class BattleCharacter
{
    public string Id { get; protected set; }
    public int Health { get; protected set; }
    public Stats Stats { get; protected set; }
    public Weapon Weapon { get; protected set; }
    public List<BonusBase> Bonuses { get; protected set; }

    public void TakeDamage(int damage)
    {
        Health = Mathf.Max(0, Health - damage);
    }

    public int CalculateBonusDamage(BattleCharacter attacker, BattleCharacter target,
        int turn, BonusType type)
    {
        int totalBonus = 0;
        for (int i = 0; i < Bonuses.Count; i++)
            if (Bonuses[i].Type == type)
                totalBonus += Bonuses[i].Use(attacker, target, turn);
        return totalBonus;
    }
}
