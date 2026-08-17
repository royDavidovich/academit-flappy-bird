# C# Coding Conventions

Conventions for all C# code in this Unity project. Keep new and substantially-changed code consistent with this document; don't refactor unrelated code just to conform.

## General principles

- Prefer meaningful, descriptive English names. Clarity over brevity.
- Avoid unexplained abbreviations. `i`, `j`, etc. are fine only as simple loop counters.
- Match existing project architecture and patterns when they don't conflict with this document.
- When editing an existing file, keep surrounding code internally consistent while applying these conventions to new/changed code.

## Naming

- `PascalCase` for classes, structs, interfaces, enums, delegates, methods, properties, events, local functions.
- Prefix interfaces with `I` (e.g. `IDamageable`).
- `camelCase` for local variables and parameters.
- `_camelCase` for private instance fields.
- `s_camelCase` for private static fields.
- `PascalCase` for constants.
- Nouns for values/objects, verbs or verb phrases for methods.
- Name booleans as questions/states: `isAlive`, `hasTarget`, `canJump`, `shouldRespawn`.
- No Hungarian prefixes (`m_`, `r_`, `sr_`, `v_`, `i_`, `o_`, `io_`).
- Don't prefix enum types with `e`.
- A file with one primary type is named after that type.

```csharp
// GOOD
private int _remainingLives;
private static int s_activeInstanceCount;
public bool IsAlive => _remainingLives > 0;

// BAD
private int m_remainingLives;
private bool eGameState;
```

## Unity-specific naming and serialization

- Use Unity message names exactly as defined (`Awake`, `Start`, `Update`, `FixedUpdate`, `LateUpdate`, `OnEnable`, `OnDisable`, `OnDestroy`, `OnTriggerEnter`, etc.), `PascalCase`, even when private.
- Don't make a field `public` just to expose it in the Inspector.
- Prefer `[SerializeField] private` for Inspector-configured fields; expose read access via a property if external code needs it.
- `static`, `const`, and `readonly` fields are not normally serialized by Unity.
- Treat serialized field names as persisted data (scenes/prefabs reference them by name).
- When renaming a serialized field, add `[FormerlySerializedAs("oldName")]` if needed to preserve scene/prefab data.
- Don't casually rename serialized fields, public APIs, Unity messages, or referenced types.

```csharp
[SerializeField] private float _moveSpeed = 5f;

public float MoveSpeed => _moveSpeed;
```

## Formatting and layout

- 4 spaces for indentation. No tabs.
- Allman-style braces: opening and closing braces on their own lines.
- Always use braces for `if`, `else`, loops, and other control-flow bodies, even single statements.
- One statement/declaration per line.
- Spaces around binary operators and after commas; no space between a method name and `(`.
- Break long signatures/expressions across multiple readable lines.
- Use blank lines to separate logical sections, not by rigid mechanical rules; avoid redundant blank lines.
- Comments explain intent, constraints, or non-obvious reasoning - never restate the code.

```csharp
// GOOD
if (health <= 0)
{
    Die();
}

// BAD
if (health <= 0) Die();
```

## Methods and control flow

- Each method has one clear responsibility.
- Extract helper methods when they improve meaning or remove duplication, not just to create more methods.
- Guard clauses and multiple `return`s are fine when they reduce nesting and improve readability.
- Avoid duplicated code across conditional branches.
- Prefer direct boolean assignments over `if/else` when clear; avoid unclear positional boolean args - use named arguments or a more expressive API.

```csharp
// GOOD
_isGameOver = remainingLives <= 0;
SetInputEnabled(enabled: false);

// BAD
if (remainingLives <= 0) { _isGameOver = true; } else { _isGameOver = false; }
SetInputEnabled(false);
```

- Use `Update` for frame-based behavior/input; `FixedUpdate` for physics.
- Use `Time.deltaTime` / `Time.fixedDeltaTime` where frame-rate independence is required.
- Avoid unnecessary work and repeated component lookups (`GetComponent`, etc.) in per-frame methods; cache references instead.

## Properties and access

- Prefer properties over Java-style getters/setters.
- Prefer `Health` over `GetHealth()` unless retrieving the value does real work.
- Use the narrowest practical access modifier.
- Avoid public mutable fields.

```csharp
// GOOD
public int Score { get; private set; }

// BAD
public int score;
public int GetScore() => score;
```

## Enums

- Singular noun for a regular enum; plural or a `Flags` suffix for `[Flags]` enums.
- `PascalCase` members; don't prefix members with the enum type name.
- `[Flags]` enums include `None = 0` and use distinct bit values.

```csharp
public enum GameState
{
    Ready,
    Playing,
    GameOver,
}

[Flags]
public enum InputFlags
{
    None = 0,
    Jump = 1 << 0,
    Pause = 1 << 1,
}
```

## Events and delegates

- `PascalCase` event names describing what happened: `HealthChanged`, `PlayerDied`.
- Prefer `Action` / `Action<T>` for simple events; use `EventHandler<TEventArgs>` when standard .NET semantics help.
- Name the raising method `OnEventName` or `RaiseEventName`.
- Use null-safe invocation: `PlayerDied?.Invoke()`.
- Subscribe/unsubscribe symmetrically, commonly in `OnEnable`/`OnDisable`.

## Example

```csharp
public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private int _maxHealth = 3;

    private int _currentHealth;

    public event Action PlayerDied;

    public int CurrentHealth => _currentHealth;

    private void Awake()
    {
        _currentHealth = _maxHealth;
    }

    public void TakeDamage(int amount)
    {
        if (_currentHealth <= 0)
        {
            return;
        }

        _currentHealth = Mathf.Max(_currentHealth - amount, 0);

        if (_currentHealth == 0)
        {
            RaisePlayerDied();
        }
    }

    private void RaisePlayerDied()
    {
        PlayerDied?.Invoke();
    }
}
```
