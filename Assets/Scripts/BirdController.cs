using UnityEngine;
using UnityEngine.InputSystem;

public class BirdController : MonoBehaviour
{
    [SerializeField] float flapImpulse = 5f;
    [SerializeField] float gravityScale = 2.5f;

    Rigidbody2D _rb;
    InputAction _flapAction;

    void Awake()
    {
        _rb = GetComponent<Rigidbody2D>();

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
        _rb.linearVelocity = Vector2.up * flapImpulse;
    }

    void HandleStateChanged(GameState state)
    {
        switch (state)
        {
            case GameState.Playing:
                _rb.gravityScale = gravityScale;
                break;
            case GameState.GameOver:
                _rb.linearVelocity = Vector2.zero;
                _rb.gravityScale = 0f;
                break;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameManager.Instance.GameOver();
    }
}
