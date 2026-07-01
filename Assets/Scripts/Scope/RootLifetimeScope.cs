using MessagePipe;
using VContainer;
using VContainer.Unity;
using UnityEngine;

public class RootLifetimeScope : LifetimeScope
{
    [SerializeField] private GameDatabaseSO _database;

    protected override void Configure(IContainerBuilder builder)
    {
        RegisterMessagePipe(builder);

        builder.Register<PlayerManager>(Lifetime.Singleton);
        builder.Register<BattleManager>(Lifetime.Singleton);

        builder.RegisterInstance(_database);
    }

    private void RegisterMessagePipe(IContainerBuilder builder)
    {
        builder.RegisterMessageBroker<EventMessage>(
            builder.RegisterMessagePipe()
        );
    }
}
