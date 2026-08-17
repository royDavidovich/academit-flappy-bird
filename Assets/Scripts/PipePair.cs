using UnityEngine;

public class PipePair : MonoBehaviour
{
    [SerializeField] private Transform topPipe;
    [SerializeField] private Transform bottomPipe;
    [SerializeField] private SpriteRenderer topBody;
    [SerializeField] private SpriteRenderer bottomBody;
    [SerializeField] private SpriteRenderer topCap;
    [SerializeField] private SpriteRenderer bottomCap;
    [SerializeField] private BoxCollider2D topCollider;
    [SerializeField] private BoxCollider2D bottomCollider;
    [SerializeField] private BoxCollider2D scoreZone;

    public void Init(GameConfig config)
    {
        float gapHalf = config.GapSize * 0.5f;
        BuildPipe(topPipe, topBody, topCap, topCollider, config, gapHalf, 1f);
        BuildPipe(bottomPipe, bottomBody, bottomCap, bottomCollider, config, -gapHalf, -1f);
        scoreZone.size = new Vector2(scoreZone.size.x, config.GapSize);
    }

    // Everything is measured outward from the gap edge, so the visible pipe mouth
    // always lands exactly on the gap regardless of pipe or cap height.
    private void BuildPipe(Transform pipe, SpriteRenderer body, SpriteRenderer cap, BoxCollider2D collider, GameConfig config, float gapEdge, float direction)
    {
        pipe.localPosition = new Vector3(0f, gapEdge, 0f);
        pipe.localScale = Vector3.one;

        float bodyHeight = config.PipeHeight - config.CapHeight;

        cap.drawMode = SpriteDrawMode.Sliced;
        cap.size = new Vector2(config.PipeWidth, config.CapHeight);
        cap.transform.localPosition = new Vector3(0f, direction * config.CapHeight * 0.5f, 0f);
        // Mirrored via scale, not SpriteRenderer.flipY: flipY combined with Sliced draw
        // mode on a sprite with no defined border corrupts the UV mapping (visible as a
        // garbled/duplicated rim). A scale flip is a plain geometric mirror instead.
        cap.transform.localScale = new Vector3(1f, direction > 0f ? -1f : 1f, 1f);

        body.drawMode = SpriteDrawMode.Sliced;
        body.size = new Vector2(config.PipeWidth, bodyHeight);
        body.transform.localPosition = new Vector3(0f, direction * (config.CapHeight + bodyHeight * 0.5f), 0f);
        // The body art has a socket band near one texture edge, meant to sit flush
        // against the cap. Unflipped, that band lands at the far end for the top pipe
        // instead of at the cap seam, so mirror the body for the top pipe to match.
        body.transform.localScale = new Vector3(1f, direction > 0f ? -1f : 1f, 1f);

        collider.size = new Vector2(config.PipeWidth, config.PipeHeight);
        collider.offset = new Vector2(0f, direction * config.PipeHeight * 0.5f);
    }
}
