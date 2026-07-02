using System;

public class BattleVictoryMessage : EventMessage
{
    public Weapon Weapon { get; private set; }
    public Action OnClickAction { get; private set; }

    public BattleVictoryMessage(Weapon weapon, Action onClickAction)
    {
        Weapon = weapon;
        OnClickAction = onClickAction;
    }
}
