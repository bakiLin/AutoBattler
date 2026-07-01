public class SetBattleStatusMessage : EventMessage
{
    public string Status { get; private set; }

    public SetBattleStatusMessage(string status)
    {
        Status = status;
    }
}