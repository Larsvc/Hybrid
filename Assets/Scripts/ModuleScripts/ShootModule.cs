using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ShootModule : Module
{
    protected bool canShoot = true;
    [SerializeField] protected float bulletForce = 30f;
    [SerializeField] protected GameObject bulletPrefab;
    [SerializeField] protected float fireRate = 9f;

    [SerializeField] protected float damage = 6f;

    protected CameraShake camShake;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        camShake = player.cam.GetComponent<CameraShake>();
        canShoot = true;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    protected override void Die()
    {
        player.gunModules.Remove(this);
        base.Die();
    }

    public abstract void Fire();
}
