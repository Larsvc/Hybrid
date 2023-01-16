using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamModule : Module
{
    [SerializeField] private float damage = 100f;

    private float maxDamageVelocity = 40f;

    protected override void Start()
    {
        base.Start();
    }

    private float actualDamage
    {
        get { return damage * multiplier; }
    }

    private float force
    {
        get { return 3000f * multiplier; }
    }

    private float multiplier
    {
        get { return player.GetComponent<Rigidbody>().velocity.magnitude / maxDamageVelocity; }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform otherBase = other.transform.root;
        if (otherBase.tag == "Player" && otherBase != transform.root)
        {
            otherBase.GetComponent<HealthEntity>().TakeHit(actualDamage, player.hitmarker);
            otherBase.GetComponent<Rigidbody>().AddForce(transform.forward * force, ForceMode.Force);
            player.GetComponent<Rigidbody>().velocity *= 0.8f;
            player.cam.GetComponent<CameraShake>().startShaking(0.2f, 1f * multiplier, 80f);
        }
    }

}
