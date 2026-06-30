public interface ICharacter
{
    Stats Stats { get; }

    int UseBonus(TurnData battleData, BonusType requiredBonusType);
}
