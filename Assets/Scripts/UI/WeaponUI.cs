using Cysharp.Threading.Tasks;
using MessagePipe;
using TMPro;
using UnityEngine;
using VContainer;

public class WeaponUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _name;
    [SerializeField] private TextMeshProUGUI _type;
    [SerializeField] private TextMeshProUGUI _damage;

    [Inject]
    private void Construct(ISubscriber<BattleVictoryMessage> battleVictory)
    {
        DisposableBag.Create(
            battleVictory.Subscribe(x => SetWeaponUI(x.Weapon))
        ).AddTo(destroyCancellationToken);
    }

    private void SetWeaponUI(Weapon weapon)
    {
        _name.text = weapon.Id;
        _type.text = weapon.Type.ToString();
        _damage.text = weapon.Damage.ToString();
    }
}
