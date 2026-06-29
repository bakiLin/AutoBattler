using UnityEngine;

[CreateAssetMenu(menuName = "SO/Class", fileName = "ClassSO")]
public class ClassSO : ScriptableObject
{
    [field: SerializeField] public ClassData Data { get; private set; }
}
