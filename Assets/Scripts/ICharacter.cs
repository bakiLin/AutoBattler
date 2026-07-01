public interface ICharacter
{
    Stats Stats { get; }

    int CalculateBonusDamage(TurnData data, BonusType type);
}
