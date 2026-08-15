using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] GameObject pipePairPrefab;
    [SerializeField] float spawnInterval = 1.5f;
    [SerializeField] float spawnX = 9f;
    [SerializeField] float gapYRange = 2f;

    void Start()
    {
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    void OnDisable()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }
    }

    void HandleStateChanged(GameState state)
    {
        if (state == GameState.Playing)
        {
            InvokeRepeating(nameof(SpawnPipe), 0f, spawnInterval);
        }
        else
        {
            CancelInvoke(nameof(SpawnPipe));
        }
    }

    void SpawnPipe()
    {
        float gapY = Random.Range(-gapYRange, gapYRange);
        Instantiate(pipePairPrefab, new Vector3(spawnX, gapY, 0f), Quaternion.identity);
    }
}
