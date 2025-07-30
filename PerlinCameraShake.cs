using UnityEngine;

public class PerlinCameraShake : MonoBehaviour
{
    private Vector3 initialPosition;
    private Quaternion initialRotation;

    private float positionalTimer = 0f;
    private float positionalDuration = 0f;
    private float positionalAmplitude = 0f;
    private float positionalFrequency = 0f;

    private float rotationalTimer = 0f;
    private float rotationalDuration = 0f;
    private float rotationalAmplitude = 0f;
    private float rotationalFrequency = 0f;

    public Quaternion CurrentShakeRotation { get; private set; } = Quaternion.identity;

    void Start()
    {
        initialPosition = transform.localPosition;
        initialRotation = transform.localRotation;
    }

    void Update()
    {
        // Positional shake
        if (positionalTimer > 0)
        {
            positionalTimer -= Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(positionalTimer / positionalDuration);
            float currentAmplitude = positionalAmplitude * normalizedTime; // fade out

            float shakeX = (Mathf.PerlinNoise(Time.time * positionalFrequency, 0f) - 0.5f) * 2f * currentAmplitude;
            float shakeY = (Mathf.PerlinNoise(0f, Time.time * positionalFrequency) - 0.5f) * 2f * currentAmplitude;

            transform.localPosition = initialPosition + new Vector3(shakeX, shakeY, 0f);
        }
        else
        {
            positionalTimer = 0f;
            positionalAmplitude = 0f;
            positionalFrequency = 0f;
            transform.localPosition = initialPosition;
        }

        // Rotational shake
        if (rotationalTimer > 0)
        {
            rotationalTimer -= Time.deltaTime;
            float normalizedTime = Mathf.Clamp01(rotationalTimer / rotationalDuration);
            float currentAmplitude = rotationalAmplitude * normalizedTime; // fade out

            float shakeRotX = (Mathf.PerlinNoise(Time.time * rotationalFrequency, 1f) - 0.5f) * 2f * currentAmplitude;
            float shakeRotY = (Mathf.PerlinNoise(1f, Time.time * rotationalFrequency) - 0.5f) * 2f * currentAmplitude;

            CurrentShakeRotation = Quaternion.Euler(shakeRotX, shakeRotY, 0f);
            transform.localRotation = initialRotation * CurrentShakeRotation;
        }
        else
        {
            rotationalTimer = 0f;
            rotationalAmplitude = 0f;
            rotationalFrequency = 0f;

            CurrentShakeRotation = Quaternion.identity;
            transform.localRotation = initialRotation;
        }

        // Testing input
        /*
        if (Input.GetMouseButtonDown(0)) // LMB: rotational shake
        {
            TriggerRotationalShake(2f, 0.5f, 5f);
        }
        if (Input.GetMouseButtonDown(1)) // RMB: positional shake
        {
            TriggerPositionalShake(4f, 0.5f, 5f);
        }*/
    }

    public void TriggerPositionalShake(float amplitude, float duration, float frequency = 1f)
    {
        positionalAmplitude = Mathf.Max(positionalAmplitude, amplitude);
        positionalFrequency = frequency;
        positionalDuration = Mathf.Max(positionalDuration, duration);
        positionalTimer = Mathf.Max(positionalTimer, duration);
    }

    public void TriggerRotationalShake(float amplitude, float duration, float frequency = 1f)
    {
        rotationalAmplitude = Mathf.Max(rotationalAmplitude, amplitude);
        rotationalFrequency = frequency;
        rotationalDuration = Mathf.Max(rotationalDuration, duration);
        rotationalTimer = Mathf.Max(rotationalTimer, duration);
    }
}
