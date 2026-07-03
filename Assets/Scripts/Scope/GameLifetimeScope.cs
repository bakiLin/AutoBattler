using VContainer;
using VContainer.Unity;

public class GameLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        builder.Register<PlayerManager>(Lifetime.Singleton);
        builder.Register<BattleManager>(Lifetime.Singleton);
    }
}
