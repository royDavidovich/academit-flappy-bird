using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    [SerializeField] private Transform[] pieces;
    [SerializeField] private float scrollSpeed = 0.8f;

    private float _pieceWidth;
    private bool _scrolling;
    private Camera _cam;

    private void Awake()
    {
        _cam = Camera.main;
        var sr = pieces[0].GetComponent<SpriteRenderer>();
        _pieceWidth = sr.bounds.size.x;
    }

    private void Start()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }
    }

    private void HandleStateChanged(GameState state)
    {
        _scrolling = state == GameState.Playing;
    }

    private void Update()
    {
        if (!_scrolling)
        {
            return;
        }

        float delta = scrollSpeed * Time.deltaTime;
        float cameraLeftEdge = _cam.transform.position.x - _cam.orthographicSize * _cam.aspect;

        Transform leftmost = pieces[0];
        Transform rightmost = pieces[0];
        foreach (var piece in pieces)
        {
            piece.position += Vector3.left * delta;
            if (piece.position.x < leftmost.position.x)
            {
                leftmost = piece;
            }

            if (piece.position.x > rightmost.position.x)
            {
                rightmost = piece;
            }
        }

        // Once the trailing piece's right edge clears the camera's left bound it has
        // nothing left to show, so recycle it to extend the strip just past the
        // current rightmost piece instead of leaving a gap on the right.
        if (leftmost.position.x + _pieceWidth * 0.5f < cameraLeftEdge)
        {
            leftmost.position = new Vector3(rightmost.position.x + _pieceWidth, leftmost.position.y, leftmost.position.z);
        }
    }
}
