using UnityEngine;

public class PerlinCameraShake : MonoBehaviour
{
    private float shakeDuration = 0f;
    private float shakeAmplitude = 0f;
    private float shakeFrequency = 0f;

    private float seedX;
    private float seedY;
    private Vector3 initialPosition;

    void Start()
    {
        initialPosition = transform.localPosition;

        // Random seeds to ensure variation between sessions
        seedX = Random.Range(0f, 100f);
        seedY = Random.Range(0f, 100f);
    }

    void Update()
    {
        if (shakeDuration > 0f)
        {
            float noiseX = Mathf.PerlinNoise(seedX, Time.time * shakeFrequency) * 2 - 1;
            float noiseY = Mathf.PerlinNoise(seedY, Time.time * shakeFrequency) * 2 - 1;

            Vector3 shakeOffset = new Vector3(noiseX, noiseY, 0f) * shakeAmplitude;
            transform.localPosition = initialPosition + shakeOffset;

            shakeDuration -= Time.deltaTime;

            if (shakeDuration <= 0f)
            {
                transform.localPosition = initialPosition;
            }
        }

        // TEST: Shake when Left Mouse Button is pressed
        if (Input.GetMouseButtonDown(0))  // 0 = LMB
        {
            TriggerShake(0.5f, 0.1f, 15f);  // Customize as needed
        }
    }

    /// <summary>
    /// Triggers a camera shake with custom parameters.
    /// </summary>
    /// <param name="duration">How long the shake should last (seconds)</param>
    /// <param name="amplitude">How intense the shake is</param>
    /// <param name="frequency">How fast the shake oscillates</param>
    public void TriggerShake(float duration, float amplitude, float frequency)
    {
        shakeDuration = duration;
        shakeAmplitude = amplitude;
        shakeFrequency = frequency;
    }
}
