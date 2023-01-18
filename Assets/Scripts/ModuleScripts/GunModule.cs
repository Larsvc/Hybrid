using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModule : ShootModule
{
    private float chargeAmount;

    private Transform minigunBarrel;
    private float minigunRotationSpeed = 1000f;
    private int bulletCount;

    private float nextFire;
    private float waitTime;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        camShake = player.cam.GetComponent<CameraShake>();

        minigunBarrel = transform.GetChild(1).GetChild(0);
        waitTime = 1f / fireRate;
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        /*Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 aimPoint = ray.GetPoint(40f);
        Vector3 dir = (aimPoint - transform.position).normalized;

        Debug.DrawRay(transform.position, dir * 100f, Color.red);*/
        minigunBarrel.Rotate(Vector3.forward * minigunRotationSpeed * Time.deltaTime * chargeAmount);

        if (chargeAmount > 0)
            chargeAmount -= 1f * Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (Time.time > nextFire && !canShoot)
        {
            canShoot = true;
            nextFire = Time.time + waitTime;
        }
    }

    public override void Fire()
    {
        if (chargeAmount < 1)
        {
            chargeAmount += 3.5f * Time.deltaTime;
            return;
        }

        if (!canShoot)
            return;

        transform.GetComponentInChildren<ParticleSystem>().Play();

        int mask = 1 << gameObject.layer;
        mask = ~mask;

        Ray ray = player.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit camHit;
        bool hasHit = Physics.Raycast(player.cam.transform.position, player.cam.transform.forward, out camHit, 200f, mask);
        Vector3 aimPoint = ray.GetPoint(200f);

        float angle = 2 * Mathf.PI * bulletCount / 10;
        Vector2 spawnPos = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * 0.3f;

        // Transform random position from local to world space
        Vector3 spawnPoint = minigunBarrel.TransformPoint(new Vector3(spawnPos.x, spawnPos.y, 0f));

        Vector3 dir = (aimPoint - spawnPoint).normalized;
        if (hasHit)
            dir = (camHit.point - spawnPoint).normalized; //(aimPoint - transform.position).normalized;


        RaycastHit hit;
        if (Physics.Raycast(spawnPoint, dir, out hit, 200f, mask))
        {
            GameObject hitEffect = Instantiate(PrefabManager.instance.hitEffectParticles, hit.point, Quaternion.identity);
            if (FirstParent(hit.collider.transform))
            {
                float dmg = damage;
                if (FirstParent(hit.collider.transform) is Module)
                    dmg *= 2;
                FirstParent(hit.collider.transform).TakeHit(dmg, player.hitmarker);
            }
            Destroy(hitEffect, 1f);
        }


        Rigidbody bullet = Instantiate(bulletPrefab, spawnPoint, Quaternion.identity).GetComponent<Rigidbody>();
        bullet.AddForce(dir * bulletForce * bullet.mass, ForceMode.Impulse);

        camShake.startShaking(0.1f, 0.03f, 150f);

        if (hit.transform)
        bullet.GetComponent<Bullet>().hitPoint = hit.point;

        Destroy(bullet.gameObject, 2f);

        canShoot = false;

        bulletCount  = (bulletCount + 1) % 10;
    }

    private HealthEntity FirstParent(Transform t)
    {
        if (t.GetComponent<HealthEntity>() && t != transform.root && t != transform)
            return t.GetComponent<HealthEntity>();
        else if (t.parent != null)
            return FirstParent(t.parent);
        else
            return null;
    }

    /*IEnumerator WaitForFireRate()
    {
        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
    }*/
}
