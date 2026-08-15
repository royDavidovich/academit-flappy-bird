using UnityEngine;

public class PipePair : MonoBehaviour
{
    [SerializeField] Transform topPipe;
    [SerializeField] Transform bottomPipe;
    [SerializeField] SpriteRenderer topBody;
    [SerializeField] SpriteRenderer bottomBody;
    [SerializeField] SpriteRenderer topCap;
    [SerializeField] SpriteRenderer bottomCap;
    [SerializeField] BoxCollider2D topCollider;
    [SerializeField] BoxCollider2D bottomCollider;
    [SerializeField] BoxCollider2D scoreZone;

    public void Init(GameConfig config)
    {
        float gapHalf = config.GapSize * 0.5f;
        BuildPipe(topPipe, topBody, topCap, topCollider, config, gapHalf, 1f);
        BuildPipe(bottomPipe, bottomBody, bottomCap, bottomCollider, config, -gapHalf, -1f);
        scoreZone.size = new Vector2(scoreZone.size.x, config.GapSize);
    }

    // Everything is measured outward from the gap edge, so the visible pipe mouth
    // always lands exactly on the gap regardless of pipe or cap height.
    void BuildPipe(Transform pipe, SpriteRenderer body, SpriteRenderer cap, BoxCollider2D collider, GameConfig config, float gapEdge, float direction)
    {
        pipe.localPosition = new Vector3(0f, gapEdge, 0f);
        pipe.localScale = Vector3.one;

        float bodyHeight = config.PipeHeight - config.CapHeight;

        cap.drawMode = SpriteDrawMode.Sliced;
        cap.size = new Vector2(config.PipeWidth, config.CapHeight);
        cap.flipY = direction > 0f;
        cap.transform.localPosition = new Vector3(0f, direction * config.CapHeight * 0.5f, 0f);

        body.drawMode = SpriteDrawMode.Tiled;
        body.size = new Vector2(config.PipeWidth, bodyHeight);
        body.transform.localPosition = new Vector3(0f, direction * (config.CapHeight + bodyHeight * 0.5f), 0f);

        collider.size = new Vector2(config.PipeWidth, config.PipeHeight);
        collider.offset = new Vector2(0f, direction * config.PipeHeight * 0.5f);
    }
}
