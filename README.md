Requires:
- Camera shake system that has Amplitude, duration and frequency callable upon function call.
- I personally use a custom Perlin noise script (PerlinCameraShake.cs)



The system is very modualr and fits for any type of weapon by simply modifying the values.
<img width="1708" height="1175" alt="image" src="https://github.com/user-attachments/assets/1a1f8bc1-6dd1-4dde-a3c4-ecd00d203812" />
<img width="2023" height="638" alt="image" src="https://github.com/user-attachments/assets/e072624d-e08b-4a5a-8558-263ceb3bd917" />
<img width="2035" height="682" alt="image" src="https://github.com/user-attachments/assets/e41fd80b-930d-4f87-93f7-d87f526c19f2" />

USING CUSTOM CAMERA SHAKE METHODS:

In this system, I have used a perlin noise to generate camera shake, they are smoother than regular camera shake methods, however if you want to use your own shake system:

1. Comment out line 26 `//private PerlinCameraShake perlinCameraShake;`
   This removes the reference to my own camera shake script, replace it with your own system.
   
2. comment out the camera shake logic:
  `/*
  // Trigger our Perlin noise camera shake if available
        if (perlinCameraShake != null)
        {
            perlinCameraShake.TriggerShake(camShakeDuration, camShakeMagnitude, camShakeFrequency);
        } */`

  replace it with your own camera shake function that triggers the shake.
  Obviously the camera shake script is attached to your camera and has a function that should trigger it from another script referencing it.

