using System;

public class BattleVictoryMessage : EventMessage
{
    public int BattleNumber { get; private set; }
    public Weapon Weapon { get; private set; }
    public Action OnClickAction { get; private set; }

    public BattleVictoryMessage(int battleNumber, Weapon weapon, Action onClickAction)
    {
        BattleNumber = battleNumber;
        Weapon = weapon;
        OnClickAction = onClickAction;
    }
}
