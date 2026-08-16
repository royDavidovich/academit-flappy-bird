using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioController : MonoBehaviour
{
    [SerializeField] BirdController bird;

    const int SampleRate = 44100;

    AudioSource _source;
    AudioClip _flapClip;
    AudioClip _scoreClip;
    AudioClip _hitClip;

    void Awake()
    {
        _source = GetComponent<AudioSource>();
        _flapClip = GenerateTone(600f, 0.08f, 0.5f, square: false);
        _scoreClip = GenerateChime();
        _hitClip = GenerateTone(110f, 0.25f, 0.5f, square: true);
    }

    void Start()
    {
        // GameManager.Instance is only guaranteed to exist once every object's Awake
        // has run; OnEnable doesn't carry that guarantee, so subscribing here (not
        // OnEnable) avoids a race against GameManager's own Awake on scene start.
        bird.OnFlap += PlayFlap;
        GameManager.Instance.OnScoreChanged += HandleScoreChanged;
        GameManager.Instance.OnStateChanged += HandleStateChanged;
    }

    void OnDisable()
    {
        bird.OnFlap -= PlayFlap;
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnScoreChanged -= HandleScoreChanged;
            GameManager.Instance.OnStateChanged -= HandleStateChanged;
        }
    }

    void PlayFlap() => _source.PlayOneShot(_flapClip);

    void HandleScoreChanged(int score)
    {
        // GameManager also fires this on StartGame() to reset the HUD to 0;
        // only a real cleared pipe (score > 0) should play the chime.
        if (score > 0) _source.PlayOneShot(_scoreClip);
    }

    void HandleStateChanged(GameState state)
    {
        if (state == GameState.GameOver) _source.PlayOneShot(_hitClip);
    }

    static AudioClip GenerateTone(float frequency, float duration, float volume, bool square)
    {
        int sampleCount = Mathf.CeilToInt(SampleRate * duration);
        var data = new float[sampleCount];
        FillTone(data, 0, sampleCount, frequency, volume, square);

        var clip = AudioClip.Create("Tone", sampleCount, 1, SampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    static AudioClip GenerateChime()
    {
        int n1 = Mathf.CeilToInt(SampleRate * 0.08f);
        int n2 = Mathf.CeilToInt(SampleRate * 0.1f);
        var data = new float[n1 + n2];
        FillTone(data, 0, n1, 880f, 0.45f, square: false);
        FillTone(data, n1, n2, 1320f, 0.45f, square: false);

        var clip = AudioClip.Create("Chime", data.Length, 1, SampleRate, false);
        clip.SetData(data, 0);
        return clip;
    }

    // Writes `count` samples of a sine or square wave into `data` starting at `offset`,
    // with a linear fade-out so the clip doesn't end in an audible click.
    static void FillTone(float[] data, int offset, int count, float frequency, float volume, bool square)
    {
        for (int i = 0; i < count; i++)
        {
            float t = (float)i / SampleRate;
            float envelope = 1f - (float)i / count;
            float raw = Mathf.Sin(2f * Mathf.PI * frequency * t);
            float sample = square ? Mathf.Sign(raw) : raw;
            data[offset + i] = sample * volume * envelope;
        }
    }
}
