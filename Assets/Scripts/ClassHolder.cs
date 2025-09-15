using UnityEngine;

public class ClassHolder : MonoBehaviour
{
    [SerializeField]
    private ClassSO _thief, _warrior, _barbarian;

    public ClassSO Thief { get => _thief; }

    public ClassSO Warrior { get => _warrior; }

    public ClassSO Barbarian { get => _barbarian; }
}
