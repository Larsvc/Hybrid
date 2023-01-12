using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonModule : ShootModule
{

    [SerializeField] private AudioClip cannonShot;
    private AudioSource audioSource;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        camShake = player.cam.GetComponent<CameraShake>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
    }

    public override void Fire()
    {
        if (!canShoot)
            return;

        transform.GetComponentInChildren<ParticleSystem>().Play();

        audioSource.PlayOneShot(cannonShot);

        int mask = 1 << gameObject.layer;
        mask = ~mask;

        Ray ray = player.cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit camHit;
        bool hasHit = Physics.Raycast(player.cam.transform.position, player.cam.transform.forward, out camHit, 100f, mask);
        Vector3 aimPoint = ray.GetPoint(100f);

        player.GetComponent<Rigidbody>().AddForce(-transform.forward * 120f);

        // Transform random position from local to world space
        Vector3 spawnPoint = transform.position + transform.forward * 3f;

        Vector3 dir = (aimPoint - spawnPoint).normalized;
        if (hasHit)
            dir = (camHit.point - spawnPoint).normalized; //(aimPoint - transform.position).normalized;
/*

        RaycastHit hit;
        if (Physics.Raycast(spawnPoint, dir, out hit, 100f, mask))
        {
            GameObject hitEffect = Instantiate(PrefabManager.instance.hitEffectParticles, hit.point, Quaternion.identity);
            if (FirstParent(hit.collider.transform))
            {
                FirstParent(hit.collider.transform).TakeHit(damage);
            }
            Destroy(hitEffect, 1f);
        }*/


        Rigidbody bullet = Instantiate(bulletPrefab, spawnPoint, Quaternion.identity).GetComponent<Rigidbody>();
        bullet.AddForce(dir * bulletForce * bullet.mass, ForceMode.Impulse);
        bullet.GetComponent<Projectile>().mask = mask;

        camShake.startShaking(0.15f, 0.1f, 120f);

        /*if (hit.transform)
        bullet.GetComponent<Bullet>().hitPoint = hit.point;*/

        Destroy(bullet.gameObject, 5f);

        canShoot = false;

       StartCoroutine(WaitForFireRate());
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

    IEnumerator WaitForFireRate()
    {
        yield return new WaitForSeconds(1 / fireRate);
        canShoot = true;
    }
}
