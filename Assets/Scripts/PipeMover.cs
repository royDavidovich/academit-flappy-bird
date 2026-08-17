using UnityEngine;

public class PipeMover : MonoBehaviour
{
    private GameConfig _config;
    private Rigidbody2D _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    public void Init(GameConfig config)
    {
        _config = config;
    }

    private void FixedUpdate()
    {
        if (_config == null)
        {
            return;
        }

        _rb.MovePosition(_rb.position + Vector2.left * _config.MoveSpeed * Time.fixedDeltaTime);

        if (_rb.position.x < _config.DespawnX)
        {
            Destroy(gameObject);
        }
    }
}
