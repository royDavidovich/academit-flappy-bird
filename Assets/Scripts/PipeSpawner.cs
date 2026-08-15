using UnityEngine;

public class PipeSpawner : MonoBehaviour
{
    [SerializeField] GameObject pipePairPrefab;
    [SerializeField] GameConfig config;

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
            InvokeRepeating(nameof(SpawnPipe), 0f, config.SpawnInterval);
        }
        else
        {
            CancelInvoke(nameof(SpawnPipe));
        }
    }

    void SpawnPipe()
    {
        float gapY = Random.Range(-config.GapYRange, config.GapYRange);
        GameObject pipe = Instantiate(pipePairPrefab, new Vector3(config.SpawnX, gapY, 0f), Quaternion.identity);

        // Config is injected rather than serialized on the prefab: a prefab asset
        // cannot hold a reference to a scene object.
        pipe.GetComponent<PipePair>().Init(config);
        pipe.GetComponent<PipeMover>().Init(config);
    }
}
