using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.XR;

[AddComponentMenu("Nokobot/Modern Guns/Simple Shoot")]
public class SimpleShoot : MonoBehaviour
{
    [Header("Prefab References")]
    public GameObject bulletPrefab;
    public GameObject casingPrefab;
    public GameObject muzzleFlashPrefab;
    public GameObject bulletHolePrefab;

    [Header("Hit Feedback")]
    public AudioClip hitSound;
    public HitmarkerPopup hitmarker;

    [Header("Audio")]
    public AudioClip gunSound;
    private AudioSource audioSource;
    public AudioClip headshotSound;
    public AudioClip reloadSound;

    [Header("Location References")]
    [SerializeField] private Animator gunAnimator;
    [SerializeField] private Transform barrelLocation;
    [SerializeField] private Transform casingExitLocation;

    [Header("Settings")]
    [SerializeField] private float destroyTimer = 2f;
    [SerializeField] private float shotPower = 500f;
    [SerializeField] private float ejectPower = 150f;

    [Header("Ammo Settings")]
    public int magazineSize = 12;
    public int currentAmmo = 12;
    public int damage = 40;

    [Header("XR Input")]
    public InputActionProperty triggerAction;
    public InputActionProperty reloadAction;

    [Header("Recoil")]
    [SerializeField] private float recoilKickBack = 0.05f;
    [SerializeField] private float recoilUp = 2f;
    [SerializeField] private float recoilSide = 1f;
    [SerializeField] private float recoilReturnSpeed = 10f;

    private Vector3 originalLocalPosition;
    private Quaternion originalLocalRotation;

    void Start()
    {
        if (barrelLocation == null)
            barrelLocation = transform;

        if (gunAnimator == null)
            gunAnimator = GetComponentInChildren<Animator>();

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        originalLocalPosition = transform.localPosition;
        originalLocalRotation = transform.localRotation;
    }

    void Update()
    {
        if (triggerAction.action != null && triggerAction.action.WasPressedThisFrame())
        {
            if (currentAmmo > 0)
            {
                currentAmmo--;
                gunAnimator.SetTrigger("Fire");
                Debug.Log("Ammo: " + currentAmmo + " / " + magazineSize);
            }
            else
            {
                Debug.Log("Out of ammo! Press R to reload.");
            }
        }

        if (reloadAction.action != null && reloadAction.action.WasPressedThisFrame())
        {
            Reload();
        }

        transform.localPosition = Vector3.Lerp(
            transform.localPosition,
            originalLocalPosition,
            Time.deltaTime * recoilReturnSpeed
        );

        transform.localRotation = Quaternion.Lerp(
            transform.localRotation,
            originalLocalRotation,
            Time.deltaTime * recoilReturnSpeed
        );
    }

    void Reload()
    {
        if (reloadSound != null)
            audioSource.PlayOneShot(reloadSound);
        currentAmmo = magazineSize;
        Debug.Log("Reloaded!");
    }

    // Called by animation event
    void Shoot()
    {
        ApplyRecoil();

        // Gun sound
        if (gunSound != null)
            audioSource.PlayOneShot(gunSound);

        // Muzzle flash
        if (muzzleFlashPrefab)
        {
            GameObject tempFlash = Instantiate(
                muzzleFlashPrefab,
                barrelLocation.position,
                barrelLocation.rotation
            );

            Destroy(tempFlash, destroyTimer);
        }

        RaycastHit hit;

        if (Physics.Raycast(barrelLocation.position, barrelLocation.forward, out hit, 100f))
        {
            Debug.Log("Hit: " + hit.collider.name);
            Debug.DrawRay(hit.point, hit.normal, Color.green, 2f);

            HeadshotTarget headshot = hit.collider.GetComponent<HeadshotTarget>();
            Target target = hit.collider.GetComponent<Target>();

            if (headshot != null)
            {
                headshot.TakeHeadshot(damage);

                if (hitmarker != null)
                    hitmarker.ShowHitmarker();

                if (headshotSound != null)
                    audioSource.PlayOneShot(headshotSound);

                Debug.Log("HEADSHOT!");
            }
            else if (target != null)
            {
                target.TakeDamage(damage);

                if (hitmarker != null)
                    hitmarker.ShowHitmarker();

                if (hitSound != null)
                    audioSource.PlayOneShot(hitSound);
            }

            if (bulletHolePrefab != null)
            {
                GameObject hole = Instantiate(
                    bulletHolePrefab,
                    hit.point + hit.normal * 0.02f,
                    Quaternion.LookRotation(-hit.normal)
                );

                // random rotation around surface
                hole.transform.Rotate(0f, 0f, Random.Range(0f, 360f));

                // slight tilt so they don't look identical
                hole.transform.Rotate(
                    Random.Range(-8f, 8f),
                    Random.Range(-8f, 8f),
                    0f
                );

                // random size
                float size = Random.Range(0.2f, 0.5f);
                hole.transform.localScale = Vector3.one * size;

                // attach to hit object
                hole.transform.SetParent(hit.collider.transform);

                // cleanup after time
                Destroy(hole, 30f);
            }

        }

        // Optional projectile bullet
        if (bulletPrefab)
        {
            Rigidbody bulletRB = Instantiate(
                bulletPrefab,
                barrelLocation.position,
                barrelLocation.rotation
            ).GetComponent<Rigidbody>();

            bulletRB.AddForce(barrelLocation.forward * shotPower);
        }
    }

    // Called by animation event
    void CasingRelease()
    {
        if (!casingExitLocation || !casingPrefab)
            return;

        GameObject tempCasing = Instantiate(
            casingPrefab,
            casingExitLocation.position,
            casingExitLocation.rotation
        );

        Rigidbody casingRB = tempCasing.GetComponent<Rigidbody>();

        casingRB.AddExplosionForce(
            Random.Range(ejectPower * 0.7f, ejectPower),
            (casingExitLocation.position
            - casingExitLocation.right * 0.3f
            - casingExitLocation.up * 0.6f),
            1f
        );

        casingRB.AddTorque(
            new Vector3(
                0,
                Random.Range(100f, 500f),
                Random.Range(100f, 1000f)
            ),
            ForceMode.Impulse
        );

        Destroy(tempCasing, destroyTimer);
    }

    void ApplyRecoil()
    {
        transform.localPosition -= new Vector3(0, 0, recoilKickBack);

        float sideKick = Random.Range(-recoilSide, recoilSide);

        transform.localRotation *= Quaternion.Euler(-recoilUp, sideKick, 0);
    }
}