using UnityEngine;

public class PipePair : MonoBehaviour
{
    [SerializeField] Transform topPipe;
    [SerializeField] Transform bottomPipe;
    [SerializeField] BoxCollider2D scoreZone;

    public void Init(GameConfig config)
    {
        float offset = config.GapSize * 0.5f + topPipe.localScale.y * 0.5f;
        topPipe.localPosition = new Vector3(0f, offset, 0f);
        bottomPipe.localPosition = new Vector3(0f, -offset, 0f);
        scoreZone.size = new Vector2(scoreZone.size.x, config.GapSize);
    }
}
