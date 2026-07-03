using Cysharp.Threading.Tasks;
using MessagePipe;
using UnityEngine;
using VContainer;

public class BattleSoundPlayer : MonoBehaviour
{
    private ISoundManager _soundManager;
    private GameDatabaseSO _database;

    [Inject]
    private void Construct(
        ISubscriber<UpdateUIInBattleMessage> updateUIInBattle,
        ISubscriber<CharacterMissedMessage> characterMissed,
        ISoundManager soundManager,
        GameDatabaseSO database)
    {
        _soundManager = soundManager;
        _database = database;

        DisposableBag.Create(
            updateUIInBattle.Subscribe(_ => PlayAttackSound()),
            characterMissed.Subscribe(_ => PlayMissSound())
        ).AddTo(destroyCancellationToken);
    }

    private void PlayAttackSound()
    {
        _soundManager.Get().Play(_database.AttackSound, destroyCancellationToken);
    }

    private void PlayMissSound()
    {
        _soundManager.Get().Play(_database.MissSound, destroyCancellationToken);
    }
}