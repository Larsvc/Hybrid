using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCar : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float bulletForce = 30f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip hitSound;

    private float fireRate = 2f;

    private Rigidbody rb;
    private AudioSource audioSource;
    private Animator animator;

    private float hor;
    private float vert;

    private float normalVolume;
    private bool canShoot = true;

    private Transform barrel;

    private ParticleSystem shootEffect;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        normalVolume = audioSource.volume;

        barrel = transform.GetChild(1).GetChild(0);

        shootEffect = GetComponentInChildren<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovement();

        if (Input.GetAxisRaw("Fire1") != 0 && canShoot)
            Shoot();
    }

    private void FixedUpdate()
    {
        rb.AddTorque(transform.up * hor * rotateSpeed);
        //transform.position += transform.forward * vert * moveSpeed * Time.deltaTime;
        rb.AddForce(transform.forward * vert * moveSpeed, ForceMode.Acceleration);
    }

    private void HandleMovement()
    {
        hor = Input.GetAxisRaw("Horizontal");
        vert = Input.GetAxisRaw("Vertical");

        audioSource.volume = Mathf.Lerp(0, normalVolume, rb.velocity.magnitude / 5f);
        audioSource.pitch = Mathf.Lerp(0.6f, 1f, rb.velocity.magnitude / 5f);

        animator.SetFloat("speed", rb.velocity.magnitude);
    }

    private void Shoot()
    {
        Rigidbody bullet = Instantiate(bulletPrefab, barrel.position + transform.forward, Quaternion.identity).GetComponent<Rigidbody>();
        bullet.AddForce(transform.forward * bulletForce * bullet.mass, ForceMode.Impulse);
        animator.SetTrigger("shoot");
        shootEffect.Play();

        canShoot = false;

        StartCoroutine(WaitForFireRate());
    }

    IEnumerator WaitForFireRate()
    {
        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
    }

    public void TakeHit()
    {
        animator.SetTrigger("hit");
        audioSource.PlayOneShot(hitSound);
    }
}
