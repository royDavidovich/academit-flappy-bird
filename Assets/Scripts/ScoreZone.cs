using UnityEngine;

public class ScoreZone : MonoBehaviour
{
    private bool _scored;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_scored)
        {
            return;
        }

        if (!other.CompareTag("Bird"))
        {
            return;
        }

        _scored = true;
        GameManager.Instance.AddScore();
    }
}
