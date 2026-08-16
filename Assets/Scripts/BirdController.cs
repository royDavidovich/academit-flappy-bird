using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    [SerializeField] GameConfig config;

    public event Action OnFlap;

    Rigidbody2D _rb;
    CircleCollider2D _collider;
    InputAction _flapAction;
    float _tiltAngle;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();
        _collider = GetComponent<CircleCollider2D>();

        // Sprite's native diameter is 1 world unit, so scaling by radius*2 keeps the
        // visual bird sized as tuned. The hitbox is intentionally smaller than that:
        // the toucan's opaque pixels don't fill the full square frame, so a circle
        // matching the frame size would register hits before the drawn bird touches
        // anything. HitboxScale is measured from the actual sprite content.
        _collider.radius = config.BirdRadius * config.HitboxScale;
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

    void LateUpdate()
    {
        if (GameManager.Instance.CurrentState != GameState.Playing) return;

        // Purely cosmetic: a CircleCollider2D is rotation-invariant, so tilting the
        // bird cannot change what it collides with.
        float t = Mathf.InverseLerp(-config.DiveVelocity, config.FlapImpulse, _rb.linearVelocity.y);
        float targetAngle = Mathf.Lerp(-config.MaxDiveAngle, config.MaxRiseAngle, t);

        // The rise target is only momentarily at its peak right after a flap impulse,
        // then gravity pulls it back down immediately, while the dive target saturates
        // and holds. A single shared speed lets the dive fully catch up but never the
        // rise, so rising and falling are blended at different rates on purpose.
        float speed = targetAngle > _tiltAngle ? config.RiseRotationSpeed : config.DiveRotationSpeed;
        _tiltAngle = Mathf.Lerp(_tiltAngle, targetAngle, speed * Time.deltaTime);
        transform.rotation = Quaternion.Euler(0f, 0f, _tiltAngle);
    }

    void Flap()
    {
        _rb.linearVelocity = Vector2.up * config.FlapImpulse;
        OnFlap?.Invoke();
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
