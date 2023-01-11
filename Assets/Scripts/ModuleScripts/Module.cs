using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Module : HealthEntity
{
    protected PlayerCar player;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        player = transform.root.GetComponent<PlayerCar>();
        hitmarker = GameObject.Find("Hitmarker" + player.playerNumber).GetComponent<Animator>();

        gameObject.layer = player.gameObject.layer;
        foreach (Transform child in transform)
            child.gameObject.layer = player.gameObject.layer;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        GameObject explosion = Instantiate(PrefabManager.instance.moduleExplosion, transform.position, Quaternion.identity);
        Destroy(explosion, 2f);

        transform.SetParent(null);
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.mass = 0.01f;
        rb.AddTorque(UnityEngine.Random.insideUnitSphere * 1.5f);
        rb.AddForce(Vector3.up * 3, ForceMode.Force);
        Destroy(this);
        Destroy(gameObject, 4f);
    }
}
