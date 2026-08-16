using UnityEngine;

public class GameConfig : MonoBehaviour
{
    [Header("Bird")]
    [SerializeField] float flapImpulse = 6f;
    [SerializeField] float gravityScale = 2.5f;
    [SerializeField] float birdRadius = 0.5f;
    // Toucan_0's opaque pixels only reach 5px above/below the pivot out of an 8px
    // half-frame (measured from the source sprite), so the hitbox must be smaller
    // than the visual sprite or it registers hits past the drawn bird.
    [SerializeField] float hitboxScale = 0.625f;

    [Header("Bird Rotation")]
    [SerializeField] float maxRiseAngle = 25f;
    [SerializeField] float maxDiveAngle = 25f;
    [SerializeField] float diveVelocity = 5f;
    [SerializeField] float riseRotationSpeed = 12f;
    [SerializeField] float diveRotationSpeed = 4f;

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
    public float HitboxScale => hitboxScale;
    public float MaxRiseAngle => maxRiseAngle;
    public float MaxDiveAngle => maxDiveAngle;
    public float DiveVelocity => diveVelocity;
    public float RiseRotationSpeed => riseRotationSpeed;
    public float DiveRotationSpeed => diveRotationSpeed;
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
