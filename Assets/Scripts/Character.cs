using UnityEngine;

public abstract class Character : MonoBehaviour
{
    [field: SerializeField] public float speed { get; protected set; } = 4f;
    public Vector3 velocity {  get; protected set; }
}
