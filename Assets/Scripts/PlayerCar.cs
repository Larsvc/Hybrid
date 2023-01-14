using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class PlayerCar : HealthEntity
{
    [Header("Properties")]
    public float rotateSpeed = 3f;
    public float moveSpeed
    {
        get { return 35f - 32f * 0.1f * moduleSlots.Where(m => m.transform.childCount > 0).ToArray().Length; }
    }

    public int playerNumber = 1;

    [SerializeField] private AudioClip hitSound;

    private Rigidbody rb;
    private AudioSource audioSource;

    public Camera cam;

    private HealthBar greenHealthBar;
    private HealthBar redHealthBar;

    [Header("Respawn")]
    public float respawnTime = 5f;
    public GameObject deathScreen;
    private Transform respawnPoint;
    private float respawnTimer = 5f;

    private float hor;
    private float vert;

    private const string horizontalControls = "MoveHorizontalP";
    private const string verticalControls = "MoveVerticalP";
    private const string shoot = "FireP";
    private const string ability = "AbilityP";

    private float normalVolume;
    private bool canShoot = true;

    private TextMeshPro healthText;

    #region Modules
    [SerializeField] private Transform[] moduleSlots;
    [SerializeField] private string[] selectedModules;

    [HideInInspector]public List<ShootModule> gunModules = new List<ShootModule>();
    private List<AbilityModule> abilityModules = new List<AbilityModule>();
    #endregion

    public bool pickingModules;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        normalVolume = audioSource.volume;

        healthText = GetComponentInChildren<TextMeshPro>();
        greenHealthBar = GameObject.Find("HealthBar" + playerNumber).GetComponent<HealthBar>();
        respawnPoint = GameObject.Find($"Player{playerNumber}Spawn").transform;

        //LoadModules();

        CheckForModules();
        FinalizeModuleSelection();

        redHealthBar = GetComponentInChildren<HealthBar>();

        greenHealthBar.SetMaxHealth(health);
        redHealthBar.SetMaxHealth(health);
    }

    /*private void LoadModules()
    {
        //TODO: selectedModules = read from chips

        foreach (Module m in allModules)
        {
            Destroy(m.gameObject);
        }

        allModules.Clear();

        selectedModules = ReadModulesFromChips();

        for (int i = 0; i < moduleSlots.Length; i++)
        {
            string moduleInput = selectedModules[i];
            GameObject modulePrefab = Resources.Load<GameObject>("Modules/" + moduleInput);

            if (modulePrefab != null)
            {
                GameObject module = Instantiate(modulePrefab, moduleSlots[i].position, Quaternion.identity, moduleSlots[i]);
                module.transform.forward = transform.forward;
                allModules.Add(module.GetComponent<Module>());
            }
        }

        gunModules = transform.Find("Modules").GetComponentsInChildren<ShootModule>().ToList();
        abilityModules = transform.Find("Modules").GetComponentsInChildren<AbilityModule>().ToList();
    }*/

    private void CheckForModules()
    {
        if (!IsDead)
            selectedModules = ReadModulesFromChips();

        for (int i = 0; i < moduleSlots.Length; i++)
        {
            if (moduleSlots[i].childCount == 0 && selectedModules[i] != "")
            {
                AddModule(i, selectedModules[i]);
            }
            else if (moduleSlots[i].childCount > 0 && !moduleSlots[i].GetChild(0).name.Contains(selectedModules[i]) && selectedModules[i] != "")
            {
                RemoveModule(i);
                AddModule(i, selectedModules[i]);
            }
            else if (moduleSlots[i].childCount > 0 && selectedModules[i] == "")
            {
                RemoveModule(i);
            }
        }
    }

    private void AddModule(int slot, string moduleName)
    {
        GameObject modulePrefab = Resources.Load<GameObject>("Modules/" + moduleName);
        if (modulePrefab != null)
        {
            GameObject module = Instantiate(modulePrefab, moduleSlots[slot].position, Quaternion.identity, moduleSlots[slot]);
            module.transform.forward = transform.forward;
            //allModules[slot] = module.GetComponent<Module>();
        }
        else
        {
            Debug.LogWarning($"The entered module name '{moduleName}' does not exist in the Module folder. Check if is correct or maybe you are just typing :D");
        }
    }

    private void RemoveModule(int slot)
    {
        Destroy(moduleSlots[slot].GetChild(0).gameObject);
    }

    private void FinalizeModuleSelection()
    {
        gunModules = transform.Find("Modules").GetComponentsInChildren<ShootModule>().ToList();
        abilityModules = transform.Find("Modules").GetComponentsInChildren<AbilityModule>().ToList();
    }

    private string[] ReadModulesFromChips() //TODO: read from chips
    {
        //return selectedModules;
        return new string[] { "Gun", "Gun", "Ram", "SpeedUp" };
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        healthText.text = "Health: " + Mathf.CeilToInt(health);

        if (!IsDead)
            HandleMovement();

        if (Input.GetAxisRaw(shoot + playerNumber) != 0 && canShoot)
            SetShootTrigger();

        if (Input.GetAxisRaw(ability + playerNumber) != 0)
        {
            ActivateAbilities();
        }

        if (pickingModules)
            CheckForModules();

        if (IsDead)
        {
            respawnTimer -= Time.deltaTime;
            deathScreen.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Respawning in " + Math.Round(respawnTimer) + " seconds.";
        }
            

        if (respawnTimer <= 0)
            Respawn();
    }

    private void FixedUpdate()
    {
        //rb.AddTorque(transform.up * hor * rotateSpeed);
        rb.MoveRotation(Quaternion.Euler(transform.up * hor * rotateSpeed) * transform.rotation);
        //transform.position += transform.forward * vert * moveSpeed * Time.deltaTime;
        rb.AddForce(transform.forward * vert * moveSpeed, ForceMode.Acceleration);
    }

    private void HandleMovement()
    {
        hor = Input.GetAxisRaw(horizontalControls + playerNumber);
        vert = Input.GetAxisRaw(verticalControls + playerNumber);

        audioSource.volume = Mathf.Lerp(0, normalVolume, rb.velocity.magnitude / (moveSpeed / 4));
        audioSource.pitch = Mathf.Lerp(0.6f, 1f, rb.velocity.magnitude / (moveSpeed / 4));

        animator.SetFloat("speed", rb.velocity.magnitude);
    }

    private void SetShootTrigger()
    {
        foreach(ShootModule gun in gunModules)
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

    private void Respawn()
    {        
        CheckForModules();
        FinalizeModuleSelection();
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        health = maxHealth;
        deathScreen.SetActive(false);
        greenHealthBar.SetHealth(health);
        redHealthBar.SetHealth(health);
        IsDead = false;
        respawnTimer = respawnTime;
    }

    public override void TakeHit(float damage)
    {
        if (!IsDead)
        {
            base.TakeHit(damage);
            audioSource.PlayOneShot(hitSound);
            greenHealthBar.SetHealth(health);
            redHealthBar.SetHealth(health);
        }
    }

    protected override void Die()
    {
        foreach(Transform slot in moduleSlots)
        {
            if (slot.childCount > 0)
                slot.GetChild(0).GetComponent<Module>().TakeHit(10000);
        }

        deathScreen.SetActive(true);
        IsDead = true;
    }
}
