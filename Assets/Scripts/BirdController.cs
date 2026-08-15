using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    [SerializeField] GameConfig config;

    Rigidbody2D _rb;
    CircleCollider2D _collider;
    InputAction _flapAction;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();

        // Sprite's native diameter is 1 world unit, so scaling by radius*2 keeps the
        // visual exactly equal to the hitbox. Decoupling these makes hits look unfair.
        _collider.radius = config.BirdRadius;
        transform.localScale = Vector3.one * (config.BirdRadius * 2f);

        _flapAction = new InputAction(type: InputActionType.Button);
        _flapAction.AddBinding("<Keyboard>/space");
        _flapAction.AddBinding("<Mouse>/leftButton");
        _flapAction.AddBinding("<Touchscreen>/primaryTouch/press");
    }

    void OnEnable()
    {
        _flapAction.Enable();
    }

    void OnDisable()
    {
        _flapAction.Disable();
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }
    }

    void Start()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    void OnDestroy()
    {
        _flapAction.Dispose();
    }

    void Update()
    {
        if (GameManager.Instance.CurrentState == GameState.GameOver) return;

        if (_flapAction.WasPressedThisFrame())
        {
            if (GameManager.Instance.CurrentState == GameState.Ready)
            {
                GameManager.Instance.StartGame();
            }

            Flap();
        }
    }

    void Flap()
    {
        _rb.linearVelocity = Vector2.up * config.FlapImpulse;
    }

    void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                _rb.gravityScale = config.GravityScale;
                break;
            case GameState.GameOver:
                _rb.linearVelocity = Vector2.zero;
                _rb.gravityScale = 0f;
                _rb.simulated = false;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.GameOver();
    }
}
