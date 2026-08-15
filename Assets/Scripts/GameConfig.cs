using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [Header("Bird")]
    [SerializeField] float flapImpulse = 6f;
    [SerializeField] float gravityScale = 2.5f;
    [SerializeField] float birdRadius = 0.5f;

    [Header("Pipes")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float despawnX = -10f;

    [Header("Spawning")]
    [SerializeField] float spawnInterval = 1.5f;
    [SerializeField] float spawnX = 9f;
    [SerializeField] float gapYRange = 2f;
    [SerializeField] float gapSize = 2.5f;

    public float FlapImpulse => flapImpulse;
    public float GravityScale => gravityScale;
    public float BirdRadius => birdRadius;
    public float MoveSpeed => moveSpeed;
    public float DespawnX => despawnX;
    public float SpawnInterval => spawnInterval;
    public float SpawnX => spawnX;
    public float GapYRange => gapYRange;
    public float GapSize => gapSize;
}
