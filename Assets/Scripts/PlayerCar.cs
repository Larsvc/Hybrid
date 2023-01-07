using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerCar : HealthEntity
{
    [SerializeField] private float rotateSpeed = 5f;
    [SerializeField] public float moveSpeed = 10f;
    
    [SerializeField] private AudioClip hitSound;

    private Rigidbody rb;
    private AudioSource audioSource;

    private float hor;
    private float vert;

    private float normalVolume;
    private bool canShoot = true;

    private TextMeshPro healthText;

    #region Modules
    [SerializeField] private Transform[] moduleSlots;
    private string[] selectedModules;

    private GunModule[] gunModules;
    private AbilityModule[] abilityModules;
    #endregion

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        normalVolume = audioSource.volume;

        healthText = GetComponentInChildren<TextMeshPro>();

        LoadModules();
        gunModules = transform.Find("Modules").GetComponentsInChildren<GunModule>();
        abilityModules = transform.Find("Modules").GetComponentsInChildren<AbilityModule>();
    }

    private void LoadModules()
    {
        //TODO: selectedModules = read from chips
        selectedModules = ReadModulesFromChips();

        for (int i = 0; i < moduleSlots.Length; i++)
        {
            string moduleInput = selectedModules[i];
            GameObject modulePrefab = Resources.Load<GameObject>("Modules/" + moduleInput);

            if (modulePrefab != null)
            {
                GameObject module = Instantiate(modulePrefab, moduleSlots[i].position, Quaternion.identity, moduleSlots[i]);
                module.transform.forward = transform.forward;
            }
        }
    }

    private string[] ReadModulesFromChips() //TODO: read from chips
    {
        return new string[] { "Gun", "SpeedUp" };
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        healthText.text = "Health: " + health;

        HandleMovement();

        if (Input.GetAxisRaw("Fire1") != 0 && canShoot)
            SetShootTrigger();

        if (Input.GetAxisRaw("Fire3") != 0)
            ActivateAbilities();
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

    private void ActivateAbilities()
    {
        foreach (AbilityModule ability in abilityModules)
        {
            ability.Activate();
        }
    }

    public override void TakeHit(float damage)
    {
        base.TakeHit(damage);
        audioSource.PlayOneShot(hitSound);
    }

    protected override void Die()
    {
        Destroy(gameObject);
    }
}
