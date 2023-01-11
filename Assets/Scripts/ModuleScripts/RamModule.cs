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
        get { return (damage / maxDamageVelocity) * player.GetComponent<Rigidbody>().velocity.magnitude; }
    }

    private void OnTriggerEnter(Collider other)
    {
        Transform otherBase = other.transform.root;
        if (otherBase.tag == "Player" && otherBase != transform.root)
        {
            other.transform.GetComponent<HealthEntity>().TakeHit(actualDamage);
        }
    }
}
