using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunModule : Module
{
    private bool canShoot = true;
    [SerializeField] private float bulletForce = 30f;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 9f;

    [SerializeField] private float damage = 6f;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    protected override void Update()
    {
        base.Update();
        /*Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 aimPoint = ray.GetPoint(40f);
        Vector3 dir = (aimPoint - transform.position).normalized;

        Debug.DrawRay(transform.position, dir * 100f, Color.red);*/
    }

    public void Fire()
    {
        if (!canShoot)
            return;

       transform.GetComponentInChildren<ParticleSystem>().Play();


        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        Vector3 aimPoint = ray.GetPoint(40f);
        Vector3 dir = (aimPoint - transform.position).normalized;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, dir, out hit, 100f))
        {
            if (FirstParent(hit.collider.transform))
            {
                FirstParent(hit.collider.transform).TakeHit(damage);
            }
        }


        Rigidbody bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity).GetComponent<Rigidbody>();
        bullet.AddForce(dir * bulletForce * bullet.mass, ForceMode.Impulse);

        Camera.main.GetComponent<CameraShake>().startShaking(0.1f, 0.03f, 150f);

        if (hit.transform)
        bullet.GetComponent<Bullet>().hitPoint = hit.point;

        Destroy(bullet.gameObject, 2f);

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
