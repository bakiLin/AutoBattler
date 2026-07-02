public class CharacterMissedMessage : EventMessage
{
    public bool IsPlayerTarget { get; }

    public CharacterMissedMessage(bool isPlayerTarget)
    {
        IsPlayerTarget = isPlayerTarget;
    }
}
