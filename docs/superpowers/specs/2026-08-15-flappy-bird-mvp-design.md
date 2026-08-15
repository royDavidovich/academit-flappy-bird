# Flappy Bird MVP Design

**Date:** 2026-08-15
**Status:** Approved
**Authority:** Governed by external knowledge base `D:\REPOS\academit-unity-course-knowledge-base` — `COURSE_CONSTITUTION.md` and `projects/FLAPPY_BIRD.md` remain the single source of truth for binding requirements. This spec is the project-side execution design; it does not redefine course rules.

## Goal

Deliver the recommended MVP playable loop from `projects/FLAPPY_BIRD.md` Section 4: Ready → flap/gravity → obstacle pass with score → collision → GameOver → reliable restart. Placeholder visuals only; real art deferred until after the core loop is stable.

## Unresolved VERIFY items (not guessed, tracked here)

Exact Unity 6.3 editor identifier, build target, third-party asset restrictions, full rubric details remain `VERIFY` per the Constitution. Orientation is set below as a low-risk default, explicitly labeled `RECOMMENDATION`, not a confirmed requirement.

## Decisions

- **Control:** classic single flap. One input (space/click/tap) applies an upward impulse to the bird's `Rigidbody2D` on each press, captured in `Update`, never in `FixedUpdate`.
- **Obstacles:** `Instantiate`/`Destroy` per pipe pair (no pooling for MVP), matching the course's Session 3 lecture material directly.
- **State management:** `GameManager` singleton (course Session 3 pattern — static access point, duplicate-prevention in `Awake`), owning an explicit `Ready → Playing → GameOver → Ready` state enum and the score value.
- **Visuals:** placeholder primitives/sprites (colored shapes) for MVP. Real art is a later, separate pass gated on asset-rights verification.
- **Orientation:** landscape 16:9 (`RECOMMENDATION`, default — Unity's standard aspect, simplest desktop dev/test loop; revisit if the rubric specifies otherwise).
- **Render pipeline:** URP (Unity 6's current default 2D project template pulls in `com.unity.render-pipelines.universal`). Not originally planned but not a course-rule conflict; accepted as-is rather than recreating the project on a bare-Core template.

## Project structure

```text
Assets/
  Scenes/       (MainScene)
  Scripts/
  Prefabs/
  Materials/
  Settings/
```
Other course-presented categories (3rd Party, Animations, Audio, Models, Textures) are added only when a concrete asset needs them — empty folders add no clarity.

Git ignores `Library`, `Temp`, `Obj`, `Logs`, `Build`, `Builds`, `UserSettings`. All `.meta` files are tracked with their assets.

## Scripts and responsibilities

| Script | Responsibility |
|---|---|
| `GameManager` | Singleton. Owns game state enum and transitions, owns score value, exposes state-change/score-change events for UI. |
| `BirdController` | Reads flap input in `Update`; applies an upward impulse to the bird's `Rigidbody2D` in response. Ignores input outside the `Playing` state. |
| `PipeSpawner` | Timer-driven (`Invoke` or coroutine); instantiates the pipe-pair prefab at the right edge on a configurable interval with a configurable gap. |
| `PipeMover` | Attached to the pipe-pair prefab; translates it leftward every frame at a configurable speed; destroys itself once off-screen left. |
| `ScoreZone` | Trigger collider inside the pipe-pair gap. `OnTriggerEnter2D` with the bird calls `GameManager.AddScore()`. A one-shot flag prevents double-scoring while inside the trigger. |
| `UIController` | Subscribes to `GameManager` state/score events; shows/hides Ready, Playing HUD, and GameOver panels; updates score text. Owns no gameplay state itself. |
| `AudioController` | Deferred — added only after the core loop is stable, per the MVP boundary. |

Bird failure contact: a separate non-trigger collider on the bird registers `OnCollisionEnter2D` against pipes/ground boundary, calling `GameManager.GameOver()`.

## Scene and physics

Single scene, `MainScene`:

- **Camera:** orthographic, fixed, framed for 16:9.
- **Bird:** dynamic `Rigidbody2D` (tuned `gravityScale`), regular `CircleCollider2D`. Placeholder: colored circle sprite.
- **Ground/ceiling boundary:** static collider (`BoxCollider2D`, no `Rigidbody2D` required — static-vs-dynamic is a valid collision pair per the course's Session 4 collision matrix). Placeholder: rectangle sprite.
- **Pipe-pair prefab:** parent object with two child pipe rectangles, each a regular `BoxCollider2D`; the parent carries a kinematic `Rigidbody2D` so its moving colliders remain a valid collision pair per the matrix (kinematic + dynamic bird = valid). A separate trigger `BoxCollider2D` sits in the gap for `ScoreZone`.

Flap input is captured in `Update` and applied to `Rigidbody2D` through the normal physics step, consistent with the Constitution's input/physics lifecycle rules (Section 7).

## UI

UGUI Canvas, Screen Space – Overlay, TextMesh Pro for text:

- **Ready panel:** title + start prompt.
- **Playing HUD:** score text only.
- **GameOver panel:** final score + restart button.

`UIController` toggles panel `SetActive` purely from `GameManager` state changes; no panel owns gameplay logic.

**Restart:** `SceneManager.LoadScene` reload of the active scene. Guarantees no stale state (score, velocity, spawned pipes, UI) without hand-rolled reset code, satisfying the "reliable restart" MVP requirement directly.

## Testing

Manual play-testing against the FB-01 through FB-12 scenarios in `projects/FLAPPY_BIRD.md`'s functional test matrix (core loop, scoring, collision, restart). Cross-platform and performance scenarios (FB-13 onward) are deferred to the later QA phase in the dated delivery plan. No automated test suite — manual QA is the course-specified approach (Constitution Section 12).

## Out of scope for this spec

Sound, animation, high-score persistence, difficulty progression, background motion, and any menu beyond Ready/Playing/GameOver — per the MVP boundary's "add only after the loop is stable" list. Real art assets — pending asset-rights `VERIFY` resolution.
