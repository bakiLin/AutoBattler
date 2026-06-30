public class UpdatePlayerInMenuMessage : EventMessage
{
    public Stats Stats { get; private set; }

    public UpdatePlayerInMenuMessage(Stats stats)
    {
        Stats = stats;
    }
}
