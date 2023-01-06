using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerCar : HealthEntity
{
    [SerializeField]private float health = 1000;

    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] private float moveSpeed = 10f;
    [SerializeField] private float bulletForce = 30f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private AudioClip hitSound;

    private float fireRate = 9f;

    private Rigidbody rb;
    private AudioSource audioSource;

    private float hor;
    private float vert;

    private float normalVolume;
    private bool canShoot = true;

    private Transform barrel;

    private ParticleSystem shootEffect;

    private GunModule[] gunModules;

    private TextMeshPro healthText;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        normalVolume = audioSource.volume;

        barrel = transform.GetChild(1).GetChild(0);

        shootEffect = GetComponentInChildren<ParticleSystem>();
        healthText = GetComponentInChildren<TextMeshPro>();

        gunModules = transform.Find("Modules").GetComponentsInChildren<GunModule>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        healthText.text = "Health: " + health;

        HandleMovement();

        if (Input.GetAxisRaw("Fire1") != 0 && canShoot)
           SetShootTrigger();
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

    private void SetShootTrigger()
    {
        foreach(GunModule gun in gunModules)
        {
            gun.Fire();
        }
    }

   /* private void Shoot()
    {
        foreach (Transform gun in gunModules)
        {
            Rigidbody bullet = Instantiate(bulletPrefab, gun.position + gun.forward, Quaternion.identity).GetComponent<Rigidbody>();
            bullet.AddForce(gun.forward * bulletForce * bullet.mass, ForceMode.Impulse);
            gun.GetComponentInChildren<ParticleSystem>().Play();
        }
        animator.SetTrigger("shoot");
        //shootEffect.Play();

        canShoot = false;

        StartCoroutine(WaitForFireRate());
    }

    IEnumerator WaitForFireRate()
    {
        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
    }*/

    public override void TakeHit(float damage)
    {
        hitmarker.SetTrigger("hit");
        health -= damage;
        animator.SetTrigger("hit");
        audioSource.PlayOneShot(hitSound);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
