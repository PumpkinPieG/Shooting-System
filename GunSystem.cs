using UnityEngine;
using TMPro;

public class GunSystem : MonoBehaviour
{
    // Gun stats
    public int damage;
    public float timeBetweenShooting, spread, range, reloadTime, timeBetweenShots;
    public int magazineSize, bulletsPerTap;
    public bool allowButtonHold;
    int bulletsLeft, bulletsShot;

    // bools
    bool shooting, readyToShoot, reloading;

    // References
    public Camera fpsCam;
    public Transform attackPoint;
    public RaycastHit rayHit;
    public LayerMask whatIsEnemy;

    // Graphics
    public GameObject muzzleFlash, bulletHoleGraphic;

    // Use PerlinCameraShake
    private PerlinCameraShake perlinCameraShake;
    public float camShakeMagnitude = 2f;
    public float camShakeFrequency = 5f;
    public float camShakeDuration = 0.5f;

    public TextMeshProUGUI text;

    private void Awake()
    {
        bulletsLeft = magazineSize;
        readyToShoot = true;

        // Try to get the PerlinCameraShake component from the fpsCam
        perlinCameraShake = fpsCam.GetComponent<PerlinCameraShake>();
        if (perlinCameraShake == null)
        {
            Debug.LogWarning("PerlinCameraShake component not found on fpsCam!");
        }
    }

    private void Update()
    {
        MyInput();

        // Update UI text
        text.SetText(bulletsLeft + " / " + magazineSize);
    }

    private void MyInput()
    {
        if (allowButtonHold) shooting = Input.GetKey(KeyCode.Mouse0);
        else shooting = Input.GetKeyDown(KeyCode.Mouse0);

        if (Input.GetKeyDown(KeyCode.R) && bulletsLeft < magazineSize && !reloading) Reload();

        if (readyToShoot && shooting && !reloading && bulletsLeft > 0)
        {
            bulletsShot = bulletsPerTap;
            Shoot();
        }
    }

    private void Shoot()
    {
        readyToShoot = false;

        // Spread
        float x = Random.Range(-spread, spread);
        float y = Random.Range(-spread, spread);

        // Calculate direction with spread
        Vector3 direction = fpsCam.transform.forward + new Vector3(x, y, 0);

        // Raycast
        if (Physics.Raycast(fpsCam.transform.position, direction, out rayHit, range, whatIsEnemy))
        {
            Debug.Log(rayHit.collider.name);

            if (rayHit.collider.CompareTag("Enemy"))
                rayHit.collider.GetComponent<ShootingAi>().TakeDamage(damage);
        }
        //Debuging to see the ray cast as visual feedback
        Debug.DrawRay(fpsCam.transform.position, direction * range, Color.red, 1f);


        // Trigger rotational shake on shooting if available
        if (perlinCameraShake != null)
        {
            perlinCameraShake.TriggerRotationalShake(camShakeMagnitude, camShakeDuration, camShakeFrequency);
            // If you want positional shake instead, use:
            // perlinCameraShake.TriggerPositionalShake(camShakeMagnitude, camShakeDuration, camShakeFrequency);
        }

        // Graphics
        Instantiate(bulletHoleGraphic, rayHit.point, Quaternion.Euler(0, 180, 0));
        Instantiate(muzzleFlash, attackPoint.position, Quaternion.identity);

        bulletsLeft--;
        bulletsShot--;

        Invoke(nameof(ResetShot), timeBetweenShooting);

        if (bulletsShot > 0 && bulletsLeft > 0)
            Invoke(nameof(Shoot), timeBetweenShots);
    }

    private void ResetShot()
    {
        readyToShoot = true;
    }

    private void Reload()
    {
        reloading = true;
        Invoke(nameof(ReloadFinished), reloadTime);
    }

    private void ReloadFinished()
    {
        bulletsLeft = magazineSize;
        reloading = false;
    }
}
