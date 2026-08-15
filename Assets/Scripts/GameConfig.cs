using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [Header("Bird")]
    [SerializeField] float flapImpulse = 6f;
    [SerializeField] float gravityScale = 2.5f;
    [SerializeField] float birdRadius = 0.5f;

    [Header("Bird Rotation")]
    [SerializeField] float maxRiseAngle = 25f;
    [SerializeField] float maxDiveAngle = 70f;
    [SerializeField] float diveVelocity = 8f;
    [SerializeField] float rotationSpeed = 8f;

    [Header("Pipes")]
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float despawnX = -10f;
    [SerializeField] float pipeWidth = 2f;
    [SerializeField] float pipeHeight = 20f;
    [SerializeField] float capHeight = 1f;

    [Header("Spawning")]
    [SerializeField] float spawnInterval = 1.5f;
    [SerializeField] float spawnX = 9f;
    [SerializeField] float gapYRange = 2f;
    [SerializeField] float gapSize = 2.5f;

    public float FlapImpulse => flapImpulse;
    public float GravityScale => gravityScale;
    public float BirdRadius => birdRadius;
    public float MaxRiseAngle => maxRiseAngle;
    public float MaxDiveAngle => maxDiveAngle;
    public float DiveVelocity => diveVelocity;
    public float RotationSpeed => rotationSpeed;
    public float MoveSpeed => moveSpeed;
    public float DespawnX => despawnX;
    public float PipeWidth => pipeWidth;
    public float PipeHeight => pipeHeight;
    public float CapHeight => capHeight;
    public float SpawnInterval => spawnInterval;
    public float SpawnX => spawnX;
    public float GapYRange => gapYRange;
    public float GapSize => gapSize;
}
