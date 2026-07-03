public class UpdatePlayerInMenuMessage : EventMessage
{
    public PlayerData PlayerData { get; private set; }

    public UpdatePlayerInMenuMessage(PlayerData playerData)
    {
        PlayerData = playerData;
    }
}
