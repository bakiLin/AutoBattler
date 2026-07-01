public struct TurnData
{
    public Stats Attacker { get; private set; }
    public Stats Target { get; private set; }
    public Weapon Weapon { get; private set; }
    public int Turn { get; private set; }

    public TurnData(Stats attacker, Stats target, Weapon weapon, int turn)
    {
        Attacker = attacker;
        Target = target;
        Weapon = weapon;
        Turn = turn;
    }
}
