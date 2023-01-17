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
    public float baseSpeed = 2f;
    private float currentSpeed;
    private float moduleSlowModifier = 0.075f;

    public float moveSpeed
    {
        get { return currentSpeed - currentSpeed * moduleSlowModifier * moduleSlots.Where(m => m.transform.childCount > 0).ToArray().Length; }
    }

    public int playerNumber = 1;

    public Animator hitmarker;
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
    private const string flip = "FlipP";

    private bool flipCounting;
    private float flipTimer;
    [SerializeField] private float flipDelay = 3f;

    [SerializeField] private float regendelay = 5f;
    private float regenTimer;
    [SerializeField] private float regenPercentagePerSecond = 0.1f;

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

        if (pickingModules)
            return;

        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        normalVolume = audioSource.volume;

        healthText = GetComponentInChildren<TextMeshPro>();
        greenHealthBar = GameObject.Find("HealthBar" + playerNumber).GetComponent<HealthBar>();
        respawnPoint = GameObject.Find($"Player{playerNumber}Spawn").transform;

        //LoadModules();
        currentSpeed = baseSpeed;

        CheckForModules();
        FinalizeModuleSelection();

        redHealthBar = GetComponentInChildren<HealthBar>();

        greenHealthBar.SetMaxHealth(health);
        redHealthBar.SetMaxHealth(health);
    }

    public void SetSpeed(float speed)
    {
        currentSpeed = speed;
    }

    public void SetMass(float mass)
    {
        rb.mass = mass;
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
        if (!IsDead && !pickingModules)
            selectedModules = GetPickedModules();
        else if (!IsDead)
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
            GameObject module = Instantiate(modulePrefab, moduleSlots[slot].position + modulePrefab.GetComponent<Module>().offset, Quaternion.identity, moduleSlots[slot]);
            module.transform.forward = moduleSlots[slot].transform.TransformDirection(Vector3.forward);
            module.transform.rotation *= modulePrefab.transform.rotation;
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

        //0 = front, 1 = top 1, 3 = top 2, 2 = back;
        //return new string[] { "Ram", "Minigun", "Booster", "Shield" };
        if (ModuleController.instance != null)
            return ModuleController.instance.modules;
        else
            return null;
    }

    private string[] GetPickedModules()
    {
        if (GameManager.playerSelectedModules[playerNumber - 1] != null)
            return GameManager.playerSelectedModules[playerNumber - 1].modules;
        else return new string[] { "Ram", "Minigun", "Booster", "Shield" };
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();

        if (!pickingModules)
        {
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

            SetHp();

            if (respawnTimer <= 0)
                Respawn();
        }
        else
        {
            CheckForModules();
        }
    }

    private void SetHp()
    {
        greenHealthBar.SetHealth(health);
        redHealthBar.SetHealth(health);
        float regenValue = regenPercentagePerSecond * maxHealth * Time.deltaTime;
        if (regenTimer <= 0)
            health = Mathf.Min(health + regenValue, maxHealth);
        regenTimer -= Time.deltaTime;
    }

    private void FixedUpdate()
    {
        //rb.AddTorque(transform.up * hor * rotateSpeed);
        //rb.MoveRotation(Quaternion.Euler(Vector3.up * hor * rotateSpeed) * transform.rotation);
        //transform.position += transform.forward * vert * moveSpeed * Time.deltaTime;
        //rb.AddForce(transform.forward * vert * moveSpeed, ForceMode.Acceleration);
        //rb.MovePosition(rb.position + (transform.forward * vert * moveSpeed * Time.fixedDeltaTime));
    }

    private void HandleMovement()
    {
        hor = Input.GetAxisRaw(horizontalControls + playerNumber);
        vert = Input.GetAxisRaw(verticalControls + playerNumber);

        float horizontalSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        audioSource.volume = Mathf.Lerp(0, normalVolume, horizontalSpeed / 4);
        audioSource.pitch = Mathf.Lerp(0.6f, 1.5f, horizontalSpeed / 20f);

        animator.SetFloat("speed", rb.velocity.magnitude);

        if (transform.position.y < -2f)
            Die();

        if (Vector3.Angle(transform.up, Vector3.up) > 80 && rb.velocity.magnitude < 1)
        {
            if (!flipCounting)
                flipCounting = true;
            else
                flipTimer -= Time.deltaTime;
        }
            
        

        if (Input.GetAxisRaw(flip + playerNumber) != 0 || flipTimer <= 0)
        {
            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
            flipCounting = false;                
            flipTimer = flipDelay;
        }
            

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
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        health = maxHealth;
        deathScreen.SetActive(false);
        greenHealthBar.SetHealth(health);
        redHealthBar.SetHealth(health);
        IsDead = false;
        respawnTimer = respawnTime;
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        CheckForModules();
        FinalizeModuleSelection();
    }

    public override void TakeHit(float damage,  Animator hitmarker)
    {
        if (!IsDead)
        {
            base.TakeHit(damage, hitmarker);
            audioSource.PlayOneShot(hitSound);
            regenTimer = regendelay;
        }
    }

    protected override void Die()
    {
        foreach(Transform slot in moduleSlots)
        {
            if (slot.childCount > 0)
                slot.GetChild(0).GetComponent<Module>().TakeHit(10000, hitmarker);
        }

        deathScreen.SetActive(true);
        IsDead = true;
    }
}
