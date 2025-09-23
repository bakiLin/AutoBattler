public interface ICharacter
{
    public Stats Stats { get; }

    public int ActivateBonus(BattleData battleData, BonusType requiredBonusType);
}
