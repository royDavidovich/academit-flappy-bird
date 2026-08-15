using UnityEngine;

public class PipeMover : MonoBehaviour
{
    [SerializeField] float moveSpeed = 3f;
    [SerializeField] float despawnX = -10f;

    Rigidbody2D _rb;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        _rb.MovePosition(_rb.position + Vector2.left * moveSpeed * Time.fixedDeltaTime);

        if (_rb.position.x < despawnX)
        {
            Destroy(gameObject);
        }
    }
}
