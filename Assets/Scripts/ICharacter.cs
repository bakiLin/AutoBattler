public interface ICharacter
{
    Stats Stats { get; }

    int UseBonus(TurnData data, BonusType type);
}
